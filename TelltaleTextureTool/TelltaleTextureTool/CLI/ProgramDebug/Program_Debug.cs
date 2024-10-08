// using System;
// using TelltaleTextureTool.Main;

// namespace TelltaleTextureTool.ProgramDebug
// {
//     public static class Program_Debug
//     {
//         //----------------------CONVERSION MODES----------------------

//         /*
//          * HOW TO USE: Change the boolean value of a certain mode to 'true' to enable the desired mode.
//          * Read the output for error messages. 
//         */

//         //main conversion to and from d3dtx

//         public static bool fixes_generic_to_dds = true; //FIXES normal maps and swizzle channels
//         public static bool fixes_dds_to_generic = true; //FIXES normal maps and swizzle channels

//         public const int D3DTX_TO_DDS = 0; //.d3dtx ---> .dds
//         public const int DDS_TO_D3DTX = 1; //.dds ---> .d3dtx
//         public const int DDS_TO_OTHERS = 2; //.dds ---> .png/.jpg etc.
//         public const int OTHERS_TO_DDS = 3; //.png/.jpg etc. ---> .dds

//         //dds into standard images
//         public const int DDS_TO_PNG = 0; //.dds <---> .png
//         public const int DDS_TO_JPEG = 1; //.dds <---> .jpg
//         public const int DDS_TO_TIFF = 2; //.dds <---> .tiff
//         public const int DDS_TO_BMP = 3; //.dds <---> .bmp

//         //----------------------CONVERSION MODES END----------------------

//         /// <summary>
//         /// Main application method
//         /// </summary>
//         /// <param name="args"></param>
//         public static void Main(string[] args)
//         {
//             ConsoleFunctions.SetConsoleColor(ConsoleColor.Blue, ConsoleColor.White);
//             Console.WriteLine("Telltale Texture Mod Tool v2.4.0 CLI");

//             ConsoleFunctions.SetConsoleColor(ConsoleColor.DarkGray, ConsoleColor.White);
//             Console.WriteLine("Enter the file conversion mode:");
//             int fileConversionMode = Program_Shared.GetFileConversionMode();

//             ConsoleFunctions.SetConsoleColor(ConsoleColor.DarkGray, ConsoleColor.White);
//             Console.WriteLine("Do you want to convert whole a folder? Write Y for yes or N for no.");
//             bool bulkConvert = Program_Shared.GetBulkConversionBoolean();

//             string path;
//             if (bulkConvert)
//             {
//                 Console.WriteLine("Enter the folder path with the textures to convert.");
//                 path = Program_Shared.GetFolderPathFromUser();
//             }
//             else
//             {
//                 Console.WriteLine("Enter the texture file path to convert.");
//                 path = Program_Shared.GetFilePathFromUser();
//             }

//             Console.WriteLine("Enter the file path you wish to convert.");
//             string resultPath = Program_Shared.GetFolderPathFromUser();

//             string originalFormat;
//             string formatToConvert;
//             int d3dtxConversionMode = 0;

//             if (fileConversionMode == D3DTX_TO_DDS)
//             {
//                 originalFormat = "dds";
//                 formatToConvert = "d3dtx";

//                 Console.WriteLine("Enter the D3DTX conversion mode:");
//                 d3dtxConversionMode = Program_Shared.GetD3DTXConversionMode();
//             }
//             else if (fileConversionMode == DDS_TO_D3DTX)
//             {
//                 originalFormat = "dds";
//                 formatToConvert = "d3dtx";
//             }
//             else if (fileConversionMode == DDS_TO_OTHERS)
//             {
//                 originalFormat = "dds";

//                 Console.WriteLine("Enter the other file type mode:");
//                 int fileType = Program_Shared.GetOthersConversionMode();
//                 formatToConvert = fileType switch
//                 {
//                     DDS_TO_PNG => "png",
//                     DDS_TO_JPEG => "jpeg",
//                     DDS_TO_TIFF => "tif",
//                     DDS_TO_BMP => "bmp",
//                     _ => "png",
//                 };
//             }
//             else
//             {
//                 int fileType = Program_Shared.GetOthersConversionMode();
//                 originalFormat = fileType switch
//                 {
//                     DDS_TO_PNG => "png",
//                     DDS_TO_JPEG => "jpeg",
//                     DDS_TO_TIFF => "tif",
//                     DDS_TO_BMP => "bmp",
//                     _ => "png",
//                 };
//                 formatToConvert = "dds";
//             }

//             try
//             {
//                 ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Green);
//                 Console.WriteLine("Conversion Starting...");
//                 if (bulkConvert)
//                 {
//                    // Converter_CLI.ConvertBulk(path, resultPath, originalFormat, formatToConvert, (D3DTXVersion)d3dtxConversionMode);
//                 }
//                 else
//                 {
//                     if (originalFormat == "d3dtx")
//                     {
//                        // Converter_CLI.ConvertTextureFromD3DtxToDds(path, resultPath, (D3DTXVersion)d3dtxConversionMode);
//                     }
//                     else if (originalFormat == "dds")
//                     {
//                         if (formatToConvert != "d3dtx")
//                         {
//                           //  Converter_CLI.ConvertTextureFromD3DtxToDds(path, resultPath, (D3DTXVersion)d3dtxConversionMode);
//                         }
//                         else
//                         {
//                            // Converter_CLI.ConvertTextureFromDdsToOthersAsync(path, resultPath, formatToConvert, fixes_dds_to_generic);
//                         }
//                     }
//                     else
//                     {
//                         Converter_CLI.ConvertTextureFileFromOthersToDds(path, resultPath, fixes_generic_to_dds);
//                     }
//                 }
//                 ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Green);
//                 Console.WriteLine("Conversion Finished.");
//                 Console.ResetColor();
//             }
//             catch (Exception e)
//             {
//                 ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Red);
//                 Console.WriteLine("ERROR");
//                 Console.WriteLine(e.Message); //notify the user we found x amount of bmp files in the array
//                 Console.ResetColor();
//             }
//         }
//     }
// }
