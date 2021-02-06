using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TextureMod_GUI
{
    public class Converter
    {
        //for D3DTX Mode, IMPORTANT if you want to convert the dds back to a d3dtx
        public bool generateHeader = true;

        //dds image file extension
        public readonly string ddsExtension = ".dds";

        //telltale d3dtx texture file extension
        public readonly string d3dtxExtension = ".d3dtx";

        //custom header file extension (generated from d3dtx to dds, used to convert dds back to d3dtx)
        public readonly string headerExtension = ".header";

        private Converter_Utillities converter_utillities;
        private MainManager mainManager;

        public Converter(MainManager mainManager)
        {
            this.mainManager = mainManager;

            converter_utillities = new Converter_Utillities();
        }

        private void WriteLine(string line)
        {
            line += System.Environment.NewLine;
            mainManager.Console_Output(line);
        }

        //-----------------------------------------------D3DTX TO DDS-----------------------------------------------
        //-----------------------------------------------D3DTX TO DDS-----------------------------------------------
        //-----------------------------------------------D3DTX TO DDS-----------------------------------------------

        /// <summary>
        /// Application function for converting D3DTX to DDS
        /// </summary>
        public void App_Convert_D3DTX_Mode(string textureFolderPath, string resultFolderPath)
        {
            //-----------------START CONVERSION-----------------
            //notify the user we are starting
            Console.WriteLine("D3DTX TO DDS Conversion Starting...");

            //we got our paths, so lets begin
            Convert_D3DTX_Bulk(textureFolderPath, resultFolderPath);

            //once BeginProcess is finished, it will come back here and we will notify the user that we are done
            WriteLine(string.Format("D3DTX TO DDS Conversion Finished."));
        }

        /// <summary>
        /// Begins the conversion process. Gathers the files found in the texture folder path, filters them, and converts each one.
        /// </summary>
        /// <param name="texPath"></param>
        /// <param name="resultPath"></param>
        public void Convert_D3DTX_Bulk(string texPath, string resultPath)
        {
            WriteLine(string.Format("Collecting Files...")); //notify the user we are collecting files

            //gather the files from the texture folder path into an array
            List<string> textures = new List<string>(Directory.GetFiles(texPath));

            WriteLine(string.Format("Filtering Textures...")); //notify the user we are filtering the array

            //filter the array so we only get .d3dtx files
            textures = converter_utillities.FilterFiles(textures, d3dtxExtension);

            //if no d3dtx files were found, abort the program from going on any further (we don't have any files to convert!)
            if (textures.Count < 1)
            {
                WriteLine(string.Format("No .d3dtx files were found, aborting.")); //notify the user we found no amount of d3dtx files in the array
                return;
            }

            WriteLine(string.Format("Found {0} Textures.", textures.Count.ToString())); //notify the user we found x amount of d3dtx files in the array
            WriteLine(string.Format("Starting...")); //notify the user we are starting

            //run a loop through each of the found textures and convert each one
            foreach (string texture in textures)
            {
                //build the path for the resulting file
                string textureFileName = Path.GetFileName(texture); //get the file name of the file + extension
                string textureFileNameOnly = Path.GetFileNameWithoutExtension(texture);
                string textureResultPath = resultPath + "/" + textureFileNameOnly + ddsExtension; //add the file name to the resulting folder path, this is where our converted file will be placed

                WriteLine(string.Format("Converting '{0}'...", textureFileName)); //notify the user are converting 'x' file.

                //runs the main method for converting the texture
                ConvertTexture_FromD3DTX_ToDDS(textureFileName, texture, textureResultPath);

                WriteLine(string.Format("Finished converting '{0}'...", textureFileName)); //notify the user we finished converting 'x' file.
            }
        }

        /// <summary>
        /// The main function for reading and converting said .d3dtx into a .dds file
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <param name="destinationFile"></param>
        public void ConvertTexture_FromD3DTX_ToDDS(string sourceFileName, string sourceFile, string destinationFile)
        {
            //read the source file into a byte array
            byte[] sourceByteFile = File.ReadAllBytes(sourceFile);

            //write the result to the console for viewing
            WriteLine(string.Format("Total File Byte Size = {0}", sourceByteFile.Length));

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
            byte[] source_dword = converter_utillities.AllocateByteArray(4, sourceByteFile, bytePointerPosition);

            //parse the byte array to string
            parsed_dword = Encoding.ASCII.GetString(source_dword);

            //write the result to the console for viewing
            WriteLine(string.Format("DWORD = {0}", parsed_dword));
            //--------------------------2 = COMPRESISON TYPE?--------------------------
            //offset byte pointer location to get the COMPRESISON TYPE
            bytePointerPosition = 4;

            //allocate 4 byte array (int32)
            byte[] source_compressionType = converter_utillities.AllocateByteArray(4, sourceByteFile, bytePointerPosition);

            //parse the byte array to int32
            parsed_compressionType = BitConverter.ToInt32(source_compressionType);

            //write the result to the console for viewing
            WriteLine(string.Format("Compression Type = {0}", parsed_compressionType.ToString()));
            //--------------------------3 = TEXTURE BYTE SIZE--------------------------
            //offset byte pointer location to get the TEXTURE BYTE SIZE
            bytePointerPosition = 12;

            //allocate 4 byte array (int32)
            byte[] source_fileSize = converter_utillities.AllocateByteArray(4, sourceByteFile, bytePointerPosition);

            //parse the byte array to int32
            parsed_textureDataByteSize = BitConverter.ToInt32(source_fileSize);

            //calculating header length, parsed texture byte size - source byte size
            headerLength = sourceByteFile.Length - parsed_textureDataByteSize;

            //write the result to the console for viewing
            WriteLine(string.Format("Texture Byte Size = {0}", parsed_textureDataByteSize.ToString()));
            WriteLine(string.Format("Header Byte Size = {0}", headerLength.ToString()));
            //--------------------------4 = SCREWY TELLTALE DATA--------------------------
            //NOTE TO SELF - no need to parse this byte data, we can extract the entire header later with this info included and just change what we need

            //offset byte pointer location to get the SCREWY TELLTALE DATA
            bytePointerPosition = 20;

            int telltaleScrewyHeaderLength = 84; //screwy header offset length

            byte[] screwyHeaderData = new byte[telltaleScrewyHeaderLength];

            for (int i = 0; i < telltaleScrewyHeaderLength; i++)
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
            for (int i = 0; i < fileNameBytes.Length; i++)
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
                WriteLine(string.Format("ERROR, can't convert '{0}' because the filename bytes do not match. Skipping conversion on this.", sourceFileName));
                return; //return the method and don't continue any further
            }

            //move the cursor past the filename.extension byte string
            bytePointerPosition += fileNameBytes.Length;

            //--------------------------6 = MIP MAP COUNT--------------------------
            //offset byte pointer location to get the MIP MAP COUNT
            bytePointerPosition += 13;

            //allocate 4 byte array (int32)
            byte[] source_imageMipMapCount = converter_utillities.AllocateByteArray(4, sourceByteFile, bytePointerPosition);

            //parse the byte array to int32
            parsed_imageMipMapCount = BitConverter.ToInt32(source_imageMipMapCount);
            parsed_imageMipMapCount_decremented = parsed_imageMipMapCount - 1;

            //write the result to the console for viewing
            WriteLine(string.Format("Mip Map Count = {0} ({1})", parsed_imageMipMapCount.ToString(), parsed_imageMipMapCount_decremented.ToString()));
            //--------------------------7 = MAIN IMAGE WIDTH--------------------------
            //offset byte pointer location to get the MAIN IMAGE WIDTH
            bytePointerPosition += 4;

            //allocate 4 byte array (int32)
            byte[] source_imageWidth = converter_utillities.AllocateByteArray(4, sourceByteFile, bytePointerPosition);

            //parse the byte array to int32
            parsed_imageWidth = BitConverter.ToInt32(source_imageWidth);

            //write the result to the console for viewing
            WriteLine(string.Format("Image Width = {0}", parsed_imageWidth.ToString()));
            //--------------------------8 = MAIN IMAGE HEIGHT--------------------------
            //offset byte pointer location to get the MAIN IMAGE HEIGHT
            bytePointerPosition += 4;

            //allocate 4 byte array (int32)
            byte[] source_imageHeight = converter_utillities.AllocateByteArray(4, sourceByteFile, bytePointerPosition);

            //parse the byte array to int32
            parsed_imageHeight = BitConverter.ToInt32(source_imageHeight);

            //write the result to the console for viewing
            WriteLine(string.Format("Image Height = {0}", parsed_imageHeight.ToString()));
            //--------------------------9 = DXT TYPE?--------------------------
            //offset byte pointer location to get the DXT TYPE
            bytePointerPosition += 12;

            //allocate 4 byte array (int32)
            byte[] source_dxtType = converter_utillities.AllocateByteArray(4, sourceByteFile, bytePointerPosition);

            //parse the byte array to int32
            parsed_dxtType = BitConverter.ToInt32(source_dxtType);

            //write the result to the console for viewing
            WriteLine(string.Format("DXT TYPE = {0}", parsed_dxtType.ToString()));
            //--------------------------BUILDING DDS TEXTURE HEADER--------------------------
            //NOTE TO SELF 1 - BIGGEST ISSUE RIGHT NOW IS MIP MAPS, NEED TO PARSE MORE INFORMATION FROM D3DTX TO BE ABLE TO EXTRACT MIP MAPS PROPERLY
            //NOTE TO SELF 2 - largest mip map (main texture) extraction is successful, the next step is getting the lower mip levels seperately, thank you microsoft for the byte size calculation forumla
            //NOTE TO SELF 3 - all mip maps can be extracted sucessfully and implemented into the DDS file

            //get our dds file object ready
            Texture_DDS dds_File = new Texture_DDS();

            //assign our parsed values from the d3dtx to new dds file
            dds_File.dwWidth = (uint)parsed_imageWidth;
            dds_File.dwHeight = (uint)parsed_imageHeight;
            dds_File.dwMipMapCount = (uint)parsed_imageMipMapCount_decremented;


            //this section needs some reworking, still can't track down exactly what the compression types are, parsed_compressionType and parsed_dxtType are close
            //SET DDS COMPRESSION TYPES
            if (parsed_dxtType == 66)//if (parsed_compressionType == 200 || parsed_compressionType == 406)
            {
                //DXT5 COMPRESSION
                dds_File.ddspf_dwFourCC = "DXT5";
            }
            else if (parsed_compressionType == 67)
            {
                //DDSPF_DXT3 COMPRESSION
                dds_File.ddspf_dwFourCC = "DXT3";
            }
            else if (parsed_dxtType == 68)//else if (parsed_compressionType == 388)
            {
                //DDSPF_BC5_UNORM COMPRESSION
                dds_File.ddspf_dwFourCC = "BC5U";
            }
            else if (parsed_dxtType == 67)//else if (parsed_compressionType == 413)
            {
                //DDSPF_BC4_UNORM COMPRESSION
                dds_File.ddspf_dwFourCC = "BC4U";
            }

            //build the header and store it in a byte array
            byte[] ddsHeader = dds_File.Build_DDSHeader_ByteArray();

            //--------------------------GENERATING D3DTX HEADER FILE--------------------------
            //generates a .header file that will accompany the .dds on conversion, this .header file will contain the original .d3dtx header for converting the .dds back later
            if (generateHeader)
            {
                //build the header destination path, assuming the extnesion of the destination file path is .dds
                int headerExtensionIndex = destinationFile.Length - 4;
                string headerFilePath = string.Format("{0}{1}", destinationFile.Remove(headerExtensionIndex, 4), headerExtension);

                //allocate a byte array to contain the header data
                byte[] headerData = new byte[headerLength];

                //copy all the bytes from the source byte file after the header length, and copy that data to the texture data byte array
                Array.Copy(sourceByteFile, 0, headerData, 0, headerData.Length);

                //write the header data to a file
                File.WriteAllBytes(headerFilePath, headerData);
            }

            //--------------------------EXTRACTING TEXTURE DATA FROM D3DTX--------------------------
            //estimate how many total bytes are in the largest texture mip level (main one)
            int mainTextureByteSize_Estimation = converter_utillities.CalculateDDS_ByteSize(parsed_imageWidth, parsed_imageHeight, parsed_dxtType == 64);

            //write the result to the console for viewing
            WriteLine(string.Format("calculated Largest Mip Level Byte Size = {0}", mainTextureByteSize_Estimation.ToString()));

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

            byte[] finalDDS_textureData = converter_utillities.Combine(ddsHeader, textureData);

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
                WriteLine(string.Format("Mip Level = {0}", i.ToString()));

                //divide the dimensions by 2 when stepping down on each mip level
                mipImageWidth /= 2;
                mipImageHeight /= 2;

                //write the result to the console for viewing
                WriteLine(string.Format("Mip Resolution = {0}x{1}", mipImageWidth.ToString(), mipImageHeight.ToString()));

                //estimate how many total bytes are in the largest texture mip level (main one)
                int byteSize_estimation = converter_utillities.CalculateDDS_ByteSize(mipImageWidth, mipImageHeight, parsed_dxtType == 64);
                //offset our variable so we can get to the next mip (we are working backwards from the end of the file)
                leftoverOffset -= byteSize_estimation;

                //not required, just for viewing
                totalMipByteSize += byteSize_estimation;

                //write the result to the console for viewing
                WriteLine(string.Format("Mip Level Byte Size = {0}", byteSize_estimation.ToString()));

                //allocate a byte array with the estimated byte size
                byte[] mipTexData = new byte[byteSize_estimation];

                //check to see if we are not over the header length (we are working backwards)
                if (leftoverOffset > headerLength)
                {
                    //copy all the bytes from the source byte file after the leftoverOffset, and copy that data to the texture data byte array
                    Array.Copy(sourceByteFile, leftoverOffset, mipTexData, 0, mipTexData.Length);

                    //combine the new mip byte data to the existing texture data byte array
                    finalDDS_textureData = converter_utillities.Combine(finalDDS_textureData, mipTexData);
                }
            }

            //write the result to the console for viewing
            WriteLine(string.Format("Total Mips Byte Size = {0}", totalMipByteSize.ToString()));

            //not required, but just viewing to see if our estimated sizes match up with the parsed texture byte size
            int totalTexByteSize = totalMipByteSize + mainTextureByteSize_Estimation;

            //write the result to the console for viewing
            WriteLine(string.Format("Total Byte Size = {0}", totalTexByteSize.ToString()));

            //write the data to the file, combine the generted DDS header and our new texture byte data
            File.WriteAllBytes(destinationFile, finalDDS_textureData);
        }

        //-----------------------------------------------DDS TO D3DTX-----------------------------------------------
        //-----------------------------------------------DDS TO D3DTX-----------------------------------------------
        //-----------------------------------------------DDS TO D3DTX-----------------------------------------------

        /// <summary>
        /// Application function for converting DDS to D3DTX
        /// </summary>
        public void App_Convert_DDS_Mode(string textureFolderPath, string resultFolderPath)
        {
            //-----------------START CONVERSION-----------------
            //notify the user we are starting
            WriteLine(string.Format("DDS TO D3DTX Conversion Starting..."));

            //we got our paths, so lets begin
            Convert_DDS_Bulk(textureFolderPath, resultFolderPath);

            //once BeginProcess is finished, it will come back here and we will notify the user that we are done
            WriteLine(string.Format("DDS TO D3DTX Conversion Finished."));
        }

        /// <summary>
        /// Begins the conversion process. Gathers the files found in the texture folder path, filters them, and converts each one.
        /// </summary>
        /// <param name="texPath"></param>
        /// <param name="resultPath"></param>
        public void Convert_DDS_Bulk(string texPath, string resultPath)
        {
            WriteLine(string.Format("Collecting Files...")); //notify the user we are collecting files

            //gather the files from the texture folder path into an array
            List<string> files = new List<string>(Directory.GetFiles(texPath));

            //where our dds file paths will be stored
            List<string> ddsFiles;

            //where our header file paths will be stored
            List<string> headerFiles;

            WriteLine(string.Format("Filtering Files...")); //notify the user we are filtering the array

            //filter the array so we only get .dds files
            ddsFiles = converter_utillities.FilterFiles(files, ddsExtension);

            //filter the array so we only get .header files
            headerFiles = converter_utillities.FilterFiles(files, headerExtension);

            //if none of the arrays have any files that were found, abort the program from going on any further (we don't have any files to convert!)
            if (ddsFiles.Count < 1)
            {
                WriteLine(string.Format("No .d3dtx files were found, aborting."));
                return;
            }
            else if (headerFiles.Count < 1)
            {
                WriteLine(string.Format("No .header files were found."));
                WriteLine(string.Format(".header are required and must be generated when converting a .d3dtx to a .dds"));
                WriteLine(string.Format("aborting..."));
                return;
            }

            WriteLine(string.Format("Found {0} Textures.", ddsFiles.Count.ToString())); //notify the user we found x amount of dds files in the array
            WriteLine(string.Format("Found {0} Headers.", headerFiles.Count.ToString())); //notify the user we found x amount of header files in the array
            WriteLine(string.Format("Starting...")); //notify the user we are starting

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
                foreach (string headerFile in headerFiles)
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
                    WriteLine(string.Format("Merging '{0}'...", textureFileName)); //notify the user are converting 'x' file.
                    WriteLine(string.Format("Merging '{0}'...", Path.GetFileName(textureHeaderFile))); //notify the user are converting 'x' file.

                    //runs the main method for merging both files into a single .d3dtx
                    ConvertTexture_FromDDS_ToD3DTX(textureFileName, ddsFile, textureHeaderFile, textureResultPath);

                    WriteLine(string.Format("Finished merging '{0}'...", textureFileNameOnly)); //notify the user we finished converting 'x' file.
                }
                else
                {
                    //notify the user that we can't convert this file, so we have to skip
                    WriteLine(string.Format("Can't find the matching header file for '{0}'! Skipping this one.", textureFileName));
                }
            }
        }

        /// <summary>
        /// The main function for reading and converting said .d3dtx into a .dds file
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <param name="destinationFile"></param>
        public void ConvertTexture_FromDDS_ToD3DTX(string sourceFileName, string sourceTexFile, string sourceHeaderFile, string destinationFile)
        {
            /*
             * NOTE TO SELF
             * DDS --> D3DTX EXTRACTION, THE BYTES ARE NOT FULLY 1:1 WHEN THERE IS A CONVERSION (off by 8 bytes)
             * MABYE TRY TO CHANGE THE TEXTURE DATA BYTE SIZE IN THE D3DTX HEADER AND SEE IF THAT CHANGES ANYTHING
            */

            //read the source texture file into a byte array
            byte[] sourceTexFileData = File.ReadAllBytes(sourceTexFile);

            //read the source header file into a byte array
            byte[] sourceHeaderFileData = File.ReadAllBytes(sourceHeaderFile);

            //get our file name and convert it to a byte array (since d3dtx has the filename.extension written in the file)
            byte[] fileNameBytes = Encoding.ASCII.GetBytes(sourceFileName); //currently unused (but will be)

            //initalize our variables for the dds header
            int texture_parsed_headerLength; //total byte size of the header data
            int texture_parsed_imageWidth; //size of the dds image pixel width
            int texture_parsed_imageHeight; //size of the dds image height height
            int texture_parsed_mipMapCount; //total amount of mip maps in the dds file
            string texture_parsed_compressionType; //compression type of the dds file

            //initalize our variables for the d3dtx header
            int header_parsed_textureDataByteSize; //total byte size of the texture data, used to calculate the header length (NOT USED, BUT WILL BE)

            //write the result to the console for viewing
            WriteLine(string.Format("Total Source Texture Byte Size = {0}", sourceTexFileData.Length));

            //write the result to the console for viewing
            WriteLine(string.Format("Total Source Header Byte Size = {0}", sourceHeaderFileData.Length));

            //which byte offset we are on for the source texture (will be changed as we go through the file)
            int texture_bytePointerPosition = 0;

            //which byte offset we are on for the source header (will be changed as we go through the file)
            int header_bytePointerPosition = 0; //currently not used (but will be when we start modifying the d3dtx header)

            //--------------------------1 DDS HEADER SIZE--------------------------
            //skip the dds dword for now because we just want the size of the header
            texture_bytePointerPosition = 4;

            //allocate 4 byte array (int32)
            byte[] texture_source_headerLength = converter_utillities.AllocateByteArray(4, sourceTexFileData, texture_bytePointerPosition);

            //parse the byte array to int32
            texture_parsed_headerLength = BitConverter.ToInt32(texture_source_headerLength);

            //write the result to the console for viewing
            WriteLine(string.Format("DDS Header Length = {0}", texture_parsed_headerLength.ToString()));

            //--------------------------2 DDS IMAGE HEIGHT--------------------------
            //skip the dds dword for now because we just want the size of the header
            texture_bytePointerPosition = 12;

            //allocate 4 byte array (int32)
            byte[] texture_source_imageHeight = converter_utillities.AllocateByteArray(4, sourceTexFileData, texture_bytePointerPosition);

            //parse the byte array to int32
            texture_parsed_imageHeight = BitConverter.ToInt32(texture_source_imageHeight);

            //write the result to the console for viewing
            WriteLine(string.Format("DDS Image Height = {0}", texture_parsed_imageHeight.ToString()));

            //--------------------------3 DDS IMAGE HEIGHT--------------------------
            //skip the dds dword for now because we just want the size of the header
            texture_bytePointerPosition = 16;

            //allocate 4 byte array (int32)
            byte[] texture_source_imageWidth = converter_utillities.AllocateByteArray(4, sourceTexFileData, texture_bytePointerPosition);

            //parse the byte array to int32
            texture_parsed_imageWidth = BitConverter.ToInt32(texture_source_imageWidth);

            //write the result to the console for viewing
            WriteLine(string.Format("DDS Image Width = {0}", texture_parsed_imageWidth.ToString()));

            //--------------------------4 DDS MIP MAP COUNT--------------------------
            //skip ahead to the mip map count
            texture_bytePointerPosition = 28;

            //allocate 4 byte array (int32)
            byte[] texture_source_mipMapCount = converter_utillities.AllocateByteArray(4, sourceTexFileData, texture_bytePointerPosition);

            //parse the byte array to int32
            texture_parsed_mipMapCount = BitConverter.ToInt32(texture_source_mipMapCount);

            //write the result to the console for viewing
            WriteLine(string.Format("DDS Header Length = {0}", texture_parsed_mipMapCount.ToString()));

            //--------------------------5 DDS COMPRESSION TYPE--------------------------
            //note to self - be sure to get the pixel format header size as well later
            //skip ahead to the mip map count
            texture_bytePointerPosition = 84;

            //allocate 4 byte array (int32)
            byte[] texture_source_compressionType = converter_utillities.AllocateByteArray(4, sourceTexFileData, texture_bytePointerPosition);

            //parse the byte array to int32
            texture_parsed_compressionType = Encoding.ASCII.GetString(texture_source_compressionType);

            //write the result to the console for viewing
            WriteLine(string.Format("DDS Compression Type = {0}", texture_parsed_compressionType.ToString()));

            //--------------------------EXTRACT DDS TEXTURE DATA--------------------------
            //calculate dds header length (we add 4 because we skipped the 4 bytes which contain the dword, it isn't necessary to parse this data)
            int ddsHeaderLength = 4 + texture_parsed_headerLength;

            //calculate the length of just the dds texture data
            int ddsTextureDataLength = sourceTexFileData.Length - ddsHeaderLength;

            //allocate a byte array of dds texture length
            byte[] ddsTextureData = new byte[ddsTextureDataLength];

            //copy the data from the source byte array past the header (so we are only getting texture data)
            Array.Copy(sourceTexFileData, ddsHeaderLength, ddsTextureData, 0, ddsTextureData.Length);

            //--------------------------COMBINE DDS TEXTURE DATA WITH D3DTX HEADER--------------------------
            int total_d3dtxLength = ddsTextureData.Length + sourceHeaderFileData.Length;

            //if there are no mip maps, go ahead and just build the texture
            if (texture_parsed_mipMapCount <= 1)
            {
                //write the data to the file, combine the generted DDS header and our new texture byte data
                File.WriteAllBytes(destinationFile, converter_utillities.Combine(sourceHeaderFileData, ddsTextureData));

                //stop the function as there is no need to continue any further
                return;
            }

            //we will work through the texture data backwards, since the d3dtx format has mip map ordered reversed, so we will add it in that way
            //offset for getting mip maps, we are working backwards since d3dtx has their mip maps stored backwards
            int leftoverOffset = ddsTextureData.Length;

            //allocate a byte array to contain our texture data (ordered backwards)
            byte[] final_d3dtxData = new byte[0];

            //modify the texture file size data in the header
            sourceHeaderFileData = converter_utillities.ModifyBytes(sourceHeaderFileData, BitConverter.GetBytes(ddsTextureData.Length), 12);

            //add the d3dtx header
            //note to self - modify the header before adding it
            final_d3dtxData = converter_utillities.Combine(sourceHeaderFileData, final_d3dtxData);

            //quick fix for textures not being read properly (they are not the exact same size)
            //note to self - try to modify the d3dtx header so the texture byte size in the header matches the texture byte size we are inputing
            //byte[] fillterData = new byte[8];
            //final_d3dtxData = Combine(final_d3dtxData, fillterData);

            //add 1 since the mip map count in dds files tend to start at 0 instead of 1 
            texture_parsed_mipMapCount += 1;

            //because I suck at math, we will generate our mip map resolutions using the same method we did in d3dtx to dds (can't figure out how to calculate them in reverse properly)
            int[,] mipImageResolutions = new int[texture_parsed_mipMapCount, 2];

            //get our mip image dimensions (have to multiply by 2 as the mip calculations will be off by half)
            int mipImageWidth = texture_parsed_imageWidth * 2;
            int mipImageHeight = texture_parsed_imageHeight * 2;

            //add the resolutions in reverse
            for (int i = texture_parsed_mipMapCount - 1; i > 0; i--)
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
            for (int i = 1; i < texture_parsed_mipMapCount; i++)
            {
                //not required, just for viewing
                int mipLevel = texture_parsed_mipMapCount - i;

                //get our mip resolution from the resolution array (the values are reversed, smallest to big)
                int width = mipImageResolutions[i, 0];
                int height = mipImageResolutions[i, 1];

                //estimate how many total bytes are in the largest texture mip level (main one)
                int byteSize_estimation = converter_utillities.CalculateDDS_ByteSize(width, height, texture_parsed_compressionType.Equals("DXT1"));

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
                    Array.Copy(ddsTextureData, leftoverOffset, mipTexData, 0, mipTexData.Length);

                    //combine the new mip byte data to the existing texture data byte array
                    final_d3dtxData = converter_utillities.Combine(final_d3dtxData, mipTexData);
                }

                //write results to the console for viewing
                WriteLine(string.Format("Leftover Offset = {0}", leftoverOffset.ToString()));
                WriteLine(string.Format("Mip Level = {0}", mipLevel.ToString()));
                WriteLine(string.Format("Mip Resolution = {0}x{1}", width.ToString(), height.ToString()));
                WriteLine(string.Format("Mip Level Byte Size = {0}", byteSize_estimation.ToString()));
                WriteLine(string.Format("D3DTX Data Length Byte Size = {0}", final_d3dtxData.Length.ToString()));
            }

            //write results to the console for viewing
            WriteLine(string.Format("D3DTX Header Byte Size = {0}", sourceHeaderFileData.Length.ToString()));

            //write the data to the file, combine the generted DDS header and our new texture byte data
            File.WriteAllBytes(destinationFile, final_d3dtxData);
        }
    }
}
