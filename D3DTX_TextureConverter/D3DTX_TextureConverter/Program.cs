using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace D3DTX_TextureConverter
{
    class Program
    {
        //for D3DTX Mode, IMPORTANT if you want to convert the dds back to a d3dtx
        public static bool generateHeader = true;

        //dds image file extension
        public static string ddsExtension = ".dds";

        //telltale d3dtx texture file extension
        public static string d3dtxExtension = ".d3dtx";

        //custom header file extension (generated from d3dtx to dds, used to convert dds back to d3dtx)
        public static string headerExtension = ".header";

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
            Console.WriteLine("D3DTX to DDS Texture Converter");

            //-----------------GET TEXTURE FOLDER PATH-----------------
            Console.WriteLine("Enter the folder path with the D3DTX textures.");

            //texture folder path (containing the path to the textures to be converted)
            string textureFolderPath = "";

            //run a loop until the path is valid
            bool inTexturePathLoop = true;
            while (inTexturePathLoop)
            {
                //get path from user
                textureFolderPath = Console.ReadLine();

                //check if the path is valid
                if (Directory.Exists(textureFolderPath) == false)
                {
                    //notify the user and this loop will run again
                    Console.WriteLine("Incorrect Texture Path, try again.");
                }
                else
                {
                    //if it's sucessful, then break out of the loop
                    inTexturePathLoop = false;
                }
            }

            //-----------------GET RESULT FOLDER PATH-----------------
            Console.WriteLine("Enter the resulting path where converted DDS textures will be stored.");

            //result folder path (will contain the converted textures)
            string resultFolderPath = "";

            //run a loop until the path is valid
            bool inResultPathLoop = true;
            while (inResultPathLoop)
            {
                //get path from user
                resultFolderPath = Console.ReadLine();

                //check if the path is valid
                if (Directory.Exists(resultFolderPath) == false)
                {
                    //notify the user and this loop will run again
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
            Console.WriteLine("Conversion Starting...");

            //we got our paths, so lets begin
            Convert_D3DTX_Bulk(textureFolderPath, resultFolderPath);

            //once BeginProcess is finished, it will come back here and we will notify the user that we are done
            Console.WriteLine("Conversion Finished.");
        }

        /// <summary>
        /// Begins the conversion process. Gathers the files found in the texture folder path, filters them, and converts each one.
        /// </summary>
        /// <param name="texPath"></param>
        /// <param name="resultPath"></param>
        public static void Convert_D3DTX_Bulk(string texPath, string resultPath)
        {
            Console.WriteLine("Collecting Files..."); //notify the user we are collecting files

            //gather the files from the texture folder path into an array
            List<string> textures = new List<string>(Directory.GetFiles(texPath));

            Console.WriteLine("Filtering Textures..."); //notify the user we are filtering the array

            //filter the array so we only get .d3dtx files
            textures = FilterFiles(textures, d3dtxExtension);

            //if no d3dtx files were found, abort the program from going on any further (we don't have any files to convert!)
            if (textures.Count < 1)
            {
                Console.WriteLine("No .d3dtx files were found, aborting."); //notify the user we found x amount of d3dtx files in the array
                return;
            }

            Console.WriteLine("Found {0} Textures.", textures.Count.ToString()); //notify the user we found x amount of d3dtx files in the array
            Console.WriteLine("Starting...");//notify the user we are starting

            //run a loop through each of the found textures and convert each one
            foreach (string texture in textures)
            {
                //build the path for the resulting file
                string textureFileName = Path.GetFileName(texture); //get the file name of the file + extension
                string textureFileNameOnly = Path.GetFileNameWithoutExtension(texture);
                string textureResultPath = resultPath + "/" + textureFileNameOnly + ddsExtension; //add the file name to the resulting folder path, this is where our converted file will be placed

                Console.WriteLine("Converting '{0}'...", textureFileName); //notify the user are converting 'x' file.

                //runs the main method for converting the texture
                ConvertTexture_FromD3DTX_ToDDS(textureFileName, texture, textureResultPath);

                Console.WriteLine("Finished converting '{0}'...", textureFileName); //notify the user we finished converting 'x' file.
            }
        }

        /// <summary>
        /// The main function for reading and converting said .d3dtx into a .dds file
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <param name="destinationFile"></param>
        public static void ConvertTexture_FromD3DTX_ToDDS(string sourceFileName, string sourceFile, string destinationFile)
        {
            //read the source file into a byte array
            byte[] sourceByteFile = File.ReadAllBytes(sourceFile);

            //write the result to the console for viewing
            Console.WriteLine("Total File Byte Size = {0}", sourceByteFile.Length);

            //get our file name and convert it to a byte array (since d3dtx has the filename.extension written in the file)
            byte[] fileNameBytes = Encoding.ASCII.GetBytes(sourceFileName);

            //initalize our variables, this data will be parsed and assigned
            string parsed_dword; //magic dword
            int parsed_textureDataByteSize; //total byte size of the texture data, used to calculate the header length
            int parsed_compressionType; //compression type? not 100% certain
            int parsed_imageMipMapCount; //image mip map count
            int parsed_imageMipMapCount_decremented; //image mip map count - 1 (since images with no mip maps have this value set to 1)
            int parsed_imageWidth; //main image pixel width
            int parsed_imageHeight; //main image pixel height
            int parsed_dxtType; //dxt type?
            int headerLength; //length of the telltale d3dtx header

            //which byte offset we are on (will be changed as we go through the file)
            int bytePointerPosition = 0;

            //--------------------------1 = DWORD--------------------------
            //offset byte pointer location to get the DWORD
            bytePointerPosition = 0;

            //allocate 4 byte array (string)
            byte[] source_dword = AllocateByteArray(4, sourceByteFile, bytePointerPosition);

            //parse the byte array to string
            parsed_dword = Encoding.ASCII.GetString(source_dword);

            //write the result to the console for viewing
            Console.WriteLine("DWORD = {0}", parsed_dword);
            //--------------------------2 = COMPRESISON TYPE?--------------------------
            //offset byte pointer location to get the COMPRESISON TYPE
            bytePointerPosition = 4;

            //allocate 4 byte array (int32)
            byte[] source_compressionType = AllocateByteArray(4, sourceByteFile, bytePointerPosition);

            //parse the byte array to int32
            parsed_compressionType = BitConverter.ToInt32(source_compressionType);

            //write the result to the console for viewing
            Console.WriteLine("Compression Type = {0}", parsed_compressionType.ToString());
            //--------------------------3 = TEXTURE BYTE SIZE--------------------------
            //offset byte pointer location to get the TEXTURE BYTE SIZE
            bytePointerPosition = 12;

            //allocate 4 byte array (int32)
            byte[] source_fileSize = AllocateByteArray(4, sourceByteFile, bytePointerPosition);

            //parse the byte array to int32
            parsed_textureDataByteSize = BitConverter.ToInt32(source_fileSize);

            //calculating header length, parsed texture byte size - source byte size
            headerLength = sourceByteFile.Length - parsed_textureDataByteSize;

            //write the result to the console for viewing
            Console.WriteLine("Texture Byte Size = {0}", parsed_textureDataByteSize.ToString());
            Console.WriteLine("Header Byte Size = {0}", headerLength.ToString());
            //--------------------------4 = SCREWY TELLTALE DATA--------------------------
            //NOTE TO SELF - no need to parse this byte data, we can extract the entire header later with this info included and just change what we need

            //offset byte pointer location to get the SCREWY TELLTALE DATA
            bytePointerPosition = 20;

            int telltaleScrewyHeaderLength = 84; //screwy header offset length

            byte[] screwyHeaderData = new byte[telltaleScrewyHeaderLength];

            for(int i = 0; i < telltaleScrewyHeaderLength; i++)
            {
                screwyHeaderData[i] = sourceByteFile[bytePointerPosition + i];
            }

            //move the pointer past the screwy header
            bytePointerPosition += telltaleScrewyHeaderLength;

            //--------------------------5 = TEXTURE FILE NAME--------------------------
            //offset byte pointer location to get the TEXTURE FILE NAME
            bytePointerPosition += 28;

            //the number of 'byte matches' from the source that match the bytes in the fileName
            int byteMatches = 0;

            //for each byte in the filename, check with the filename bytes in the source file to see if they match
            for(int i = 0; i < fileNameBytes.Length; i++)
            {
                //check if the byte matches, if it does then increment 'byteMatches'
                if (sourceByteFile[bytePointerPosition + i] == fileNameBytes[i])
                {
                    byteMatches++;
                }
            }

            //if the bytes don't match 100%, we can't convert because our offsets later on will be off!
            if (!(byteMatches == fileNameBytes.Length))
            {
                Console.WriteLine("ERROR, can't convert '{0}' because the filename bytes do not match. Skipping conversion on this.", sourceFileName);
                return; //return the method and don't continue any further
            }

            //move the cursor past the filename.extension byte string
            bytePointerPosition += fileNameBytes.Length;

            //--------------------------6 = MIP MAP COUNT--------------------------
            //offset byte pointer location to get the MIP MAP COUNT
            bytePointerPosition += 13;

            //allocate 4 byte array (int32)
            byte[] source_imageMipMapCount = AllocateByteArray(4, sourceByteFile, bytePointerPosition);

            //parse the byte array to int32
            parsed_imageMipMapCount = BitConverter.ToInt32(source_imageMipMapCount);
            parsed_imageMipMapCount_decremented = parsed_imageMipMapCount - 1;

            //write the result to the console for viewing
            Console.WriteLine("Mip Map Count = {0} ({1})", parsed_imageMipMapCount.ToString(), parsed_imageMipMapCount_decremented.ToString());
            //--------------------------7 = MAIN IMAGE WIDTH--------------------------
            //offset byte pointer location to get the MAIN IMAGE WIDTH
            bytePointerPosition += 4;

            //allocate 4 byte array (int32)
            byte[] source_imageWidth = AllocateByteArray(4, sourceByteFile, bytePointerPosition);

            //parse the byte array to int32
            parsed_imageWidth = BitConverter.ToInt32(source_imageWidth);

            //write the result to the console for viewing
            Console.WriteLine("Image Width = {0}", parsed_imageWidth.ToString());
            //--------------------------8 = MAIN IMAGE HEIGHT--------------------------
            //offset byte pointer location to get the MAIN IMAGE HEIGHT
            bytePointerPosition += 4;

            //allocate 4 byte array (int32)
            byte[] source_imageHeight = AllocateByteArray(4, sourceByteFile, bytePointerPosition);

            //parse the byte array to int32
            parsed_imageHeight = BitConverter.ToInt32(source_imageHeight);

            //write the result to the console for viewing
            Console.WriteLine("Image Height = {0}", parsed_imageHeight.ToString());
            //--------------------------9 = DXT TYPE?--------------------------
            //offset byte pointer location to get the DXT TYPE
            bytePointerPosition += 12;

            //allocate 4 byte array (int32)
            byte[] source_dxtType = AllocateByteArray(4, sourceByteFile, bytePointerPosition);

            //parse the byte array to int32
            parsed_dxtType = BitConverter.ToInt32(source_dxtType);

            //write the result to the console for viewing
            Console.WriteLine("DXT TYPE = {0}", parsed_dxtType.ToString());
            //--------------------------BUILDING DDS TEXTURE HEADER--------------------------
            //NOTE TO SELF 1 - BIGGEST ISSUE RIGHT NOW IS MIP MAPS, NEED TO PARSE MORE INFORMATION FROM D3DTX TO BE ABLE TO EXTRACT MIP MAPS PROPERLY
            //NOTE TO SELF 2 - largest mip map (main texture) extraction is successful, the next step is getting the lower mip levels seperately, thank you microsoft for the byte size calculation forumla
            //NOTE TO SELF 3 - all mip maps can be extracted sucessfully and implemented into the DDS file

            //get our dds file object ready
            DDS_File dds_File = new DDS_File();

            //assign our parsed values from the d3dtx to new dds file
            dds_File.dwWidth = (uint)parsed_imageWidth;
            dds_File.dwHeight = (uint)parsed_imageHeight;
            //dds_File.dwFlags = (uint)parsed_dwFlags;
            dds_File.dwMipMapCount = (uint)parsed_imageMipMapCount_decremented;


            //this section needs some reworking, still can't track down exactly what the compression types are, parsed_compressionType and parsed_dxtType are close
            //SET DDS COMPRESSION TYPES
            if (parsed_compressionType == 200 || parsed_compressionType == 210 || parsed_compressionType == 406 || parsed_compressionType == 402)
            {
                //DXT5 COMPRESSION
                dds_File.ddspf_dwFourCC = "DXT5";
            }
            else if (parsed_compressionType == 67)
            {
                //DXT3 COMPRESSION
                dds_File.ddspf_dwFourCC = "DXT3";
            }
            else if (parsed_compressionType == 388)
            {
                //dds_File.ddspf_dwFourCC = "ATI2";
            }
            else if (parsed_compressionType == 413)
            {
                //dds_File.ddspf_dwFourCC = "ATI1";
            }

            //build the header and store it in a byte array
            byte[] ddsHeader = dds_File.Build_DDSHeader_ByteArray();

            //--------------------------GENERATING D3DTX HEADER FILE--------------------------
            //generates a .header file that will accompany the .dds on conversion, this .header file will contain the original .d3dtx header for converting the .dds back later
            if (generateHeader)
            {
                //build the header destination path, assuming the extnesion of the destination file path is .dds
                string headerFilePath = string.Format("{0}{1}", destinationFile.Remove(destinationFile.Length - 4, 4), headerExtension);

                //allocate a byte array to contain the header data
                byte[] headerData = new byte[headerLength];

                //copy all the bytes from the source byte file after the header length, and copy that data to the texture data byte array
                Array.Copy(sourceByteFile, 0, headerData, 0, headerData.Length);

                //write the header data to a file
                File.WriteAllBytes(headerFilePath, headerData);
            }

            //--------------------------EXTRACTING TEXTURE DATA FROM D3DTX--------------------------
            //estimate how many total bytes are in the largest texture mip level (main one)
            int mainTextureByteSize_Estimation = CalculateDDS_ByteSize(parsed_imageWidth, parsed_imageHeight, parsed_dxtType == 64);

            //write the result to the console for viewing
            Console.WriteLine("calculated Largest Mip Level Byte Size = {0}", mainTextureByteSize_Estimation.ToString());

            //initalize our start offset, this is used to offset the array copy
            int startOffset;

            //allocate our byte array to contain our texture data
            byte[] textureData;

            //if our estimation is not accurate, then just extract the whole fuckin thing
            if (mainTextureByteSize_Estimation > parsed_textureDataByteSize)
            {
                //offset the byte pointer position just past the header
                startOffset = headerLength;

                //allocate byte array with the parsed length of the total texture byte data from the header
                textureData = new byte[parsed_textureDataByteSize];
            }
            else
            {
                //if our estimation is accurate, then just extract the last mip map
                //note to self, this will change later.
                //as a test for self, create multiple child dds files that each have the mip map data extracted into each single one

                //offset the byte pointer position just past the header
                startOffset = sourceByteFile.Length - mainTextureByteSize_Estimation;

                //calculate main texture level
                textureData = new byte[mainTextureByteSize_Estimation];
            }

            //copy all the bytes from the source byte file after the header length, and copy that data to the texture data byte array
            Array.Copy(sourceByteFile, startOffset, textureData, 0, textureData.Length);

            byte[] finalDDS_textureData = Combine(ddsHeader, textureData);

            //if there are no mip maps, build the texture file because we are done
            if (parsed_imageMipMapCount <= 1)
            {
                //write the data to the file, combine the generted DDS header and our new texture byte data
                File.WriteAllBytes(destinationFile, finalDDS_textureData);

                //we are done!
                return;
            }

            //--------------------------MIP MAP EXTRACTION AND BUILDING--------------------------
            //offset for getting mip maps, we are working backwards since d3dtx has their mip maps stored backwards
            int leftoverOffset = sourceByteFile.Length - mainTextureByteSize_Estimation;

            //get image mip dimensions (will be modified when the loop is iterated)
            int mipImageWidth = parsed_imageWidth;
            int mipImageHeight = parsed_imageHeight;

            //not required, just for viewing
            int totalMipByteSize = 0;

            //run a loop for the amount of mip maps
            for (int i = 1; i < parsed_imageMipMapCount; i++)
            {
                //write the result to the console for viewing
                Console.WriteLine("Mip Level = {0}", i.ToString());

                //divide the dimensions by 2 when stepping down on each mip level
                mipImageWidth /= 2;
                mipImageHeight /= 2;

                //write the result to the console for viewing
                Console.WriteLine("Mip Resolution = {0}x{1}", mipImageWidth.ToString(), mipImageHeight.ToString());

                //estimate how many total bytes are in the largest texture mip level (main one)
                int byteSize_estimation = CalculateDDS_ByteSize(mipImageWidth, mipImageHeight, parsed_dxtType == 64);
                //offset our variable so we can get to the next mip (we are working backwards from the end of the file)
                leftoverOffset -= byteSize_estimation;

                //not required, just for viewing
                totalMipByteSize += byteSize_estimation;

                //write the result to the console for viewing
                Console.WriteLine("Mip Level Byte Size = {0}", byteSize_estimation.ToString());

                //allocate a byte array with the estimated byte size
                byte[] mipTexData = new byte[byteSize_estimation];

                //check to see if we are not over the header length (we are working backwards)
                if (leftoverOffset > headerLength)
                {
                    //copy all the bytes from the source byte file after the leftoverOffset, and copy that data to the texture data byte array
                    Array.Copy(sourceByteFile, leftoverOffset, mipTexData, 0, mipTexData.Length);

                    //combine the new mip byte data to the existing texture data byte array
                    finalDDS_textureData = Combine(finalDDS_textureData, mipTexData);
                }
            }

            //write the result to the console for viewing
            Console.WriteLine("Total Mips Byte Size = {0}", totalMipByteSize.ToString());

            //not required, but just viewing to see if our estimated sizes match up with the parsed texture byte size
            int totalTexByteSize = totalMipByteSize + mainTextureByteSize_Estimation;

            //write the result to the console for viewing
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
            Console.WriteLine("DDS to D3DTX Texture Converter");

            //-----------------GET TEXTURE FOLDER PATH-----------------
            Console.WriteLine("Enter the folder path with the textures.");
            Console.WriteLine("NOTE: Make sure each DDS is accompanied with a .header file");

            //texture folder path (containing the path to the textures to be converted)
            string textureFolderPath = "";

            //run a loop until the path is valid
            bool inTexturePathLoop = true;
            while (inTexturePathLoop)
            {
                //get path from user
                textureFolderPath = Console.ReadLine();

                //check if the path is valid
                if (Directory.Exists(textureFolderPath) == false)
                {
                    //notify the user and this loop will run again
                    Console.WriteLine("Incorrect Texture Path, try again.");
                }
                else
                {
                    //if it's sucessful, then break out of the loop
                    inTexturePathLoop = false;
                }
            }

            //-----------------GET RESULT FOLDER PATH-----------------
            Console.WriteLine("Enter the resulting path where converted textures will be stored.");

            //result folder path (will contain the converted textures)
            string resultFolderPath = "";

            //run a loop until the path is valid
            bool inResultPathLoop = true;
            while (inResultPathLoop)
            {
                //get path from user
                resultFolderPath = Console.ReadLine();

                //check if the path is valid
                if (Directory.Exists(resultFolderPath) == false)
                {
                    //notify the user and this loop will run again
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
            Console.WriteLine("Conversion Starting...");

            //we got our paths, so lets begin
            Convert_DDS_Bulk(textureFolderPath, resultFolderPath);

            //once BeginProcess is finished, it will come back here and we will notify the user that we are done
            Console.WriteLine("Conversion Finished.");
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

            //where our header file paths will be stored
            List<string> headerFiles;

            Console.WriteLine("Filtering Files..."); //notify the user we are filtering the array

            //filter the array so we only get .dds files
            ddsFiles = FilterFiles(files, ddsExtension);

            //filter the array so we only get .header files
            headerFiles = FilterFiles(files, headerExtension);

            //if none of the arrays have any files that were found, abort the program from going on any further (we don't have any files to convert!)
            if (ddsFiles.Count < 1)
            {
                Console.WriteLine("No .d3dtx files were found, aborting.");
                return;
            }
            else if (headerFiles.Count < 1)
            {
                Console.WriteLine("No .header files were found.");
                Console.WriteLine(".header are required and must be generated when converting a .d3dtx to a .dds");
                Console.WriteLine("aborting...");
                return;
            }

            Console.WriteLine("Found {0} Textures.", ddsFiles.Count.ToString()); //notify the user we found x amount of dds files in the array
            Console.WriteLine("Found {0} Headers.", headerFiles.Count.ToString()); //notify the user we found x amount of header files in the array
            Console.WriteLine("Starting...");//notify the user we are starting

            //run a loop through each of the found textures and convert each one
            foreach (string ddsFile in ddsFiles)
            {
                //build the path for the resulting d3dtx file
                string textureFileName = Path.GetFileName(ddsFile); //get the file name of the file + extension
                string textureFileNameOnly = Path.GetFileNameWithoutExtension(ddsFile);
                string textureResultPath = resultPath + "/" + textureFileNameOnly + d3dtxExtension; //add the file name to the resulting folder path, this is where our converted file will be placed

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
                    Console.WriteLine("Merging '{0}'...", textureFileName); //notify the user are converting 'x' file.
                    Console.WriteLine("Merging '{0}'...", Path.GetFileName(textureHeaderFile)); //notify the user are converting 'x' file.

                    //runs the main method for merging both files into a single .d3dtx
                    ConvertTexture_FromDDS_ToD3DTX(textureFileName, ddsFile, textureHeaderFile, textureResultPath);

                    Console.WriteLine("Finished merging '{0}'...", textureFileNameOnly); //notify the user we finished converting 'x' file.
                }
                else
                {
                    //notify the user that we can't convert this file, so we have to skip
                    Console.WriteLine("Can't find the matching header file for '{0}'! Skipping this one.", textureFileName);
                }
            }
        }

        /// <summary>
        /// The main function for reading and converting said .d3dtx into a .dds file
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <param name="destinationFile"></param>
        public static void ConvertTexture_FromDDS_ToD3DTX(string sourceFileName, string sourceTexFile, string sourceHeaderFile, string destinationFile)
        {
            /*
             * NOTE TO SELF
             * DDS --> D3DTX EXTRACTION UNSUCESSFUL, THE BYTES ARE NOT FULLY 1:1 WHEN THERE IS A CONVERSION
             * MABYE TRY TO CHANGE THE TEXTURE DATA BYTE SIZE IN THE D3DTX HEADER AND SEE IF THAT CHANGES ANYTHING
             * IF NOT THEN WE NEED TO LOOK INTO THE DDS --> D3DTX AGAIN AND GO THROUGH IT UNTIL WE GET THE EXACT BYTES
            */

            //read the source texture file into a byte array
            byte[] sourceTexFileData = File.ReadAllBytes(sourceTexFile);

            //read the source header file into a byte array
            byte[] sourceHeaderFileData = File.ReadAllBytes(sourceHeaderFile);

            //get our file name and convert it to a byte array (since d3dtx has the filename.extension written in the file)
            byte[] fileNameBytes = Encoding.ASCII.GetBytes(sourceFileName);


            //initalize our variables for the dds header
            int texture_parsed_headerLength; //total byte size of the header data
            int texture_parsed_imageWidth; //size of the dds image pixel width
            int texture_parsed_imageHeight; //size of the dds image height height
            int texture_parsed_mipMapCount; //total amount of mip maps in the dds file
            int texture_parsed_compressionType; //compression type of the dds file

            //initalize our variables for the d3dtx header
            int header_parsed_textureDataByteSize; //total byte size of the texture data, used to calculate the header length

            //write the result to the console for viewing
            Console.WriteLine("Total Source Texture Byte Size = {0}", sourceTexFileData.Length);

            //write the result to the console for viewing
            Console.WriteLine("Total Source Header Byte Size = {0}", sourceHeaderFileData.Length);

            //which byte offset we are on for the source texture (will be changed as we go through the file)
            int texture_bytePointerPosition = 0;

            //which byte offset we are on for the source header (will be changed as we go through the file)
            int header_bytePointerPosition = 0;

            //--------------------------1 DDS HEADER SIZE--------------------------
            //skip the dds dword for now because we just want the size of the header
            texture_bytePointerPosition = 4;

            //allocate 4 byte array (int32)
            byte[] texture_source_headerLength = AllocateByteArray(4, sourceTexFileData, texture_bytePointerPosition);

            //parse the byte array to int32
            texture_parsed_headerLength = BitConverter.ToInt32(texture_source_headerLength);

            //write the result to the console for viewing
            Console.WriteLine("DDS Header Length = {0}", texture_parsed_headerLength.ToString());

            //--------------------------2 DDS IMAGE HEIGHT--------------------------
            //skip the dds dword for now because we just want the size of the header
            texture_bytePointerPosition = 12;

            //allocate 4 byte array (int32)
            byte[] texture_source_imageHeight = AllocateByteArray(4, sourceTexFileData, texture_bytePointerPosition);

            //parse the byte array to int32
            texture_parsed_imageHeight = BitConverter.ToInt32(texture_source_imageHeight);

            //write the result to the console for viewing
            Console.WriteLine("DDS Image Height = {0}", texture_parsed_imageHeight.ToString());

            //--------------------------3 DDS IMAGE HEIGHT--------------------------
            //skip the dds dword for now because we just want the size of the header
            texture_bytePointerPosition = 16;

            //allocate 4 byte array (int32)
            byte[] texture_source_imageWidth = AllocateByteArray(4, sourceTexFileData, texture_bytePointerPosition);

            //parse the byte array to int32
            texture_parsed_imageWidth = BitConverter.ToInt32(texture_source_imageWidth);

            //write the result to the console for viewing
            Console.WriteLine("DDS Image Width = {0}", texture_parsed_imageWidth.ToString());

            //--------------------------4 DDS MIP MAP COUNT--------------------------
            //skip ahead to the mip map count
            texture_bytePointerPosition = 28;

            //allocate 4 byte array (int32)
            byte[] texture_source_mipMapCount = AllocateByteArray(4, sourceTexFileData, texture_bytePointerPosition);

            //parse the byte array to int32
            texture_parsed_mipMapCount = BitConverter.ToInt32(texture_source_mipMapCount);

            //write the result to the console for viewing
            Console.WriteLine("DDS Header Length = {0}", texture_parsed_mipMapCount.ToString());

            //--------------------------5 DDS COMPRESSION TYPE--------------------------
            //note to self - be sure to get the pixel format header size as well later
            //skip ahead to the mip map count
            texture_bytePointerPosition = 84;

            //allocate 4 byte array (int32)
            byte[] texture_source_compressionType = AllocateByteArray(4, sourceTexFileData, texture_bytePointerPosition);

            //parse the byte array to int32
            texture_parsed_compressionType = BitConverter.ToInt32(texture_source_compressionType);

            //write the result to the console for viewing
            Console.WriteLine("DDS Compression Type = {0}", texture_parsed_compressionType.ToString());

            //--------------------------EXTRACT DDS TEXTURE DATA--------------------------
            //calculate dds header length (we add 4 because we skipped the 4 bytes which contain the dword, it isn't necessary to parse this data)
            int ddsHeaderLength = 4 + texture_parsed_headerLength;
            int ddsTextureDataLength = sourceTexFileData.Length - ddsHeaderLength;

            byte[] textureData = new byte[ddsTextureDataLength];

            Array.Copy(sourceTexFileData, ddsHeaderLength, textureData, 0, textureData.Length);

            //--------------------------COMBINE DDS TEXTURE DATA WITH D3DTX HEADER--------------------------
            int total_d3dtxLength = textureData.Length + sourceHeaderFileData.Length;

            //if there are no mip maps, go ahead and just build the texture
            if(texture_parsed_mipMapCount <= 1)
            {
                //write the data to the file, combine the generted DDS header and our new texture byte data
                File.WriteAllBytes(destinationFile, Combine(sourceHeaderFileData, textureData));

                return;
            }

            //we will work through the texture data backwards, since the d3dtx format has mip map ordered reversed, so we will add it in that way
            //offset for getting mip maps, we are working backwards since d3dtx has their mip maps stored backwards
            int leftoverOffset = textureData.Length;

            //allocate a byte array to contain our texture data (ordered backwards)
            byte[] final_d3dtxData = new byte[total_d3dtxLength];

            if (texture_parsed_mipMapCount % 2 != 0)
                texture_parsed_mipMapCount += 1;

            texture_parsed_mipMapCount *= 2;

            //get image mip dimensions (will be modified when the loop is iterated)
            int mipImageWidth = texture_parsed_imageWidth/texture_parsed_mipMapCount;
            int mipImageHeight = texture_parsed_imageHeight/texture_parsed_mipMapCount;

            //not required, just for viewing
            int totalMipByteSize = 0;

            //run a loop for the amount of mip maps
            for (int i = 1; i < texture_parsed_mipMapCount; i++)
            {
                //write the result to the console for viewing
                Console.WriteLine("Mip Level = {0}", i.ToString());

                //divide the dimensions by 2 when stepping down on each mip level
                mipImageWidth *= 2;
                mipImageHeight *= 2;

                //write the result to the console for viewing
                Console.WriteLine("Mip Resolution = {0}x{1}", mipImageWidth.ToString(), mipImageHeight.ToString());

                //estimate how many total bytes are in the largest texture mip level (main one)
                int byteSize_estimation = CalculateDDS_ByteSize(mipImageWidth, mipImageHeight, texture_parsed_compressionType == 827611204);
                //offset our variable so we can get to the next mip (we are working backwards from the end of the file)
                leftoverOffset -= byteSize_estimation;

                //not required, just for viewing
                totalMipByteSize += byteSize_estimation;

                //write the result to the console for viewing
                Console.WriteLine("Mip Level Byte Size = {0}", byteSize_estimation.ToString());

                //allocate a byte array with the estimated byte size
                byte[] mipTexData = new byte[byteSize_estimation];

                //check to see if we are not over the length of the file (we are working backwards)
                if (leftoverOffset > 0)
                {
                    //copy all the bytes from the source byte file after the leftoverOffset, and copy that data to the texture data byte array
                    Array.Copy(sourceTexFileData, leftoverOffset, mipTexData, 0, mipTexData.Length);

                    //combine the new mip byte data to the existing texture data byte array
                    final_d3dtxData = Combine(final_d3dtxData, mipTexData);
                }
            }


            //combine the d3dtx header with the texture data
            final_d3dtxData = Combine(sourceHeaderFileData, final_d3dtxData);


            //write the data to the file, combine the generted DDS header and our new texture byte data
            File.WriteAllBytes(destinationFile, final_d3dtxData);
        }

        //-----------------------------------------------UTILLITIES-----------------------------------------------
        //-----------------------------------------------UTILLITIES-----------------------------------------------
        //-----------------------------------------------UTILLITIES-----------------------------------------------

        /// <summary>
        /// Filters an array of files by ".d3dtx" so only files with said extension will be in the array.
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public static List<string> FilterFiles(List<string> files, string filterExtension)
        {
            //the new filtered list
            List<string> filteredFiles = new List<string>();

            //run a loop through the existing 'files'
            foreach (string file in files)
            {
                //get the extension of a file
                string extension = Path.GetExtension(file);

                //if the file's extension matches our filter, add it to the list (naturally anything that doesn't have said filter will be ignored)
                if (extension.Equals(filterExtension))
                {
                    //add the matched extension to the list
                    filteredFiles.Add(file);
                }
            }

            //return the new filtered list
            return filteredFiles;
        }

        /// <summary>
        /// Calculates the byte size of a DDS texture
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="isDXT1"></param>
        /// <returns></returns>
        public static int CalculateDDS_ByteSize(int width, int height, bool isDXT1)
        {
            int compression = 0;

            //according to formula, if the compression is dxt1 then the number needs to be 8
            if (isDXT1)
                compression = 8;
            else
                compression = 16;

            //formula (from microsoft docs)
            //max(1, ( (width + 3) / 4 ) ) x max(1, ( (height + 3) / 4 ) ) x 8(DXT1) or 16(DXT2-5)

            //do the micorosoft magic texture byte size calculation formula
            return Math.Max(1, ((width + 3) / 4)) * Math.Max(1, ((height + 3) / 4) ) * compression;
        }

        /// <summary>
        /// Combines two byte arrays into one.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static byte[] Combine(byte[] first, byte[] second)
        {
            //allocate a byte array with both total lengths combined to accomodate both
            byte[] bytes = new byte[first.Length + second.Length];

            //copy the data from the first array into the new array
            Buffer.BlockCopy(first, 0, bytes, 0, first.Length);

            //copy the data from the second array into the new array (offset by the total length of the first array)
            Buffer.BlockCopy(second, 0, bytes, first.Length, second.Length);

            //return the final byte array
            return bytes;
        }

        /// <summary>
        /// Allocates a byte array of fixed length. 
        /// <para>Depending on 'size' it allocates 'size' amount of bytes from 'sourceByteArray' offset by 'offsetLocation'</para>
        /// </summary>
        /// <param name="size"></param>
        /// <param name="sourceByteArray"></param>
        /// <param name="offsetLocation"></param>
        /// <returns></returns>
        public static byte[] AllocateByteArray(int size, byte[] sourceByteArray, int offsetLocation)
        {
            //allocate byte array of fixed length
            byte[] source_imageHeight = new byte[size];

            //run a loop and begin gathering values
            for(int i = 0; i < size; i++)
            {
                //assign the value from the source byte array with the offset
                source_imageHeight[i] = sourceByteArray[offsetLocation + i];
            }

            //return the final byte array
            return source_imageHeight;
        }
    }
}
