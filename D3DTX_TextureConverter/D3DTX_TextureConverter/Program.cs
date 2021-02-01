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

            //which byte offset we are on
            int bytePointerPosition = 0;

            //steps to converting file
            //1. get dword
            //2. get compression type
            //3. get unknown 1
            //4. get texture byte size
            //5. get unknown 2
            //6. check file name string
            //7. get image width
            //8. get image height

            //--------------------------1 = DWORD--------------------------
            //offset location of the DWORD
            bytePointerPosition = 0;

            //allocate 4 byte array (int32)
           // byte[] source_dword = AllocateByteArray(4, sourceByteFile, bytePointerPosition);

            //parse the byte array to int32
            //string parsed_dword = BitConverter.(source_dword);

            //Console.WriteLine("DWORD = {0}", parsed_dword);

            //--------------------------2 = COMPRESSION TYPE--------------------------
            //offset location of the compression type byte
            bytePointerPosition = 4;

            //allocate 4 byte array (int32)
            byte[] source_compressionType = AllocateByteArray(4, sourceByteFile, bytePointerPosition);

            //parse the byte array to int32
            int parsed_compressionType = BitConverter.ToInt32(source_compressionType);

            Console.WriteLine("Compression Type = {0}", parsed_compressionType.ToString());

            //--------------------------3 = UNKNOWN 1--------------------------
            //offset location of the unknown 1
            bytePointerPosition = 8;

            //allocate 4 byte array (int32)
            byte[] source_unknown1 = AllocateByteArray(4, sourceByteFile, bytePointerPosition);

            //parse the byte array to int32
            int parsed_unknown1 = BitConverter.ToInt32(source_unknown1);

            if(!ignoreUnknownValues)
                Console.WriteLine("UNKNOWN 1 = {0}", parsed_unknown1.ToString());

            //--------------------------4 = TEXTURE BYTE SIZE--------------------------
            //offset location of the file size
            bytePointerPosition = 12;

            //allocate 4 byte array (int32)
            byte[] source_fileSize = AllocateByteArray(4, sourceByteFile, bytePointerPosition);

            //parse the byte array to int32
            int parsed_fileSize = BitConverter.ToInt32(source_fileSize);

            Console.WriteLine("Texture Byte Size = {0}", parsed_fileSize.ToString());

            //--------------------------5 = UNKNOWN 2--------------------------
            //offset location of the unknown 2
            bytePointerPosition = 16;

            //allocate 4 byte array (int32)
            byte[] source_unknown2 = AllocateByteArray(4, sourceByteFile, bytePointerPosition);

            //parse the byte array to int32
            int parsed_unknown2 = BitConverter.ToInt32(source_unknown2);

            if (!ignoreUnknownValues)
                Console.WriteLine("UNKNOWN 2 = {0}", parsed_unknown2.ToString());

            //--------------------------6 = CHECK FILE NAME STRING--------------------------
            bytePointerPosition = 20;

            int telltaleScrewyHeaderLength = 84; //screwy header offset length (goes all the way until it hits the first byte of the filename string in the file)
            bytePointerPosition += 28; //skip the potentially 7 (4 bytes) data
            bytePointerPosition += telltaleScrewyHeaderLength; //skip the chunk of data since it doesn't seem to change

            //get our file name and convert it to a byte array (since d3dtx has the filename.extension written in the file)
            byte[] fileNameBytes = Encoding.ASCII.GetBytes(sourceFileName);

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

            //--------------------------7 = GET IMAGE WIDTH--------------------------
            //Image Pixel Width offset location
            bytePointerPosition += 17;

            //allocate 4 byte array (int32)
            byte[] source_imageWidth = AllocateByteArray(4, sourceByteFile, bytePointerPosition);

            //parse the byte array to int32
            int parsed_imageWidth = BitConverter.ToInt32(source_imageWidth);

            Console.WriteLine("Image Width = {0}", parsed_imageWidth.ToString());

            //--------------------------8 = GET IMAGE HEIGHT--------------------------
            //Image Pixel Height offset location
            bytePointerPosition += 4;

            //allocate 4 byte array (int32)
            byte[] source_imageHeight = AllocateByteArray(4, sourceByteFile, bytePointerPosition);

            //parse the byte array to int32
            int parsed_imageHeight = BitConverter.ToInt32(source_imageHeight);

            Console.WriteLine("Image Height = {0}", parsed_imageHeight.ToString());

            //--------------------------BUILDING DDS TEXTURE--------------------------
            //calculating header length, parsed texture byte size - source byte size
            int headerLength = sourceByteFile.Length - parsed_fileSize;

            //allocate our byte array to contain our texture data
            byte[] textureData = new byte[parsed_fileSize];

            //copy the bytes from the source byte file after the header length, and copy that data to the texture data byte array
            Array.Copy(sourceByteFile, headerLength, textureData, 0, parsed_fileSize);

            //get our dds file ready
            DDS_File dds_File = new DDS_File();

            //assign our parsed values from the d3dtx to new dds file
            dds_File.dwWidth = (uint)parsed_imageWidth;
            dds_File.dwHeight = (uint)parsed_imageHeight;

            //build the header
            byte[] ddsHeader = dds_File.Build_DDSHeader_ByteArray();
            
            //write the data to the file
            File.WriteAllBytes(destinationFile, Combine(ddsHeader, textureData));
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
