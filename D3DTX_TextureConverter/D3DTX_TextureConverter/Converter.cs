using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace D3DTX_TextureConverter
{
    public static class Converter
    {
        //dds image file extension
        public static string ddsExtension = ".dds";

        //telltale d3dtx texture file extension
        public static string d3dtxExtension = ".d3dtx";

        /*
        /// <summary>
        /// Gets the DWORD value from the source file
        /// </summary>
        /// <param name="sourceByteFile"></param>
        /// <param name="bytePointerPosition"></param>
        /// <returns></returns>
        public static string D3DTX_GetDWORD(byte[] sourceByteFile, int bytePointerPosition)
        {
            //allocate 4 byte array (string)
            byte[] source_dword = Utilities.AllocateBytes(4, sourceByteFile, bytePointerPosition);

            //parse the byte array to string
            string parsed_dword = Encoding.ASCII.GetString(source_dword);

            //write the result to the console for viewing
            Utilities.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White); 
            Console.WriteLine("D3DTX DWORD = {0}", parsed_dword);

            return parsed_dword;
        }

        /// <summary>
        /// Gets the File Version? value from the source file.
        /// </summary>
        /// <param name="parsed_dword"></param>
        /// <param name="sourceByteFile"></param>
        /// <param name="bytePointerPosition"></param>
        /// <returns></returns>
        public static int D3DTX_GetFileVersion(string parsed_dword, byte[] sourceByteFile, int bytePointerPosition)
        {
            //allocate 4 byte array (int32)
            byte[] source_fileVersion = Utilities.AllocateBytes(4, sourceByteFile, bytePointerPosition);

            //parse the byte array to int32
            int parsed_fileVersion = BitConverter.ToInt32(source_fileVersion);

            //write the result to the console for viewing
            Utilities.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White); 
            Console.WriteLine("D3DTX File Version = {0}", parsed_fileVersion.ToString());

            return parsed_fileVersion;
        }

        /// <summary>
        /// Gets the Texture Byte Size value from the source file
        /// </summary>
        /// <param name="parsed_dword"></param>
        /// <param name="sourceByteFile"></param>
        /// <param name="bytePointerPosition"></param>
        /// <returns></returns>
        public static int D3DTX_GetTextureByteSize(string parsed_dword, byte[] sourceByteFile, int bytePointerPosition)
        {
            //allocate 4 byte array (int32)
            byte[] source_fileSize = Utilities.AllocateBytes(4, sourceByteFile, bytePointerPosition);

            //parse the byte array to int32
            int parsed_textureDataByteSize = BitConverter.ToInt32(source_fileSize);

            //write the result to the console for viewing
            Utilities.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White); 
            Console.WriteLine("D3DTX Texture Byte Size = {0}", parsed_textureDataByteSize.ToString());

            return parsed_textureDataByteSize;
        }

        /// <summary>
        /// Calculates the length of the D3DTX Header
        /// </summary>
        /// <param name="parsed_dword"></param>
        /// <param name="parsed_textureDataByteSize"></param>
        /// <param name="sourceByteFileSize"></param>
        /// <returns></returns>
        public static int D3DTX_CalculateHeaderLength(string parsed_dword, int parsed_textureDataByteSize, int sourceByteFileSize)
        {
            //calculating header length, parsed texture byte size - source byte size
            int headerLength = sourceByteFileSize - parsed_textureDataByteSize;

            //write the result to the console for viewing
            Utilities.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White); 
            Console.WriteLine("D3DTX Header Byte Size = {0}", headerLength.ToString());

            return headerLength;
        }

        /// <summary>
        /// Gets the TelltaleD3DTXClassNames data value from the source file
        /// </summary>
        /// <param name="parsed_dword"></param>
        /// <param name="sourceByteFile"></param>
        /// <param name="bytePointerPosition"></param>
        /// <returns></returns>
        public static byte[] D3DTX_GetTelltaleD3DTXClassNamesData(string parsed_dword, byte[] sourceByteFile, int bytePointerPosition)
        {
            int classNamesLength = 84; //length of the class names (they are CRC'd which is why it looks garbled)

            if (parsed_dword.Equals("5VSM"))
                classNamesLength = 72;

            byte[] telltaleD3DTXClassNames = new byte[classNamesLength];

            for (int i = 0; i < classNamesLength; i++)
            {
                telltaleD3DTXClassNames[i] = sourceByteFile[bytePointerPosition + i];
            }

            return telltaleD3DTXClassNames;
        }

        /// <summary>
        /// Gets the Length of the texture file name
        /// </summary>
        /// <param name="parsed_dword"></param>
        /// <param name="sourceByteFile"></param>
        /// <param name="bytePointerPosition"></param>
        /// <returns></returns>
        public static int D3DTX_GetTextureByteSize2(string parsed_dword, byte[] sourceByteFile, int bytePointerPosition)
        {
            //allocate 4 byte array (int32)
            byte[] source_textureByteSize2 = Utilities.AllocateBytes(4, sourceByteFile, bytePointerPosition);

            //parse the byte array to int32
            int parsed_textureByteSize2 = BitConverter.ToInt32(source_textureByteSize2);

            //write the result to the console for viewing
            Utilities.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White); 
            Console.WriteLine("D3DTX Parsed Texture Byte Size = {0}", parsed_textureByteSize2.ToString());

            return parsed_textureByteSize2;
        }

        /// <summary>
        /// Gets the Length of the texture file name
        /// </summary>
        /// <param name="parsed_dword"></param>
        /// <param name="sourceByteFile"></param>
        /// <param name="bytePointerPosition"></param>
        /// <returns></returns>
        public static int D3DTX_GetLengthOfTextureFileNameFromHeader(string parsed_dword, byte[] sourceByteFile, int bytePointerPosition)
        {
            //allocate 4 byte array (int32)
            byte[] source_fileNameLength = Utilities.AllocateBytes(4, sourceByteFile, bytePointerPosition);

            //parse the byte array to int32
            int parsed_fileNameHeaderLength = BitConverter.ToInt32(source_fileNameLength);

            //write the result to the console for viewing
            Utilities.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White); 
            Console.WriteLine("D3DTX File Name Header Length = {0}", parsed_fileNameHeaderLength.ToString());

            return parsed_fileNameHeaderLength;
        }

        /// <summary>
        /// Gets the Texture File Name value from the source file
        /// </summary>
        /// <param name="sourceByteFile"></param>
        /// <param name="bytePointerPosition"></param>
        /// <returns></returns>
        public static string D3DTX_GetTextureFileNameFromHeader(int fileNameHeaderLength, byte[] sourceByteFile, int bytePointerPosition)
        {
            //allocate a byte array to get the bytes
            byte[] parsedName = Utilities.AllocateBytes(fileNameHeaderLength, sourceByteFile, bytePointerPosition);

            //parse it to a string
            string parsed_fileNameFromHeader = Encoding.ASCII.GetString(parsedName);

            //show it to the user
            Utilities.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White); 
            Console.WriteLine("D3DTX File Name in Header = '{0}'", parsed_fileNameFromHeader);

            return parsed_fileNameFromHeader;
        }

        public static int D3DTX_GetMipMapCount(byte[] sourceByteFile, int bytePointerPosition)
        {
            //allocate 4 byte array (int32)
            byte[] source_imageMipMapCount = Utilities.AllocateBytes(4, sourceByteFile, bytePointerPosition);

            //parse the byte array to int32
            int parsed_imageMipMapCount = BitConverter.ToInt32(source_imageMipMapCount);
            int parsed_imageMipMapCount_decremented = parsed_imageMipMapCount - 1;

            //write the result to the console for viewing
            Utilities.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White); 
            Console.WriteLine("D3DTX Mip Map Count = {0} ({1})", parsed_imageMipMapCount.ToString(), parsed_imageMipMapCount_decremented.ToString());

            return parsed_imageMipMapCount;
        }

        public static int D3DTX_GetImageWidth(byte[] sourceByteFile, int bytePointerPosition)
        {
            //allocate 4 byte array (int32)
            byte[] source_imageWidth = Utilities.AllocateBytes(4, sourceByteFile, bytePointerPosition);

            //parse the byte array to int32
            int parsed_imageWidth = BitConverter.ToInt32(source_imageWidth);

            //write the result to the console for viewing
            Utilities.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White); 
            Console.WriteLine("D3DTX Image Width = {0}", parsed_imageWidth.ToString());

            return parsed_imageWidth;
        }

        public static int D3DTX_GetImageHeight(byte[] sourceByteFile, int bytePointerPosition)
        {
            //allocate 4 byte array (int32)
            byte[] source_imageHeight = Utilities.AllocateBytes(4, sourceByteFile, bytePointerPosition);

            //parse the byte array to int32
            int parsed_imageHeight = BitConverter.ToInt32(source_imageHeight);

            //write the result to the console for viewing
            Utilities.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White); 
            Console.WriteLine("D3DTX Image Height = {0}", parsed_imageHeight.ToString());

            return parsed_imageHeight;
        }

        public static int D3DTX_GetDXTType(byte[] sourceByteFile, int bytePointerPosition)
        {
            //allocate 4 byte array (int32)
            byte[] source_dxtType = Utilities.AllocateBytes(4, sourceByteFile, bytePointerPosition);

            //parse the byte array to int32
            int parsed_dxtType = BitConverter.ToInt32(source_dxtType);

            //write the result to the console for viewing
            Utilities.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White); 
            Console.WriteLine("D3DTX DXT TYPE = {0}", parsed_dxtType.ToString());

            return parsed_dxtType;
        }
        */

        public static string D3DTX_GetDXTType_ForDDS(string parsed_dword, int parsed_dxtType)
        {
            /*
             * DXT1 - DXGI_FORMAT_BC1_UNORM / D3DFMT_DXT1
             * DXT2 - D3DFMT_DXT2
             * DXT3 - DXGI_FORMAT_BC2_UNORM / D3DFMT_DXT3
             * DXT4 - D3DFMT_DXT4
             * DXT5 - DXGI_FORMAT_BC3_UNORM / D3DFMT_DXT5
             * ATI1
             * ATI2 - DXGI_FORMAT_BC5_UNORM
             * BC5U - 
             * BC5S - DXGI_FORMAT_BC5_SNORM
             * BC4U - DXGI_FORMAT_BC4_UNORM
             * BC4S - DXGI_FORMAT_BC4_SNORM
             * RGBG - DXGI_FORMAT_R8G8_B8G8_UNORM / D3DFMT_R8G8_B8G8
             * GRGB - DXGI_FORMAT_G8R8_G8B8_UNORM / D3DFMT_G8R8_G8B8
             * UYVY - D3DFMT_UYVY
             * YUY2 - D3DFMT_YUY2
             * DX10 - Any DXGI format
             * 36 - DXGI_FORMAT_R16G16B16A16_UNORM / D3DFMT_A16B16G16R16
             * 110 - DXGI_FORMAT_R16G16B16A16_SNORM / D3DFMT_Q16W16V16U16
             * 111 - DXGI_FORMAT_R16_FLOAT / D3DFMT_R16F
             * 112 - DXGI_FORMAT_R16G16_FLOAT / D3DFMT_G16R16F
             * 113 - DXGI_FORMAT_R16G16B16A16_FLOAT / D3DFMT_A16B16G16R16F
             * 114 - DXGI_FORMAT_R32_FLOAT / D3DFMT_R32F
             * 115 - DXGI_FORMAT_R32G32_FLOAT / D3DFMT_G32R32F
             * 116 - DXGI_FORMAT_R32G32B32A32_FLOAT / D3DFMT_A32B32G32R32F
             * 117 - D3DFMT_CxV8U8
            */

            string result = "DXT1";

            //this section needs some reworking, still can't track down exactly what the compression types are, parsed_compressionType and parsed_dxtType are close
            //SET DDS COMPRESSION TYPES
            if (parsed_dxtType == 66)
            {
                //DXT5 COMPRESSION
                result = "DXT5";
            }
            else if (parsed_dxtType == 68)
            {
                //DDSPF_BC5_UNORM COMPRESSION
                result = "BC5U";
            }
            else if (parsed_dxtType == 69)
            {
                //DDSPF_BC4_UNORM COMPRESSION
                result = "BC4U";
            }
            else if (parsed_dxtType == 646)
            {
                //DDSPF_BC4_UNORM COMPRESSION
                result = "BC5S";
            }

            //Utilities.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Yellow); 
            //Console.WriteLine("Selecting '{0}' DDS Compression.", result);

            return result;
        }

        /*
        public static int DDS_GetHeaderLength(byte[] sourceByteFile, int bytePointerPosition)
        {
            //allocate 4 byte array (int32)
            byte[] texture_source_headerLength = Utilities.AllocateBytes(4, sourceByteFile, bytePointerPosition);

            //parse the byte array to int32
            int texture_parsed_headerLength = BitConverter.ToInt32(texture_source_headerLength);

            //write the result to the console for viewing
            Console.WriteLine("DDS Header Length = {0}", texture_parsed_headerLength.ToString());

            return texture_parsed_headerLength;
        }

        public static int DDS_GetImageHeight(byte[] sourceByteFile, int bytePointerPosition)
        {
            //allocate 4 byte array (int32)
            byte[] texture_source_imageHeight = Utilities.AllocateBytes(4, sourceByteFile, bytePointerPosition);

            //parse the byte array to int32
            int texture_parsed_imageHeight = BitConverter.ToInt32(texture_source_imageHeight);

            //write the result to the console for viewing
            Console.WriteLine("DDS Image Height = {0}", texture_parsed_imageHeight.ToString());

            return texture_parsed_imageHeight;
        }

        public static int DDS_GetImageWidth(byte[] sourceByteFile, int bytePointerPosition)
        {
            //allocate 4 byte array (int32)
            byte[] texture_source_imageWidth = Utilities.AllocateBytes(4, sourceByteFile, bytePointerPosition);

            //parse the byte array to int32
            int texture_parsed_imageWidth = BitConverter.ToInt32(texture_source_imageWidth);

            //write the result to the console for viewing
            Console.WriteLine("DDS Image Width = {0}", texture_parsed_imageWidth.ToString());

            return texture_parsed_imageWidth;
        }

        public static int DDS_GetMipMapCount(byte[] sourceByteFile, int bytePointerPosition)
        {
            //allocate 4 byte array (int32)
            byte[] texture_source_mipMapCount = Utilities.AllocateBytes(4, sourceByteFile, bytePointerPosition);

            //parse the byte array to int32
            int texture_parsed_mipMapCount = BitConverter.ToInt32(texture_source_mipMapCount);

            //write the result to the console for viewing
            Console.WriteLine("DDS Header Length = {0}", texture_parsed_mipMapCount.ToString());

            return texture_parsed_mipMapCount;
        }

        public static string DDS_CompressionType(byte[] sourceByteFile, int bytePointerPosition)
        {
            //allocate 4 byte array (int32)
            byte[] texture_source_compressionType = Utilities.AllocateBytes(4, sourceByteFile, bytePointerPosition);

            //parse the byte array to int32
            string texture_parsed_compressionType = Encoding.ASCII.GetString(texture_source_compressionType);

            //write the result to the console for viewing
            Console.WriteLine("DDS Compression Type = {0}", texture_parsed_compressionType.ToString());

            return texture_parsed_compressionType;
        }
        */
    }
}
