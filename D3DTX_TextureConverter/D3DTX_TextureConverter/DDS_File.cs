using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace D3DTX_TextureConverter
{
    //NOTE TO SELF: IF THERE IS AN EASIER WAY OF DOING THIS, DO IT SINCE THIS PROBABLY A RATHER LAZY WAY
    //DDS Docs - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-header
    //DDS PIXEL FORMAT - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-pixelformat
    //DDS File Layout https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-file-layout-for-textures
    //Texutre Block Compression in D3D11 - https://docs.microsoft.com/en-us/windows/win32/direct3d11/texture-block-compression-in-direct3d-11
    //DDS Programming Guide - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dx-graphics-dds-pguide

    /// <summary>
    /// Main class for generating a dds byte header
    /// </summary>
    public class DDS_File
    {
        //dds image file extension
        public static string ddsExtension = ".dds";

        //main dword (with space) [4 bytes]
        public readonly string dword = "DDS ";

        //--------------------------------------MAIN CHUNKS--------------------------------------
        //Size of structure. This member must be set to 124. [4 bytes]
        public readonly uint dwSize = 124;

        //Flags to indicate which members contain valid data. [4 bytes]
        public uint dwFlags = 528391;

        //Surface height (in pixels). [4 bytes]
        public uint dwHeight = 1024;

        //Surface width (in pixels). [4 bytes]
        public uint dwWidth = 1024;

        //The pitch or number of bytes per scan line in an uncompressed texture; the total number of bytes in the top level texture for a compressed texture. [4 bytes]
        public uint dwPitchOrLinearSize = 8192;

        //Depth of a volume texture (in pixels), otherwise unused. [4 bytes]
        public uint dwDepth = 0;

        //Number of mipmap levels, otherwise unused. [4 bytes]
        public uint dwMipMapCount = 0;

        //dds reserved bits
        //these are all unused according to DDS docs [all 4 bytes]
        public readonly uint dwReserved1 = 0;
        public readonly uint dwReserved2 = 0;
        public readonly uint dwReserved3 = 0;
        public readonly uint dwReserved4 = 0;
        public readonly uint dwReserved5 = 0;
        public readonly uint dwReserved6 = 0;
        public readonly uint dwReserved7 = 0;
        public readonly uint dwReserved8 = 0;
        public readonly uint dwReserved9 = 0;
        public readonly uint dwReserved10 = 0;
        public readonly uint dwReserved11 = 0;

        //--------------------------------------DDS PIXEL FORMAT STRUCT--------------------------------------
        //Structure size; set to 32 (bytes). [4 bytes]
        private readonly uint ddspf_size = 32;

        //Values which indicate what type of data is in the surface. [4 bytes]
        public uint ddspf_flags = 4;

        //Four-character codes for specifying compressed or custom formats. [4 bytes]
        //DXT1, DXT2, DXT3, DXT4, DXT5, ATI1, ATI2
        public string ddspf_dwFourCC = "DXT1";

        //Number of bits in an RGB (possibly including alpha) format. [4 bytes]
        public uint ddspf_rgbBitCount = 0;

        //Red (or lumiannce or Y) mask for reading color data. [4 bytes]
        public uint ddspf_RBitMask = 0;

        //Green (or U) mask for reading color data. [4 bytes]
        public uint ddspf_GBitMask = 0;

        //Blue (or V) mask for reading color data. [4 bytes]
        public uint ddspf_BBitMask = 0;

        //Alpha mask for reading alpha data. [4 bytes]
        public uint ddspf_ABitMask = 0;
        //--------------------------------------DDS PIXEL FORMAT STRUCT END--------------------------------------
        //--------------------------------------OTHER CHUNKS--------------------------------------

        //Specifies the complexity of the surfaces stored. [4 bytes]
        public uint dwCaps = 4096;

        //Additional detail about the surfaces stored. [4 bytes]
        public uint dwCaps2 = 0;

        //these 3 are unused according to DDS docs [all 4 bytes]
        public readonly uint dwCaps3 = 0;
        public readonly uint dwCaps4 = 0;
        public readonly uint dwReserved2_2 = 0;

        //
        public string sourceFileName; //file name + extension
        public string sourceFile; //file path
        public byte[] sourceFileData; //file data
        public int[,] mipImageResolutions; //calculated mip resolutions [Pixel Value, Width or Height (0 or 1)]
        public byte[] ddsTextureData;
        public int dwMipMapCount_incremented; //total amount of mip maps in the dds file (plus 1)


        //--------------------------------------VARIABLES END--------------------------------------

        /// <summary>
        /// Manually builds a byte array of a dds header
        /// </summary>
        /// <returns></returns>
        public byte[] Build_DDSHeader_ByteArray()
        {
            //allocate our header byte array
            byte[] ddsHeader = new byte[128];

            //begin assigning our values main chunks
            ddsHeader = Utilities.AllocateBytes(dword, ddsHeader, 0);
            ddsHeader = Utilities.AllocateBytes(BitConverter.GetBytes(dwSize), ddsHeader, 4);
            ddsHeader = Utilities.AllocateBytes(BitConverter.GetBytes(dwFlags), ddsHeader, 8);
            ddsHeader = Utilities.AllocateBytes(BitConverter.GetBytes(dwHeight), ddsHeader, 12);
            ddsHeader = Utilities.AllocateBytes(BitConverter.GetBytes(dwWidth), ddsHeader, 16);
            ddsHeader = Utilities.AllocateBytes(BitConverter.GetBytes(dwPitchOrLinearSize), ddsHeader, 20);
            ddsHeader = Utilities.AllocateBytes(BitConverter.GetBytes(dwDepth), ddsHeader, 24);
            ddsHeader = Utilities.AllocateBytes(BitConverter.GetBytes(dwMipMapCount), ddsHeader, 28);

            //write a bunch of unused chunks
            ddsHeader = Utilities.AllocateBytes(BitConverter.GetBytes(dwReserved1), ddsHeader, 32);
            ddsHeader = Utilities.AllocateBytes(BitConverter.GetBytes(dwReserved2), ddsHeader, 36);
            ddsHeader = Utilities.AllocateBytes(BitConverter.GetBytes(dwReserved3), ddsHeader, 40);
            ddsHeader = Utilities.AllocateBytes(BitConverter.GetBytes(dwReserved4), ddsHeader, 44);
            ddsHeader = Utilities.AllocateBytes(BitConverter.GetBytes(dwReserved5), ddsHeader, 48);
            ddsHeader = Utilities.AllocateBytes(BitConverter.GetBytes(dwReserved6), ddsHeader, 52);
            ddsHeader = Utilities.AllocateBytes(BitConverter.GetBytes(dwReserved7), ddsHeader, 56);
            ddsHeader = Utilities.AllocateBytes(BitConverter.GetBytes(dwReserved8), ddsHeader, 60);
            ddsHeader = Utilities.AllocateBytes(BitConverter.GetBytes(dwReserved9), ddsHeader, 64);
            ddsHeader = Utilities.AllocateBytes(BitConverter.GetBytes(dwReserved10), ddsHeader, 68);
            ddsHeader = Utilities.AllocateBytes(BitConverter.GetBytes(dwReserved11), ddsHeader, 72);

            //write dds pixel format struct
            ddsHeader = Utilities.AllocateBytes(BitConverter.GetBytes(ddspf_size), ddsHeader, 76);
            ddsHeader = Utilities.AllocateBytes(BitConverter.GetBytes(ddspf_flags), ddsHeader, 80);
            ddsHeader = Utilities.AllocateBytes(ddspf_dwFourCC, ddsHeader, 84);
            ddsHeader = Utilities.AllocateBytes(BitConverter.GetBytes(ddspf_rgbBitCount), ddsHeader, 88);
            ddsHeader = Utilities.AllocateBytes(BitConverter.GetBytes(ddspf_RBitMask), ddsHeader, 92);
            ddsHeader = Utilities.AllocateBytes(BitConverter.GetBytes(ddspf_GBitMask), ddsHeader, 96);
            ddsHeader = Utilities.AllocateBytes(BitConverter.GetBytes(ddspf_BBitMask), ddsHeader, 100);
            ddsHeader = Utilities.AllocateBytes(BitConverter.GetBytes(ddspf_ABitMask), ddsHeader, 104);

            //write leftover main data
            ddsHeader = Utilities.AllocateBytes(BitConverter.GetBytes(dwCaps), ddsHeader, 108);
            ddsHeader = Utilities.AllocateBytes(BitConverter.GetBytes(dwCaps2), ddsHeader, 112);
            ddsHeader = Utilities.AllocateBytes(BitConverter.GetBytes(dwCaps3), ddsHeader, 116);
            ddsHeader = Utilities.AllocateBytes(BitConverter.GetBytes(dwCaps4), ddsHeader, 120);
            ddsHeader = Utilities.AllocateBytes(BitConverter.GetBytes(dwReserved2_2), ddsHeader, 124);

            //return the result
            return ddsHeader;
        }

        public bool DDS_CompressionBool()
        {
            return ddspf_dwFourCC.Equals("DXT1");
        }

        public void Read_DDS_File(string sourceFile, string sourceFileName, bool readHeaderOnly)
        {
            /*
             * NOTE TO SELF
             * DDS --> D3DTX EXTRACTION, THE BYTES ARE NOT FULLY 1:1 WHEN THERE IS A CONVERSION (off by 8 bytes)
             * MABYE TRY TO CHANGE THE TEXTURE DATA BYTE SIZE IN THE D3DTX HEADER AND SEE IF THAT CHANGES ANYTHING?
            */

            //read the source texture file into a byte array
            sourceFileData = File.ReadAllBytes(sourceFile);
            this.sourceFileName = sourceFileName;
            this.sourceFile = sourceFile;

            //write the result to the console for viewing
            Utilities.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White); //just a display thing, not needed
            Console.WriteLine("Total Source Texture Byte Size = {0}", sourceFileData.Length);

            //which byte offset we are on for the source texture (will be changed as we go through the file)
            int bytePointerPosition = 0;

            //--------------------------1 DDS IMAGE HEIGHT--------------------------
            //skip the dds dword for now because we just want the size of the header
            bytePointerPosition = 12;

            //allocate 4 byte array (int32)
            byte[] source_dwHeight = Utilities.AllocateBytes(4, sourceFileData, bytePointerPosition);

            //parse the byte array to int32
            int parsed_dwHeight = BitConverter.ToInt32(source_dwHeight);

            //write the result to the console for viewing
            Console.WriteLine("DDS dwHeight = {0}", parsed_dwHeight.ToString());

            //assign the parsed value
            dwHeight = (uint)parsed_dwHeight;
            //--------------------------2 DDS IMAGE WIDTH--------------------------
            //skip the dds dword for now because we just want the size of the header
            bytePointerPosition = 16;

            //allocate 4 byte array (int32)
            byte[] source_dwWidth = Utilities.AllocateBytes(4, sourceFileData, bytePointerPosition);

            //parse the byte array to int32
            int parsed_dwWidth = BitConverter.ToInt32(source_dwWidth);

            //write the result to the console for viewing
            Console.WriteLine("DDS dwWidth = {0}", parsed_dwWidth.ToString());

            //assign the parsed value
            dwWidth = (uint)parsed_dwWidth;
            //--------------------------3 DDS MIP MAP COUNT--------------------------
            //skip ahead to the mip map count
            bytePointerPosition = 28;

            //allocate 4 byte array (int32)
            byte[] source_dwMipMapCount = Utilities.AllocateBytes(4, sourceFileData, bytePointerPosition);

            //parse the byte array to int32
            int parsed_dwMipMapCount = BitConverter.ToInt32(source_dwMipMapCount);

            //write the result to the console for viewing
            Console.WriteLine("DDS dwMipMapCount = {0}", parsed_dwMipMapCount.ToString());

            //assign the parsed value
            dwMipMapCount = (uint)parsed_dwMipMapCount;
            dwMipMapCount_incremented = (int)dwMipMapCount + 1;
            //--------------------------4 DDS COMPRESSION TYPE--------------------------
            //note to self - be sure to get the pixel format header size as well later
            //skip ahead to the mip map count
            bytePointerPosition = 84;

            //allocate 4 byte array (int32)
            byte[] source_ddspf_dwFourCC = Utilities.AllocateBytes(4, sourceFileData, bytePointerPosition);

            //parse the byte array to int32
            string parsed_ddspf_dwFourCC = Encoding.ASCII.GetString(source_ddspf_dwFourCC);

            //write the result to the console for viewing
            Console.WriteLine("DDS ddspf_dwFourCC = {0}", parsed_ddspf_dwFourCC.ToString());

            //assign the parsed value
            ddspf_dwFourCC = parsed_ddspf_dwFourCC;

            //if we are reading a whole dds file
            if(readHeaderOnly == false)
            {
                //--------------------------EXTRACT DDS TEXTURE DATA--------------------------
                //calculate dds header length (we add 4 because we skipped the 4 bytes which contain the dword, it isn't necessary to parse this data)
                int ddsHeaderLength = 4 + (int)dwSize;

                //calculate the length of just the dds texture data
                int ddsTextureDataLength = sourceFileData.Length - ddsHeaderLength;

                //allocate a byte array of dds texture length
                ddsTextureData = new byte[ddsTextureDataLength];

                //copy the data from the source byte array past the header (so we are only getting texture data)
                Array.Copy(sourceFileData, ddsHeaderLength, ddsTextureData, 0, ddsTextureData.Length);

                //--------------------------CALCULATE MIP MAP RESOLUTIONS--------------------------
                //because I suck at math, we will generate our mip map resolutions using the same method we did in d3dtx to dds (can't figure out how to calculate them in reverse properly)
                mipImageResolutions = new int[dwMipMapCount_incremented, 2];

                //get our mip image dimensions (have to multiply by 2 as the mip calculations will be off by half)
                int mipImageWidth = (int)dwWidth * 2;
                int mipImageHeight = (int)dwHeight * 2;

                //add the resolutions in reverse
                for (int i = (int)dwMipMapCount; i > 0; i--)
                {
                    //divide the resolutions by 2
                    mipImageWidth /= 2;
                    mipImageHeight /= 2;

                    //assign the resolutions
                    mipImageResolutions[i, 0] = mipImageWidth;
                    mipImageResolutions[i, 1] = mipImageHeight;
                }
            }
        }
    }
}
