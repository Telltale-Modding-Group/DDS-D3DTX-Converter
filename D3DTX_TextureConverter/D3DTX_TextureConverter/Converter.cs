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
    }
}
