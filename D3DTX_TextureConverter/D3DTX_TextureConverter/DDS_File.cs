using System;
using System.Collections.Generic;
using System.Text;

namespace D3DTX_TextureConverter
{
    //NOTE TO SELF: IF THERE IS AN EASIER WAY OF DOING THIS, DO IT SINCE THIS IS A RATHER LAZY WAY
    //DDS Docs - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-header
    //DDS PIXEL FORMAT - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-pixelformat
    //DDS File Layout https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-file-layout-for-textures

    /// <summary>
    /// Main class for generating a dds byte header
    /// </summary>
    public class DDS_File
    {
        //main dword [4 bytes]
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
            ddsHeader = AllocateBytes_String(dword, ddsHeader, 0);
            ddsHeader = AllocateBytes(BitConverter.GetBytes(dwSize), ddsHeader, 4);
            ddsHeader = AllocateBytes(BitConverter.GetBytes(dwFlags), ddsHeader, 8);
            ddsHeader = AllocateBytes(BitConverter.GetBytes(dwHeight), ddsHeader, 12);
            ddsHeader = AllocateBytes(BitConverter.GetBytes(dwWidth), ddsHeader, 16);
            ddsHeader = AllocateBytes(BitConverter.GetBytes(dwPitchOrLinearSize), ddsHeader, 20);
            ddsHeader = AllocateBytes(BitConverter.GetBytes(dwDepth), ddsHeader, 24);
            ddsHeader = AllocateBytes(BitConverter.GetBytes(dwMipMapCount), ddsHeader, 28);

            //write a bunch of unused chunks
            ddsHeader = AllocateBytes(BitConverter.GetBytes(dwReserved1), ddsHeader, 32);
            ddsHeader = AllocateBytes(BitConverter.GetBytes(dwReserved2), ddsHeader, 36);
            ddsHeader = AllocateBytes(BitConverter.GetBytes(dwReserved3), ddsHeader, 40);
            ddsHeader = AllocateBytes(BitConverter.GetBytes(dwReserved4), ddsHeader, 44);
            ddsHeader = AllocateBytes(BitConverter.GetBytes(dwReserved5), ddsHeader, 48);
            ddsHeader = AllocateBytes(BitConverter.GetBytes(dwReserved6), ddsHeader, 52);
            ddsHeader = AllocateBytes(BitConverter.GetBytes(dwReserved7), ddsHeader, 56);
            ddsHeader = AllocateBytes(BitConverter.GetBytes(dwReserved8), ddsHeader, 60);
            ddsHeader = AllocateBytes(BitConverter.GetBytes(dwReserved9), ddsHeader, 64);
            ddsHeader = AllocateBytes(BitConverter.GetBytes(dwReserved10), ddsHeader, 68);
            ddsHeader = AllocateBytes(BitConverter.GetBytes(dwReserved11), ddsHeader, 72);

            //write dds pixel format struct
            ddsHeader = AllocateBytes(BitConverter.GetBytes(ddspf_size), ddsHeader, 76);
            ddsHeader = AllocateBytes(BitConverter.GetBytes(ddspf_flags), ddsHeader, 80);
            ddsHeader = AllocateBytes_String(ddspf_dwFourCC, ddsHeader, 84);
            ddsHeader = AllocateBytes(BitConverter.GetBytes(ddspf_rgbBitCount), ddsHeader, 88);
            ddsHeader = AllocateBytes(BitConverter.GetBytes(ddspf_RBitMask), ddsHeader, 92);
            ddsHeader = AllocateBytes(BitConverter.GetBytes(ddspf_GBitMask), ddsHeader, 96);
            ddsHeader = AllocateBytes(BitConverter.GetBytes(ddspf_BBitMask), ddsHeader, 100);
            ddsHeader = AllocateBytes(BitConverter.GetBytes(ddspf_ABitMask), ddsHeader, 104);

            //write leftover main data
            ddsHeader = AllocateBytes(BitConverter.GetBytes(dwCaps), ddsHeader, 108);
            ddsHeader = AllocateBytes(BitConverter.GetBytes(dwCaps2), ddsHeader, 112);
            ddsHeader = AllocateBytes(BitConverter.GetBytes(dwCaps3), ddsHeader, 116);
            ddsHeader = AllocateBytes(BitConverter.GetBytes(dwCaps4), ddsHeader, 120);
            ddsHeader = AllocateBytes(BitConverter.GetBytes(dwReserved2_2), ddsHeader, 124);

            //return the result
            return ddsHeader;
        }

        /// <summary>
        /// Allocates 4 fixed bytes to the destination bytes array with the offset
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="destinationBytes"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public byte[] AllocateBytes(byte[] bytes, byte[] destinationBytes, int offset)
        {
            //for the length of the byte array, assign each byte value to the destination byte array values with the given offset
            for (int i = 0; i < bytes.Length; i++)
            {
                destinationBytes[offset + i] = bytes[i];
            }

            //return the result
            return destinationBytes;
        }

        public byte[] AllocateBytes_String(string stringValue, byte[] destinationBytes, int offset)
        {
            //for the length of the byte array, assign each byte value to the destination byte array values with the given offset
            for (int i = 0; i < stringValue.Length; i++)
            {
                destinationBytes[offset + i] = Convert.ToByte(stringValue[i]);
            }

            //return the result
            return destinationBytes;
        }
    }
}
