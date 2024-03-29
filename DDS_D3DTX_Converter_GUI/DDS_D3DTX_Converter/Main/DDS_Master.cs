﻿using System;
using System.Collections.Generic;
using System.IO;
using D3DTX_Converter.Utilities;
using D3DTX_Converter.DirectX;
using D3DTX_Converter.TelltaleEnums;
using DirectXTexNet;
using System.ComponentModel;
using ExCSS;
using D3DTX_Converter.TelltaleD3DTX;

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

        public DDS_HEADER_DXT10 dxt10_header;

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
            //TODO add depth textures

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

            if (header.ddspf.dwFourCC == ByteFunctions.Convert_String_To_UInt32("DX10"))
            {
                dxt10_header = DDS.GetPresetDXT10Header();
                dxt10_header.dxgiFormat = DDS.GetSurfaceFormatAsDXGI(surfaceFormat);
                //Needs checking
                dxt10_header.resourceDimension = d3dtx.IsCubeTexture() ? D3D10_RESOURCE_DIMENSION.D3D10_RESOURCE_DIMENSION_TEXTURE3D : D3D10_RESOURCE_DIMENSION.D3D10_RESOURCE_DIMENSION_TEXTURE2D;

                //TODO NEEDS TESTING
                if (d3dtx.d3dtx9 != null)
                {
                    dxt10_header.arraySize = d3dtx.d3dtx9.mArraySize;
                }
                else
                {
                    dxt10_header.arraySize = 1;
                }
            }

            //header.dwPitchOrLinearSize;

            //Get the channel count for all formats in case they are not specified 
            //  header.ddspf.dwRGBBitCount = uint.Parse(d3dtx.GetChannelCount()) * 8;

            Console.WriteLine(d3dtx.GetChannelCount());
            header.dwCaps = 4198408; //ALL FLAGS ARE ENABLED

            //TODO ADD OTHER FORMATS
            //TODO REFACTOR THIS SPAGHETTI CODE
            switch (surfaceFormat)
            {
                case T3SurfaceFormat.eSurface_A8: //DDSCAPS_COMPLEX | DDSCAPS_TEXTURE | DDSCAPS_MIPMAP
                    SetPixelFormatHeader(ref header.ddspf, 32, 0x01, ByteFunctions.Convert_String_To_UInt32("DX10"), 8, 0x00, 0x00, 0x00, 0xFF);
                    break;
                case T3SurfaceFormat.eSurface_ARGB8: //TODO This is actually a legacy format // DDPF_RGB | DDPF_ALPHAPIXELS
                    SetPixelFormatHeader(ref header.ddspf, 32, 0x41, ByteFunctions.Convert_String_To_UInt32("DX10"), 32, 0x00FF0000, 0x0000FF00, 0x000000FF, 0xFF000000);
                    break;
                case T3SurfaceFormat.eSurface_ARGB16: // DDPF_RGB | DDPF_ALPHAPIXELS
                    SetPixelFormatHeader(ref header.ddspf, 32, 0x41, ByteFunctions.Convert_String_To_UInt32("DX10"), 64, 0, 0, 0, 0);
                    break;
                case T3SurfaceFormat.eSurface_RGB565:
                    SetPixelFormatHeader(ref header.ddspf, 32, 0x40, ByteFunctions.Convert_String_To_UInt32("DX10"), 16, 0x0000F800, 0x000007E0, 0x0000001F, 0x00); // DDPF_RGB
                    break;
                case T3SurfaceFormat.eSurface_ARGB1555: // DDPF_RGB | DDPF_ALPHAPIXELS
                    SetPixelFormatHeader(ref header.ddspf, 32, 0x41, ByteFunctions.Convert_String_To_UInt32("DX10"), 16, 0x00007C00, 0x000003E0, 0x0000001F, 0x00008000);
                    break;
                case T3SurfaceFormat.eSurface_ARGB4:// DDPF_RGB | DDPF_ALPHAPIXELS
                    SetPixelFormatHeader(ref header.ddspf, 32, 0x41, ByteFunctions.Convert_String_To_UInt32("DX10"), 16, 0x00000F00, 0x000000F0, 0x0000000F, 0x0000F000);
                    break;
                case T3SurfaceFormat.eSurface_ARGB2101010: // DDPF_RGB | DDPF_ALPHAPIXELS
                    SetPixelFormatHeader(ref header.ddspf, 32, 0x41, ByteFunctions.Convert_String_To_UInt32("DX10"), 32, 0, 0, 0, 0);
                    break;
                case T3SurfaceFormat.eSurface_R16:// DDPF_RGB
                    SetPixelFormatHeader(ref header.ddspf, 32, 0x01, ByteFunctions.Convert_String_To_UInt32("DX10"), 16, 0xFFFF, 0x00, 0x00, 0x00);
                    break;
                case T3SurfaceFormat.eSurface_RG16: // DDPF_RGB
                    SetPixelFormatHeader(ref header.ddspf, 32, 0x01, ByteFunctions.Convert_String_To_UInt32("DX10"), 32, 0xFFFF0000, 0x0000FFFF, 0x00, 0x00);
                    break;
                case T3SurfaceFormat.eSurface_RGBA16:  // DDPF_RGB | DDPF_ALPHAPIXELS
                    SetPixelFormatHeader(ref header.ddspf, 32, 0x01, ByteFunctions.Convert_String_To_UInt32("DX10"), 64, 0, 0, 0x00000000FFFF0000, 0);
                    break;
                case T3SurfaceFormat.eSurface_RG8: // DDPF_RGB
                    SetPixelFormatHeader(ref header.ddspf, 32, 0x01, ByteFunctions.Convert_String_To_UInt32("DX10"), 16, 0xFF00, 0x00FF, 0x00, 0x00);
                    break;
                case T3SurfaceFormat.eSurface_RGBA8: // DDPF_RGB | DDPF_ALPHAPIXELS
                    SetPixelFormatHeader(ref header.ddspf, 32, 0x01, ByteFunctions.Convert_String_To_UInt32("DX10"), 32, 0xFF000000, 0x00FF0000, 0x0000FF00, 0x000000FF);
                    break;
                case T3SurfaceFormat.eSurface_R32:  // DDPF_RGB
                    SetPixelFormatHeader(ref header.ddspf, 32, 0x01, ByteFunctions.Convert_String_To_UInt32("DX10"), 32, 0xFFFFFFFF, 0x00, 0x00, 0x00);
                    break;
                case T3SurfaceFormat.eSurface_RG32:
                    SetPixelFormatHeader(ref header.ddspf, 32, 0x01, ByteFunctions.Convert_String_To_UInt32("DX10"), 64, 0, 0, 0, 0);
                    break;
                case T3SurfaceFormat.eSurface_RGBA32:
                    SetPixelFormatHeader(ref header.ddspf, 32, 0x01, ByteFunctions.Convert_String_To_UInt32("DX10"), 128, 0, 0, 0, 0);
                    break;
                case T3SurfaceFormat.eSurface_R8:
                    SetPixelFormatHeader(ref header.ddspf, 32, 0x01, ByteFunctions.Convert_String_To_UInt32("DX10"), 8, 0xFF, 0x00, 0x00, 0x00);
                    break;
                case T3SurfaceFormat.eSurface_RGBA8S:
                    SetPixelFormatHeader(ref header.ddspf, 32, 0x41, ByteFunctions.Convert_String_To_UInt32("DX10"), 32, 0xFF000000, 0x00FF0000, 0x0000FF00, 0x000000FF);
                    break;
                case T3SurfaceFormat.eSurface_L8:
                    SetPixelFormatHeader(ref header.ddspf, 32, 0x01, ByteFunctions.Convert_String_To_UInt32("DX10"), 8, 0xFF, 0x00, 0x00, 0x00);
                    break;
                case T3SurfaceFormat.eSurface_AL8:
                    SetPixelFormatHeader(ref header.ddspf, 32, 0x01, ByteFunctions.Convert_String_To_UInt32("DX10"), 8, 0x00, 0x00, 0x00, 0xFF);
                    break;
                case T3SurfaceFormat.eSurface_L16:
                    SetPixelFormatHeader(ref header.ddspf, 32, 0x01, ByteFunctions.Convert_String_To_UInt32("DX10"), 16, 0xFFFF, 0x00, 0x00, 0x00);
                    break;
                case T3SurfaceFormat.eSurface_RG16S:
                    SetPixelFormatHeader(ref header.ddspf, 32, 0x41, ByteFunctions.Convert_String_To_UInt32("DX10"), 32, 0xFFFF0000, 0x0000FFFF, 0x00, 0x00);
                    break;
                case T3SurfaceFormat.eSurface_RGBA16S:
                    SetPixelFormatHeader(ref header.ddspf, 32, 0x01, ByteFunctions.Convert_String_To_UInt32("DX10"), 64, 0, 0, 0, 0);
                    break;
                case T3SurfaceFormat.eSurface_R16UI:
                    SetPixelFormatHeader(ref header.ddspf, 32, 0x01, ByteFunctions.Convert_String_To_UInt32("DX10"), 16, 0xFFFF, 0x00, 0x00, 0x00);
                    break;
                case T3SurfaceFormat.eSurface_RG16UI:
                    SetPixelFormatHeader(ref header.ddspf, 32, 0x01, ByteFunctions.Convert_String_To_UInt32("DX10"), 32, 0xFFFF0000, 0x0000FFFF, 0x00, 0x00);
                    break;
                case T3SurfaceFormat.eSurface_R16F:
                    SetPixelFormatHeader(ref header.ddspf, 32, 0x01, ByteFunctions.Convert_String_To_UInt32("DX10"), 16, 0xFFFF, 0x00, 0x00, 0x00);
                    break;
                case T3SurfaceFormat.eSurface_RG16F:
                    SetPixelFormatHeader(ref header.ddspf, 32, 0x01, ByteFunctions.Convert_String_To_UInt32("DX10"), 32, 0xFFFF0000, 0x0000FFFF, 0x00, 0x00);
                    break;
                case T3SurfaceFormat.eSurface_RGBA16F:
                    SetPixelFormatHeader(ref header.ddspf, 32, 0x01, ByteFunctions.Convert_String_To_UInt32("DX10"), 64, 0, 0, 0, 0);
                    break;
                case T3SurfaceFormat.eSurface_R32F:
                    SetPixelFormatHeader(ref header.ddspf, 32, 0x01, ByteFunctions.Convert_String_To_UInt32("DX10"), 32, 0xFFFFFFFF, 0x00, 0x00, 0x00);
                    break;
                case T3SurfaceFormat.eSurface_RG32F:
                    SetPixelFormatHeader(ref header.ddspf, 32, 0x01, ByteFunctions.Convert_String_To_UInt32("DX10"), 64, 0, 0x00000000FFFFFFFF, 0x00, 0x00);
                    break;
                case T3SurfaceFormat.eSurface_RGBA32F:
                    SetPixelFormatHeader(ref header.ddspf, 32, 0x01, ByteFunctions.Convert_String_To_UInt32("DX10"), 128, 0, 0, 0, 0);
                    break;
                case T3SurfaceFormat.eSurface_RGBA1010102F:
                    SetPixelFormatHeader(ref header.ddspf, 32, 0x01, ByteFunctions.Convert_String_To_UInt32("DX10"), 32, 0, 0, 0, 0);
                    break;
                case T3SurfaceFormat.eSurface_RGB111110F:
                    SetPixelFormatHeader(ref header.ddspf, 32, 0x01, ByteFunctions.Convert_String_To_UInt32("DX10"), 32, 0, 0, 0, 0);
                    break;
                case T3SurfaceFormat.eSurface_RGB9E5F:
                    SetPixelFormatHeader(ref header.ddspf, 32, 0x01, ByteFunctions.Convert_String_To_UInt32("DX10"), 32, 0, 0, 0, 0);
                    break;
                case T3SurfaceFormat.eSurface_DepthPCF16:
                    SetPixelFormatHeader(ref header.ddspf, 32, 0x04, ByteFunctions.Convert_String_To_UInt32("DX10"), 16, 0x0000FFFF, 0x00, 0x00, 0x00);
                    break;
                case T3SurfaceFormat.eSurface_DepthPCF24:
                    SetPixelFormatHeader(ref header.ddspf, 32, 0x04, ByteFunctions.Convert_String_To_UInt32("DX10"), 24, 0x00FFFFFF, 0x00, 0x00, 0x00);
                    break;
                case T3SurfaceFormat.eSurface_Depth16:
                    SetPixelFormatHeader(ref header.ddspf, 32, 0x04, ByteFunctions.Convert_String_To_UInt32("DX10"), 16, 0x0000FFFF, 0x00, 0x00, 0x00);
                    break;
                case T3SurfaceFormat.eSurface_Depth24:
                    SetPixelFormatHeader(ref header.ddspf, 32, 0x04, ByteFunctions.Convert_String_To_UInt32("DX10"), 24, 0x00FFFFFF, 0x00, 0x00, 0x00);
                    break;
                case T3SurfaceFormat.eSurface_DepthStencil32:
                    SetPixelFormatHeader(ref header.ddspf, 32, 0x04, ByteFunctions.Convert_String_To_UInt32("DX10"), 32, 0xFFFFFFFF, 0x00, 0x00, 0x00);
                    break;
                case T3SurfaceFormat.eSurface_Depth32F:
                    SetPixelFormatHeader(ref header.ddspf, 32, 0x04, ByteFunctions.Convert_String_To_UInt32("DX10"), 32, 0xFFFFFFFF, 0x00, 0x00, 0x00);
                    break;
                case T3SurfaceFormat.eSurface_Depth32F_Stencil8:
                    SetPixelFormatHeader(ref header.ddspf, 32, 0x04, ByteFunctions.Convert_String_To_UInt32("DX10"), 32, 0xFFFFFFFF, 0x00, 0x00, 0x00);
                    break;
                case T3SurfaceFormat.eSurface_Depth24F_Stencil8:
                    SetPixelFormatHeader(ref header.ddspf, 32, 0x04, ByteFunctions.Convert_String_To_UInt32("DX10"), 32, 0xFFFFFFFF, 0x00, 0x00, 0x00);
                    break;
                case T3SurfaceFormat.eSurface_BC1:
                    SetPixelFormatHeader(ref header.ddspf, 32, 0x04, ByteFunctions.Convert_String_To_UInt32("DX10"), 0, 0, 0, 0, 0); // 'DXT1'
                    break;
                case T3SurfaceFormat.eSurface_DXT1:
                    SetPixelFormatHeader(ref header.ddspf, 32, 0x04, ByteFunctions.Convert_String_To_UInt32("DXT1"), 0, 0, 0, 0, 0); // 'DXT1'
                    break;
                case T3SurfaceFormat.eSurface_BC2:
                    SetPixelFormatHeader(ref header.ddspf, 32, 0x04, ByteFunctions.Convert_String_To_UInt32("DX10"), 0, 0, 0, 0, 0); // 'DXT2'
                    break;
                case T3SurfaceFormat.eSurface_DXT3:
                    SetPixelFormatHeader(ref header.ddspf, 32, 0x04, ByteFunctions.Convert_String_To_UInt32("DXT3"), 0, 0, 0, 0, 0); // 'DXT3'
                    break;
                case T3SurfaceFormat.eSurface_BC3:
                    SetPixelFormatHeader(ref header.ddspf, 32, 0x04, ByteFunctions.Convert_String_To_UInt32("DX10"), 0, 0, 0, 0, 0); // 'DXT4'
                    break;
                case T3SurfaceFormat.eSurface_DXT5:
                    SetPixelFormatHeader(ref header.ddspf, 32, 0x04, ByteFunctions.Convert_String_To_UInt32("DXT5"), 0, 0, 0, 0, 0); // 'DXT5'
                    break;
                case T3SurfaceFormat.eSurface_DXT5A:
                    SetPixelFormatHeader(ref header.ddspf, 32, 0x04, ByteFunctions.Convert_String_To_UInt32("ATI1"), 0, 0, 0, 0, 0); // 'DXT5'
                    break;
                case T3SurfaceFormat.eSurface_BC6:
                    SetPixelFormatHeader(ref header.ddspf, 32, 0x04, ByteFunctions.Convert_String_To_UInt32("DX10"), 0, 0, 0, 0, 0); // 'BC6H'
                    break;
                case T3SurfaceFormat.eSurface_BC7:
                    SetPixelFormatHeader(ref header.ddspf, 32, 0x04, ByteFunctions.Convert_String_To_UInt32("DX10"), 0, 0, 0, 0, 0); // 'BC7U'
                    break;
                case T3SurfaceFormat.eSurface_DXN:
                    SetPixelFormatHeader(ref header.ddspf, 32, 0x04, ByteFunctions.Convert_String_To_UInt32("ATI2"), 0, 0, 0, 0, 0); // 'DXN'
                    break;
                default:
                    break;
            }
            header.ddspf.dwFlags = header.ddspf.dwFlags | 0x4;
            header.dwPitchOrLinearSize = DDS.GetPitchOrLinearSizeFromD3DTX(surfaceFormat, header.dwWidth);
        }

        private void SetPixelFormatHeader(ref DDS_PIXELFORMAT ddspf, uint dwSize, uint dwFlags, uint dwFourCC, uint dwRGBBitCount, uint dwRBitMask, uint dwGBitMask, uint dwBBitMask, uint dwABitMask)
        {
            ddspf.dwSize = dwSize;
            ddspf.dwFlags = dwFlags;
            ddspf.dwFourCC = dwFourCC;
            ddspf.dwRGBBitCount = dwRGBBitCount;
            ddspf.dwRBitMask = dwRBitMask;
            ddspf.dwGBitMask = dwGBitMask;
            ddspf.dwBBitMask = dwBBitMask;
            ddspf.dwABitMask = dwABitMask;
        }


        /// <summary>
        /// Parses the data from a DDS byte array. (Can also read just the header only)
        /// </summary>
        /// <param name="fileData"></param>
        /// <param name="headerOnly"></param>
        private void GetData(byte[] fileData, bool headerOnly)
        {
            Console.WriteLine("Total Source Texture Byte Size = {0}", fileData.Length);

            //which byte offset we are on for the source texture (will be changed as we go through the file)
            byte[] headerBytes = ByteFunctions.AllocateBytes(124, fileData, 4); //skip past the 'DDS '

            //this will automatically read all of the byte data in the header
            header = DDS.GetHeaderFromBytes(headerBytes);

            Console.WriteLine("DDS Height = {0}", header.dwHeight);
            Console.WriteLine("DDS Width = {0}", header.dwWidth);
            Console.WriteLine("DDS Mip Map Count = {0}", header.dwMipMapCount);
            Console.WriteLine("DDS Compression = {0}", header.ddspf.dwFourCC);

            //get dxt10 header if it exists
            if (header.ddspf.dwFourCC == ByteFunctions.Convert_String_To_UInt32("DX10"))
            {
                byte[] dxt10headerBytes = ByteFunctions.AllocateBytes(20, fileData, 128); //skip the main header
                dxt10_header = DDS.GetDX10HeaderFromBytes(dxt10headerBytes);
            }

            if (headerOnly)
                return;

            //--------------------------EXTRACT DDS TEXTURE DATA--------------------------
            //calculate dds header length (we add 4 because we skipped the 4 bytes which contain the ddsPrefix, it isn't necessary to parse this data)
            uint ddsHeaderLength = 4 + header.dwSize;

            //if dxt10Header is present, add additional 20 bytes
            uint dxt10HeaderLength = (uint)((header.ddspf.dwFourCC == ByteFunctions.Convert_String_To_UInt32("DX10")) ? 20 : 0);

            //calculate the length of just the dds texture data
            uint ddsTextureDataLength = (uint)sourceFileData.Length - ddsHeaderLength - dxt10HeaderLength;

            //allocate a byte array of dds texture length
            byte[] ddsTextureData = new byte[ddsTextureDataLength];

            //copy the data from the source byte array past the header (so we are only getting texture data)
            Array.Copy(sourceFileData, ddsHeaderLength + dxt10HeaderLength, ddsTextureData, 0, ddsTextureData.Length);

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
                mipMapResolutions =
                    DDS.CalculateMipResolutions(header.dwMipMapCount - 1, header.dwWidth, header.dwHeight);

                //TODO CALCULATE BLOCKSIZE CORRECTLY
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

            byte[] finalData = Array.Empty<byte>();

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
            return faceIndex switch
            {
                0 => "XPOS",
                1 => "XNEG",
                2 => "YPOS",
                3 => "YNEG",
                4 => "ZPOS",
                5 => "ZNEG",
                _ => ""
            };
        }

        /// <summary>
        /// Writes a D3DTX into a DDS file on the disk.
        /// </summary>
        /// <param name="d3dtx"></param>
        /// <param name="destinationPath"></param>
        public void Write_D3DTX_AsDDS(D3DTX_Master d3dtx, string destinationDirectory)
        {
            string d3dtxFilePath = d3dtx.filePath;
            string fileName = Path.GetFileNameWithoutExtension(d3dtxFilePath);

            if (d3dtx.IsCubeTexture())
            {
                int regionCount = d3dtx.GetRegionCount();
                int mipCount = (int)d3dtx.GetMipMapCount();
                int cubeSurfacesAmount = regionCount / mipCount;

                string newCubeDirectory = destinationDirectory + Path.DirectorySeparatorChar + fileName + Path.DirectorySeparatorChar;

                if (!Directory.Exists(newCubeDirectory))
                {
                    Directory.CreateDirectory(newCubeDirectory);
                }

                for (int i = 0; i < cubeSurfacesAmount; i++)
                {
                    string cubeFaceName = GetCubeFaceName(i);

                    string newFileName = string.Format("{0}{1}_Cube{2}{3}", newCubeDirectory, fileName, cubeFaceName,
                        Main_Shared.ddsExtension);

                    //turn our header data into bytes to be written into a file
                    byte[] dds_header =
                        ByteFunctions.Combine(ByteFunctions.GetBytes("DDS "), DDS.GetHeaderBytes(header));

                    //copy the dds header to the file
                    byte[] finalData = Array.Empty<byte>();
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

                if (header.ddspf.dwFourCC == ByteFunctions.Convert_String_To_UInt32("DX10"))
                {
                    dds_header = ByteFunctions.Combine(dds_header, DDS.GetDXT10HeaderBytes(dxt10_header));
                }

                //copy the dds header to the file
                byte[] finalData = Array.Empty<byte>();
                finalData = ByteFunctions.Combine(finalData, dds_header);

                List<byte[]> pixelData = d3dtx.GetPixelData();

                //copy the images
                for (int i = pixelData.Count - 1; i >= 0; i--)
                {
                    finalData = ByteFunctions.Combine(finalData, pixelData[i]);
                }

                //write the file to the disk
                string newDDSPath = destinationDirectory + Path.DirectorySeparatorChar + fileName +
                                    Main_Shared.ddsExtension;
                File.WriteAllBytes(newDDSPath, finalData);
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

        public byte[] GetData(D3DTX_Master d3dtx)
        {
            if (d3dtx.IsCubeTexture())
            {
                int regionCount = d3dtx.GetRegionCount();
                int mipCount = (int)d3dtx.GetMipMapCount();
                int cubeSurfacesAmount = regionCount / mipCount;


                for (int i = 0; i < cubeSurfacesAmount; i++)
                {
                    string cubeFaceName = GetCubeFaceName(i);

                    //turn our header data into bytes to be written into a file
                    byte[] dds_header =
                        ByteFunctions.Combine(ByteFunctions.GetBytes("DDS "), DDS.GetHeaderBytes(header));

                    //copy the dds header to the file
                    byte[] finalData = Array.Empty<byte>();
                    finalData = ByteFunctions.Combine(finalData, dds_header);

                    List<byte[]> pixelData = d3dtx.GetPixelDataByFaceIndex(i);

                    //copy the images
                    for (int x = mipCount - 1; x >= 0; x--)
                    {
                        //int newPixelDataIndex = startingPixelDataIndex + x;
                        finalData = ByteFunctions.Combine(finalData, pixelData[x]);
                    }

                    return finalData;
                }
            }
            else
            {
                //turn our header data into bytes to be written into a file
                byte[] dds_header = ByteFunctions.Combine(ByteFunctions.GetBytes("DDS "), DDS.GetHeaderBytes(header));
                if (header.ddspf.dwFourCC == ByteFunctions.Convert_String_To_UInt32("DX10"))
                {
                    dds_header = ByteFunctions.Combine(dds_header, DDS.GetDXT10HeaderBytes(dxt10_header));
                }

                //copy the dds header to the file
                byte[] finalData = Array.Empty<byte>();
                finalData = ByteFunctions.Combine(finalData, dds_header);
                Console.WriteLine(finalData.Length);
                List<byte[]> pixelData = d3dtx.GetPixelData();

                //copy the images
                for (int i = pixelData.Count - 1; i >= 0; i--)
                {
                    finalData = ByteFunctions.Combine(finalData, pixelData[i]);
                }
                Console.WriteLine(finalData.Length);
                return finalData;
                //write the file to the disk
            }

            return null;
        }
    }
}