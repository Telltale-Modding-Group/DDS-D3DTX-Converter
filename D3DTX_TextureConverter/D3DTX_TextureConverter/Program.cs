using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace D3DTX_TextureConverter
{
    class Program
    {
        //----------------------CONVERSION OPTIONS----------------------
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
            //remove '//' before App_Convert_D3DTX_Mode to convert d3dtx textures to dds
            //App_Convert_D3DTX_Mode();

            //remove '//' before App_Convert_DDS_Mode to convert dds textures to d3dtx
            App_Convert_DDS_Mode();
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
            Utilities.SetConsoleColor(ConsoleColor.Blue, ConsoleColor.White); //just a display thing, not needed
            Console.WriteLine("D3DTX to DDS Texture Converter");

            //-----------------GET TEXTURE FOLDER PATH-----------------
            Utilities.SetConsoleColor(ConsoleColor.DarkGray, ConsoleColor.White); //just a display thing, not needed
            Console.WriteLine("Enter the folder path with the D3DTX textures.");

            //texture folder path (containing the path to the textures to be converted)
            string textureFolderPath = "";

            //run a loop until the path is valid
            bool inTexturePathLoop = true;
            while (inTexturePathLoop)
            {
                //get path from user
                Utilities.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White); //just a display thing, not needed
                textureFolderPath = Console.ReadLine();

                //check if the path is valid
                if (Directory.Exists(textureFolderPath) == false)
                {
                    //notify the user and this loop will run again
                    Utilities.SetConsoleColor(ConsoleColor.Red, ConsoleColor.White); //just a display thing, not needed
                    Console.WriteLine("Incorrect Texture Path, try again.");
                }
                else
                {
                    //if it's sucessful, then break out of the loop
                    inTexturePathLoop = false;
                }
            }

            //-----------------GET RESULT FOLDER PATH-----------------
            Utilities.SetConsoleColor(ConsoleColor.DarkGray, ConsoleColor.White); //just a display thing, not needed
            Console.WriteLine("Enter the resulting path where converted DDS textures will be stored.");

            //result folder path (will contain the converted textures)
            string resultFolderPath = "";

            //run a loop until the path is valid
            bool inResultPathLoop = true;
            while (inResultPathLoop)
            {
                //get path from user
                Utilities.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White); //just a display thing, not needed
                resultFolderPath = Console.ReadLine();

                //check if the path is valid
                if (Directory.Exists(resultFolderPath) == false)
                {
                    //notify the user and this loop will run again
                    Utilities.SetConsoleColor(ConsoleColor.Red, ConsoleColor.White); //just a display thing, not needed
                    Console.WriteLine("Incorrect Result Path, try again.");
                }
                else
                {
                    //if it's sucessful, then break out of the loop
                    inResultPathLoop = false;
                }
            }

            //-----------------START CONVERSION-----------------

            //notify the user we are starting
            Utilities.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Green); //just a display thing, not needed
            Console.WriteLine("Conversion Starting...");

            //we got our paths, so lets begin
            Convert_D3DTX_Bulk(textureFolderPath, resultFolderPath);

            //once the process is finished, it will come back here and we will notify the user that we are done

            Utilities.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Green); //just a display thing, not needed
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
            Utilities.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Yellow); //just a display thing, not needed
            Console.WriteLine("Collecting Files..."); //notify the user we are collecting files

            //gather the files from the texture folder path into an array
            List<string> textures = new List<string>(Directory.GetFiles(texPath));

            Utilities.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Yellow); //just a display thing, not needed
            Console.WriteLine("Filtering Textures..."); //notify the user we are filtering the array

            //filter the array so we only get .d3dtx files
            textures = Utilities.FilterFiles(textures, D3DTX_File.d3dtxExtension);

            //if no d3dtx files were found, abort the program from going on any further (we don't have any files to convert!)
            if (textures.Count < 1)
            {
                Utilities.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Red); //just a display thing, not needed
                Console.WriteLine("No .d3dtx files were found, aborting."); //notify the user we found x amount of d3dtx files in the array
                Console.ResetColor();
                return;
            }

            Utilities.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Green); //just a display thing, not needed
            Console.WriteLine("Found {0} Textures.", textures.Count.ToString()); //notify the user we found x amount of d3dtx files in the array
            Console.WriteLine("Starting...");//notify the user we are starting

            //run a loop through each of the found textures and convert each one
            foreach (string texture in textures)
            {
                //build the path for the resulting file
                string textureFileName = Path.GetFileName(texture); //get the file name of the file + extension
                string textureFileNameOnly = Path.GetFileNameWithoutExtension(texture);
                string textureResultPath = resultPath + "/" + textureFileNameOnly + DDS_File.ddsExtension; //add the file name to the resulting folder path, this is where our converted file will be placed

                Utilities.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White); //just a display thing, not needed
                Console.WriteLine("||||||||||||||||||||||||||||||||");
                Utilities.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Blue); //just a display thing, not needed
                Console.WriteLine("Converting '{0}'...", textureFileName); //notify the user are converting 'x' file.
                Console.ResetColor();

                //runs the main method for converting the texture
                ConvertTexture_FromD3DTX_ToDDS(textureFileName, texture, textureResultPath);

                Utilities.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Green); //just a display thing, not needed
                Console.WriteLine("Finished converting '{0}'...", textureFileName); //notify the user we finished converting 'x' file.
                Utilities.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White); //just a display thing, not needed
            }
        }

        /// <summary>
        /// The main function for reading and converting said .d3dtx into a .dds file
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <param name="destinationFile"></param>
        public static void ConvertTexture_FromD3DTX_ToDDS(string sourceFileName, string sourceFile, string destinationFile)
        {
            string file_dword = D3DTX_File.Read_D3DTX_File_DWORD_Only(sourceFile);
            D3DTX_File d3dtx_file = new D3DTX_File();

            if (file_dword.Equals("6VSM"))
            {
                d3dtx_file.Read_D3DTX_File_6VSM(sourceFileName, sourceFile, false);
            }
            else if(file_dword.Equals("5VSM"))
            {
                d3dtx_file.Read_D3DTX_File_5VSM(sourceFileName, sourceFile, false);
            }
            else if(file_dword.Equals("ERTM"))
            {
                byte[] ddsFileData = null;

                d3dtx_file.Read_D3DTX_File_ERTM(sourceFileName, sourceFile, false, out ddsFileData);

                //generates a .header file that will accompany the .dds on conversion, this .header file will contain the original .d3dtx header for converting the .dds back later
                if (generateHeader)
                {
                    //write the header data to a file
                    d3dtx_file.Write_D3DTX_Header(destinationFile);
                }

                if (ddsFileData != null)
                {
                    //write the data to the file because it's literally a DDS texture (nothing needs to change!)
                    File.WriteAllBytes(destinationFile, ddsFileData);

                    //we are done!
                    return;
                }
                else
                {
                    return;
                }
            }

            //generates a .header file that will accompany the .dds on conversion, this .header file will contain the original .d3dtx header for converting the .dds back later
            if (generateHeader)
            {
                //write the header data to a file
                d3dtx_file.Write_D3DTX_Header(destinationFile);
            }

            //get our dds file object ready and assign our parsed values from the d3dtx to new dds file
            DDS_File dds_file = new DDS_File();
            dds_file.dwWidth = (uint)d3dtx_file.mWidth;
            dds_file.dwHeight = (uint)d3dtx_file.mHeight;
            //dds_file.dwFlags = (uint)parsed_dwFlags;
            dds_file.dwMipMapCount = (uint)d3dtx_file.mNumMipLevels;
            dds_file.ddspf_dwFourCC = d3dtx_file.mSurfaceFormat_converted;

            //build the header and store it in a byte array
            byte[] ddsHeader = dds_file.Build_DDSHeader_ByteArray();

            //--------------------------EXTRACTING TEXTURE DATA FROM D3DTX--------------------------
            //estimate how many total bytes are in the largest texture mip level (main one)
            int mainTextureByteSize_Estimation = Utilities.CalculateDDS_ByteSize(d3dtx_file.mWidth, d3dtx_file.mHeight, d3dtx_file.mSurfaceFormat_dxtTypeBoolSize);

            //write the result to the console for viewing
            Utilities.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White); //just a display thing, not needed
            Console.WriteLine("Estimated Largest Mip Level Byte Size = {0}", mainTextureByteSize_Estimation.ToString());

            //initalize our start offset, this is used to offset the array copy
            int startOffset;

            //allocate our byte array to contain our texture data
            byte[] textureData;

            //if our estimation is not accurate, then just extract the whole fuckin thing
            if (mainTextureByteSize_Estimation > d3dtx_file.mDataSize)
            {
                //offset the byte pointer position just past the header
                startOffset = d3dtx_file.headerLength;

                Utilities.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Yellow); //just a display thing, not needed
                Console.WriteLine("Estimation was off, extracting whole thing.");

                //allocate byte array with the parsed length of the total texture byte data from the header
                textureData = new byte[d3dtx_file.mDataSize];
            }
            else
            {
                //if our estimation is accurate, then just extract the last mip map
                //note to self, this will change later.
                //as a test for self, create multiple child dds files that each have the mip map data extracted into each single one

                //offset the byte pointer position just past the header
                startOffset = d3dtx_file.sourceByteFile.Length - mainTextureByteSize_Estimation;

                if (mainTextureByteSize_Estimation < 0)
                {
                    Utilities.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Red); //just a display thing, not needed
                    Console.WriteLine("ERROR, ESTIMATION WAS WRONG '{0}'", mainTextureByteSize_Estimation);

                    return;
                }

                Utilities.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Green); //just a display thing, not needed
                Console.WriteLine("Estimation is accurate, '{0}'", mainTextureByteSize_Estimation);

                //calculate main texture level
                textureData = new byte[mainTextureByteSize_Estimation];
            }

            //copy all the bytes from the source byte file after the header length, and copy that data to the texture data byte array
            Array.Copy(d3dtx_file.sourceByteFile, startOffset, textureData, 0, textureData.Length);

            byte[] finalDDS_textureData = Utilities.Combine(ddsHeader, textureData);

            //if there are no mip maps, build the texture file because we are done
            if (d3dtx_file.mNumMipLevels <= 1)
            {
                //write the data to the file, combine the generted DDS header and our new texture byte data
                File.WriteAllBytes(destinationFile, finalDDS_textureData);

                //we are done!
                return;
            }

            //--------------------------MIP MAP EXTRACTION AND BUILDING--------------------------
            Utilities.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Yellow); //just a display thing, not needed
            Console.WriteLine("Calculating Mip Tables..."); //notify the user we are calculating the mip table

            //offset for getting mip maps, we are working backwards since d3dtx has their mip maps stored backwards
            int leftoverOffset = d3dtx_file.sourceByteFile.Length - mainTextureByteSize_Estimation;

            //get image mip dimensions (will be modified when the loop is iterated)
            int mipImageWidth = d3dtx_file.mWidth;
            int mipImageHeight = d3dtx_file.mHeight;

            //not required, just for viewing
            int totalMipByteSize = 0;

            //run a loop for the amount of mip maps
            for (int i = 1; i < d3dtx_file.mNumMipLevels; i++)
            {
                //write the result to the console for viewing
                Utilities.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White); //just a display thing, not needed
                Console.WriteLine("Mip Level = {0}", i.ToString());

                //divide the dimensions by 2 when stepping down on each mip level
                mipImageWidth /= 2;
                mipImageHeight /= 2;

                //write the result to the console for viewing
                Utilities.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White); //just a display thing, not needed
                Console.WriteLine("Mip Resolution = {0}x{1}", mipImageWidth.ToString(), mipImageHeight.ToString());

                //estimate how many total bytes are in the largest texture mip level (main one)
                int byteSize_estimation = Utilities.CalculateDDS_ByteSize(mipImageWidth, mipImageHeight, d3dtx_file.mSurfaceFormat_dxtTypeBoolSize);
                //offset our variable so we can get to the next mip (we are working backwards from the end of the file)
                leftoverOffset -= byteSize_estimation;

                //not required, just for viewing
                totalMipByteSize += byteSize_estimation;

                //write the result to the console for viewing
                Utilities.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White); //just a display thing, not needed
                Console.WriteLine("Mip Level Byte Size = {0}", byteSize_estimation.ToString());

                if (byteSize_estimation < 0)
                {
                    Utilities.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Red); //just a display thing, not needed
                    Console.WriteLine("Mip Level Byte Size is wrong! {0}", byteSize_estimation);

                    break;
                }

                //allocate a byte array with the estimated byte size
                byte[] mipTexData = new byte[byteSize_estimation];

                //check to see if we are not over the header length (we are working backwards)
                if (leftoverOffset > d3dtx_file.headerLength)
                {
                    //copy all the bytes from the source byte file after the leftoverOffset, and copy that data to the texture data byte array
                    Array.Copy(d3dtx_file.sourceByteFile, leftoverOffset, mipTexData, 0, mipTexData.Length);

                    //combine the new mip byte data to the existing texture data byte array
                    finalDDS_textureData = Utilities.Combine(finalDDS_textureData, mipTexData);
                }
            }

            //write the result to the console for viewing
            Utilities.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White); //just a display thing, not needed
            Console.WriteLine("Total Mips Byte Size = {0}", totalMipByteSize.ToString());

            //not required, but just viewing to see if our estimated sizes match up with the parsed texture byte size
            int totalTexByteSize = totalMipByteSize + mainTextureByteSize_Estimation;

            //write the result to the console for viewing
            Utilities.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White); //just a display thing, not needed
            Console.WriteLine("Total Byte Size = {0}", totalTexByteSize.ToString());

            //write the data to the file, combine the generted DDS header and our new texture byte data
            File.WriteAllBytes(destinationFile, finalDDS_textureData);
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
            Utilities.SetConsoleColor(ConsoleColor.Blue, ConsoleColor.White); //just a display thing, not needed
            Console.WriteLine("DDS to D3DTX Texture Converter");

            //-----------------GET TEXTURE FOLDER PATH-----------------
            Utilities.SetConsoleColor(ConsoleColor.DarkGray, ConsoleColor.White); //just a display thing, not needed
            Console.WriteLine("Enter the folder path with the textures.");
            Utilities.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Yellow); //just a display thing, not needed
            Console.WriteLine("NOTE: Make sure each DDS is accompanied with a .header file");

            //texture folder path (containing the path to the textures to be converted)
            string textureFolderPath = "";

            //run a loop until the path is valid
            bool inTexturePathLoop = true;
            while (inTexturePathLoop)
            {
                //get path from user
                Utilities.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White); //just a display thing, not needed
                textureFolderPath = Console.ReadLine();

                //check if the path is valid
                if (Directory.Exists(textureFolderPath) == false)
                {
                    //notify the user and this loop will run again
                    Utilities.SetConsoleColor(ConsoleColor.Red, ConsoleColor.White); //just a display thing, not needed
                    Console.WriteLine("Incorrect Texture Path, try again.");
                }
                else
                {
                    //if it's sucessful, then break out of the loop
                    inTexturePathLoop = false;
                }
            }

            //-----------------GET RESULT FOLDER PATH-----------------
            Utilities.SetConsoleColor(ConsoleColor.DarkGray, ConsoleColor.White); //just a display thing, not needed
            Console.WriteLine("Enter the resulting path where converted textures will be stored.");

            //result folder path (will contain the converted textures)
            string resultFolderPath = "";

            //run a loop until the path is valid
            bool inResultPathLoop = true;
            while (inResultPathLoop)
            {
                //get path from user
                Utilities.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White); //just a display thing, not needed
                resultFolderPath = Console.ReadLine();

                //check if the path is valid
                if (Directory.Exists(resultFolderPath) == false)
                {
                    //notify the user and this loop will run again
                    Utilities.SetConsoleColor(ConsoleColor.Red, ConsoleColor.White); //just a display thing, not needed
                    Console.WriteLine("Incorrect Result Path, try again.");
                }
                else
                {
                    //if it's sucessful, then break out of the loop
                    inResultPathLoop = false;
                }
            }

            //-----------------START CONVERSION-----------------

            //notify the user we are starting
            Utilities.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Green); //just a display thing, not needed
            Console.WriteLine("Conversion Starting...");

            //we got our paths, so lets begin
            Convert_DDS_Bulk(textureFolderPath, resultFolderPath);

            //once BeginProcess is finished, it will come back here and we will notify the user that we are done
            Utilities.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Green); //just a display thing, not needed
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
            Utilities.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Yellow); //just a display thing, not needed
            Console.WriteLine("Collecting Files..."); //notify the user we are collecting files

            //gather the files from the texture folder path into an array
            List<string> files = new List<string>(Directory.GetFiles(texPath));

            //where our dds file paths will be stored
            List<string> ddsFiles;

            //where our header file paths will be stored
            List<string> headerFiles;

            Utilities.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Yellow); //just a display thing, not needed
            Console.WriteLine("Filtering Files..."); //notify the user we are filtering the array

            //filter the array so we only get .dds files
            ddsFiles = Utilities.FilterFiles(files, DDS_File.ddsExtension);

            //filter the array so we only get .header files
            headerFiles = Utilities.FilterFiles(files, D3DTX_File.headerExtension);

            //if none of the arrays have any files that were found, abort the program from going on any further (we don't have any files to convert!)
            if (ddsFiles.Count < 1)
            {
                Utilities.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Red); //just a display thing, not needed
                Console.WriteLine("No .d3dtx files were found, aborting.");
                Console.ResetColor();
                return;
            }
            else if (headerFiles.Count < 1)
            {
                Utilities.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Red); //just a display thing, not needed
                Console.WriteLine("No .header files were found.");
                Console.WriteLine(".header are required and must be generated when converting a .d3dtx to a .dds");
                Console.WriteLine("aborting...");
                Console.ResetColor();
                return;
            }

            Utilities.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Green); //just a display thing, not needed
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
                    Utilities.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White); //just a display thing, not needed
                    Console.WriteLine("||||||||||||||||||||||||||||||||");
                    Utilities.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Blue); //just a display thing, not needed
                    Console.WriteLine("Merging '{0}'...", textureFileName); //notify the user are converting 'x' file.
                    Console.WriteLine("Merging '{0}'...", Path.GetFileName(textureHeaderFile)); //notify the user are converting 'x' file.
                    Console.ResetColor();

                    //runs the main method for merging both files into a single .d3dtx
                    ConvertTexture_FromDDS_ToD3DTX(textureFileName, ddsFile, textureHeaderFile, textureResultPath, textureFileNameWithD3DTX);

                    Utilities.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Green); //just a display thing, not needed
                    Console.WriteLine("Finished merging '{0}'...", textureFileNameOnly); //notify the user we finished converting 'x' file.
                    Utilities.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White); //just a display thing, not needed
                }
                else
                {
                    //notify the user that we can't convert this file, so we have to skip
                    Utilities.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Red); //just a display thing, not needed
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
            DDS_File dds_file = new DDS_File();

            string file_dword = D3DTX_File.Read_D3DTX_File_DWORD_Only(sourceHeaderFile);

            if (file_dword.Equals("6VSM"))
            {
                d3dtx_file.Read_D3DTX_File_6VSM(sourceFileName, sourceHeaderFile, true);
            }
            else if (file_dword.Equals("5VSM"))
            {
                d3dtx_file.Read_D3DTX_File_5VSM(sourceFileName, sourceHeaderFile, true);
            }
            else if (file_dword.Equals("ERTM"))
            {
                byte[] ddsFileData = null; //because we are reading just a .header version of the .d3dtx, this won't have the data

                d3dtx_file.Read_D3DTX_File_ERTM(sourceFileName, sourceHeaderFile, true, out ddsFileData);
                dds_file.Read_DDS_File(sourceTexFile, sourceFileName, false);

                //apply the dds property data to the d3dtx header
                d3dtx_file.Apply_DDS_Data_To_D3DTX_Data(dds_file, true);

                //write the data to the file, combine the generted DDS header and the dds file data
                File.WriteAllBytes(destinationFile, Utilities.Combine(d3dtx_file.headerData, dds_file.sourceFileData));

                return;
            }




            dds_file.Read_DDS_File(sourceTexFile, sourceFileName, false);

            //apply the dds property data to the d3dtx header
            d3dtx_file.Apply_DDS_Data_To_D3DTX_Data(dds_file, true);

            //--------------------------COMBINE DDS TEXTURE DATA WITH D3DTX HEADER--------------------------
            //if there are no mip maps, go ahead and just build the texture
            if (dds_file.dwMipMapCount <= 1)
            {
                //write the data to the file, combine the generted DDS header and our new texture byte data
                File.WriteAllBytes(destinationFile, Utilities.Combine(d3dtx_file.headerData, dds_file.ddsTextureData));

                //stop the function as there is no need to continue any further
                return;
            }

            //we will work through the texture data backwards, since the d3dtx format has mip map ordered reversed, so we will add it in that way
            //offset for getting mip maps, we are working backwards since d3dtx has their mip maps stored backwards
            int leftoverOffset = dds_file.ddsTextureData.Length;

            //allocate a byte array to contain our texture data (ordered backwards)
            byte[] final_d3dtxData = new byte[0];

            //add the d3dtx header
            //note to self - modify the header before adding it
            final_d3dtxData = Utilities.Combine(d3dtx_file.headerData, final_d3dtxData);

            //quick fix for textures not being read properly (they are not the exact same size)
            //note to self - try to modify the d3dtx header so the texture byte size in the header matches the texture byte size we are inputing
            //byte[] fillterData = new byte[8];
            //final_d3dtxData = Combine(final_d3dtxData, fillterData);

            //start at 1 since the mip map count in dds files tend to start at 0 instead of 1 
            //because I suck at math, we will generate our mip map resolutions using the same method we did in d3dtx to dds (can't figure out how to calculate them in reverse properly)
            int[,] mipImageResolutions = new int[(int)dds_file.dwMipMapCount, 2];

            //get our mip image dimensions (have to multiply by 2 as the mip calculations will be off by half)
            int mipImageWidth = (int)dds_file.dwWidth * 2;
            int mipImageHeight = (int)dds_file.dwHeight * 2;

            //add the resolutions in reverse
            for (int i = (int)dds_file.dwMipMapCount - 1; i > 0; i--)
            {
                //divide the resolutions by 2
                mipImageWidth /= 2;
                mipImageHeight /= 2;

                //assign the resolutions
                mipImageResolutions[i, 0] = mipImageWidth;
                mipImageResolutions[i, 1] = mipImageHeight;
            }

            //not required, just for viewing
            int totalMipByteSize = 0;

            //run a loop for the amount of mip maps
            for (int i = 1; i < (int)dds_file.dwMipMapCount; i++)
            {
                //not required, just for viewing
                int mipLevel = (int)(dds_file.dwMipMapCount) - i;

                //get our mip resolution from the resolution array (the values are reversed, smallest to big)
                int width = mipImageResolutions[i, 0];
                int height = mipImageResolutions[i, 1];

                //estimate how many total bytes are in the largest texture mip level (main one)
                int byteSize_estimation = Utilities.CalculateDDS_ByteSize(width, height, dds_file.DDS_CompressionBool());

                //offset our variable so we can get to the next mip (we are working backwards from the end of the file)
                leftoverOffset -= byteSize_estimation;

                //not required, just for viewing
                totalMipByteSize += byteSize_estimation;

                //allocate a byte array with the estimated byte size
                byte[] mipTexData = new byte[byteSize_estimation];

                //check to see if we are not over the length of the file (we are working backwards)
                if (leftoverOffset >= 0)
                {
                    //copy all the bytes from the texture byte array after the leftoverOffset, and copy that data to the mip map tex data byte array
                    Array.Copy(dds_file.ddsTextureData, leftoverOffset, mipTexData, 0, mipTexData.Length);

                    //combine the new mip byte data to the existing texture data byte array
                    final_d3dtxData = Utilities.Combine(final_d3dtxData, mipTexData);
                }

                //write results to the console for viewing
                Utilities.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Yellow); //just a display thing, not needed
                Console.WriteLine("Leftover Offset = {0}", leftoverOffset.ToString());
                Console.WriteLine("Mip Level = {0}", mipLevel.ToString());
                Console.WriteLine("Mip Resolution = {0}x{1}", width.ToString(), height.ToString());
                Console.WriteLine("Mip Level Byte Size = {0}", byteSize_estimation.ToString());
                Console.WriteLine("D3DTX Data Length Byte Size = {0}", final_d3dtxData.Length.ToString());
            }

            //write results to the console for viewing
            Utilities.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Yellow); //just a display thing, not needed
            Console.WriteLine("D3DTX Header Byte Size = {0}", d3dtx_file.headerData.Length.ToString());
            Console.WriteLine("D3DTX Total Estimated Byte Size = {0}", totalMipByteSize.ToString());

            //write the data to the file, combine the generted DDS header and our new texture byte data
            File.WriteAllBytes(destinationFile, final_d3dtxData);
        }
    }
}
