using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DirectXTexNet;
using D3DTX_TextureConverter.Utilities;
using D3DTX_TextureConverter.DirectX;

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

namespace D3DTX_TextureConverter
{
    /// <summary>
    /// Main class for generating a dds byte header
    /// </summary>
    public class DDS_File
    {
        //dds image file extension
        public static string ddsExtension = ".dds";

        //main ddsPrefix (with space) [4 bytes]
        public readonly string ddsPrefix = "DDS ";

        public string sourceFileName; //file name + extension
        public string sourceFile; //file path
        public byte[] sourceFileData; //file data
        public uint[,] mipImageResolutions; //calculated mip resolutions [Pixel Value, Width or Height (0 or 1)]
        public byte[] ddsTextureData;

        public DDS_HEADER header;

        public DDS_File(D3DTX_File d3dtx)
        {
            header = DDS_Functions.GetPresetHeader();
            header.dwWidth = (uint)d3dtx.T3TextureBase.mWidth;
            header.dwHeight = (uint)d3dtx.T3TextureBase.mHeight;
            header.dwMipMapCount = (uint)d3dtx.T3TextureBase.mNumMipLevelsAllocated;
            header.dwDepth = (uint)d3dtx.T3TextureBase.mDepth;
            
            switch(d3dtx.T3TextureBase.mSurfaceFormat)
            {
                default:
                    header.ddspf.dwFourCC = uint.Parse("DXT1");
                    break;
                case Telltale.T3SurfaceFormat.eSurface_DXT1:
                    header.ddspf.dwFourCC = uint.Parse("DXT1");
                    break;
                case Telltale.T3SurfaceFormat.eSurface_DXT3:
                    header.ddspf.dwFourCC = uint.Parse("DXT3");
                    break;
                case Telltale.T3SurfaceFormat.eSurface_DXT5:
                    header.ddspf.dwFourCC = uint.Parse("DXT5");
                    break;
                case Telltale.T3SurfaceFormat.eSurface_BC5:
                    header.ddspf.dwFourCC = uint.Parse("BC5U");
                    break;
                case Telltale.T3SurfaceFormat.eSurface_BC4:
                    header.ddspf.dwFourCC = uint.Parse("BC4U");
                    break;
                case Telltale.T3SurfaceFormat.eSurface_RGBA16:
                    header.ddspf.dwFourCC = uint.Parse("36");
                    break;
                case Telltale.T3SurfaceFormat.eSurface_RGBA16F:
                    header.ddspf.dwFourCC = uint.Parse("110");
                    break;
            }
        }

        /// <summary>
        /// Manually builds a byte array of a dds header
        /// </summary>
        /// <returns></returns>
        public byte[] Build_DDSHeader_ByteArray()
        {
            //allocate our header byte array
            byte[] ddsHeader = new byte[128];

            //return the result
            return ddsHeader;
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

            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White); 
            Console.WriteLine("Total Source Texture Byte Size = {0}", sourceFileData.Length);

            //which byte offset we are on for the source texture (will be changed as we go through the file)
            uint bytePointerPosition = 4; //skip past the 'DDS '
            byte[] headerBytes = ByteFunctions.AllocateBytes(124, sourceFileData, bytePointerPosition);

            //this will automatically read all of the byte data in the header
            header = DDS_Functions.GetHeaderFromBytes(headerBytes);

            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("DDS Height = {0}", header.dwHeight);
            Console.WriteLine("DDS Width = {0}", header.dwWidth);
            Console.WriteLine("DDS Compression = {0}", header.ddspf.dwFourCC);

            //if we are not reading
            if(!readHeaderOnly)
            {
                //--------------------------EXTRACT DDS TEXTURE DATA--------------------------
                //calculate dds header length (we add 4 because we skipped the 4 bytes which contain the ddsPrefix, it isn't necessary to parse this data)
                uint ddsHeaderLength = 4 + header.dwSize;

                //calculate the length of just the dds texture data
                uint ddsTextureDataLength = (uint)sourceFileData.Length - ddsHeaderLength;

                //allocate a byte array of dds texture length
                ddsTextureData = new byte[ddsTextureDataLength];

                //copy the data from the source byte array past the header (so we are only getting texture data)
                Array.Copy(sourceFileData, ddsHeaderLength, ddsTextureData, 0, ddsTextureData.Length);

                //--------------------------CALCULATE MIP MAP RESOLUTIONS--------------------------
                //because I suck at math, we will generate our mip map resolutions using the same method we did in d3dtx to dds (can't figure out how to calculate them in reverse properly)
                mipImageResolutions = new uint[header.dwMipMapCount + 1, 2];

                //get our mip image dimensions (have to multiply by 2 as the mip calculations will be off by half)
                uint mipImageWidth = header.dwWidth * 2;
                uint mipImageHeight = header.dwHeight * 2;

                //add the resolutions in reverse
                for (uint i = header.dwMipMapCount; i > 0; i--)
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
