using System;
using System.Collections.Generic;
using System.IO;
using D3DTX_Converter.DirectX;
using D3DTX_Converter.ImageProcessing;
using D3DTX_Converter.Main;
using D3DTX_Converter.Texconv;
using D3DTX_Converter.TexconvOptions;
using D3DTX_Converter.Utilities;
using System.Threading;
using DirectXTexNet;
using System.Linq;
using System.ComponentModel;
using System.Threading.Tasks;

namespace DDS_D3DTX_Converter;

public class Converter
{
    private Converter()
    {
    }

    private static Converter _instance;

    public static Converter GetInstance()
    {
        return _instance ??= new Converter();
    }

    /// <param name="texPath"></param>
    /// <param name="resultPath"></param>
    /// <param name="fixesGenericToDds"></param>
    /// <summary>
    /// Converts multiple texture files from one format to another.
    /// </summary>
    /// <param name="texPath">The path to the folder containing the texture files.</param>
    /// <param name="resultPath">The path to save the converted texture files.</param>
    /// <param name="oldFileType">The file type of the original texture files.</param>
    /// <param name="newFileType">The desired file type for the converted texture files. If not specified, the default is an empty string.</param>
    /// <param name="version">The version of the D3DTX file that the converter should use. If not specified, the default is D3DTXVersion.DEFAULT.</param>
    /// <returns>True if the bulk conversion is successful; otherwise, false.</returns>
    /// <exception cref="FileNotFoundException">Thrown when no files with the specified file type were found in the directory.</exception>
    /// <exception cref="Exception">Thrown when an invalid file type is provided or when one or more conversions fail.</exception>
    public static bool ConvertBulk(string texPath, string resultPath, string oldFileType, string newFileType = "", D3DTXConversionType conversionType = D3DTXConversionType.DEFAULT)
    {
        // Gather the files from the texture folder path into an array
        List<string> textures = new(Directory.GetFiles(texPath));

        // Filter the array so we only get the files required to convert
        textures = IOManagement.FilterFiles(textures, "." + oldFileType);

        // If no image files were found, throw an exception
        if (textures.Count < 1)
        {
            throw new FileNotFoundException($"No files with file type {oldFileType} were found in the directory.");
        }

        // Determine the conversion mode based on the file types
        short mode = GetConversionMode(oldFileType, newFileType);

        // Create an array of threads
        Thread[] threads = new Thread[(int)Math.Ceiling(textures.Count / (double)30)];
        int failedConversions = 0;
        for (int i = 0; i < threads.Length; i++)
        {
            var texturesMiniBulkList = textures.Skip(30 * i).Take(30);

            // Create a new thread and pass the conversion method as a parameter
            threads[i] = new Thread(async () =>
            {
                foreach (var texture in texturesMiniBulkList)
                {
                    try
                    {
                        await PerformConversionAsync(mode, texture, resultPath, newFileType, conversionType);
                    }
                    catch (Exception)
                    {
                        // If an exception is thrown, increment the failedConversions count
                        Interlocked.Increment(ref failedConversions);
                    }
                }
            });

            // Start the thread
            threads[i].Start();
        }

        // Wait for all threads to finish
        foreach (var thread in threads)
        {
            thread.Join();
        }

        // If there are failed conversions, throw an exception
        if (failedConversions > 0)
        {
            throw new Exception($"{failedConversions} conversions failed. Please check the files and try again.");
        }

        // Return true to indicate successful bulk conversion
        return true;
    }

    private static short GetConversionMode(string oldFileType, string newFileType)
    {
        if ("d3dtx".Equals(oldFileType))
        {
            return 0; // d3dtx to dds
        }
        else if ("dds".Equals(oldFileType) && "d3dtx".Equals(newFileType))
        {
            return 1; // dds to d3dtx
        }
        else if ("dds".Equals(oldFileType) &&
            ("png".Equals(newFileType)
            || "jpg".Equals(newFileType)
            || "jpeg".Equals(newFileType)
            || "bmp".Equals(newFileType)
            || "tiff".Equals(newFileType)
            || "tif".Equals(newFileType)))
        {
            return 2; // dds to others
        }
        else if ("png".Equals(oldFileType)
            || "jpg".Equals(oldFileType)
            || "jpeg".Equals(oldFileType)
            || "bmp".Equals(oldFileType)
            || "tiff".Equals(oldFileType)
            || "tif".Equals(oldFileType))
        {
            return 3; // others to dds
        }
        else
        {
            throw new Exception("Invalid file type.");
        }
    }

    private static async Task PerformConversionAsync(short mode, string sourceFilePath, string destinationDirectory, string newFileType, D3DTXConversionType conversionType = D3DTXConversionType.DEFAULT)
    {
        switch (mode)
        {
            case 0:
                ConvertTextureFromD3DtxToDds(sourceFilePath, destinationDirectory, conversionType);
                break;
            case 1:
                ConvertTextureFromDdsToD3Dtx(sourceFilePath, destinationDirectory);
                break;
            case 2:
                await ConvertTextureFromDdsToOthersAsync(sourceFilePath, destinationDirectory, newFileType, true);
                break;
            case 3:
                await ConvertTextureFileFromOthersToDdsAsync(sourceFilePath, destinationDirectory, true);
                break;
            default:
                throw new Exception("Invalid conversion mode.");
        }
    }

    /// <summary>
    /// The main function for reading and converting said .d3dtx into a .dds file
    /// </summary>
    /// <param name="sourceFilePath"></param>
    /// <param name="destinationDirectory"></param>
    public static void ConvertTextureFromD3DtxToDds(string? sourceFilePath, string destinationDirectory, D3DTXConversionType conversionType = D3DTXConversionType.DEFAULT)
    {
        // Null safety validation of inputs.
        if (string.IsNullOrEmpty(sourceFilePath) || string.IsNullOrEmpty(destinationDirectory))
        {
            throw new ArgumentException("Arguments cannot be null in D3DtxToDds function.");
        }

        D3DTX_Master d3dtxFile = new();
        d3dtxFile.Read_D3DTX_File(sourceFilePath, conversionType);

        DDS_Master ddsFile = new(d3dtxFile);

        // Write the dds file to disk
        ddsFile.WriteD3DTXAsDDS(d3dtxFile, destinationDirectory);

        // Write the d3dtx data into a file
        d3dtxFile.WriteD3DTXJSON(Path.GetFileNameWithoutExtension(d3dtxFile.filePath), destinationDirectory);
    }

    /// <summary>
    /// The main function for reading and converting said .dds back into a .d3dtx file
    /// </summary>
    /// <param name="sourceFilePath"></param>
    /// <param name="destinationDirectory"></param>
    public static void ConvertTextureFromDdsToD3Dtx(string? sourceFilePath, string destinationDirectory)
    {
        // Null safety validation of inputs.
        if (string.IsNullOrEmpty(sourceFilePath) || string.IsNullOrEmpty(destinationDirectory))
        {
            throw new ArgumentException("Arguments cannot be null in DdsToD3Dtx function.");
        }

        // Deconstruct the source file path
        string? textureFileDirectory = Path.GetDirectoryName(sourceFilePath);
        string textureFileNameOnly = Path.GetFileNameWithoutExtension(sourceFilePath);

        // Create the names of the following files
        string textureFileNameWithD3Dtx = textureFileNameOnly + Main_Shared.d3dtxExtension;
        string textureFileNameWithJSON = textureFileNameOnly + Main_Shared.jsonExtension;

        // Create the path of these files. If things go well, these files (depending on the version) should exist in the same directory at the original .dds file.
        string textureFilePathJson =
            textureFileDirectory + Path.DirectorySeparatorChar + textureFileNameWithJSON;

        // Create the final path of the d3dtx
        string textureResultPathD3Dtx =
            destinationDirectory + Path.DirectorySeparatorChar + textureFileNameWithD3Dtx;

        // If a json file exists
        if (File.Exists(textureFilePathJson))
        {
            // Create a new d3dtx object
            D3DTX_Master d3dtxMaster = new()
            {
                fileName = sourceFilePath
            };

            // Parse the .json file as a d3dtx
            d3dtxMaster.ReadD3DTXJSON(textureFilePathJson);

            // If the d3dtx is a legacy D3DTX, force the use of the DX9 legacy flag
            DDS_FLAGS flags = d3dtxMaster.isLegacyD3DTX() ? DDS_FLAGS.FORCE_DX9_LEGACY : DDS_FLAGS.NONE;

            // Get the image
            var image = DDS_DirectXTexNet.GetDDSImage(sourceFilePath, flags);

            // Modify the d3dtx file using our dds data
            d3dtxMaster.Modify_D3DTX(image); //ISSUE HERE WITH DXT5 AND MIP MAPS WITH UPSCALED TEXTURES // Edit: This may not be an issue anymore

            // Write our final d3dtx file to disk
            d3dtxMaster.WriteFinalD3DTX(textureResultPathD3Dtx);
        }
        // if we didn't find a json file, we're screwed!
        else
        {
            throw new FileNotFoundException("Conversion failed.\nNo .json file was found for the file.");
        }
    }

    /// <summary>
    /// The main function for reading and converting said .bmp into a .dds file
    /// </summary>
    /// <param name="sourceFilePath"></param>
    /// <param name="destinationDirectory"></param>
    public static async Task ConvertTextureFileFromOthersToDdsAsync(string sourceFilePath, string destinationDirectory,
        bool fixes_generic_to_dds)
    {
        if (string.IsNullOrEmpty(sourceFilePath) || string.IsNullOrEmpty(destinationDirectory))
        {
            throw new ArgumentException("Arguments cannot be null in OthersToDds function.");
        }

        // Deconstruct the source file path
        string textureFileDirectory = Path.GetDirectoryName(sourceFilePath);
        string textureFileNameOnly = Path.GetFileNameWithoutExtension(sourceFilePath);

        // Create the names of the following files
        string textureFileNameWithJSON = textureFileNameOnly + Main_Shared.jsonExtension;

        // Create the path of these files. If things go well, these files (depending on the version) should exist in the same directory at the original .dds file.
        string textureFilePath_JSON = Path.Combine(textureFileDirectory, textureFileNameWithJSON);

        //TODO Update to DirectXTexNet

        // If a json file exists (for newer 5VSM and 6VSM)
        if (File.Exists(textureFilePath_JSON))
        {
            // Create a new d3dtx object
            D3DTX_Master d3dtxFile = new();

            // Parse the .json file as a d3dtx
            d3dtxFile.ReadD3DTXJSON(textureFilePath_JSON);

            MasterOptions options = new()
            {
                outputDirectory = new() { directory = destinationDirectory },
                outputOverwrite = new(),
                outputFileType = new() { fileType = D3DTX_Converter.TexconvEnums.TexconvEnumFileTypes.dds }
            };

            if (d3dtxFile.HasMipMaps() == false)
                options.outputMipMaps = new() { remove = true };

            switch (d3dtxFile.GetTextureType())
            {
                case D3DTX_Converter.TelltaleEnums.T3TextureType.eTxSingleChannelSDFDetailMap:
                    options.outputFormat = new() { format = DXGI_FORMAT.BC3_UNORM };

                    await TexconvApp.RunTexconvAsync(sourceFilePath, options);
                    break;
                case D3DTX_Converter.TelltaleEnums.T3TextureType.eTxBumpmap:
                case D3DTX_Converter.TelltaleEnums.T3TextureType.eTxNormalMap:

                    options.outputFormat = new() { format = DXGI_FORMAT.BC3_UNORM };
                    options.outputTreatTypelessAsUNORM = new();

                    if (fixes_generic_to_dds)
                        options.outputSwizzle = new() { mask = "abgr" };

                    await TexconvApp.RunTexconvAsync(sourceFilePath, options);
                    break;
                case D3DTX_Converter.TelltaleEnums.T3TextureType.eTxNormalXYMap:

                    options.outputFormat = new() { format = DXGI_FORMAT.BC5_UNORM };
                    //options.outputSRGB = new() { srgbMode = TexconvEnums.TexconvEnumSrgb.srgbo };
                    options.outputTreatTypelessAsUNORM = new();

                    if (fixes_generic_to_dds)
                        options.outputSwizzle = new() { mask = "rg00" };

                    await TexconvApp.RunTexconvAsync(sourceFilePath, options);
                    break;
                default:
                    if (ImageUtilities.IsImageOpaque(sourceFilePath))
                        options.outputFormat = new() { format = DXGI_FORMAT.BC1_UNORM };
                    else
                        options.outputFormat = new() { format = DXGI_FORMAT.BC3_UNORM };

                    await TexconvApp.RunTexconvAsync(sourceFilePath, options);
                    break;
            }
        }
        // If we didn't find a json file, we're screwed!
        else
        {
            throw new FileNotFoundException("Conversion failed.\nNo .json file was found for the file.");
        }

        string outputTextureFilePath = Path.Combine(destinationDirectory, Path.GetFileNameWithoutExtension(sourceFilePath) + Main_Shared.ddsExtension);

        // Check if the output file exists, if it doesn't then the conversion failed so notify the user
        if (File.Exists(outputTextureFilePath) == false)
        {
            throw new FileNotFoundException("Conversion failed. Output file was not created.");
        }
    }

    /// <summary>
    /// The main function for reading and converting said .dds into the  more accessible supported file formats.
    /// </summary>
    /// <param name="sourceFilePath"></param>
    /// <param name="destinationDirectory"></param>
    /// <param name="newFileType"></param>
    /// <param name="fixesDdsToGeneric"></param>
    public static async Task ConvertTextureFromDdsToOthersAsync(string? sourceFilePath, string destinationDirectory,
        string newFileType, bool fixesDdsToGeneric)
    {
        // Null safety validation of inputs.
        if (string.IsNullOrEmpty(sourceFilePath) || string.IsNullOrEmpty(destinationDirectory) ||
            string.IsNullOrEmpty(newFileType))
        {
            throw new ArgumentException("Arguments cannot be null in DdsToOthers function.");
        }

        // Deconstruct the source file path
        string? textureFileDirectory = Path.GetDirectoryName(sourceFilePath);
        string textureFileNameOnly = Path.GetFileNameWithoutExtension(sourceFilePath);

        D3DTX_Converter.TexconvEnums.TexconvEnumFileTypes fileType =
            GetEnumFileType(newFileType);

        // Create the names of the following files
        string textureFileNameWithJson = textureFileNameOnly + Main_Shared.jsonExtension;

        // Create the path of these files. If things go well, these files (depending on the version) should exist in the same directory at the original .dds file.
        string textureFilePathJson = textureFileDirectory + Path.DirectorySeparatorChar + textureFileNameWithJson;
        string outputTextureFilePath =
            destinationDirectory + Path.DirectorySeparatorChar + textureFileNameOnly + "." + newFileType;

        // If a json file exists (for newer 5VSM and 6VSM)
        if (File.Exists(textureFilePathJson))
        {
            // Create a new d3dtx object
            D3DTX_Master d3dtxFile = new();

            // Parse the .json file as a d3dtx
            d3dtxFile.ReadD3DTXJSON(textureFilePathJson);

            //TODO Update to DirectXTexNet

            // Get the d3dtx texture type
            D3DTX_Converter.TelltaleEnums.T3TextureType d3dtxTextureType = d3dtxFile.GetTextureType();

            if (d3dtxTextureType == D3DTX_Converter.TelltaleEnums.T3TextureType.eTxBumpmap ||
                d3dtxTextureType == D3DTX_Converter.TelltaleEnums.T3TextureType.eTxNormalMap)
            {
                MasterOptions options = new()
                {
                    outputDirectory = new() { directory = destinationDirectory },
                    outputOverwrite = new(),
                    outputFileType = new() { fileType = fileType }
                };

                if (fixesDdsToGeneric)
                    options.outputSwizzle = new() { mask = "abgr" };

                await TexconvApp.RunTexconvAsync(sourceFilePath, options);
            }
            else if (d3dtxTextureType == D3DTX_Converter.TelltaleEnums.T3TextureType.eTxNormalXYMap)
            {
                if (fixesDdsToGeneric)
                    NormalMapProcessing.FromDDS_NormalMapReconstructZ(sourceFilePath, outputTextureFilePath);
                else
                    NormalMapConvert.ConvertNormalMapToOthers(sourceFilePath, newFileType);
            }
            else
            {
                MasterOptions options = new()
                {
                    outputDirectory = new() { directory = destinationDirectory },
                    outputOverwrite = new(),
                    outputFileType = new() { fileType = fileType }
                };
                await TexconvApp.RunTexconvAsync(sourceFilePath, options);
            }
        }
        // If we didn't find a json file, we're screwed!
        else
        {
            MasterOptions options = new()
            {
                outputDirectory = new() { directory = destinationDirectory },
                outputOverwrite = new(),
                outputFileType = new() { fileType = fileType }
            };
            await TexconvApp.RunTexconvAsync(sourceFilePath, options);
            throw new FileNotFoundException(
                "No .json file was found for the file.\nDefaulting to classic conversion.");
        }

        // Check if the output file exists, if it doesn't then the conversion failed so notify the user
        if (!File.Exists(outputTextureFilePath))
        {
            throw new FileNotFoundException("Conversion failed. Output file was not created.");
        }
    }


    /// <summary>
    /// Helper function for the converter. Gets the Texconv enum from the provided extension.
    /// </summary>
    private static D3DTX_Converter.TexconvEnums.TexconvEnumFileTypes GetEnumFileType(string extension)
    {
        if (string.IsNullOrEmpty(extension))
        {
            throw new ArgumentNullException("File type cannot be null.");
        }

        return extension switch
        {
            "bmp" => D3DTX_Converter.TexconvEnums.TexconvEnumFileTypes.bmp,
            "png" => D3DTX_Converter.TexconvEnums.TexconvEnumFileTypes.png,
            "jpg" => D3DTX_Converter.TexconvEnums.TexconvEnumFileTypes.jpg,
            "jpeg" => D3DTX_Converter.TexconvEnums.TexconvEnumFileTypes.jpeg,
            "tif" => D3DTX_Converter.TexconvEnums.TexconvEnumFileTypes.tif,
            "tiff" => D3DTX_Converter.TexconvEnums.TexconvEnumFileTypes.tiff,
            _ => throw new Exception("File type " + extension + " is not supported."),
        };
    }
}
