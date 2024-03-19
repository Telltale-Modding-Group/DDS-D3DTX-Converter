using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using D3DTX_Converter_Avalonia_GUI.DirectX;
using D3DTX_Converter.DirectX;
using D3DTX_Converter.ImageProcessing;
using D3DTX_Converter.Main;
using D3DTX_Converter.ProgramDebug;
using D3DTX_Converter.Texconv;
using D3DTX_Converter.TexconvOptions;
using D3DTX_Converter.Utilities;

namespace DDS_D3DTX_Converter
{
    public class Converter
    {
        private WorkingDirectory _workingDirectory;
        private ConsoleWriter consoleWriter;

        private Converter()
        {
        }

        private static Converter _instance;

        public static Converter GetInstance()
        {
            return _instance ??= new Converter();
        }

        public void Execute(bool fixes_generic_to_dds)
        {
            //intro message
            //   ConsoleFunctions.SetConsoleColor(ConsoleColor.Blue, ConsoleColor.White);
            //  Console.WriteLine("BMP to DDS Texture Converter");

            //-----------------GET TEXTURE FOLDER PATH-----------------
            //  ConsoleFunctions.SetConsoleColor(ConsoleColor.DarkGray, ConsoleColor.White);
            //Console.WriteLine("Enter the folder path with the BMP textures.");

            //texture folder path (containing the path to the textures to be converted)
            // string textureFolderPath = Program_Shared.GetFolderPathFromUser();

            //-----------------GET RESULT FOLDER PATH-----------------
            // ConsoleFunctions.SetConsoleColor(ConsoleColor.DarkGray, ConsoleColor.White);
            Console.WriteLine("Enter the resulting path where converted DDS textures will be stored.");

            //result folder path (will contain the converted textures)
            string resultFolderPath = Program_Shared.GetFolderPathFromUser();

            //-----------------START CONVERSION-----------------
            //notify the user we are starting
            Console.WriteLine("Conversion Starting...");

            //we got our paths, so lets begin
            ConvertBulk(_workingDirectory.WorkingDirectoryPath, resultFolderPath, fixes_generic_to_dds);


            //  ConvertTextureFile(_workingDirectory.workingDirectoryFile, _workingDirectory.workingDirectoryPath, fixes_generic_to_dds);

            //once the process is finished, it will come back here and we will notify the user that we are done
            Console.WriteLine("Conversion Finished.");
        }

        /// <summary>
        /// Starts the conversion process for the CLI app.
        /// Originally used for converting all files in a folder in the user's desired format.
        /// </summary>
        /// <param name="texPath"></param>
        /// <param name="resultPath"></param>
        /// <param name="fixesGenericToDds"></param>
        public static void ConvertBulk(string texPath, string resultPath, bool fixesGenericToDds)
        {
            Console.WriteLine("Collecting Files..."); //notify the user we are collecting files

            //gather the files from the texture folder path into an array
            List<string> textures = new List<string>(Directory.GetFiles(texPath));

            Console.WriteLine("Filtering Textures..."); //notify the user we are filtering the array

            //filter the array so we only get .bmp files
            textures = IOManagement.FilterFiles(textures, Main_Shared.bmpExtension);

            //if no bmp files were found, abort the program from going on any further (we don't have any files to convert!)
            if (textures.Count < 1)
            {
                Console.WriteLine(
                    "No .bmp files were found, aborting."); //notify the user we found x amount of bmp files in the array
                Console.ResetColor();
                return;
            }

            Console.WriteLine("Found {0} Textures.",
                textures.Count.ToString()); //notify the user we found x amount of bmp files in the array
            Console.WriteLine("Starting..."); //notify the user we are starting

            //Thread[] threads = new Thread[textures.Count];

            //run a loop through each of the found textures and convert each one
            foreach (var name in textures)
            {
                //build the path for the resulting file
                string textureFileName = Path.GetFileName(name); //get the file name of the file + extension

                Console.WriteLine("||||||||||||||||||||||||||||||||");
                Console.WriteLine("Converting '{0}'...", textureFileName); //notify the user are converting 'x' file.
                Console.ResetColor();

                //runs the main method for converting the texture
                //   ConvertTextureFile(textures[i], resultPath, fixes_generic_to_dds);

                Console.WriteLine("Finished converting '{0}'...",
                    textureFileName); //notify the user we finished converting 'x' file.
            }
        }

        /// <summary>
        /// The main function for reading and converting said .bmp into a .dds file
        /// </summary>
        /// <param name="sourceFilePath"></param>
        /// <param name="destinationDirectory"></param>
        public static void ConvertTextureFileFromOthersToDds(string sourceFilePath, string destinationDirectory,
            bool fixes_generic_to_dds)
        {
            if (string.IsNullOrEmpty(sourceFilePath) || string.IsNullOrEmpty(destinationDirectory))
            {
                throw new ArgumentException("Arguments cannot be null in OthersToDds function.");
            }

            //DOC
            //If for some reason someone deletes a file in the file explorer while the directory is displayed, it should result in an error
            if (!File.Exists(sourceFilePath))
            {
                throw new FileNotFoundException("Conversion failed.\nFile no longer exists in the current directory.");
            }

            if (!Directory.Exists(destinationDirectory))
            {
                throw new DirectoryNotFoundException("Conversion failed.\nInvalid output directory.");
            }

            //deconstruct the source file path
            string textureFileDirectory = Path.GetDirectoryName(sourceFilePath);
            string textureFileNameOnly = Path.GetFileNameWithoutExtension(sourceFilePath);

            //create the names of the following files
            string textureFileNameWithJSON = textureFileNameOnly + Main_Shared.jsonExtension;

            //create the path of these files. If things go well, these files (depending on the version) should exist in the same directory at the original .dds file.
            string textureFilePath_JSON = textureFileDirectory + Path.DirectorySeparatorChar + textureFileNameWithJSON;

            //if a json file exists (for newer 5VSM and 6VSM)
            if (File.Exists(textureFilePath_JSON))
            {
                //create a new d3dtx object
                D3DTX_Master d3dtxFile = new();

                //parse the .json file as a d3dtx
                d3dtxFile.Read_D3DTX_JSON(textureFilePath_JSON);

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
                        options.outputFormat = new() { format = DirectXTexNet.DXGI_FORMAT.BC3_UNORM };

                        TexconvApp.RunTexconv(sourceFilePath, options);
                        break;
                    case D3DTX_Converter.TelltaleEnums.T3TextureType.eTxBumpmap:
                    case D3DTX_Converter.TelltaleEnums.T3TextureType.eTxNormalMap:

                        options.outputFormat = new() { format = DirectXTexNet.DXGI_FORMAT.BC3_UNORM };
                        options.outputTreatTypelessAsUNORM = new();

                        if (fixes_generic_to_dds)
                            options.outputSwizzle = new() { mask = "abgr" };

                        TexconvApp.RunTexconv(sourceFilePath, options);

                        break;
                    case D3DTX_Converter.TelltaleEnums.T3TextureType.eTxNormalXYMap:

                        options.outputFormat = new() { format = DirectXTexNet.DXGI_FORMAT.BC5_UNORM };
                        //options.outputSRGB = new() { srgbMode = TexconvEnums.TexconvEnumSrgb.srgbo };
                        options.outputTreatTypelessAsUNORM = new();

                        if (fixes_generic_to_dds)
                            options.outputSwizzle = new() { mask = "rg00" };

                        TexconvApp.RunTexconv(sourceFilePath, options);

                        break;
                    default:
                        if (ImageUtilities.IsImageOpaque(sourceFilePath))
                            options.outputFormat = new() { format = DirectXTexNet.DXGI_FORMAT.BC1_UNORM };
                        else
                            options.outputFormat = new() { format = DirectXTexNet.DXGI_FORMAT.BC3_UNORM };

                        TexconvApp.RunTexconv(sourceFilePath, options);
                        break;
                }
            }
            //if we didn't find a json file, we're screwed!
            else
            {
                throw new FileNotFoundException("Conversion failed.\nNo .json file was found for the file.");
            }

            string outputTextureFilePath = destinationDirectory + Path.DirectorySeparatorChar +
                                           Path.GetFileNameWithoutExtension(sourceFilePath) + Main_Shared.ddsExtension;

            //check if the output file exists, if it doesn't then the conversion failed so notify the user
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
        public static void ConvertTextureFromDdsToOthers(string? sourceFilePath, string destinationDirectory,
            string newFileType,
            bool fixesDdsToGeneric)
        {
            //Null safety validation of inputs.
            if (string.IsNullOrEmpty(sourceFilePath) || string.IsNullOrEmpty(destinationDirectory) ||
                string.IsNullOrEmpty(newFileType))
            {
                throw new ArgumentException("Arguments cannot be null in DdsToOthers function.");
            }

            //Check if the source file exists.
            if (!File.Exists(sourceFilePath))
            {
                throw new FileNotFoundException("Conversion failed.\nFile no longer exists in the current directory.");
            }

            //Check if the destination directory exists.
            if (!Directory.Exists(destinationDirectory))
            {
                throw new DirectoryNotFoundException("Conversion failed.\nInvalid output directory.");
            }

            //deconstruct the source file path
            string? textureFileDirectory = Path.GetDirectoryName(sourceFilePath);
            string textureFileNameOnly = Path.GetFileNameWithoutExtension(sourceFilePath);

            D3DTX_Converter.TexconvEnums.TexconvEnumFileTypes fileType =
                GetEnumFileType(newFileType);

            //create the names of the following files
            string textureFileNameWithJson = textureFileNameOnly + Main_Shared.jsonExtension;

            //create the path of these files. If things go well, these files (depending on the version) should exist in the same directory at the original .dds file.
            string textureFilePathJson = textureFileDirectory + Path.DirectorySeparatorChar + textureFileNameWithJson;
            string outputTextureFilePath =
                destinationDirectory + Path.DirectorySeparatorChar + textureFileNameOnly + newFileType;

            //if a json file exists (for newer 5VSM and 6VSM)
            if (File.Exists(textureFilePathJson))
            {
                //create a new d3dtx object
                D3DTX_Master d3dtxFile = new();

                //parse the .json file as a d3dtx
                d3dtxFile.Read_D3DTX_JSON(textureFilePathJson);

                //get the d3dtx texture type
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

                    TexconvApp.RunTexconv(sourceFilePath, options);
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
                    TexconvApp.RunTexconv(sourceFilePath, options);
                }
            }
            //if we didn't find a json file, we're screwed!
            else
            {
                MasterOptions options = new()
                {
                    outputDirectory = new() { directory = destinationDirectory },
                    outputOverwrite = new(),
                    outputFileType = new() { fileType = fileType }
                };

                TexconvApp.RunTexconv(sourceFilePath, options);
                throw new FileNotFoundException(
                    "No .json file was found for the file.\nDefaulting to classic conversion.");
            }

            //check if the output file exists, if it doesn't then the conversion failed so notify the user
            if (File.Exists(outputTextureFilePath) == false)
            {
                throw new FileNotFoundException("Conversion failed. Output file was not created.");
            }
        }

        /// <summary>
        /// The main function for reading and converting said .d3dtx into a .dds file
        /// </summary>
        /// <param name="sourceFilePath"></param>
        /// <param name="destinationDirectory"></param>
        public static void ConvertTextureFromD3DtxToDds(string? sourceFilePath, string destinationDirectory)
        {
            //Null safety validation of inputs.
            if (string.IsNullOrEmpty(sourceFilePath) || string.IsNullOrEmpty(destinationDirectory))
            {
                throw new ArgumentException("Arguments cannot be null in D3DtxToDds function.");
            }

            D3DTX_Master d3dtxFile = new();
            d3dtxFile.Read_D3DTX_File(sourceFilePath);

            D3DTX_Converter.TelltaleEnums.T3TextureType textureType = d3dtxFile.GetTextureType();

            DDS_Master ddsFile = new(d3dtxFile);

            //write cubemap
            if (textureType == D3DTX_Converter.TelltaleEnums.T3TextureType.eTxEnvMap ||
                textureType == D3DTX_Converter.TelltaleEnums.T3TextureType.eTxPrefilteredEnvCubeMapHDR || textureType ==
                D3DTX_Converter.TelltaleEnums.T3TextureType.eTxPrefilteredEnvCubeMapHDRScaled)
            {
                //write the dds file to disk
                ddsFile.Write_D3DTX_AsDDS(d3dtxFile, destinationDirectory);

                //write the d3dtx data into a file
                string jsonPath = destinationDirectory + Path.DirectorySeparatorChar +
                                  Path.GetFileNameWithoutExtension(d3dtxFile.filePath);
                d3dtxFile.Write_D3DTX_JSON(Path.GetFileNameWithoutExtension(d3dtxFile.filePath), jsonPath);
            }
            //write regular single images
            else
            {
                //write the dds file to disk
                ddsFile.Write_D3DTX_AsDDS(d3dtxFile, destinationDirectory);

                //write the d3dtx data into a file
                d3dtxFile.Write_D3DTX_JSON(Path.GetFileNameWithoutExtension(d3dtxFile.filePath),
                    destinationDirectory);
            }
        }

        /// <summary>
        /// The main function for reading and converting said .dds back into a .d3dtx file
        /// </summary>
        /// <param name="sourceFilePath"></param>
        /// <param name="destinationDirectory"></param>
        public static void ConvertTextureFromDdsToD3Dtx(string? sourceFilePath, string destinationDirectory)
        {
            //Null safety validation of inputs.
            if (string.IsNullOrEmpty(sourceFilePath) || string.IsNullOrEmpty(destinationDirectory))
            {
                throw new ArgumentException("Arguments cannot be null in DdsToD3Dtx function.");
            }

            if (!File.Exists(sourceFilePath))
            {
                throw new FileNotFoundException("Conversion failed.\nFile no longer exists in the current directory.");
            }

            //deconstruct the source file path
            string? textureFileDirectory = Path.GetDirectoryName(sourceFilePath);
            string textureFileNameOnly = Path.GetFileNameWithoutExtension(sourceFilePath);

            //create the names of the following files
            string textureFileNameWithD3Dtx = textureFileNameOnly + Main_Shared.d3dtxExtension;
            string textureFileNameWithJSON = textureFileNameOnly + Main_Shared.jsonExtension;

            //create the path of these files. If things go well, these files (depending on the version) should exist in the same directory at the original .dds file.
            string textureFilePathJson =
                textureFileDirectory + Path.DirectorySeparatorChar + textureFileNameWithJSON;

            //create the final path of the d3dtx
            string textureResultPathD3Dtx =
                destinationDirectory + Path.DirectorySeparatorChar + textureFileNameWithD3Dtx;

            //if a json file exists
            if (File.Exists(textureFilePathJson))
            {
                //create a new d3dtx object
                D3DTX_Master d3dtxFile = new();

                //parse the .json file as a d3dtx
                d3dtxFile.Read_D3DTX_JSON(textureFilePathJson);

                //read in our DDS file
                DDS_Master dds = new(sourceFilePath, false);

                //maybe this??
                DDS_DirectXTexNet_ImageSection[] sections = DDS_DirectXTexNet.GetDDSImageSections(sourceFilePath);

                //dds parse test
                //dds.TEST_WriteDDSToDisk(textureResultPath_DDS); //<-------- THIS IS CORRECT AND PARSES A DDS FILE PERFECTLY

                //modify the d3dtx file using our dds data
                d3dtxFile.Modify_D3DTX(dds, sections); //ISSUE HERE WITH DXT5 AND MIP MAPS WITH UPSCALED TEXTURES

                //write our final d3dtx file to disk
                d3dtxFile.Write_Final_D3DTX(textureResultPathD3Dtx);
            }
            //if we didn't find a json file, we're screwed!
            else
            {
                throw new FileNotFoundException("Conversion failed.\nNo .json file was found for the file.");
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

            switch (extension)
            {
                case ".bmp": return D3DTX_Converter.TexconvEnums.TexconvEnumFileTypes.bmp;
                case ".png": return D3DTX_Converter.TexconvEnums.TexconvEnumFileTypes.png;
                case ".jpg": return D3DTX_Converter.TexconvEnums.TexconvEnumFileTypes.jpg;
                case ".jpeg": return D3DTX_Converter.TexconvEnums.TexconvEnumFileTypes.jpeg;
                case ".tif": return D3DTX_Converter.TexconvEnums.TexconvEnumFileTypes.tif;
                case ".tiff": return D3DTX_Converter.TexconvEnums.TexconvEnumFileTypes.tiff;
                default: throw new Exception("File type " + extension + " is not supported.");
            }
        }
    }
}