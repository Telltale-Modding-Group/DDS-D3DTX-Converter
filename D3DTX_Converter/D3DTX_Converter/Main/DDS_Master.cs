using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DirectXTexNet;
using D3DTX_Converter.Utilities;
using D3DTX_Converter.DirectX;
using D3DTX_Converter.TelltaleEnums;

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

namespace D3DTX_Converter.Main
{
    /// <summary>
    /// Main class for generating a dds byte header
    /// </summary>
    public class DDS_Master
    {
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
            //NOTES: Remember that mip tables are reversed
            //So in that vein cubemap textures are likely in order but reversed
            //Some normal maps specifically with type 4 (eTxNormalMap) channels are all reversed (ABGR instead of RGBA)

            header = DDS.GetPresetHeader();

            T3SurfaceFormat surfaceFormat = T3SurfaceFormat.eSurface_DXT1;

            //header.dwCaps = DDSCAPS.DDSCAPS_TEXTURE | DDSCAPS.DDSCAPS_MIPMAP;

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
                //header.dwDepth = d3dtx.d3dtx7.mDepth;
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

            header.ddspf.dwFourCC = DDS.Get_FourCC_FromTellale(surfaceFormat);

            switch (surfaceFormat)
            {
                case T3SurfaceFormat.eSurface_A8:
                    header.ddspf.dwABitMask = 255;
                    header.dwCaps = 4198408; //DDSCAPS_COMPLEX | DDSCAPS_TEXTURE | DDSCAPS_MIPMAP
                    break;
                case T3SurfaceFormat.eSurface_ARGB8:
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
            byte[] headerBytes = ByteFunctions.AllocateBytes(124, fileData, 4); //skip past the 'DDS '

            //this will automatically read all of the byte data in the header
            header = DDS.GetHeaderFromBytes(headerBytes);

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

            textureData = new();

            //if there are no mip maps
            if (header.dwMipMapCount <= 1)
            {
                textureData.Add(ddsTextureData);
                Console.WriteLine("DDS Texture Byte Size = {0}", textureData[0].Length);
            }
            else //if there are mip maps
            {
                //get mip resolutions
                //calculated mip resolutions [Pixel Value, Width or Height (0 or 1)]
                mipMapResolutions = DDS.CalculateMipResolutions(header.dwMipMapCount - 1, header.dwWidth, header.dwHeight);

                //get block size
                uint blockSize = DDS.GetDDSBlockSize(header);

                //get byte sizes
                //uint[] byteSizes = DDS_Functions.DDS_GetImageByteSizes(mipMapResolutions, header.dwPitchOrLinearSize, ((header.ddspf.dwFourCC == 0x44585435u || header.ddspf.dwFourCC == 0x35545844u) ? false : true));
                uint[] byteSizes = DDS.GetImageByteSizes(mipMapResolutions, header.dwPitchOrLinearSize, blockSize);

                int offset = 0;

                for (int i = 0; i < byteSizes.Length; i++)
                {
                    byte[] temp = new byte[byteSizes[i]];

                    //issue length
                    Array.Copy(ddsTextureData, offset, temp, 0, temp.Length);

                    offset += temp.Length;

                    textureData.Add(temp);
                }
            }
        }

        public void TEST_WriteDDSToDisk(string destinationPath)
        {
            //THIS WORKS
            string extension = Path.GetExtension(destinationPath);
            string newPath = destinationPath.Remove(destinationPath.Length - extension.Length, extension.Length);
            newPath += "_parseTest" + extension;

            byte[] finalData = new byte[0];

            //turn our header data into bytes to be written into a file
            byte[] dds_header = ByteFunctions.Combine(ByteFunctions.GetBytes("DDS "), DDS.GetHeaderBytes(header));

            //copy the dds header to the file
            finalData = ByteFunctions.Combine(finalData, dds_header);

            //copy the images
            for (int i = 0; i <= textureData.Count - 1; i++)
            {
                finalData = ByteFunctions.Combine(finalData, textureData[i]);
            }

            //write the file to the disk
            File.WriteAllBytes(newPath, finalData);
        }

        public static string GetCubeFaceName(int faceIndex)
        {
            switch(faceIndex)
            {
                case 0: return "XPOS";
                case 1: return "XNEG";
                case 2: return "YPOS";
                case 3: return "YNEG";
                case 4: return "ZPOS";
                case 5: return "ZNEG";
                default: return "";
            }
        }

        /// <summary>
        /// Writes a D3DTX into a DDS file on the disk.
        /// </summary>
        /// <param name="d3dtx"></param>
        /// <param name="destinationPath"></param>
        public void Write_D3DTX_AsDDS(D3DTX_Master d3dtx, string destinationPath)
        {
            if(d3dtx.IsCubeTexture())
            {
                int regionCount = d3dtx.GetRegionCount();
                int mipCount = (int)d3dtx.GetMipMapCount();
                int cubeSurfacesAmount = regionCount / mipCount;

                string fileExtension = Path.GetExtension(destinationPath);
                string fileName = Path.GetFileNameWithoutExtension(destinationPath);
                string originalPath = destinationPath.Remove(destinationPath.Length - fileExtension.Length, fileExtension.Length);
                string newCubeDirectory = originalPath + "/";

                if(Directory.Exists(newCubeDirectory) == false)
                {
                    Directory.CreateDirectory(newCubeDirectory);
                }

                for (int i = 0; i < cubeSurfacesAmount; i++)
                {
                    string cubeFaceName = GetCubeFaceName(i);

                    string newFileName = string.Format("{0}{1}_Cube{2}{3}", newCubeDirectory, fileName, cubeFaceName, fileExtension);

                    //turn our header data into bytes to be written into a file
                    byte[] dds_header = ByteFunctions.Combine(ByteFunctions.GetBytes("DDS "), DDS.GetHeaderBytes(header));

                    //copy the dds header to the file
                    byte[] finalData = new byte[0];
                    finalData = ByteFunctions.Combine(finalData, dds_header);

                    List<byte[]> pixelData = d3dtx.GetPixelDataByFaceIndex(i);

                    //copy the images
                    for (int x = mipCount - 1; x >= 0; x--)
                    {
                        //int newPixelDataIndex = startingPixelDataIndex + x;
                        finalData = ByteFunctions.Combine(finalData, pixelData[x]);
                    }

                    //write the file to the disk
                    File.WriteAllBytes(newFileName, finalData);
                }
            }
            else
            {
                //turn our header data into bytes to be written into a file
                byte[] dds_header = ByteFunctions.Combine(ByteFunctions.GetBytes("DDS "), DDS.GetHeaderBytes(header));

                //copy the dds header to the file
                byte[] finalData = new byte[0];
                finalData = ByteFunctions.Combine(finalData, dds_header);

                List<byte[]> pixelData = d3dtx.GetPixelData();

                //copy the images
                for (int i = pixelData.Count - 1; i >= 0; i--)
                {
                    finalData = ByteFunctions.Combine(finalData, pixelData[i]);
                }

                //write the file to the disk
                File.WriteAllBytes(destinationPath, finalData);
            }
        }

        /// <summary>
        /// Matches a given DDS file on the disk to a D3DTX object.
        /// </summary>
        /// <param name="ddsPath"></param>
        /// <param name="d3dtx"></param>
        /// <param name="options"></param>
        public void Match_DDS_With_D3DTX(string ddsPath, D3DTX_Master d3dtx, DDS_Matching_Options options)
        {
            
        }
    }
}
