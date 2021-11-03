using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DirectXTexNet;
using D3DTX_TextureConverter.Utilities;
using D3DTX_TextureConverter.DirectX;
using D3DTX_TextureConverter.Telltale;

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

namespace D3DTX_TextureConverter.Main
{
    /// <summary>
    /// Main class for generating a dds byte header
    /// </summary>
    public class DDS_Master
    {
        //dds image file extension
        public static string ddsExtension = ".dds";

        //main ddsPrefix (with space) [4 bytes]
        public readonly string ddsPrefix = "DDS ";

        public string sourceFileName; //file name + extension
        public string sourceFile; //file path
        public byte[] sourceFileData; //file data
        public List<byte[]> textureData;
        public uint[,] mipMapResolutions;

        public DDS_HEADER header;

        /// <summary>
        /// A struct used when matching a DDS with a D3DTX.
        /// </summary>
        public struct DDS_Matching_Options
        {
            public bool MatchResolution { get; set; }
            public bool MatchCompression { get; set; }
            public bool GenerateMipMaps { get; set; }
            public bool MatchMipMapCount { get; set; }
        }

        /// <summary>
        /// Reads a DDS file from disk. (Can also read just the header only)
        /// </summary>
        /// <param name="ddsFilePath"></param>
        /// <param name="headerOnly"></param>
        public DDS_Master(string ddsFilePath, bool headerOnly)
        {
            //read the source texture file into a byte array
            sourceFileData = File.ReadAllBytes(ddsFilePath);
            sourceFileName = Path.GetFileName(ddsFilePath);
            sourceFile = ddsFilePath;

            //read the DDS file
            GetData(sourceFileData, headerOnly);
        }

        /// <summary>
        /// Reads a DDS file from a byte array. (Can also just read the header only)
        /// </summary>
        /// <param name="data"></param>
        /// <param name="headerOnly"></param>
        public DDS_Master(byte[] data, bool headerOnly)
        {
            //get the byte data
            sourceFileData = data;

            //read the DDS file
            GetData(sourceFileData, headerOnly);
        }

        /// <summary>
        /// Create a DDS file from a D3DTX
        /// </summary>
        /// <param name="d3dtx"></param>
        public DDS_Master(D3DTX_Master d3dtx)
        {
            header = DDS_Functions.GetPresetHeader();
            T3SurfaceFormat surfaceFormat = T3SurfaceFormat.eSurface_DXT1;

            if (d3dtx.d3dtx4 != null)
            {
                header.dwWidth = d3dtx.d3dtx4.mWidth;
                header.dwHeight = d3dtx.d3dtx4.mHeight;
                header.dwMipMapCount = d3dtx.d3dtx4.mNumMipLevels;
                surfaceFormat = d3dtx.d3dtx4.mSurfaceFormat;
            }
            else if (d3dtx.d3dtx5 != null)
            {
                header.dwWidth = d3dtx.d3dtx5.mWidth;
                header.dwHeight = d3dtx.d3dtx5.mHeight;
                header.dwMipMapCount = d3dtx.d3dtx5.mNumMipLevels;
                surfaceFormat = d3dtx.d3dtx5.mSurfaceFormat;
            }
            else if (d3dtx.d3dtx6 != null)
            {
                header.dwWidth = d3dtx.d3dtx6.mWidth;
                header.dwHeight = d3dtx.d3dtx6.mHeight;
                header.dwMipMapCount = d3dtx.d3dtx6.mNumMipLevels;
                surfaceFormat = d3dtx.d3dtx6.mSurfaceFormat;
            }
            else if (d3dtx.d3dtx7 != null)
            {
                header.dwWidth = d3dtx.d3dtx7.mWidth;
                header.dwHeight = d3dtx.d3dtx7.mHeight;
                header.dwMipMapCount = d3dtx.d3dtx7.mNumMipLevels;
                header.dwDepth = d3dtx.d3dtx7.mDepth;
                surfaceFormat = d3dtx.d3dtx7.mSurfaceFormat;
            }
            else if (d3dtx.d3dtx8 != null)
            {
                header.dwWidth = d3dtx.d3dtx8.mWidth;
                header.dwHeight = d3dtx.d3dtx8.mHeight;
                header.dwMipMapCount = d3dtx.d3dtx8.mNumMipLevels;
                header.dwDepth = d3dtx.d3dtx8.mDepth;
                surfaceFormat = d3dtx.d3dtx8.mSurfaceFormat;
            }
            else if (d3dtx.d3dtx9 != null)
            {
                header.dwWidth = d3dtx.d3dtx9.mWidth;
                header.dwHeight = d3dtx.d3dtx9.mHeight;
                header.dwMipMapCount = d3dtx.d3dtx9.mNumMipLevels;
                header.dwDepth = d3dtx.d3dtx9.mDepth;
                surfaceFormat = d3dtx.d3dtx9.mSurfaceFormat;
            }

            header.ddspf.dwFourCC = DDS_Functions.Get_FourCC_FromTellale(surfaceFormat);

            switch (surfaceFormat)
            {
                case Telltale.T3SurfaceFormat.eSurface_A8:
                    header.ddspf.dwABitMask = 255;
                    header.dwCaps = 4198408; //DDSCAPS_COMPLEX | DDSCAPS_TEXTURE | DDSCAPS_MIPMAP
                    break;
                case Telltale.T3SurfaceFormat.eSurface_ARGB8:
                    header.ddspf.dwABitMask = 255;
                    header.ddspf.dwRBitMask = 255;
                    header.ddspf.dwGBitMask = 255;
                    header.ddspf.dwBBitMask = 255;
                    header.dwCaps = 4198408; //DDSCAPS_COMPLEX | DDSCAPS_TEXTURE | DDSCAPS_MIPMAP
                    break;
            }
        }

        /// <summary>
        /// Parses the data from a DDS byte array. (Can also read just the header only)
        /// </summary>
        /// <param name="fileData"></param>
        /// <param name="headerOnly"></param>
        private void GetData(byte[] fileData, bool headerOnly)
        {
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("Total Source Texture Byte Size = {0}", fileData.Length);

            //which byte offset we are on for the source texture (will be changed as we go through the file)
            uint bytePointerPosition = 4; //skip past the 'DDS '
            byte[] headerBytes = ByteFunctions.AllocateBytes(124, fileData, bytePointerPosition);

            //this will automatically read all of the byte data in the header
            header = DDS_Functions.GetHeaderFromBytes(headerBytes);

            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("DDS Height = {0}", header.dwHeight);
            Console.WriteLine("DDS Width = {0}", header.dwWidth);
            Console.WriteLine("DDS Mip Map Count = {0}", header.dwMipMapCount);
            Console.WriteLine("DDS Compression = {0}", header.ddspf.dwFourCC);

            if (headerOnly)
                return;

            //--------------------------EXTRACT DDS TEXTURE DATA--------------------------
            //calculate dds header length (we add 4 because we skipped the 4 bytes which contain the ddsPrefix, it isn't necessary to parse this data)
            uint ddsHeaderLength = 4 + header.dwSize;

            //calculate the length of just the dds texture data
            uint ddsTextureDataLength = (uint)sourceFileData.Length - ddsHeaderLength;

            //allocate a byte array of dds texture length
            byte[] ddsTextureData = new byte[ddsTextureDataLength];

            //copy the data from the source byte array past the header (so we are only getting texture data)
            Array.Copy(sourceFileData, ddsHeaderLength, ddsTextureData, 0, ddsTextureData.Length);

            //if there are no mip maps
            if(header.dwMipMapCount <= 0)
            {
                textureData = new();
                textureData.Add(ddsTextureData);
            }
            else //if there are mip maps
            {
                //get mip resolutions
                //calculated mip resolutions [Pixel Value, Width or Height (0 or 1)]
                mipMapResolutions = DDS_Functions.DDS_CalculateMipResolutions(header.dwMipMapCount, header.dwWidth, header.dwHeight);

                //get byte sizes
                uint[] byteSizes = DDS_Functions.DDS_GetImageByteSizes(mipMapResolutions, DDS_Functions.DDS_CompressionBool(header));

                textureData = new();
                int test = ddsTextureData.Length;
                int offset = 0;

                for (int i = 0; i < byteSizes.Length; i++)
                {
                    byte[] temp = new byte[byteSizes[i]];

                    //issue length
                    //Array.Copy(ddsTextureData, offset, temp, 0, temp.Length);

                    //offset += temp.Length - 1;

                    //textureData.Add(temp);

                    test -= (int)byteSizes[i];
                    //test = test;
                }

                test = test;
            }
        }

        /// <summary>
        /// Writes a D3DTX into a DDS file on the disk.
        /// </summary>
        /// <param name="d3dtx"></param>
        /// <param name="destinationPath"></param>
        public void Write_D3DTX_AsDDS(D3DTX_Master d3dtx, string destinationPath)
        {
            byte[] finalData = new byte[0];

            //turn our header data into bytes to be written into a file
            byte[] dds_header = ByteFunctions.Combine(ByteFunctions.GetBytes("DDS "), DDS_Functions.GetHeaderBytes(header));

            //copy the dds header to the file
            finalData = ByteFunctions.Combine(finalData, dds_header);

            if (d3dtx.d3dtx4 != null)
            {
                //copy the images
                for (int i = d3dtx.d3dtx4.mPixelData.Count - 1; i >= 0; i--)
                {
                    finalData = ByteFunctions.Combine(finalData, d3dtx.d3dtx4.mPixelData[i]);
                }
            }
            else if (d3dtx.d3dtx5 != null)
            {
                //copy the images
                for (int i = d3dtx.d3dtx5.mPixelData.Count - 1; i >= 0; i--)
                {
                    finalData = ByteFunctions.Combine(finalData, d3dtx.d3dtx5.mPixelData[i]);
                }
            }
            else if (d3dtx.d3dtx6 != null)
            {
                //copy the images
                for (int i = d3dtx.d3dtx6.mPixelData.Count - 1; i >= 0; i--)
                {
                    finalData = ByteFunctions.Combine(finalData, d3dtx.d3dtx6.mPixelData[i]);
                }
            }
            else if (d3dtx.d3dtx7 != null)
            {
                //copy the images
                for (int i = d3dtx.d3dtx7.mPixelData.Count - 1; i >= 0; i--)
                {
                    finalData = ByteFunctions.Combine(finalData, d3dtx.d3dtx7.mPixelData[i]);
                }
            }
            else if (d3dtx.d3dtx8 != null)
            {
                //copy the images
                for (int i = d3dtx.d3dtx8.mPixelData.Count - 1; i >= 0; i--)
                {
                    finalData = ByteFunctions.Combine(finalData, d3dtx.d3dtx8.mPixelData[i]);
                }
            }
            else if (d3dtx.d3dtx9 != null)
            {
                //copy the images
                for (int i = d3dtx.d3dtx9.mPixelData.Count - 1; i >= 0; i--)
                {
                    finalData = ByteFunctions.Combine(finalData, d3dtx.d3dtx9.mPixelData[i]);
                }
            }

            //write the file to the disk
            File.WriteAllBytes(destinationPath, finalData);
        }

        /// <summary>
        /// Matches a given DDS file on the disk to a D3DTX object.
        /// </summary>
        /// <param name="ddsPath"></param>
        /// <param name="d3dtx"></param>
        /// <param name="options"></param>
        public void Match_DDS_With_D3DTX(string ddsPath, D3DTX_Master d3dtx, DDS_Matching_Options options)
        {
            //load in the DDS image using DirectXTexNet
            ScratchImage scratchImage = TexHelper.Instance.LoadFromDDSFile(ddsPath, DDS_FLAGS.NONE);

            //create our main variables that will be used when doing conversion operations
            int d3dtx_width = 0;
            int d3dtx_height = 0;
            int d3dtx_mipAmount = 0;
            T3SurfaceFormat d3dtx_format = T3SurfaceFormat.eSurface_DXT1;
            T3SurfaceGamma d3dtx_gamma = T3SurfaceGamma.eSurfaceGamma_sRGB;

            //assign the values depending on which version is active
            if(d3dtx.d3dtx9 != null)
            {
                d3dtx_width = (int)d3dtx.d3dtx9.mWidth;
                d3dtx_height = (int)d3dtx.d3dtx9.mHeight;
                d3dtx_mipAmount = (int)d3dtx.d3dtx9.mNumMipLevels;
                d3dtx_format = d3dtx.d3dtx9.mSurfaceFormat;
                d3dtx_gamma = d3dtx.d3dtx9.mSurfaceGamma;
            }
            else if (d3dtx.d3dtx8 != null)
            {
                d3dtx_width = (int)d3dtx.d3dtx8.mWidth;
                d3dtx_height = (int)d3dtx.d3dtx8.mHeight;
                d3dtx_mipAmount = (int)d3dtx.d3dtx8.mNumMipLevels;
                d3dtx_format = d3dtx.d3dtx8.mSurfaceFormat;
                d3dtx_gamma = d3dtx.d3dtx8.mSurfaceGamma;
            }
            else if (d3dtx.d3dtx7 != null)
            {
                d3dtx_width = (int)d3dtx.d3dtx7.mWidth;
                d3dtx_height = (int)d3dtx.d3dtx7.mHeight;
                d3dtx_mipAmount = (int)d3dtx.d3dtx7.mNumMipLevels;
                d3dtx_format = d3dtx.d3dtx7.mSurfaceFormat;
                d3dtx_gamma = d3dtx.d3dtx7.mSurfaceGamma;
            }
            else if (d3dtx.d3dtx6 != null)
            {
                d3dtx_width = (int)d3dtx.d3dtx6.mWidth;
                d3dtx_height = (int)d3dtx.d3dtx6.mHeight;
                d3dtx_mipAmount = (int)d3dtx.d3dtx6.mNumMipLevels;
                d3dtx_format = d3dtx.d3dtx6.mSurfaceFormat;
                d3dtx_gamma = T3SurfaceGamma.eSurfaceGamma_sRGB; //this version doesn't have a surface gamma field, so give it an SRGB by default
            }
            else if (d3dtx.d3dtx5 != null)
            {
                d3dtx_width = (int)d3dtx.d3dtx5.mWidth;
                d3dtx_height = (int)d3dtx.d3dtx5.mHeight;
                d3dtx_mipAmount = (int)d3dtx.d3dtx5.mNumMipLevels;
                d3dtx_format = d3dtx.d3dtx5.mSurfaceFormat;
                d3dtx_gamma = T3SurfaceGamma.eSurfaceGamma_sRGB; //this version doesn't have a surface gamma field, so give it an SRGB by default
            }
            else if (d3dtx.d3dtx4 != null)
            {
                d3dtx_width = (int)d3dtx.d3dtx4.mWidth;
                d3dtx_height = (int)d3dtx.d3dtx4.mHeight;
                d3dtx_mipAmount = (int)d3dtx.d3dtx4.mNumMipLevels;
                d3dtx_format = d3dtx.d3dtx4.mSurfaceFormat;
                d3dtx_gamma = T3SurfaceGamma.eSurfaceGamma_sRGB; //this version doesn't have a surface gamma field, so give it an SRGB by default
            }

            //-------------------------------------- CONVERSION START --------------------------------------
            //change the compression if needed
            if (options.MatchCompression)
            {
                DXGI_FORMAT dxgi_format = DDS_Functions.GetSurfaceFormatAsDXGI(d3dtx_format, d3dtx_gamma);
                scratchImage.Convert(dxgi_format, TEX_FILTER_FLAGS.DITHER, 0.0f);
            }

            //rescale the image to match if needed
            if (options.MatchResolution)
            {
                scratchImage.Resize(d3dtx_width, d3dtx_height, TEX_FILTER_FLAGS.CUBIC);
            }

            //generate mip maps if needed
            if (options.GenerateMipMaps)
            {
                if(options.MatchMipMapCount)
                {
                    scratchImage.GenerateMipMaps(0, TEX_FILTER_FLAGS.CUBIC, d3dtx_mipAmount, false);
                }
                else
                {
                    scratchImage.GenerateMipMaps(0, TEX_FILTER_FLAGS.CUBIC, 0, false);
                }
            }

            //resave the newly modified DDS
            scratchImage.SaveToDDSFile(DDS_FLAGS.NONE, ddsPath);
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
    }
}
