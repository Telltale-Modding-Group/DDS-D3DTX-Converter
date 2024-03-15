using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using D3DTX_Converter_Avalonia_GUI.DirectX;
using D3DTX_Converter.ImageProcessing;
using D3DTX_Converter.Utilities;
using D3DTX_Converter.DirectX;
using D3DTX_Converter.Main;
using D3DTX_Converter.ImageProcessing;
using D3DTX_Converter.Texconv;
using D3DTX_Converter.TexconvOptions;
using DirectXTexNet;
using Newtonsoft.Json;

namespace D3DTX_Converter.ProgramDebug
{
    public static class Program_DDS_TO_D3DTX
    {
        public static void Execute()
        {
            //intro message
            Console.WriteLine("DDS to D3DTX Texture Converter");

            //-----------------GET TEXTURE FOLDER PATH-----------------
            Console.WriteLine("Enter the folder path with the textures.");
            Console.WriteLine("NOTE: Make sure each .dds is accompanied with a .json file of the same name");

            //texture folder path (containing the path to the textures to be converted)
            string textureFolderPath = Program_Shared.GetFolderPathFromUser();

            //-----------------GET RESULT FOLDER PATH-----------------
            Console.WriteLine("Enter the resulting path where converted textures will be stored.");

            //result folder path (will contain the converted textures)
            string resultFolderPath = Program_Shared.GetFolderPathFromUser();

            //-----------------START CONVERSION-----------------
            //notify the user we are starting
            Console.WriteLine("Conversion Starting...");

            //we got our paths, so lets begin
            Convert_DDS_Bulk(textureFolderPath, resultFolderPath);

            //once BeginProcess is finished, it will come back here and we will notify the user that we are done
            Console.WriteLine("Conversion Finished.");
            Console.ResetColor();
        }

        /// <summary>
        /// Begins the conversion process. Gathers the files found in the texture folder path, filters them, and converts each one.
        /// </summary>
        /// <param name="texPath"></param>
        /// <param name="resultPath"></param>
        public static void Convert_DDS_Bulk(string texPath, string resultPath)
        {
            Console.WriteLine("Collecting Files..."); //notify the user we are collecting files

            //gather the files from the texture folder path into an array
            List<string> files = new List<string>(Directory.GetFiles(texPath));

            //where our dds file paths will be stored
            List<string> ddsFiles;

            Console.WriteLine("Filtering Files..."); //notify the user we are filtering the array

            //filter the array so we only get .dds files
            ddsFiles = IOManagement.FilterFiles(files, Main_Shared.ddsExtension);

            //if none of the arrays have any files that were found, abort the program from going on any further (we don't have any files to convert!)
            if (ddsFiles.Count < 1)
            {
                Console.WriteLine("No .dds files were found, aborting.");
                Console.ResetColor();
                return;
            }

            Console.WriteLine("Found {0} Textures.", ddsFiles.Count.ToString()); //notify the user we found x amount of dds files in the array
            Console.WriteLine("Starting...");//notify the user we are starting

            //run a loop through each of the found textures and convert each one
            for (int i = 0; i < ddsFiles.Count; i++)
            {
                ConvertTexture_FromDDS_ToD3DTX(ddsFiles[i], resultPath);
            }
        }

        /// <summary>
        /// The main function for reading and converting said .dds back into a .d3dtx file
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <param name="destinationFile"></param>
        public static void ConvertTexture_FromDDS_ToD3DTX(string sourceFilePath, string resultDirectoryPath)
        {
            //deconstruct the source file path
            string textureFileDirectory = Path.GetDirectoryName(sourceFilePath);
            string textureFileNameOnly = Path.GetFileNameWithoutExtension(sourceFilePath);

            //create the names of the following files
            string textureFileNameWithD3DTX = textureFileNameOnly + Main_Shared.d3dtxExtension;
            string textureFileNameWithJSON = textureFileNameOnly + Main_Shared.jsonExtension;

            //create the path of these files. If things go well, these files (depending on the version) should exist in the same directory at the original .dds file.
            string textureFilePath_JSON = textureFileDirectory + "/" + textureFileNameWithJSON;

            //create the final path of the d3dtx
            string textureResultPath_D3DTX = resultDirectoryPath + "/" + textureFileNameWithD3DTX;

            //if a json file exists
            if (File.Exists(textureFilePath_JSON))
            {
                //create a new d3dtx object
                D3DTX_Master d3dtx_file = new();

                //parse the .json file as a d3dtx
                d3dtx_file.Read_D3DTX_JSON(textureFilePath_JSON);

                //read in our DDS file
                DDS_Master dds = new(sourceFilePath, false);

                DDS_DirectXTexNet_ImageSection[] sections = DDS_DirectXTexNet.GetDDSImageSections(sourceFilePath);

                //dds parse test
                //dds.TEST_WriteDDSToDisk(textureResultPath_DDS); //<-------- THIS IS CORRECT AND PARSES A DDS FILE PERFECTLY

                //modify the d3dtx file using our dds data
                d3dtx_file.Modify_D3DTX(dds, sections); //ISSUE HERE WITH DXT5 AND MIP MAPS WITH UPSCALED TEXTURES

                //write our final d3dtx file to disk
                d3dtx_file.Write_Final_D3DTX(textureResultPath_D3DTX);
            }
            //if we didn't find a json file, we're screwed!
            else
            {
                Console.WriteLine("No .json was found for the file were trying to convert!!!!");
                Console.WriteLine("{0}", textureFileNameOnly);
                Console.WriteLine("Skipping conversion on this file.", textureFileNameOnly);

                return;
            }
        }
    }
}
