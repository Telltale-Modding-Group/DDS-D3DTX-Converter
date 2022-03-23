using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using D3DTX_TextureConverter.Utilities;
using D3DTX_TextureConverter.DirectX;
using D3DTX_TextureConverter.Main;
using Newtonsoft.Json;

namespace D3DTX_TextureConverter
{
    class Program
    {
        //----------------------CONVERSION OPTIONS----------------------
        public static bool d3dtxMode = false; //true = in d3dtx to dds mode, false = dds to d3dtx mode
        //----------------------CONVERSION OPTIONS END----------------------

        /// <summary>
        /// Main application method
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            if(d3dtxMode)
            {
                //intro message
                ConsoleFunctions.SetConsoleColor(ConsoleColor.Blue, ConsoleColor.White);
                Console.WriteLine("D3DTX to DDS Texture Converter");

                //Convert d3dtx textures to dds
                App_Convert_D3DTX_Mode();
            }
            else
            {
                //intro message
                ConsoleFunctions.SetConsoleColor(ConsoleColor.Blue, ConsoleColor.White);
                Console.WriteLine("DDS to D3DTX Texture Converter");

                //Convert dds textures to d3dtx
                App_Convert_DDS_Mode();
            }
        }

        //-----------------------------------------------D3DTX TO DDS-----------------------------------------------
        //-----------------------------------------------D3DTX TO DDS-----------------------------------------------
        //-----------------------------------------------D3DTX TO DDS-----------------------------------------------

        /// <summary>
        /// Application function for converting D3DTX to DDS
        /// </summary>
        public static void App_Convert_D3DTX_Mode()
        {
            //-----------------GET TEXTURE FOLDER PATH-----------------
            ConsoleFunctions.SetConsoleColor(ConsoleColor.DarkGray, ConsoleColor.White); 
            Console.WriteLine("Enter the folder path with the D3DTX textures.");

            //texture folder path (containing the path to the textures to be converted)
            string textureFolderPath = GetFolderPathFromUser();

            //-----------------GET RESULT FOLDER PATH-----------------
            ConsoleFunctions.SetConsoleColor(ConsoleColor.DarkGray, ConsoleColor.White); 
            Console.WriteLine("Enter the resulting path where converted DDS textures will be stored.");

            //result folder path (will contain the converted textures)
            string resultFolderPath = GetFolderPathFromUser();

            //-----------------START CONVERSION-----------------
            //notify the user we are starting
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Green); 
            Console.WriteLine("Conversion Starting...");

            //we got our paths, so lets begin
            Convert_D3DTX_Bulk(textureFolderPath, resultFolderPath);

            //once the process is finished, it will come back here and we will notify the user that we are done
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Green); 
            Console.WriteLine("Conversion Finished.");
            Console.ResetColor();
        }

        /// <summary>
        /// Begins the conversion process. Gathers the files found in the texture folder path, filters them, and converts each one.
        /// </summary>
        /// <param name="texPath"></param>
        /// <param name="resultPath"></param>
        public static void Convert_D3DTX_Bulk(string texPath, string resultPath)
        {
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Yellow); 
            Console.WriteLine("Collecting Files..."); //notify the user we are collecting files

            //gather the files from the texture folder path into an array
            List<string> textures = new List<string>(Directory.GetFiles(texPath));

            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Yellow); 
            Console.WriteLine("Filtering Textures..."); //notify the user we are filtering the array

            //filter the array so we only get .d3dtx files
            textures = IOManagement.FilterFiles(textures, D3DTX_Master.d3dtxExtension);

            //if no d3dtx files were found, abort the program from going on any further (we don't have any files to convert!)
            if (textures.Count < 1)
            {
                ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Red); 
                Console.WriteLine("No .d3dtx files were found, aborting."); //notify the user we found x amount of d3dtx files in the array
                Console.ResetColor();
                return;
            }

            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Green); 
            Console.WriteLine("Found {0} Textures.", textures.Count.ToString()); //notify the user we found x amount of d3dtx files in the array
            Console.WriteLine("Starting...");//notify the user we are starting

            Thread[] threads = new Thread[textures.Count];

            int index = 0;
            //run a loop through each of the found textures and convert each one
            foreach (string texturePath in textures)
            {
                //build the path for the resulting file
                string textureFileName = Path.GetFileName(texturePath); //get the file name of the file + extension
                string textureFileNameOnly = Path.GetFileNameWithoutExtension(texturePath);
                string textureResultPath = resultPath + "/" + textureFileNameOnly + DDS_Master.ddsExtension; //add the file name to the resulting folder path, this is where our converted file will be placed

                ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White); 
                Console.WriteLine("||||||||||||||||||||||||||||||||");
                ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Blue); 
                Console.WriteLine("Converting '{0}'...", textureFileName); //notify the user are converting 'x' file.
                Console.ResetColor();

                //var thre = new Thread(() => ConvertTexture_FromD3DTX_ToDDS(textureFileName, texture, textureResultPath));
                //thre.IsBackground = true;
                //thre.
                //thre.Start();

                //runs the main method for converting the texture
                ConvertTexture_FromD3DTX_ToDDS(texturePath, textureResultPath);

                ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Green); 
                Console.WriteLine("Finished converting '{0}'...", textureFileName); //notify the user we finished converting 'x' file.
                ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
                index++;
            }
        }

        /// <summary>
        /// The main function for reading and converting said .d3dtx into a .dds file
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <param name="destinationFile"></param>
        public static void ConvertTexture_FromD3DTX_ToDDS(string sourceFile, string destinationFile)
        {
            //string file_dword = D3DTX_File.Read_D3DTX_File_MetaVersionOnly(sourceFile);
            D3DTX_Master d3dtx_file = new D3DTX_Master();
            d3dtx_file.Read_D3DTX_File(sourceFile);

            DDS_Master dds_file = new DDS_Master(d3dtx_file);

            //write the dds file to disk
            dds_file.Write_D3DTX_AsDDS(d3dtx_file, destinationFile);

            //write the d3dtx data into a file (json for newer versions, .header for older versions)
            d3dtx_file.Write_D3DTX_JSON(destinationFile);

            //GenericImageFormats.ConvertDDS_To_PSD(destinationFile);
        }

        //-----------------------------------------------DDS TO D3DTX-----------------------------------------------
        //-----------------------------------------------DDS TO D3DTX-----------------------------------------------
        //-----------------------------------------------DDS TO D3DTX-----------------------------------------------

        /// <summary>
        /// Application function for converting DDS to D3DTX
        /// </summary>
        public static void App_Convert_DDS_Mode()
        {
            //-----------------GET TEXTURE FOLDER PATH-----------------
            ConsoleFunctions.SetConsoleColor(ConsoleColor.DarkGray, ConsoleColor.White); 
            Console.WriteLine("Enter the folder path with the textures.");
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Yellow); 
            Console.WriteLine("NOTE: Make sure each DDS is accompanied with a .header or .json file (depending on the version)");

            //texture folder path (containing the path to the textures to be converted)
            string textureFolderPath = GetFolderPathFromUser();

            //-----------------GET RESULT FOLDER PATH-----------------
            ConsoleFunctions.SetConsoleColor(ConsoleColor.DarkGray, ConsoleColor.White); 
            Console.WriteLine("Enter the resulting path where converted textures will be stored.");

            //result folder path (will contain the converted textures)
            string resultFolderPath = GetFolderPathFromUser();

            //-----------------START CONVERSION-----------------
            //notify the user we are starting
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Green); 
            Console.WriteLine("Conversion Starting...");

            //we got our paths, so lets begin
            Convert_DDS_Bulk(textureFolderPath, resultFolderPath);

            //once BeginProcess is finished, it will come back here and we will notify the user that we are done
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Green); 
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
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Yellow); 
            Console.WriteLine("Collecting Files..."); //notify the user we are collecting files

            //gather the files from the texture folder path into an array
            List<string> files = new List<string>(Directory.GetFiles(texPath));

            //where our dds file paths will be stored
            List<string> ddsFiles;

            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Yellow); 
            Console.WriteLine("Filtering Files..."); //notify the user we are filtering the array

            //filter the array so we only get .dds files
            ddsFiles = IOManagement.FilterFiles(files, DDS_Master.ddsExtension);

            //if none of the arrays have any files that were found, abort the program from going on any further (we don't have any files to convert!)
            if (ddsFiles.Count < 1)
            {
                ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Red); 
                Console.WriteLine("No .dds files were found, aborting.");
                Console.ResetColor();
                return;
            }

            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Green); 
            Console.WriteLine("Found {0} Textures.", ddsFiles.Count.ToString()); //notify the user we found x amount of dds files in the array
            Console.WriteLine("Starting...");//notify the user we are starting

            //run a loop through each of the found textures and convert each one
            foreach (string ddsFile in ddsFiles)
            {
                ConvertTexture_FromDDS_ToD3DTX(ddsFile, resultPath);
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
            string textureFileNameWithD3DTX = textureFileNameOnly + D3DTX_Master.d3dtxExtension;
            string textureFileNameWithHEADER = textureFileNameOnly + D3DTX_Master.headerExtension;
            string textureFileNameWithJSON = textureFileNameOnly + D3DTX_Master.jsonExtension;

            //create the path of these files. If things go well, these files (depending on the version) should exist in the same directory at the original .dds file.
            string textureFilePath_HEADER = textureFileDirectory + "/" + textureFileNameWithHEADER;
            string textureFilePath_JSON = textureFileDirectory + "/" + textureFileNameWithJSON;

            //create the final path of the d3dtx
            string textureResultPath_D3DTX = resultDirectoryPath + "/" + textureFileNameWithD3DTX;

            //if a json file exists (for newer 5VSM and 6VSM)
            if(File.Exists(textureFilePath_JSON))
            {
                //read in our DDS file
                DDS_Master dds = new DDS_Master(sourceFilePath, false);

                //dds parse test
                //dds.TEST_WriteDDSToDisk(sourceFilePath);

                //create our d3dtx object
                D3DTX_Master d3dtx_file = new D3DTX_Master();

                //parse the .json file as a d3dtx
                d3dtx_file.Read_D3DTX_JSON(textureFilePath_JSON);

                //modify the d3dtx file using our dds data
                d3dtx_file.Modify_D3DTX(dds); //ISSUE HERE WITH DXT5 AND MIP MAPS WITH UPSCALED TEXTURES

                //write our final d3dtx file to disk
                d3dtx_file.Write_Final_D3DTX(textureResultPath_D3DTX);

                //File.Delete(textureFilePath_JSON);
                //File.Delete(sourceFilePath);
            }
            //if there is no .json, check for a .header (for versions older than 5VSM)
            else if(File.Exists(textureFilePath_HEADER))
            {
                ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Red);
                Console.WriteLine("Converting d3dtx older than 5VSM not supported yet!!!!");
                Console.WriteLine("{0}", textureFileNameOnly);
                Console.WriteLine("Skipping conversion on this file.", textureFileNameOnly);
                ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);

                return;
            }
            //if we found neither, we're screwed!
            else
            {
                ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Red);
                Console.WriteLine("No .json or .header was found for the file were trying to convert!!!!");
                Console.WriteLine("{0}", textureFileNameOnly);
                Console.WriteLine("Skipping conversion on this file.", textureFileNameOnly);
                ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);

                return;
            }
        }

        public static string GetFolderPathFromUser()
        {
            string folderPath = "";

            //run a loop until the path is valid
            while (true)
            {
                //get path from user
                ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
                folderPath = Console.ReadLine();

                //check if the path is valid
                if (Directory.Exists(folderPath) == false)
                {
                    //notify the user and this loop will run again
                    ConsoleFunctions.SetConsoleColor(ConsoleColor.Red, ConsoleColor.White);
                    Console.WriteLine("Incorrect Texture Path, try again.");
                }
                else
                {
                    break; //if it's sucessful, then break out of the loop
                }
            }

            return folderPath;
        }
    }
}
