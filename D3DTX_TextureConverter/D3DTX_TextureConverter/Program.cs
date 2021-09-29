using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using D3DTX_TextureConverter.Utilities;
using D3DTX_TextureConverter.DirectX;

namespace D3DTX_TextureConverter
{
    class Program
    {
        //----------------------CONVERSION OPTIONS----------------------
        public static bool d3dtxMode = true; //true = in d3dtx to dds mode, false = dds to d3dtx mode
        public static bool generateHeader = true; //for D3DTX Mode, IMPORTANT if you want to convert the dds back to a d3dtx

        //attempts to change the resolution IF the NEW DDS file resolution is DIFFERENT than the original D3DTX (ONLY ON FILES WITH 0 MIP MAPS)
        //this can be prone to conversion errors and artifacts
        public static bool enableExperimentalResolutionChange = true;
        //----------------------CONVERSION OPTIONS END----------------------

        /// <summary>
        /// Main application method
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            if(d3dtxMode)
            {
                //Convert d3dtx textures to dds
                App_Convert_D3DTX_Mode();
            }
            else
            {
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
            //introduction
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Blue, ConsoleColor.White); 
            Console.WriteLine("D3DTX to DDS Texture Converter");

            //-----------------GET TEXTURE FOLDER PATH-----------------
            ConsoleFunctions.SetConsoleColor(ConsoleColor.DarkGray, ConsoleColor.White); 
            Console.WriteLine("Enter the folder path with the D3DTX textures.");

            //texture folder path (containing the path to the textures to be converted)
            string textureFolderPath = "";

            //run a loop until the path is valid
            while (true)
            {
                //get path from user
                ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White); 
                textureFolderPath = Console.ReadLine();

                //check if the path is valid
                if (Directory.Exists(textureFolderPath) == false)
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

            //-----------------GET RESULT FOLDER PATH-----------------
            ConsoleFunctions.SetConsoleColor(ConsoleColor.DarkGray, ConsoleColor.White); 
            Console.WriteLine("Enter the resulting path where converted DDS textures will be stored.");

            //result folder path (will contain the converted textures)
            string resultFolderPath = "";

            //run a loop until the path is valid
            while (true)
            {
                //get path from user
                ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White); 
                resultFolderPath = Console.ReadLine();

                //check if the path is valid
                if (Directory.Exists(resultFolderPath) == false)
                {
                    //notify the user and this loop will run again
                    ConsoleFunctions.SetConsoleColor(ConsoleColor.Red, ConsoleColor.White); 
                    Console.WriteLine("Incorrect Result Path, try again.");
                }
                else
                {
                    break; //if it's sucessful, then break out of the loop
                }
            }

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
            textures = IOManagement.FilterFiles(textures, D3DTX_File.d3dtxExtension);

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

            //run a loop through each of the found textures and convert each one
            foreach (string texture in textures)
            {
                //build the path for the resulting file
                string textureFileName = Path.GetFileName(texture); //get the file name of the file + extension
                string textureFileNameOnly = Path.GetFileNameWithoutExtension(texture);
                string textureResultPath = resultPath + "/" + textureFileNameOnly + DDS_File.ddsExtension; //add the file name to the resulting folder path, this is where our converted file will be placed

                ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White); 
                Console.WriteLine("||||||||||||||||||||||||||||||||");
                ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Blue); 
                Console.WriteLine("Converting '{0}'...", textureFileName); //notify the user are converting 'x' file.
                Console.ResetColor();

                //runs the main method for converting the texture
                ConvertTexture_FromD3DTX_ToDDS(textureFileName, texture, textureResultPath);

                ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Green); 
                Console.WriteLine("Finished converting '{0}'...", textureFileName); //notify the user we finished converting 'x' file.
                ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White); 
            }
        }

        /// <summary>
        /// The main function for reading and converting said .d3dtx into a .dds file
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <param name="destinationFile"></param>
        public static void ConvertTexture_FromD3DTX_ToDDS(string sourceFileName, string sourceFile, string destinationFile)
        {
            string file_dword = D3DTX_File.Read_D3DTX_File_MetaVersionOnly(sourceFile);
            D3DTX_File d3dtx_file = new D3DTX_File();

            if (file_dword.Equals("6VSM"))
            {
                //read D3DTX file
                d3dtx_file.D3DTX_6VSM = new D3DTX_6VSM(sourceFile, false);
            }
            else if (file_dword.Equals("5VSM"))
            {
                //read D3DTX file
                d3dtx_file.D3DTX_5VSM = new D3DTX_5VSM(sourceFile, false);
            }
            else if (file_dword.Equals("ERTM"))
            {
                //read D3DTX file
                d3dtx_file.D3DTX_ERTM = new D3DTX_ERTM(sourceFile, false);

                return;
            }

            //create a DDS file in memory based off the d3dtx
            DDS_File ddsFile = new DDS_File(d3dtx_file);

            //write the dds file to disk
            //ddsFile.Write_D3DTX_AsDDS(d3dtx_file, destinationFile);

            //write the d3dtx header to disk
            //d3dtx_file.Write_D3DTX_Header(destinationFile);

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
            //introduction
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Blue, ConsoleColor.White); 
            Console.WriteLine("DDS to D3DTX Texture Converter");

            //-----------------GET TEXTURE FOLDER PATH-----------------
            ConsoleFunctions.SetConsoleColor(ConsoleColor.DarkGray, ConsoleColor.White); 
            Console.WriteLine("Enter the folder path with the textures.");
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Yellow); 
            Console.WriteLine("NOTE: Make sure each DDS is accompanied with a .header file");

            //texture folder path (containing the path to the textures to be converted)
            string textureFolderPath = "";

            //run a loop until the path is valid
            while (true)
            {
                //get path from user
                ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White); 
                textureFolderPath = Console.ReadLine();

                //check if the path is valid
                if (Directory.Exists(textureFolderPath) == false)
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

            //-----------------GET RESULT FOLDER PATH-----------------
            ConsoleFunctions.SetConsoleColor(ConsoleColor.DarkGray, ConsoleColor.White); 
            Console.WriteLine("Enter the resulting path where converted textures will be stored.");

            //result folder path (will contain the converted textures)
            string resultFolderPath = "";

            //run a loop until the path is valid
            while (true)
            {
                //get path from user
                ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White); 
                resultFolderPath = Console.ReadLine();

                //check if the path is valid
                if (Directory.Exists(resultFolderPath) == false)
                {
                    //notify the user and this loop will run again
                    ConsoleFunctions.SetConsoleColor(ConsoleColor.Red, ConsoleColor.White); 
                    Console.WriteLine("Incorrect Result Path, try again.");
                }
                else
                {
                    break; //if it's sucessful, then break out of the loop
                }
            }

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

            //where our header file paths will be stored
            List<string> headerFiles;

            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Yellow); 
            Console.WriteLine("Filtering Files..."); //notify the user we are filtering the array

            //filter the array so we only get .dds files
            ddsFiles = IOManagement.FilterFiles(files, DDS_File.ddsExtension);

            //filter the array so we only get .header files
            headerFiles = IOManagement.FilterFiles(files, D3DTX_File.headerExtension);

            //if none of the arrays have any files that were found, abort the program from going on any further (we don't have any files to convert!)
            if (ddsFiles.Count < 1)
            {
                ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Red); 
                Console.WriteLine("No .d3dtx files were found, aborting.");
                Console.ResetColor();
                return;
            }
            else if (headerFiles.Count < 1)
            {
                ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Red); 
                Console.WriteLine("No .header files were found.");
                Console.WriteLine(".header are required and must be generated when converting a .d3dtx to a .dds");
                Console.WriteLine("aborting...");
                Console.ResetColor();
                return;
            }

            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Green); 
            Console.WriteLine("Found {0} Textures.", ddsFiles.Count.ToString()); //notify the user we found x amount of dds files in the array
            Console.WriteLine("Found {0} Headers.", headerFiles.Count.ToString()); //notify the user we found x amount of header files in the array
            Console.WriteLine("Starting...");//notify the user we are starting

            //run a loop through each of the found textures and convert each one
            foreach (string ddsFile in ddsFiles)
            {
                //build the path for the resulting d3dtx file
                string textureFileName = Path.GetFileName(ddsFile); //get the file name of the file + extension
                string textureFileNameOnly = Path.GetFileNameWithoutExtension(ddsFile);
                string textureFileNameWithD3DTX = textureFileNameOnly + D3DTX_File.d3dtxExtension;
                string textureResultPath = resultPath + "/" + textureFileNameOnly + D3DTX_File.d3dtxExtension; //add the file name to the resulting folder path, this is where our converted file will be placed

                //texture header file path, this is assigned when we find the matching header file with the texture
                string textureHeaderFile = "";

                //before we start converting, lets check to see we can find the matching header file
                foreach(string headerFile in headerFiles)
                {
                    //get the name for the header file (should match the d3dtx texture we are currently on)
                    string headerFileNameOnly = Path.GetFileNameWithoutExtension(headerFile); //get the file name with no extension

                    //if the names of both files match (which they should) then we found our header file
                    if (headerFileNameOnly.Equals(textureFileNameOnly))
                    {
                        //assign the path of our header file
                        textureHeaderFile = headerFile;

                        //break out of this loop since we don't need to iterate any more
                        break;
                    }
                }

                if (!string.IsNullOrEmpty(textureHeaderFile) || !string.IsNullOrWhiteSpace(textureHeaderFile))
                {
                    //notify the user are converting 'x' file.
                    ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White); 
                    Console.WriteLine("||||||||||||||||||||||||||||||||");
                    ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Blue); 
                    Console.WriteLine("Merging '{0}'...", textureFileName); //notify the user are converting 'x' file.
                    Console.WriteLine("Merging '{0}'...", Path.GetFileName(textureHeaderFile)); //notify the user are converting 'x' file.
                    Console.ResetColor();

                    //runs the main method for merging both files into a single .d3dtx
                    ConvertTexture_FromDDS_ToD3DTX(textureFileName, ddsFile, textureHeaderFile, textureResultPath, textureFileNameWithD3DTX);

                    ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Green); 
                    Console.WriteLine("Finished merging '{0}'...", textureFileNameOnly); //notify the user we finished converting 'x' file.
                    ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White); 
                }
                else
                {
                    //notify the user that we can't convert this file, so we have to skip
                    ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Red); 
                    Console.WriteLine("Can't find the matching header file for '{0}'! Skipping this one.", textureFileName);
                }
            }
        }

        /// <summary>
        /// The main function for reading and converting said .d3dtx into a .dds file
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <param name="destinationFile"></param>
        public static void ConvertTexture_FromDDS_ToD3DTX(string sourceFileName, string sourceTexFile, string sourceHeaderFile, string destinationFile, string fileNameWithD3DTX)
        {
            /*
             * NOTE TO SELF
             * DDS --> D3DTX EXTRACTION, THE BYTES ARE NOT FULLY 1:1 WHEN THERE IS A CONVERSION (off by 8 bytes)
             * MABYE TRY TO CHANGE THE TEXTURE DATA BYTE SIZE IN THE D3DTX HEADER AND SEE IF THAT CHANGES ANYTHING?
            */

            D3DTX_File d3dtx_file = new D3DTX_File();

            string file_dword = D3DTX_File.Read_D3DTX_File_MetaVersionOnly(sourceHeaderFile);

            if (file_dword.Equals("6VSM"))
            {
                d3dtx_file.D3DTX_6VSM = new D3DTX_6VSM(sourceHeaderFile, true);
            }
            else if (file_dword.Equals("5VSM"))
            {
                d3dtx_file.D3DTX_5VSM = new D3DTX_5VSM(sourceHeaderFile, true);
            }

            DDS_File dds_file = new DDS_File(d3dtx_file);
        }
    }
}
