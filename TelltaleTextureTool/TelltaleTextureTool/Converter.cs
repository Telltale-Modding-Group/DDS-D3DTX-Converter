using System;
using System.Collections.Generic;
using System.IO;
using TelltaleTextureTool.DirectX;
using TelltaleTextureTool.Main;
using TelltaleTextureTool.Utilities;
using System.Threading;
using System.Linq;
using System.ComponentModel;
using Hexa.NET.DirectXTex;
using TelltaleTextureTool.Telltale.FileTypes.D3DTX;
using TelltaleTextureTool.TelltaleEnums;
using TelltaleTextureTool.Graphics;

namespace TelltaleTextureTool;

public static class Converter
{
    public static string[] GetExtension(TextureType textureType)
    {
        return textureType switch
        {
            TextureType.D3DTX => [Main_Shared.d3dtxExtension],
            TextureType.DDS => [Main_Shared.ddsExtension],
            TextureType.KTX => [Main_Shared.ktxExtension],
            TextureType.KTX2 => [Main_Shared.ktx2Extension],
            TextureType.PNG => [Main_Shared.pngExtension],
            TextureType.JPEG => [Main_Shared.jpegExtension, Main_Shared.jpgExtension],
            TextureType.BMP => [Main_Shared.bmpExtension],
            TextureType.TIFF => [Main_Shared.tiffExtension, Main_Shared.tifExtension],
            TextureType.TGA => [Main_Shared.tgaExtension],
            TextureType.HDR => [Main_Shared.hdrExtension],
            _ => throw new InvalidEnumArgumentException("Invalid texture type."),
        };
    }

    /// <summary>
    /// Converts multiple texture files from one format to another.
    /// </summary>
    /// <param name="texPath">The path to the folder containing the texture files.</param>
    /// <param name="resultPath">The path to save the converted texture files.</param>
    /// <param name="options">The advanced options to apply to the texture files.</param>
    /// <param name="oldTextureType">The file type of the original texture files.</param>
    /// <param name="newTextureType">The file type to convert the texture files to.</param>
    /// <returns>True if the bulk conversion is successful; otherwise, false.</returns>
    /// <exception cref="FileNotFoundException">Thrown when no files with the specified file type were found in the directory.</exception>
    /// <exception cref="Exception">Thrown when an invalid file type is provided or when one or more conversions fail.</exception>
    public static bool ConvertBulk(string texPath, string resultPath, ImageAdvancedOptions options, TextureType oldTextureType, TextureType newTextureType = TextureType.Unknown)
    {
        // Gather the files from the texture folder path into an array
        List<string> textures = new(Directory.GetFiles(texPath));

        // Filter the array so we only get the files required to convert
        textures = IOManagement.FilterFiles(textures, GetExtension(oldTextureType));

        // If no image files were found, throw an exception
        if (textures.Count < 1)
        {
            throw new FileNotFoundException($"No files with file type {Enum.GetName(oldTextureType)} were found in the directory.");
        }

        // Create an array of threads
        Thread[] threads = new Thread[(int)Math.Ceiling(textures.Count / (double)30)];
        int failedConversions = 0;
        for (int i = 0; i < threads.Length; i++)
        {
            var texturesMiniBulkList = textures.Skip(30 * i).Take(30);

            // Create a new thread and pass the conversion method as a parameter
            threads[i] = new Thread(() =>
            {
                foreach (var texture in texturesMiniBulkList)
                {
                    try
                    {
                        ConvertTexture(texture, resultPath, options, oldTextureType, newTextureType);
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

    public static void ConvertTexture(string sourcePath, string resultPath, ImageAdvancedOptions options, TextureType oldTextureType, TextureType newTextureType)
    {
        if (oldTextureType == newTextureType)
        {
            return;
        }

        if (oldTextureType == TextureType.D3DTX)
        {
            switch (newTextureType)
            {
                case TextureType.DDS:
                case TextureType.PNG:
                case TextureType.JPEG:
                case TextureType.BMP:
                case TextureType.TIFF:
                case TextureType.TGA:
                case TextureType.HDR:
                    ConvertTextureFromD3DtxToOthers(sourcePath, resultPath, newTextureType, options); break;
                default:
                    throw new Exception("Invalid file type.");
            }
        }
        else if (oldTextureType == TextureType.DDS)
        {
            switch (newTextureType)
            {
                case TextureType.D3DTX:
                    ConvertTextureFromOthersToD3Dtx(sourcePath, resultPath, oldTextureType, options);
                    break;
                default:
                    throw new Exception("Invalid file type.");
            }
        }
        else if (oldTextureType is TextureType.PNG or TextureType.JPEG or TextureType.BMP or TextureType.TIFF or TextureType.TGA or TextureType.HDR)
        {
            switch (newTextureType)
            {
                case TextureType.D3DTX:
                    ConvertTextureFromOthersToD3Dtx(sourcePath, resultPath, oldTextureType, options); break;
                default:
                    throw new Exception("Invalid file type.");
            }
        }
        else
        {
            throw new Exception("Invalid file type.");
        }
    }

    /// <summary>
    /// The main function for reading and converting said .d3dtx into a .dds file
    /// </summary>
    /// <param name="sourceFilePath"></param>
    /// <param name="destinationDirectory"></param>
    public static void ConvertTextureFromD3DtxToOthers(string sourceFilePath, string destinationDirectory, TextureType newTextureType, ImageAdvancedOptions options)
    {
        // Null safety validation of inputs.
        if (string.IsNullOrEmpty(sourceFilePath) || string.IsNullOrEmpty(destinationDirectory))
        {
            throw new ArgumentException("Arguments cannot be null in D3DtxToDds function.");
        }

        D3DTX_Master d3dtxFile = new();
        d3dtxFile.ReadD3DTXFile(sourceFilePath, options.GameID, options.IsLegacyConsole);

        DDS_Master ddsFile = new(d3dtxFile);

        var array = ddsFile.GetData(d3dtxFile);

        Texture texture = new(array, TextureType.D3DTX);

        texture.ChangePreviewImage(options, true);
        texture.SaveTexture(Path.Combine(destinationDirectory, Path.GetFileNameWithoutExtension(sourceFilePath)), newTextureType);
        texture.Release();

        // Write the d3dtx data into a file
        d3dtxFile.WriteD3DTXJSON(Path.GetFileNameWithoutExtension(d3dtxFile.FilePath), destinationDirectory);
    }

    /// <summary>
    /// The main function for reading and converting said .dds back into a .d3dtx file
    /// </summary>
    /// <param name="sourceFilePath"></param>
    /// <param name="destinationDirectory"></param>
    public static void ConvertTextureFromOthersToD3Dtx(string sourceFilePath, string destinationDirectory, TextureType oldTextureType, ImageAdvancedOptions options)
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
            D3DTX_Master d3dtxMaster = new();

            // Parse the .json file as a d3dtx
            try
            {
                d3dtxMaster.ReadD3DTXJSON(textureFilePathJson);
            }
            catch (Exception)
            {
                throw new Exception("Conversion failed.\nFailed to read the .d3dtx file.");
            }

            // If the d3dtx is a legacy D3DTX, force the use of the DX9 legacy flag
            DDSFlags flags = d3dtxMaster.IsLegacyD3DTX() ? DDSFlags.ForceDx9Legacy : DDSFlags.None;

            Texture texture = new(sourceFilePath, oldTextureType, flags);

            // Set the options for the converter
            if (d3dtxMaster.d3dtxMetadata.TextureType is T3TextureType.eTxBumpmap or
                            T3TextureType.eTxNormalMap)
            {
                options.IsTelltaleNormalMap = true;
            }
            else if (d3dtxMaster.d3dtxMetadata.TextureType is T3TextureType.eTxNormalXYMap)
            {
                options.IsTelltaleNormalMap = true;
            }

            if (d3dtxMaster.d3dtxMetadata.SurfaceGamma is T3SurfaceGamma.sRGB)
            {
                options.IsSRGB = true;
            }

            texture.ChangePreviewImage(options, true);

            // Get the image
            texture.GetDDSInformation(out D3DTXMetadata metadata, out ImageSection[] sections, flags);

            if (options.EnableSwizzle)
            {
                metadata.Platform = options.PlatformType;
            }

            Console.WriteLine("DXGI: " + texture.Metadata.Format);
            Console.WriteLine(metadata.Format);
            // Modify the d3dtx file using our dds data
            d3dtxMaster.ModifyD3DTX(metadata, sections);

            texture.Release();

            // Write our final d3dtx file to disk
            d3dtxMaster.WriteFinalD3DTX(textureResultPathD3Dtx);
        }
        // if we didn't find a json file, we're screwed!
        else
        {
            throw new FileNotFoundException("Conversion failed.\nNo .json file was found for the file.");
        }
    }
}
