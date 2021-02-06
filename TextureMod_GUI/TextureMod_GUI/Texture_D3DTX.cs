using System;
using System.Collections.Generic;
using System.Text;

namespace TextureMod_GUI
{
    public class Texture_D3DTX
    {
        public readonly string dword = "6VSM";

        //Compression Type [4 bytes]
        public uint compressionType = 0;

        //[4 bytes]
        public uint unknown1 = 0;

        //Texture Byte Size [4 bytes]
        public uint fileSize = 0;

        //[4 bytes]
        public uint unknown2 = 0;

        //File Name + Extension [varied bytes]
        public string fileName = "";

        //Texture Width Pixels [4 bytes]
        public uint width = 0;

        //Texture Height Pixels [4 bytes]
        public uint height = 0;

        /// <summary>
        /// Manually builds a byte array of a dds header
        /// </summary>
        /// <returns></returns>
        public byte[] Build_D3DTXHeader_ByteArray()
        {
            //allocate our header byte array
            byte[] d3dtxHeader = new byte[128];

            //begin assigning our values
            //ddsHeader = AllocateBytes_String(dword, ddsHeader, 0);
            //ddsHeader = AllocateBytes(BitConverter.GetBytes(ddsPixelFormat_rgbBitCount), ddsHeader, 88);

            //return the result
            return d3dtxHeader;
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
