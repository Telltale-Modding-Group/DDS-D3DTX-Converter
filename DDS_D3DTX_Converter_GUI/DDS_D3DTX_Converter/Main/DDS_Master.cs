using System;
using System.Collections.Generic;
using System.IO;
using D3DTX_Converter.Utilities;
using D3DTX_Converter.DirectX;
using D3DTX_Converter.TelltaleEnums;
using DirectXTexNet;
using System.ComponentModel;
using ExCSS;
using D3DTX_Converter.TelltaleD3DTX;
using System.Linq;
using Pfim;
using CommunityToolkit.Mvvm.ComponentModel.__Internals;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using D3DTX_Converter.TelltaleTypes;

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

        public DDS dds;

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
        /// <param name="ddsFilePath">The file path to the DDS file.</param>
        /// <param name="headerOnly">A boolean that toggles if only the header should be read.</param>
        public DDS_Master(string ddsFilePath, bool headerOnly = false)
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
        /// <param name="data">A byte array of the DDS data.</param>
        /// <param name="headerOnly">A boolean that toggles if only the header should be read.</param>
        public DDS_Master(byte[] data, bool headerOnly = false)
        {
            //get the byte data
            sourceFileData = data;

            //read the DDS file
            GetData(sourceFileData, headerOnly);
        }

        /// <summary>
        /// Create a DDS file from a D3DTX
        /// </summary>
        /// <param name="d3dtx">The D3DTX data that will be used.</param>
        public DDS_Master(D3DTX_Master d3dtx)
        {
            // NOTES: Remember that mip tables are reversed
            // So in that vein cubemap textures are likely in order but reversed
            // Some normal maps specifically with type 4 (eTxNormalMap) channels are all reversed (ABGR instead of RGBA)

            // Initialize the DDS with a defailt header
            dds = new DDS
            {
                header = DDS_HEADER.GetPresetHeader()
            };

            // If the D3DTX is a legacy D3DTX and we don't know from which game it is, we can just get the DDS header the file. This is only for extraction purposes
            if (d3dtx.genericDDS != null)
            {
                dds = d3dtx.genericDDS;
                return;
            }

            // If the D3DTX is a legacy D3DTX (before mVersions exist), we can just get the DDS header from the file
            if (d3dtx.isLegacyD3DTX())
            {
                dds.header = d3dtx.GetDDSHeaderFromLegacyD3DTX();
                return;
            }

            T3SurfaceFormat surfaceFormat = d3dtx.GetCompressionType();
            T3SurfaceGamma surfaceGamma = d3dtx.GetSurfaceGamma();

            // Initialize DDS flags
            dds.header.dwFlags |= d3dtx.GetMipMapCount() > 1 ? (uint)DDSD.MIPMAPCOUNT : 0x0;
            dds.header.dwFlags |= d3dtx.IsTextureCompressed() ? (uint)DDSD.LINEARSIZE : (uint)DDSD.PITCH;
            dds.header.dwFlags |= d3dtx.IsVolumeTexture() ? (uint)DDSD.DEPTH : 0x0;

            // Initialize DDS witdh, height, depth, pitch, mip map count
            dds.header.dwWidth = d3dtx.GetWidth();
            dds.header.dwHeight = d3dtx.GetHeight();
            dds.header.dwPitchOrLinearSize = DDS_HELPER.GetPitchOrLinearSizeFromD3DTX(surfaceFormat, dds.header.dwWidth);
            dds.header.dwDepth = d3dtx.GetDepth();

            // If the texture has more than 1 mipmap, set the mipmap count
            if (d3dtx.GetMipMapCount() > 1)
                dds.header.dwMipMapCount = d3dtx.GetMipMapCount();
            else dds.header.dwMipMapCount = 0;

            // Set the DDS pixel format info
            dds.header.ddspf = GetPixelFormatHeaderFromT3Surface(surfaceFormat);

            // We will enable DX10 header if the format is not a legacy format. Telltale won't use old formats in DirectX 11 and above.
            if (dds.header.ddspf.dwFourCC == ByteFunctions.Convert_String_To_UInt32("DX10") || d3dtx.IsArrayTexture())
            {
                dds.header.ddspf.dwFourCC = ByteFunctions.Convert_String_To_UInt32("DX10");
                dds.dxt10Header = DDS_HEADER_DXT10.GetPresetDXT10Header();

                dds.dxt10Header.dxgiFormat = DDS_HELPER.GetSurfaceFormatAsDXGI(surfaceFormat, surfaceGamma);

                // 1D textures don't exist in Telltale games
                dds.dxt10Header.resourceDimension = d3dtx.IsVolumeTexture() ? D3D10_RESOURCE_DIMENSION.TEXTURE3D : D3D10_RESOURCE_DIMENSION.TEXTURE2D;

                dds.dxt10Header.arraySize = d3dtx.GetArraySize();

                if (d3dtx.IsCubeTexture())
                {
                    dds.dxt10Header.miscFlag |= (uint)DDS_RESOURCE.MISC_TEXTURECUBE;
                }
            }

            // Mandatory flag
            dds.header.dwCaps |= (uint)DDSCAPS.TEXTURE;

            // If the texture has mipmaps, enable the mipmap flags
            if (d3dtx.GetMipMapCount() > 1)
            {
                dds.header.dwCaps |= (uint)DDSCAPS.COMPLEX;
                dds.header.dwCaps |= (uint)DDSCAPS.MIPMAP;
            }

            // If the texture is a cube texture, enable the cube texture flags
            if (d3dtx.IsCubeTexture())
            {
                dds.header.dwCaps |= (uint)DDSCAPS.COMPLEX;

                dds.header.dwCaps2 |= (uint)DDSCAPS2.CUBEMAP;
                dds.header.dwCaps2 |= (uint)DDSCAPS2.CUBEMAP_POSITIVEX;
                dds.header.dwCaps2 |= (uint)DDSCAPS2.CUBEMAP_NEGATIVEX;
                dds.header.dwCaps2 |= (uint)DDSCAPS2.CUBEMAP_POSITIVEY;
                dds.header.dwCaps2 |= (uint)DDSCAPS2.CUBEMAP_NEGATIVEY;
                dds.header.dwCaps2 |= (uint)DDSCAPS2.CUBEMAP_POSITIVEZ;
                dds.header.dwCaps2 |= (uint)DDSCAPS2.CUBEMAP_NEGATIVEZ;
            }
            // If the texture is a volume texture, enable the volume texture flags
            else if (d3dtx.IsVolumeTexture())
            {
                dds.header.dwCaps |= (uint)DDSCAPS.COMPLEX;

                dds.header.dwCaps2 |= (uint)DDSCAPS2.VOLUME;
            }

            // Extract pixel data using streamheaders to make my life easier
            RegionStreamHeader[] streamHeaders = d3dtx.GetRegionStreamHeaders();

            textureData = new List<byte[]>();

            //Extract all pixel data from the stream headers and then convert all arrays to a single array
            var d3dtxTextureData = d3dtx.GetPixelData();
            byte[] d3dtxTextureDataArray = d3dtxTextureData.SelectMany(b => b).ToArray();

            long offset = 0;

            for (int i = 0; i < streamHeaders.Length; i++)
            {
                // Get the pixel data from the stream header
                byte[] data = new byte[streamHeaders[i].mDataSize];
                Array.Copy(d3dtxTextureDataArray, offset, data, 0, streamHeaders[i].mDataSize);

                // Add the region pixel data to the list
                textureData.Add(data);

                // Increment the offset
                offset += streamHeaders[i].mDataSize;
            }
        }

        /// <summary>
        /// Get the DDS Pixelformat data from a Telltale surface format.
        /// </summary>
        /// <param name="surface">The Telltale surface format.</param>
        /// <returns>DDS Pixelformat object with the correct surface format settings.</returns>
        private DDS_PIXELFORMAT GetPixelFormatHeaderFromT3Surface(T3SurfaceFormat surface)
        {
            return surface switch
            {
                // Uncompressed formats
                T3SurfaceFormat.eSurface_ARGB8 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.ALPHAPIXELS, DDPF.RGB), 0, 32, 0xff0000, 0xff00, 0xff, 0xff000000),// 'ARGB8'
                //Needs swizzling if it's used in DirectX 9
                T3SurfaceFormat.eSurface_ARGB16 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), ByteFunctions.Convert_String_To_UInt32("DX10"), 64, 0x00, 0x00, 0x00, 0x00),// 'ARGB16'
                T3SurfaceFormat.eSurface_RGB565 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.RGB), 0, 16, 0x0000F800, 0x000007E0, 0x0000001F, 0x00),// 'RGB565'
                T3SurfaceFormat.eSurface_ARGB1555 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), ByteFunctions.Convert_String_To_UInt32("DX10"), 16, 0x00, 0x00, 0x00, 0x00),// 'ARGB1555'
                T3SurfaceFormat.eSurface_ARGB4 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.ALPHAPIXELS, DDPF.RGB), 0, 16, 0x00000F00, 0x000000F0, 0x0000000F, 0x0000F000),// 'ARGB4'  //Due to a long-standing issue in DDS readers and writers, it's better to use DX10 header for this format
                T3SurfaceFormat.eSurface_ARGB2101010 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.ALPHAPIXELS, DDPF.RGB), 0, 32, 0x3ff00000, 0xffc00, 0x3ff, 0xc0000000),// 'ARGB2101010'   
                //TODO, COULD BE L16
                T3SurfaceFormat.eSurface_R16 => DDS_PIXELFORMAT.Of(32, 0x01, 0, 16, 0xFFFF, 0x00, 0x00, 0x00),// 'R16'
                T3SurfaceFormat.eSurface_RG16 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.ALPHAPIXELS, DDPF.RGB), 0, 32, 0xffff, 0xffff0000, 0x00, 0x00),// 'RG16'
                T3SurfaceFormat.eSurface_RGBA16 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.ALPHAPIXELS, DDPF.FOURCC), 36, 64, 0x00, 0x00, 0x00, 0x00),// 'RGBA16'
                T3SurfaceFormat.eSurface_RG8 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.ALPHAPIXELS, DDPF.RGB), 0, 16, 0xFF00, 0x00FF, 0x00, 0x00),// 'RG8'
                T3SurfaceFormat.eSurface_RGBA8 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.ALPHAPIXELS, DDPF.RGB), 0, 32, 0xFF000000, 0x00FF0000, 0x0000FF00, 0x000000FF),// 'RGBA8'
                T3SurfaceFormat.eSurface_R32 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), ByteFunctions.Convert_String_To_UInt32("DX10"), 32, 0x00, 0x00, 0x00, 0x00),// 'R32'
                T3SurfaceFormat.eSurface_RG32 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), ByteFunctions.Convert_String_To_UInt32("DX10"), 64, 0x00, 0x00, 0x00, 0x00),// 'RG32' 
                T3SurfaceFormat.eSurface_RGBA32 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), ByteFunctions.Convert_String_To_UInt32("DX10"), 128, 0x00, 0x00, 0x00, 0x00),// 'RGBA32'
                T3SurfaceFormat.eSurface_R8 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.ALPHAPIXELS), 0, 8, 0xFF, 0x00, 0x00, 0x00),// 'R8'
                T3SurfaceFormat.eSurface_RGBA8S => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.ALPHAPIXELS, DDPF.RGB), 0, 32, 0xFF000000, 0x00FF0000, 0x0000FF00, 0x000000FF),// 'RGBA8S'
                T3SurfaceFormat.eSurface_A8 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.ALPHAPIXELS), 0, 8, 0x00, 0x00, 0x00, 0xFF),// 'A8'
                T3SurfaceFormat.eSurface_L8 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.LUMINANCE), 0, 8, 0xFF, 0x00, 0x00, 0x00),// 'L8'
                T3SurfaceFormat.eSurface_AL8 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.LUMINANCE), 0, 8, 0x00, 0x00, 0x00, 0xFF),// 'AL8'
                T3SurfaceFormat.eSurface_L16 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.LUMINANCE), 0, 16, 0xFFFF, 0x00, 0x00, 0x00),// 'L16'
                T3SurfaceFormat.eSurface_RG16S => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), ByteFunctions.Convert_String_To_UInt32("DX10"), 32, 0x00, 0x00, 0x00, 0x00),// 'RG16S'
                T3SurfaceFormat.eSurface_RGBA16S => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), ByteFunctions.Convert_String_To_UInt32("DX10"), 64, 0x00, 0x00, 0x00, 0x00),// 'RGBA16S'
                T3SurfaceFormat.eSurface_R16UI => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), ByteFunctions.Convert_String_To_UInt32("DX10"), 16, 0x00, 0x00, 0x00, 0x00),// 'R16UI'
                T3SurfaceFormat.eSurface_RG16UI => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), ByteFunctions.Convert_String_To_UInt32("DX10"), 32, 0x00, 0x00, 0x00, 0x00),// 'RG16UI'
                T3SurfaceFormat.eSurface_RGBA1010102F => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), ByteFunctions.Convert_String_To_UInt32("DX10"), 32, 0, 0, 0, 0),// 'RGBA1010102F'
                T3SurfaceFormat.eSurface_RGB111110F => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), ByteFunctions.Convert_String_To_UInt32("DX10"), 32, 0, 0, 0, 0),// 'RGB111110F'
                T3SurfaceFormat.eSurface_R16F => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), ByteFunctions.Convert_String_To_UInt32("DX10"), 16, 0x00, 0x00, 0x00, 0x00),// 'R16F'
                T3SurfaceFormat.eSurface_RG16F => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), ByteFunctions.Convert_String_To_UInt32("DX10"), 32, 0x00, 0x00, 0x00, 0x00),// 'RG16F'
                T3SurfaceFormat.eSurface_RGBA16F => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), ByteFunctions.Convert_String_To_UInt32("DX10"), 64, 0x00, 0x00, 0x00, 0x00),// 'RGBA16F'
                T3SurfaceFormat.eSurface_R32F => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), ByteFunctions.Convert_String_To_UInt32("DX10"), 32, 0x00, 0x00, 0x00, 0x00),// 'R32F'
                T3SurfaceFormat.eSurface_RG32F => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), ByteFunctions.Convert_String_To_UInt32("DX10"), 64, 0x00, 0x00, 0x00, 0x00),// 'RG32F'
                T3SurfaceFormat.eSurface_RGBA32F => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), ByteFunctions.Convert_String_To_UInt32("DX10"), 128, 0x00, 0x00, 0x00, 0x00),// 'RGBA32F'
                T3SurfaceFormat.eSurface_RGB9E5F => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), ByteFunctions.Convert_String_To_UInt32("DX10"), 32, 0x00, 0x00, 0x00, 0x00),// 'RGB9E5F'  

                // Compressed formats
                T3SurfaceFormat.eSurface_DXT1 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), ByteFunctions.Convert_String_To_UInt32("DXT1"), 0x00, 0x00, 0x00, 0x00, 0x00),// 'DXT1'
                T3SurfaceFormat.eSurface_DXT3 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), ByteFunctions.Convert_String_To_UInt32("DXT3"), 0x00, 0x00, 0x00, 0x00, 0x00),// 'DXT3'
                T3SurfaceFormat.eSurface_DXT5 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), ByteFunctions.Convert_String_To_UInt32("DXT5"), 0x00, 0x00, 0x00, 0x00, 0x00),// 'DXT5'
                T3SurfaceFormat.eSurface_DXT5A => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), ByteFunctions.Convert_String_To_UInt32("ATI1"), 0x00, 0x00, 0x00, 0x00, 0x00),// 'DXT5'
                T3SurfaceFormat.eSurface_DXN => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), ByteFunctions.Convert_String_To_UInt32("ATI2"), 0x00, 0x00, 0x00, 0x00, 0x00),// 'DXN'
                T3SurfaceFormat.eSurface_BC1 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), ByteFunctions.Convert_String_To_UInt32("DXT1"), 0x00, 0x00, 0x00, 0x00, 0x00),// 'BC1
                T3SurfaceFormat.eSurface_BC2 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), ByteFunctions.Convert_String_To_UInt32("DXT3"), 0x00, 0x00, 0x00, 0x00, 0x00),// 'DXT2'
                T3SurfaceFormat.eSurface_BC3 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), ByteFunctions.Convert_String_To_UInt32("DXT5"), 0x00, 0x00, 0x00, 0x00, 0x00),// 'DXT4'
                T3SurfaceFormat.eSurface_BC4 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), ByteFunctions.Convert_String_To_UInt32("DX10"), 0x00, 0x00, 0x00, 0x00, 0x00),// 'BC4U'
                T3SurfaceFormat.eSurface_BC5 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), ByteFunctions.Convert_String_To_UInt32("DX10"), 0x00, 0x00, 0x00, 0x00, 0x00),// 'BC5U'
                T3SurfaceFormat.eSurface_BC6 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), ByteFunctions.Convert_String_To_UInt32("DX10"), 0x00, 0x00, 0x00, 0x00, 0x00),// 'BC6H'
                T3SurfaceFormat.eSurface_BC7 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), ByteFunctions.Convert_String_To_UInt32("DX10"), 0x00, 0x00, 0x00, 0x00, 0x00),// 'BC7U'

                // Unneeded depth conversions. These are probably inaccurate headers if they ever existed
                T3SurfaceFormat.eSurface_DepthPCF16 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), 80, 16, 0x00, 0x00, 0x00, 0x00),// 'DepthPCF16'
                T3SurfaceFormat.eSurface_DepthPCF24 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), 77, 24, 0x00, 0x00, 0x00, 0x00),// 'DepthPCF24'
                T3SurfaceFormat.eSurface_Depth16 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), 80, 16, 0x00, 0x00, 0x00, 0x00),// 'Depth16'
                T3SurfaceFormat.eSurface_Depth24 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), 77, 24, 0x00, 0x00, 0x00, 0x00),// 'Depth24'
                T3SurfaceFormat.eSurface_DepthStencil32 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), 71, 32, 0x00, 0x00, 0x00, 0x00),// 'DepthStencil32'
                T3SurfaceFormat.eSurface_Depth32F => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), ByteFunctions.Convert_String_To_UInt32("DX10"), 0x00, 0x00, 0x00, 0x00, 0x00),// 'Depth32F'
                T3SurfaceFormat.eSurface_Depth32F_Stencil8 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), ByteFunctions.Convert_String_To_UInt32("DX10"), 32, 0x00, 0x00, 0x00, 0x00),// 'Depth32F_Stencil8'
                T3SurfaceFormat.eSurface_Depth24F_Stencil8 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), 83, 32, 0x00, 0x00, 0x00, 0x00),// 'Depth24F_Stencil8'

                // Default to DXT1 Compression
                _ => DDS_PIXELFORMAT.Of(32, 0x04, ByteFunctions.Convert_String_To_UInt32("DXT1"), 0x00, 0x00, 0x00, 0x00, 0x00),// 'DXT1'
            };

        }

        /// <summary>
        /// Get the DDS Pixelformat data from a Telltale surface format. (For preview images, if possible use without DX10 header).
        /// </summary>
        /// <param name="surface">The Telltale surface format.</param>
        /// <returns>DDS Pixelformat object with the correct surface format settings.</returns>
        private DDS_PIXELFORMAT GetPixelFormatHeaderFromT3SurfacePreviewImage(T3SurfaceFormat surface)
        {
            return surface switch
            {
                // Uncompressed formats
                T3SurfaceFormat.eSurface_ARGB8 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.ALPHAPIXELS, DDPF.RGB), 0, 32, 0xff0000, 0xff00, 0xff, 0xff000000),// 'ARGB8'
                //Needs swizzling if it's used in DirectX 9
                T3SurfaceFormat.eSurface_ARGB16 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.ALPHAPIXELS, DDPF.FOURCC), 36, 64, 0x00, 0x00, 0x00, 0x00), // 'ARGB16'
                T3SurfaceFormat.eSurface_RGB565 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.RGB), 0, 16, 0x0000F800, 0x000007E0, 0x0000001F, 0x00),// 'RGB565'
                T3SurfaceFormat.eSurface_ARGB1555 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.ALPHAPIXELS, DDPF.RGB), 0, 16, 0x00007C00, 0x000003E0, 0x0000001F, 0x00008000),// 'ARGB1555'
                T3SurfaceFormat.eSurface_ARGB4 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.ALPHAPIXELS, DDPF.RGB), 0, 16, 0x00000F00, 0x000000F0, 0x0000000F, 0x0000F000),// 'ARGB4' //Due to a long-standing issue in DDS readers and writers, it's better to use DX10 header for this format
                T3SurfaceFormat.eSurface_ARGB2101010 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.ALPHAPIXELS, DDPF.FOURCC, DDPF.RGB), ByteFunctions.Convert_String_To_UInt32("DX10"), 32, 0x3ff00000, 0xffc00, 0x3ff, 0xc0000000),// 'ARGB2101010'    //TODO, COULD BE L16
                T3SurfaceFormat.eSurface_R16 => DDS_PIXELFORMAT.Of(32, 0x01, 0, 16, 0xFFFF, 0x00, 0x00, 0x00),// 'R16'
                T3SurfaceFormat.eSurface_RG16 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.ALPHAPIXELS, DDPF.RGB), 0, 32, 0xffff, 0xffff0000, 0x00, 0x00),// 'RG16'
                T3SurfaceFormat.eSurface_RGBA16 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.ALPHAPIXELS, DDPF.FOURCC), 36, 64, 0x00, 0x00, 0x00, 0x00),// 'RGBA16'
                T3SurfaceFormat.eSurface_RG8 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.ALPHAPIXELS, DDPF.RGB), 0, 16, 0xFF00, 0x00FF, 0x00, 0x00),// 'RG8'
                T3SurfaceFormat.eSurface_RGBA8 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.ALPHAPIXELS, DDPF.RGB), 0, 32, 0xFF000000, 0x00FF0000, 0x0000FF00, 0x000000FF),// 'RGBA8'
                T3SurfaceFormat.eSurface_R32 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), ByteFunctions.Convert_String_To_UInt32("DX10"), 32, 0x00, 0x00, 0x00, 0x00),// 'R32'
                T3SurfaceFormat.eSurface_RG32 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), ByteFunctions.Convert_String_To_UInt32("DX10"), 64, 0x00, 0x00, 0x00, 0x00),// 'RG32' 
                T3SurfaceFormat.eSurface_RGBA32 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), ByteFunctions.Convert_String_To_UInt32("DX10"), 128, 0x00, 0x00, 0x00, 0x00),// 'RGBA32'
                T3SurfaceFormat.eSurface_R8 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.ALPHAPIXELS), 0, 8, 0xFF, 0x00, 0x00, 0x00),// 'R8'
                T3SurfaceFormat.eSurface_RGBA8S => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.ALPHAPIXELS, DDPF.RGB), 0, 32, 0xFF000000, 0x00FF0000, 0x0000FF00, 0x000000FF),// 'RGBA8S'
                T3SurfaceFormat.eSurface_A8 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.ALPHAPIXELS), 0, 8, 0x00, 0x00, 0x00, 0xFF),// 'A8'
                T3SurfaceFormat.eSurface_L8 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.LUMINANCE), 0, 8, 0xFF, 0x00, 0x00, 0x00),// 'L8'
                T3SurfaceFormat.eSurface_AL8 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.LUMINANCE), 0, 8, 0x00, 0x00, 0x00, 0xFF),// 'AL8'
                T3SurfaceFormat.eSurface_L16 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.LUMINANCE), 0, 16, 0xFFFF, 0x00, 0x00, 0x00),// 'L16'
                T3SurfaceFormat.eSurface_RG16S => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), ByteFunctions.Convert_String_To_UInt32("DX10"), 32, 0x00, 0x00, 0x00, 0x00),// 'RG16S'
                T3SurfaceFormat.eSurface_RGBA16S => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), 110, 64, 0x00, 0x00, 0x00, 0x00),// 'RGBA16S'
                T3SurfaceFormat.eSurface_R16UI => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), ByteFunctions.Convert_String_To_UInt32("DX10"), 16, 0x00, 0x00, 0x00, 0x00),// 'R16UI'
                T3SurfaceFormat.eSurface_RG16UI => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), ByteFunctions.Convert_String_To_UInt32("DX10"), 32, 0x00, 0x00, 0x00, 0x00),// 'RG16UI'
                T3SurfaceFormat.eSurface_RGBA1010102F => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), ByteFunctions.Convert_String_To_UInt32("DX10"), 32, 0, 0, 0, 0),// 'RGBA1010102F'
                T3SurfaceFormat.eSurface_RGB111110F => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), ByteFunctions.Convert_String_To_UInt32("DX10"), 32, 0, 0, 0, 0),// 'RGB111110F'
                T3SurfaceFormat.eSurface_R16F => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), 111, 16, 0x00, 0x00, 0x00, 0x00),// 'R16F'
                T3SurfaceFormat.eSurface_RG16F => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), 112, 32, 0x00, 0x00, 0x00, 0x00),// 'RG16F'
                T3SurfaceFormat.eSurface_RGBA16F => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), 113, 64, 0x00, 0x00, 0x00, 0x00),// 'RGBA16F'
                T3SurfaceFormat.eSurface_R32F => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), 114, 32, 0x00, 0x00, 0x00, 0x00),// 'R32F'
                T3SurfaceFormat.eSurface_RG32F => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), 115, 64, 0x00, 0x00, 0x00, 0x00),// 'RG32F'
                T3SurfaceFormat.eSurface_RGBA32F => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), 116, 128, 0x00, 0x00, 0x00, 0x00),// 'RGBA32F'
                T3SurfaceFormat.eSurface_RGB9E5F => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), ByteFunctions.Convert_String_To_UInt32("DX10"), 32, 0x00, 0x00, 0x00, 0x00),// 'RGB9E5F'  

                // Compressed formats
                T3SurfaceFormat.eSurface_DXT1 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), ByteFunctions.Convert_String_To_UInt32("DXT1"), 0x00, 0x00, 0x00, 0x00, 0x00),// 'DXT1'
                T3SurfaceFormat.eSurface_DXT3 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), ByteFunctions.Convert_String_To_UInt32("DXT3"), 0x00, 0x00, 0x00, 0x00, 0x00),// 'DXT3'
                T3SurfaceFormat.eSurface_DXT5 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), ByteFunctions.Convert_String_To_UInt32("DXT5"), 0x00, 0x00, 0x00, 0x00, 0x00),// 'DXT5'
                T3SurfaceFormat.eSurface_DXT5A => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), ByteFunctions.Convert_String_To_UInt32("ATI1"), 0x00, 0x00, 0x00, 0x00, 0x00),// 'DXT5'
                T3SurfaceFormat.eSurface_DXN => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), ByteFunctions.Convert_String_To_UInt32("ATI2"), 0x00, 0x00, 0x00, 0x00, 0x00),// 'DXN'
                T3SurfaceFormat.eSurface_BC1 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), ByteFunctions.Convert_String_To_UInt32("DXT1"), 0x00, 0x00, 0x00, 0x00, 0x00),// 'BC1
                T3SurfaceFormat.eSurface_BC2 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), ByteFunctions.Convert_String_To_UInt32("DXT3"), 0x00, 0x00, 0x00, 0x00, 0x00),// 'DXT2'
                T3SurfaceFormat.eSurface_BC3 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), ByteFunctions.Convert_String_To_UInt32("DXT5"), 0x00, 0x00, 0x00, 0x00, 0x00),// 'DXT4'
                T3SurfaceFormat.eSurface_BC4 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), ByteFunctions.Convert_String_To_UInt32("DX10"), 0x00, 0x00, 0x00, 0x00, 0x00),// 'BC4U'
                T3SurfaceFormat.eSurface_BC5 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), ByteFunctions.Convert_String_To_UInt32("DX10"), 0x00, 0x00, 0x00, 0x00, 0x00),// 'BC5U'
                T3SurfaceFormat.eSurface_BC6 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), ByteFunctions.Convert_String_To_UInt32("DX10"), 0x00, 0x00, 0x00, 0x00, 0x00),// 'BC6H'
                T3SurfaceFormat.eSurface_BC7 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), ByteFunctions.Convert_String_To_UInt32("DX10"), 0x00, 0x00, 0x00, 0x00, 0x00),// 'BC7U'

                // Unneeded depth conversions. These are probably inaccurate headers if they ever existed
                T3SurfaceFormat.eSurface_DepthPCF16 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), 80, 16, 0x00, 0x00, 0x00, 0x00),// 'DepthPCF16'
                T3SurfaceFormat.eSurface_DepthPCF24 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), 77, 24, 0x00, 0x00, 0x00, 0x00),// 'DepthPCF24'
                T3SurfaceFormat.eSurface_Depth16 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), 80, 16, 0x00, 0x00, 0x00, 0x00),// 'Depth16'
                T3SurfaceFormat.eSurface_Depth24 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), 77, 24, 0x00, 0x00, 0x00, 0x00),// 'Depth24'
                T3SurfaceFormat.eSurface_DepthStencil32 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), 71, 32, 0x00, 0x00, 0x00, 0x00),// 'DepthStencil32'
                T3SurfaceFormat.eSurface_Depth32F => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), ByteFunctions.Convert_String_To_UInt32("DX10"), 0x00, 0x00, 0x00, 0x00, 0x00),// 'Depth32F'
                T3SurfaceFormat.eSurface_Depth32F_Stencil8 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), ByteFunctions.Convert_String_To_UInt32("DX10"), 32, 0x00, 0x00, 0x00, 0x00),// 'Depth32F_Stencil8'
                T3SurfaceFormat.eSurface_Depth24F_Stencil8 => DDS_PIXELFORMAT.Of(32, SetDDPFFlags(DDPF.FOURCC), 83, 32, 0x00, 0x00, 0x00, 0x00),// 'Depth24F_Stencil8'

                // Default to DXT1 Compression
                _ => DDS_PIXELFORMAT.Of(32, 0x04, ByteFunctions.Convert_String_To_UInt32("DXT1"), 0x00, 0x00, 0x00, 0x00, 0x00),// 'DXT1'
            };

        }

        /// <summary>
        /// Set the DDS Pixel Format flags with a bitwise-OR operation.
        /// </summary>
        /// <param name="flags">The byte flags.</param>
        /// <returns>The final byte flag.</returns>
        private uint SetDDPFFlags(params DDPF[] flags)
        {
            uint result = 0;

            foreach (DDPF flag in flags)
            {
                result |= (uint)flag;
            }

            return result;
        }

        /// <summary>
        /// Parses the data from a DDS byte array. (Can also read just the header only)
        /// </summary>
        /// <param name="fileData"></param>
        /// <param name="headerOnly"></param>
        private void GetData(byte[] fileData, bool headerOnly = false)
        {
            Console.WriteLine("Total Source Texture Byte Size = {0}", fileData.Length);

            //which byte offset we are on for the source texture (will be changed as we go through the file)
            byte[] headerBytes = ByteFunctions.AllocateBytes(124, fileData, 4); //skip past the 'DDS '

            dds = new DDS();

            //this will automatically read all of the byte data in the header
            dds.header = DDS_HEADER.GetHeaderFromBytes(headerBytes);

            Console.WriteLine("DDS Height = {0}", dds.header.dwHeight);
            Console.WriteLine("DDS Width = {0}", dds.header.dwWidth);
            Console.WriteLine("DDS Mip Map Count = {0}", dds.header.dwMipMapCount);
            Console.WriteLine("DDS Compression = {0}", dds.header.ddspf.dwFourCC);

            //get dxt10 header if it exists
            if (dds.header.ddspf.dwFourCC == ByteFunctions.Convert_String_To_UInt32("DX10"))
            {
                byte[] dxt10headerBytes = ByteFunctions.AllocateBytes(20, fileData, 128); //skip the main header
                dds.dxt10Header = DDS_HEADER_DXT10.GetHeaderFromBytes(dxt10headerBytes);
            }

            if (headerOnly)
                return;

            //--------------------------EXTRACT DDS TEXTURE DATA--------------------------
            //calculate dds header length (we add 4 because we skipped the 4 bytes which contain the ddsPrefix, it isn't necessary to parse this data)
            uint ddsHeaderLength = 4 + dds.header.dwSize;

            //if dxt10Header is present, add additional 20 bytes
            uint dxt10HeaderLength = (uint)((dds.header.ddspf.dwFourCC == ByteFunctions.Convert_String_To_UInt32("DX10")) ? 20 : 0);

            //calculate the length of just the dds texture data
            uint ddsTextureDataLength = (uint)sourceFileData.Length - ddsHeaderLength - dxt10HeaderLength;

            //allocate a byte array of dds texture length
            byte[] ddsTextureData = new byte[ddsTextureDataLength];

            //copy the data from the source byte array past the header (so we are only getting texture data)
            Array.Copy(sourceFileData, ddsHeaderLength + dxt10HeaderLength, ddsTextureData, 0, ddsTextureData.Length);

            textureData = [];

            //NOTE: MIPMAPS CAN EXIST IN CUBE TEXTURES AND ARRAY TEXTURES
            //if there are no mip maps. Works for all types of textures
            if (dds.header.dwMipMapCount <= 1)
            {
                textureData.Add(ddsTextureData);
                Console.WriteLine("DDS Texture Byte Size = {0}", textureData[0].Length);
            }
            else //if there are mip maps
            {
                //get mip resolutions
                //calculated mip resolutions [Pixel Value, Width or Height (0 or 1)]
                mipMapResolutions =
                    DDS_HELPER.CalculateMipResolutions(dds.header.dwMipMapCount, dds.header.dwWidth, dds.header.dwHeight);

                uint byteSize;
                bool isCompressed = false;
                bool isFormatLegacy = true;
                uint format = dds.header.ddspf.dwFourCC;

                if (dds.header.ddspf.dwFourCC == ByteFunctions.Convert_String_To_UInt32("DX10"))
                {
                    format = (uint)dds.dxt10Header.dxgiFormat;
                    isFormatLegacy = false;
                }

                //check if it's compressed
                //get the block size if true
                //get the pixel size if false
                if (DDS_HELPER.IsTextureFormatCompressed(format))
                {
                    isCompressed = true;
                    byteSize = DDS_HELPER.GetDDSBlockSize(dds.header, dds.dxt10Header);
                }
                else
                {
                    if (isFormatLegacy)
                    {
                        byteSize = DDS_HELPER.GetD3D9FORMATBitsPerPifxel((D3DFORMAT)dds.header.ddspf.dwFourCC) / 8; //this needs reformating
                    }
                    else
                    {
                        byteSize = DDS_HELPER.GetDXGIBitsPerPixel(dds.dxt10Header.dxgiFormat) / 8;
                    }
                }

                uint[] byteSizes = DDS_HELPER.GetImageByteSizes(mipMapResolutions, dds.header.dwPitchOrLinearSize, byteSize, isCompressed);

                int offset = 0;

                //if it's a 2d texture
                if (dds.header.dwCaps2 == 0)
                {

                    //get the mipmaps from larger to smaller
                    for (int i = 0; i < byteSizes.Length; i++)
                    {
                        //get the largest mipmap
                        byte[] temp = new byte[byteSizes[i]];

                        //issue length
                        Array.Copy(ddsTextureData, offset, temp, 0, temp.Length);

                        offset += temp.Length;

                        textureData.Add(temp);
                    }

                    //reverse the mipmaps to match the Telltale type
                    textureData.Reverse();
                }
                // if it's a cubemap texture
                else if ((dds.header.dwCaps2 & (uint)DDSCAPS2.CUBEMAP) != 0)
                {
                    for (int faceIndex = 0; faceIndex < 6; faceIndex++)
                    {
                        for (int i = 0; i < byteSizes.Length; i++)
                        {
                            //get the largest mipmap
                            byte[] temp = new byte[byteSizes[i]];

                            //issue length
                            Array.Copy(ddsTextureData, offset, temp, 0, temp.Length);

                            offset += temp.Length;

                            textureData.Add(temp);
                        }
                    }

                    List<byte[]> tempData = [];

                    for (int i = 0; i < 6; i++)
                    {
                        for (int j = 0; j < dds.header.dwMipMapCount; j++)
                        {
                            tempData.Add(textureData[i + (int)(j * dds.header.dwMipMapCount)]);
                        }
                    }

                    textureData.Reverse();
                }
                //UNFINISHED
                else if ((dds.header.dwCaps2 & (uint)DDSCAPS2.VOLUME) != 0)
                {
                    //TODO RENAME SOME VARIABLES
                    mipMapResolutions = DDS_HELPER.CalculateVolumeMipResolutions(dds.header.dwMipMapCount, dds.header.dwWidth, dds.header.dwHeight, dds.header.dwDepth);
                    byteSizes = DDS_HELPER.GetImageByteSizes(mipMapResolutions, dds.header.dwPitchOrLinearSize, byteSize, isCompressed);

                    for (int i = 0; i < byteSizes.Length; i++)
                    {
                        byte[] temp = new byte[byteSizes[i]];

                        //issue length
                        Array.Copy(ddsTextureData, offset, temp, 0, temp.Length);

                        offset += temp.Length;

                        textureData.Add(temp);
                    }

                    int faceCountIndex = (int)DDS_HELPER.GetVolumeFaceCount(dds.header.dwDepth, dds.header.dwMipMapCount) - 1;
                    uint depthCopy = dds.header.dwDepth;

                    for (int i = 0; i < dds.header.dwMipMapCount; i++)
                    {
                        List<byte[]> tempData = [];
                        for (int j = 0; j < depthCopy; j++)
                        {
                            tempData.Add(textureData[faceCountIndex]);
                            faceCountIndex--;
                        }
                        tempData.Reverse();
                        textureData.InsertRange(0, tempData);

                        depthCopy = Math.Max(1, depthCopy / 2);
                    }
                }

            }
        }

        /// <summary>
        /// Writes a D3DTX into a DDS file on the disk.
        /// </summary>
        /// <param name="d3dtx"></param>
        /// <param name="destinationPath"></param>
        public void WriteD3DTXAsDDS(D3DTX_Master d3dtx, string destinationDirectory)
        {
            string d3dtxFilePath = d3dtx.filePath;
            string fileName = Path.GetFileNameWithoutExtension(d3dtxFilePath);

            //turn our header data into bytes to be written into a file
            byte[] dds_header = ByteFunctions.Combine(ByteFunctions.GetBytes(DDS.MAGIC_WORD), DDS_HELPER.GetHeaderBytes(dds.header));

            int arraySize = 1;

            if (dds.header.ddspf.dwFourCC == ByteFunctions.Convert_String_To_UInt32(DDS.DX10_FOURCC))
            {
                dds_header = ByteFunctions.Combine(dds_header, DDS_HELPER.GetDXT10HeaderBytes(dds.dxt10Header));
                arraySize = (int)dds.dxt10Header.arraySize;
            }

            byte[] finalData = Array.Empty<byte>();
            finalData = ByteFunctions.Combine(finalData, dds_header);

            string newDDSPath = destinationDirectory + Path.DirectorySeparatorChar + fileName +
                                 Main_Shared.ddsExtension;

            if (d3dtx.isLegacyD3DTX())
            {
                var legacyByteArray = d3dtx.GetPixelData().SelectMany(b => b).ToArray();
                finalData = ByteFunctions.Combine(finalData, legacyByteArray);
                File.WriteAllBytes(newDDSPath, finalData);
                return;
            }

            if (d3dtx.IsArrayTexture() && d3dtx.IsCubeTexture())
            {
                throw new NotSupportedException("Array Cubemaps are not supported yet.");
            }

            if (d3dtx.IsCubeTexture())
            {
                int regionCount = d3dtx.GetRegionCount(); //24
                uint mipCount = dds.header.dwMipMapCount > 1 ? dds.header.dwMipMapCount : 1; //4

                if (regionCount > mipCount * 6 * arraySize)
                {
                    throw new Exception("Region count is greater than mip count times 6. This is not a valid cube texture.");
                }

                List<byte[]> newPixelData = textureData;

                int regionIndex;

                for (int i = 0; i < arraySize; i++)
                {
                    for (int f = 0; f < 6; f++)
                    {
                        regionIndex = i;

                        List<byte[]> cubeSingleFaceTexture = [];

                        for (int j = 0; j < mipCount; j++)
                        {
                            cubeSingleFaceTexture.Add(newPixelData[regionIndex + f]);

                            // Increment region index by 6 times the array size to get the next face
                            regionIndex += i + arraySize * 6;
                        }

                        cubeSingleFaceTexture.Reverse();

                        byte[] textureMipMapDataArray = cubeSingleFaceTexture.SelectMany(b => b).ToArray();

                        finalData = ByteFunctions.Combine(finalData, textureMipMapDataArray);
                    }
                }
            }
            //CHECK FOR VOLUME TEXTURES
            //NOT TESTED
            else if (d3dtx.IsVolumeTexture())
            {
                // uint depthCopy = d3dtx.GetDepth();
                // uint mipMap = d3dtx.GetMipMapCount();

                // int faceCountIndex = d3dtx.GetRegionCount() - 1;
                // List<byte[]> pixelData = d3dtx.GetPixelData();

                // for (int i = 0; i < mipMap; i++)
                // {
                //     List<byte[]> tempData = [];
                //     for (int j = 0; j < depthCopy; j++)
                //     {
                //         tempData.Add(pixelData[faceCountIndex]);
                //         faceCountIndex--;
                //     }
                //     tempData.Reverse();

                //     finalData = ByteFunctions.Combine(finalData, tempData.SelectMany(x => x).ToArray());

                //     depthCopy = Math.Max(1, depthCopy / 2);
                // }

                List<byte[]> newPixelData = textureData;
                newPixelData.Reverse();

                //copy the images
                for (int i = 0; i <= newPixelData.Count; i++)
                {
                    finalData = ByteFunctions.Combine(finalData, newPixelData[i]);
                }
            }
            else
            {
                List<byte[]> newPixelData = textureData;

                int regionIndex;

                for (int i = 0; i < arraySize; i++)
                {
                    regionIndex = i;
                    List<byte[]> textureMipMapData = [];

                    uint mipCount = dds.header.dwMipMapCount > 1 ? dds.header.dwMipMapCount : 1;

                    for (int j = 0; j < mipCount; j++)
                    {
                        textureMipMapData.Add(newPixelData[regionIndex]);

                        regionIndex += arraySize;
                    }

                    textureMipMapData.Reverse();

                    byte[] textureMipMapDataArray = textureMipMapData.SelectMany(b => b).ToArray();

                    finalData = ByteFunctions.Combine(finalData, textureMipMapDataArray);
                }
            }

            File.WriteAllBytes(newDDSPath, finalData);
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
            if (d3dtx.genericDDS != null)
            {
                //Return DDS byte array
                return dds.GetBytes();
            }

            byte[] dds_header = ByteFunctions.Combine(ByteFunctions.GetBytes(DDS.MAGIC_WORD), DDS_HELPER.GetHeaderBytes(dds.header));

            int arraySize = 1;

            if (dds.header.ddspf.dwFourCC == ByteFunctions.Convert_String_To_UInt32(DDS.DX10_FOURCC))
            {
                dds_header = ByteFunctions.Combine(dds_header, DDS_HELPER.GetDXT10HeaderBytes(dds.dxt10Header));
                arraySize = (int)dds.dxt10Header.arraySize;
            }

            byte[] finalData = Array.Empty<byte>();
            finalData = ByteFunctions.Combine(finalData, dds_header);

            if (d3dtx.IsArrayTexture() && d3dtx.IsCubeTexture())
            {
                throw new NotSupportedException("Array Cubemaps are not supported yet.");
            }

            if (d3dtx.IsCubeTexture())
            {
                int regionCount = d3dtx.GetRegionCount(); //24
                int mipCount = (int)d3dtx.GetMipMapCount(); //4
                if (regionCount > mipCount * 6)
                {
                    throw new Exception("Region count is greater than mip count times 6. This is not a valid cube texture.");
                }

                int cubeSurfacesAmount = regionCount / mipCount;

                List<byte[]> pixelData = d3dtx.GetPixelData();

                for (int i = 0; i < arraySize; i++) //need to check order of the cubemap in arrays...
                {
                    List<byte[]> posX = d3dtx.GetPixelDataByFaceIndex(0);
                    List<byte[]> negX = d3dtx.GetPixelDataByFaceIndex(1);
                    List<byte[]> posY = d3dtx.GetPixelDataByFaceIndex(2);
                    List<byte[]> negY = d3dtx.GetPixelDataByFaceIndex(3);
                    List<byte[]> posZ = d3dtx.GetPixelDataByFaceIndex(4);
                    List<byte[]> negZ = d3dtx.GetPixelDataByFaceIndex(5);

                    finalData = ByteFunctions.CombineCubeface(finalData, posX);
                    finalData = ByteFunctions.CombineCubeface(finalData, negX);
                    finalData = ByteFunctions.CombineCubeface(finalData, posY);
                    finalData = ByteFunctions.CombineCubeface(finalData, negY);
                    finalData = ByteFunctions.CombineCubeface(finalData, posZ);
                    finalData = ByteFunctions.CombineCubeface(finalData, negZ);
                }
            }
            else
            {
                List<byte[]> pixelData = d3dtx.GetPixelData();

                //copy the images
                for (int i = pixelData.Count - 1; i >= 0; i--)
                {
                    finalData = ByteFunctions.Combine(finalData, pixelData[i]);
                }
            }

            return finalData;
        }

        /// <summary>
        /// Converts a DXGI format to an sRGB format.
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public static DXGI_FORMAT MakeSRGB(DXGI_FORMAT format)
        {
            return format switch
            {
                DXGI_FORMAT.R8G8B8A8_UNORM => DXGI_FORMAT.R8G8B8A8_UNORM_SRGB,
                DXGI_FORMAT.BC1_UNORM => DXGI_FORMAT.BC1_UNORM_SRGB,
                DXGI_FORMAT.BC2_UNORM => DXGI_FORMAT.BC2_UNORM_SRGB,
                DXGI_FORMAT.BC3_UNORM => DXGI_FORMAT.BC3_UNORM_SRGB,
                DXGI_FORMAT.B8G8R8A8_UNORM => DXGI_FORMAT.B8G8R8A8_UNORM_SRGB,
                DXGI_FORMAT.B8G8R8X8_UNORM => DXGI_FORMAT.B8G8R8X8_UNORM_SRGB,
                DXGI_FORMAT.BC7_UNORM => DXGI_FORMAT.BC7_UNORM_SRGB,
                _ => format
            };
        }

        /// <summary>
        /// Converts a DXGI format to a linear format.
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public static DXGI_FORMAT MakeLinear(DXGI_FORMAT format)
        {
            return format switch
            {
                DXGI_FORMAT.R8G8B8A8_UNORM_SRGB => DXGI_FORMAT.R8G8B8A8_UNORM,
                DXGI_FORMAT.BC1_UNORM_SRGB => DXGI_FORMAT.BC1_UNORM,
                DXGI_FORMAT.BC2_UNORM_SRGB => DXGI_FORMAT.BC2_UNORM,
                DXGI_FORMAT.BC3_UNORM_SRGB => DXGI_FORMAT.BC3_UNORM,
                DXGI_FORMAT.B8G8R8A8_UNORM_SRGB => DXGI_FORMAT.B8G8R8A8_UNORM,
                DXGI_FORMAT.B8G8R8X8_UNORM_SRGB => DXGI_FORMAT.B8G8R8X8_UNORM,
                DXGI_FORMAT.BC7_UNORM_SRGB => DXGI_FORMAT.BC7_UNORM,
                _ => format
            };
        }
    }
}