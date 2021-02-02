using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace D3DTX_TextureConverter
{
    class Program
    {
        /// <summary>
        /// Main application method
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //introduction
            Console.WriteLine("D3DTX Texture Converter");

            //-----------------GET TEXTURE FOLDER PATH-----------------
            Console.WriteLine("Enter the folder path with the textures.");

            //texture folder path (containing the path to the textures to be converted)
            string textureFolderPath = "";

            //run a loop until the path is valid
            bool inTexturePathLoop = true;
            while(inTexturePathLoop)
            {
                //get path from user
                textureFolderPath = Console.ReadLine();

                //check if the path is valid
                if(Directory.Exists(textureFolderPath) == false)
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
            BeginProcess(textureFolderPath, resultFolderPath);

            //once BeginProcess is finished, it will come back here and we will notify the user that we are done
            Console.WriteLine("Conversion Finished.");
        }

        /// <summary>
        /// Filters an array of files by ".d3dtx" so only files with said extension will be in the array.
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public static List<string> FilterFiles(List<string> files)
        {
            //our extension to be filtering by
            string filterExtension = ".d3dtx";

            //the new filtered list
            List<string> filteredFiles = new List<string>();

            //run a loop through the existing 'files'
            foreach(string file in files)
            {
                //get the extension of a file
                string extension = Path.GetExtension(file);

                //if the file's extension matches our filter, add it to the list (naturally anything that doesn't have said filter will be ignored)
                if(extension.Equals(filterExtension))
                {
                    //add the matched extension to the list
                    filteredFiles.Add(file);
                }
            }

            //return the new filtered list
            return filteredFiles;
        }

        /// <summary>
        /// Begins the conversion process. Gathers the files found in the texture folder path, filters them, and converts each one.
        /// </summary>
        /// <param name="texPath"></param>
        /// <param name="resultPath"></param>
        public static void BeginProcess(string texPath, string resultPath)
        {
            Console.WriteLine("Collecting Files..."); //notify the user we are collecting files

            //gather the files from the texture folder path into an array
            List<string> textures = new List<string>(Directory.GetFiles(texPath));

            Console.WriteLine("Filtering Textures..."); //notify the user we are filtering the array

            //filter the array so we only get .d3dtx files
            textures = FilterFiles(textures);

            //if no d3dtx files were found, abort the program from going on any further (we don't have any files to convert!)
            if (textures.Count < 1)
            {
                Console.WriteLine("No .d3dtx files were found, aborting."); //notify the user we found x amount of d3dtx files in the array
                return;
            }

            Console.WriteLine("Found {0} Textures.", textures.Count.ToString()); //notify the user we found x amount of d3dtx files in the array
            Console.WriteLine("Starting...");//notify the user we found x amount of d3dtx files in the array

            //run a loop through each of the found textures and convert each one
            foreach (string texture in textures)
            {
                //build the path for the resulting file
                string textureFileName = Path.GetFileName(texture); //get the file name of the file + extension
                string textureFileNameOnly = Path.GetFileNameWithoutExtension(texture);
                string ddsFileExtension = ".dds";
                string textureResultPath = resultPath + "/" + textureFileNameOnly + ddsFileExtension; //add the file name to the resulting folder path, this is where our converted file will be placed

                Console.WriteLine("Converting '{0}'...", textureFileName); //notify the user are converting 'x' file.

                //runs the main method for converting the texture
                ConvertTexture(textureFileName, texture, textureResultPath);

                Console.WriteLine("Finished converting '{0}'...", textureFileName); //notify the user we finished converting 'x' file.
            }
        }

        /// <summary>
        /// The main function for reading and converting said .d3dtx into a .dds file
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <param name="destinationFile"></param>
        public static void ConvertTexture(string sourceFileName, string sourceFile, string destinationFile)
        {
            //for me
            bool ignoreUnknownValues = true;

            //read the source file into a byte array
            byte[] sourceByteFile = File.ReadAllBytes(sourceFile);

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

            //which byte offset we are on
            int bytePointerPosition = 0;

            //--------------------------1 = DWORD--------------------------
            //offset byte pointer location to get the DWORD
            bytePointerPosition = 0;

            //allocate 4 byte array (int32)
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

            //write the result to the console for viewing
            Console.WriteLine("Texture Byte Size = {0}", parsed_textureDataByteSize.ToString());
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

            //estimate how many total bytes are in the largest texture mip level (main one)
            int mainTextureByteSize_Estimation = CalculateDDS_ByteSize(parsed_imageWidth, parsed_imageHeight, parsed_dxtType == 64);

            //write the result to the console for viewing
            Console.WriteLine("calculated Largest Mip Level Byte Size = {0}", mainTextureByteSize_Estimation.ToString());

            //build the header and store it in a byte array
            byte[] ddsHeader = dds_File.Build_DDSHeader_ByteArray();

            //--------------------------EXTRACTING TEXTURE DATA FROM D3DTX--------------------------
            //calculating header length, parsed texture byte size - source byte size
            int headerLength = sourceByteFile.Length - parsed_textureDataByteSize;

            //initalize our start offset, this is used to offset the array copy
            int startOffset = 0;

            //allocate our byte array to contain our texture data
            byte[] textureData;

            //if our estimation is not accurate, then just extract the whole fuckin thing
            if (mainTextureByteSize_Estimation > parsed_textureDataByteSize)
            {
                //offset the byte pointer position just past the header
                startOffset = headerLength;

                //calculate main texture level
                textureData = new byte[parsed_textureDataByteSize];
            }
            else //if our estimation is accurate, then just extract the last mip map
            {
                //note to self, this will change later.
                //as a test for self, create multiple child dds files that each have the mip map data extracted into each single one

                //offset the byte pointer position just past the header
                startOffset = sourceByteFile.Length - mainTextureByteSize_Estimation;

                //calculate main texture level
                textureData = new byte[mainTextureByteSize_Estimation];
            }

            //copy all the bytes from the source byte file after the header length, and copy that data to the texture data byte array
            Array.Copy(sourceByteFile, startOffset, textureData, 0, textureData.Length);

            //write the data to the file
            File.WriteAllBytes(destinationFile, Combine(ddsHeader, textureData));
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
            byte[] bytes = new byte[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, bytes, 0, first.Length);
            Buffer.BlockCopy(second, 0, bytes, first.Length, second.Length);
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
