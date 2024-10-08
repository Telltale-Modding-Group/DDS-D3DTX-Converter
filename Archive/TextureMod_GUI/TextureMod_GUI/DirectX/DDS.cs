using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DirectXTexNet;

namespace D3DTX_TextureConverter.DirectX
{
    //DDS Docs - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-header
    //DDS PIXEL FORMAT - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-pixelformat
    //DDS DDS_HEADER_DXT10 - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-header-dxt10
    //DDS File Layout https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-file-layout-for-textures
    //Texutre Block Compression in D3D11 - https://docs.microsoft.com/en-us/windows/win32/direct3d11/texture-block-compression-in-direct3d-11
    //DDS Programming Guide - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dx-graphics-dds-pguide

    /// <summary>
    /// Describes a DDS file header.
    /// </summary>
    public struct DDS_HEADER
    {
        /// <summary>
        /// [4 bytes] Size of structure. This member must be set to 124.
        /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
        /// </summary>
        public uint dwSize { get; set; }

        /// <summary>
        /// [4 bytes] Flags to indicate which members contain valid data.
        /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
        /// </summary>
        public uint dwFlags { get; set; }

        /// <summary>
        /// [4 bytes] Surface height (in pixels).
        /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
        /// </summary>
        public uint dwHeight { get; set; }

        /// <summary>
        /// [4 bytes] Surface width (in pixels).
        /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
        /// </summary>
        public uint dwWidth { get; set; }

        /// <summary>
        /// [4 bytes] The pitch or number of bytes per scan line in an uncompressed texture.
        /// <para>The total number of bytes in the top level texture for a compressed texture.</para>
        /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
        /// </summary>
        public uint dwPitchOrLinearSize { get; set; }

        /// <summary>
        /// [4 bytes] Depth of a volume texture (in pixels), otherwise unused.
        /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
        /// </summary>
        public uint dwDepth { get; set; }

        /// <summary>
        /// [4 bytes] Number of mipmap levels, otherwise unused.
        /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
        /// </summary>
        public uint dwMipMapCount { get; set; }

        /// <summary>
        /// [4 bytes] Unused according to DDS docs.
        /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
        /// </summary>
        public uint dwReserved1 { get; set; }

        /// <summary>
        /// [4 bytes] Unused according to DDS docs.
        /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
        /// </summary>
        public uint dwReserved2 { get; set; }

        /// <summary>
        /// [4 bytes] Unused according to DDS docs.
        /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
        /// </summary>
        public uint dwReserved3 { get; set; }

        /// <summary>
        /// [4 bytes] Unused according to DDS docs.
        /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
        /// </summary>
        public uint dwReserved4 { get; set; }

        /// <summary>
        /// [4 bytes] Unused according to DDS docs.
        /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
        /// </summary>
        public uint dwReserved5 { get; set; }

        /// <summary>
        /// [4 bytes] Unused according to DDS docs.
        /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
        /// </summary>
        public uint dwReserved6 { get; set; }

        /// <summary>
        /// [4 bytes] Unused according to DDS docs.
        /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
        /// </summary>
        public uint dwReserved7 { get; set; }

        /// <summary>
        /// [4 bytes] Unused according to DDS docs.
        /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
        /// </summary>
        public uint dwReserved8 { get; set; }

        /// <summary>
        /// [4 bytes] Unused according to DDS docs.
        /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
        /// </summary>
        public uint dwReserved9 { get; set; }

        /// <summary>
        /// [4 bytes] Unused according to DDS docs.
        /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
        /// </summary>
        public uint dwReserved10 { get; set; }

        /// <summary>
        /// [4 bytes] Unused according to DDS docs.
        /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
        /// </summary>
        public uint dwReserved11 { get; set; }

        /// <summary>
        /// [36 bytes] Surface pixel format.
        /// </summary>
        public DDS_PIXELFORMAT ddspf { get; set; }

        /// <summary>
        /// [4 bytes] Specifies the complexity of the surfaces stored.
        /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
        /// </summary>
        public uint dwCaps { get; set; }

        /// <summary>
        /// [4 bytes] Additional detail about the surfaces stored.
        /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
        /// </summary>
        public uint dwCaps2 { get; set; }

        /// <summary>
        /// [4 bytes] Unused according to DDS docs.
        /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
        /// </summary>
        public uint dwCaps3 { get; set; }

        /// <summary>
        /// [4 bytes] Unused according to DDS docs.
        /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
        /// </summary>
        public uint dwCaps4 { get; set; }

        /// <summary>
        /// [4 bytes] Unused according to DDS docs.
        /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
        /// </summary>
        public uint dwReserved12 { get; set; }
    }

    /// <summary>
    /// Surface pixel format.
    /// </summary>
    public struct DDS_PIXELFORMAT
    {
        /// <summary>
        /// [4 bytes] Structure size; set to 32 (bytes).
        /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
        /// </summary>
        public uint dwSize { get; set; }

        /// <summary>
        /// [4 bytes] Values which indicate what type of data is in the surface.
        /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
        /// </summary>
        public uint dwFlags { get; set; }

        /// <summary>
        /// [4 bytes] Four-character codes for specifying compressed or custom formats.
        /// <para>Possible values include: DXT1, DXT2, DXT3, DXT4, or DXT5.</para>
        /// <para>A FourCC of DX10 indicates the prescense of the DDS_HEADER_DXT10 extended header, and the dxgiFormat member of that structure indicates the true format.</para>
        /// <para>When using a four-character code, dwFlags must include DDPF_FOURCC.</para>
        /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
        /// </summary>
        public uint dwFourCC { get; set; }

        /// <summary>
        /// [4 bytes] Number of bits in an RGB (possibly including alpha) format.
        /// <para>Valid when dwFlags includes DDPF_RGB, DDPF_LUMINANCE, or DDPF_YUV.</para>
        /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
        /// </summary>
        public uint dwRGBBitCount { get; set; }

        /// <summary>
        /// [4 bytes] Red (or lumiannce or Y) mask for reading color data.
        /// <para>For instance, given the A8R8G8B8 format, the red mask would be 0x00ff0000.</para>
        /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
        /// </summary>
        public uint dwRBitMask { get; set; }

        /// <summary>
        /// [4 bytes] Green (or U) mask for reading color data.
        /// <para>For instance, given the A8R8G8B8 format, the green mask would be 0x0000ff00.</para>
        /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
        /// </summary>
        public uint dwGBitMask { get; set; }

        /// <summary>
        /// [4 bytes] Blue (or V) mask for reading color data.
        /// <para>For instance, given the A8R8G8B8 format, the blue mask would be 0x000000ff.</para>
        /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
        /// </summary>
        public uint dwBBitMask { get; set; }

        /// <summary>
        /// [4 bytes] Alpha mask for reading alpha data.
        /// <para>dwFlags must include DDPF_ALPHAPIXELS or DDPF_ALPHA. For instance, given the A8R8G8B8 format, the alpha mask would be 0xff000000.</para>
        /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
        /// </summary>
        public uint dwABitMask { get; set; }
    }

    /// <summary>
    /// DDS header extension to handle resource arrays, DXGI pixel formats that don't map to the legacy Microsoft DirectDraw pixel format structures, and additional metadata.
    /// </summary>
    public struct DDS_HEADER_DXT10
    {
        /// <summary>
        /// [4 bytes] The surface pixel format.
        /// </summary>
        public DXGI_FORMAT dxgiFormat { get; set; }

        /// <summary>
        /// [4 bytes] Identifies the type of resource.
        /// </summary>
        public D3D10_RESOURCE_DIMENSION resourceDimension { get; set; }

        /// <summary>
        /// [4 bytes] Identifies other, less common options for resources.
        /// </summary>
        public uint miscFlag { get; set; }

        /// <summary>
        /// [4 bytes] The number of elements in the array.
        /// <para>For a 2D texture that is also a cube-map texture, this number represents the number of cubes.</para>
        /// </summary>
        public uint arraySize { get; set; }

        /// <summary>
        /// [4 bytes] Contains additional metadata (formerly was reserved). 
        /// </summary>
        public uint miscFlags2 { get; set; }
    }

    /// <summary>
    /// Identifies the type of resource being used.
    /// </summary>
    public enum D3D10_RESOURCE_DIMENSION
    {
        D3D10_RESOURCE_DIMENSION_UNKNOWN,
        D3D10_RESOURCE_DIMENSION_BUFFER,
        D3D10_RESOURCE_DIMENSION_TEXTURE1D,
        D3D10_RESOURCE_DIMENSION_TEXTURE2D,
        D3D10_RESOURCE_DIMENSION_TEXTURE3D
    };

    /// <summary>
    /// Identifies the type of resource being used.
    /// </summary>
    public enum D3D11_RESOURCE_DIMENSION
    {
        D3D11_RESOURCE_DIMENSION_UNKNOWN,
        D3D11_RESOURCE_DIMENSION_BUFFER,
        D3D11_RESOURCE_DIMENSION_TEXTURE1D,
        D3D11_RESOURCE_DIMENSION_TEXTURE2D,
        D3D11_RESOURCE_DIMENSION_TEXTURE3D
    };

    /// <summary>
    /// Values which indicate what type of data is in the surface.
    /// </summary>
    [Flags]
    public enum DDPF
    {
        /// <summary>
        /// Texture contains alpha data; dwRGBAlphaBitMask contains valid data.
        /// </summary>
        DDPF_ALPHAPIXELS = 0x1,

        /// <summary>
        /// Used in some older DDS files for alpha channel only uncompressed data (dwRGBBitCount contains the alpha channel bitcount; dwABitMask contains valid data)
        /// </summary>
        DDPF_ALPHA = 0x2,

        /// <summary>
        /// Texture contains compressed RGB data; dwFourCC contains valid data.
        /// </summary>
        DDPF_FOURCC = 0x4,

        /// <summary>
        /// Texture contains uncompressed RGB data; dwRGBBitCount and the RGB masks (dwRBitMask, dwGBitMask, dwBBitMask) contain valid data.
        /// </summary>
        DDPF_RGB = 0x40,

        /// <summary>
        /// Used in some older DDS files for YUV uncompressed data (dwRGBBitCount contains the YUV bit count; dwRBitMask contains the Y mask, dwGBitMask contains the U mask, dwBBitMask contains the V mask)
        /// </summary>
        DDPF_YUV = 0x200,

        /// <summary>
        /// Used in some older DDS files for single channel color uncompressed data (dwRGBBitCount contains the luminance channel bit count; dwRBitMask contains the channel mask). Can be combined with DDPF_ALPHAPIXELS for a two channel DDS file.
        /// </summary>
        DDPF_LUMINANCE = 0x20000
    }

    /// <summary>
    /// Flags to indicate which members contain valid data.
    /// <para>The DDS_HEADER_FLAGS_TEXTURE flag, which is defined in Dds.h, is a bitwise-OR combination of the DDSD_CAPS, DDSD_HEIGHT, DDSD_WIDTH, and DDSD_PIXELFORMAT flags.</para>
    /// </summary>
    [Flags]
    public enum DDSD
    {
        /// <summary>
        /// Required in every .dds file.
        /// </summary>
        DDSD_CAPS = 0x1,

        /// <summary>
        /// Required in every .dds file.
        /// </summary>
        DDSD_HEIGHT = 0x2,

        /// <summary>
        /// Required in every .dds file.
        /// </summary>
        DDSD_WIDTH = 0x4,

        /// <summary>
        /// Required when pitch is provided for an uncompressed texture.
        /// <para>The DDS_HEADER_FLAGS_PITCH flag, which is defined in Dds.h, is equal to the DDSD_PITCH flag.</para>
        /// </summary>
        DDSD_PITCH = 0x8,

        /// <summary>
        /// Required in every .dds file.
        /// </summary>
        DDSD_PIXELFORMAT = 0x1000,

        /// <summary>
        /// Required in a mipmapped texture.
        /// <para>The DDS_HEADER_FLAGS_MIPMAP flag, which is defined in Dds.h, is equal to the DDSD_MIPMAPCOUNT flag.</para>
        /// </summary>
        DDSD_MIPMAPCOUNT = 0x20000,

        /// <summary>
        /// Required when pitch is provided for a compressed texture.
        /// <para>The DDS_HEADER_FLAGS_LINEARSIZE flag, which is defined in Dds.h, is equal to the DDSD_LINEARSIZE flag.</para>
        /// </summary>
        DDSD_LINEARSIZE = 0x80000,

        /// <summary>
        /// Required in a depth texture.
        /// <para>The DDS_HEADER_FLAGS_VOLUME flag, which is defined in Dds.h, is equal to the DDSD_DEPTH flag.</para>
        /// </summary>
        DDSD_DEPTH = 0x800000
    }

    /// <summary>
    /// Identifies other, less common options for resources. 
    /// </summary>
    public enum DDS_RESOURCE
    {
        /// <summary>
        /// Indicates a 2D texture is a cube-map texture.
        /// </summary>
        DDS_RESOURCE_MISC_TEXTURECUBE = 0x4
    }

    public enum DDS_ALPHA
    {
        /// <summary>
        /// Alpha channel content is unknown. This is the value for legacy files, which typically is assumed to be 'straight' alpha.
        /// </summary>
        DDS_ALPHA_MODE_UNKNOWN = 0x0,

        /// <summary>
        /// Any alpha channel content is presumed to use straight alpha.
        /// </summary>
        DDS_ALPHA_MODE_STRAIGHT = 0x1,

        /// <summary>
        /// Any alpha channel content is using premultiplied alpha. The only legacy file formats that indicate this information are 'DX2' and 'DX4'.
        /// </summary>
        DDS_ALPHA_MODE_PREMULTIPLIED = 0x2,

        /// <summary>
        /// Any alpha channel content is all set to fully opaque.
        /// </summary>
        DDS_ALPHA_MODE_OPAQUE = 0x3,

        /// <summary>
        /// Any alpha channel content is being used as a 4th channel and is not intended to represent transparency (straight or premultiplied).
        /// </summary>
        DDS_ALPHA_MODE_CUSTOM = 0x4
    }

    /// <summary>
    /// Specifies the complexity of the surfaces stored.
    /// <para>The DDS_SURFACE_FLAGS_MIPMAP flag, which is defined in Dds.h, is a bitwise-OR combination of the DDSCAPS_COMPLEX and DDSCAPS_MIPMAP flags.</para>
    /// </summary>
    [Flags]
    public enum DDSCAPS
    {
        /// <summary>
        /// Optional; must be used on any file that contains more than one surface (a mipmap, a cubic environment map, or mipmapped volume texture).
        /// <para>The DDS_SURFACE_FLAGS_CUBEMAP flag, which is defined in Dds.h, is equal to the DDSCAPS_COMPLEX flag.</para>
        /// </summary>
        DDSCAPS_COMPLEX = 0x8,

        /// <summary>
        /// Optional; should be used for a mipmap.
        /// </summary>
        DDSCAPS_MIPMAP = 0x400000,

        /// <summary>
        /// Required
        /// <para>The DDS_SURFACE_FLAGS_TEXTURE flag, which is defined in Dds.h, is equal to the DDSCAPS_TEXTURE flag.</para>
        /// </summary>
        DDSCAPS_TEXTURE = 0x1000
    }

    /// <summary>
    /// Additional detail about the surfaces stored.
    /// <para>The DDS_CUBEMAP_ALLFACES flag, which is defined in Dds.h, is a bitwise-OR combination of the DDS_CUBEMAP_POSITIVEX, DDS_CUBEMAP_NEGATIVEX, DDS_CUBEMAP_POSITIVEY, DDS_CUBEMAP_NEGATIVEY, DDS_CUBEMAP_POSITIVEZ, and DDSCAPS2_CUBEMAP_NEGATIVEZ flags.</para>
    /// </summary>
    [Flags]
    public enum DDSCAPS2
    {
        /// <summary>
        /// Required for a cube map.
        /// </summary>
        DDSCAPS2_CUBEMAP = 0x200,

        /// <summary>
        /// Required when these surfaces are stored in a cube map.
        /// <para>The DDS_CUBEMAP_POSITIVEX flag, which is defined in Dds.h, is a bitwise-OR combination of the DDSCAPS2_CUBEMAP and DDSCAPS2_CUBEMAP_POSITIVEX flags.</para>
        /// </summary>
        DDSCAPS2_CUBEMAP_POSITIVEX = 0x400,

        /// <summary>
        /// Required when these surfaces are stored in a cube map.
        /// <para>The DDS_CUBEMAP_NEGATIVEX flag, which is defined in Dds.h, is a bitwise-OR combination of the DDSCAPS2_CUBEMAP and DDSCAPS2_CUBEMAP_NEGATIVEX flags.</para>
        /// </summary>
        DDSCAPS2_CUBEMAP_NEGATIVEX = 0x800,

        /// <summary>
        /// Required when these surfaces are stored in a cube map.
        /// <para>The DDS_CUBEMAP_POSITIVEY flag, which is defined in Dds.h, is a bitwise-OR combination of the DDSCAPS2_CUBEMAP and DDSCAPS2_CUBEMAP_POSITIVEY flags.</para>
        /// </summary>
        DDSCAPS2_CUBEMAP_POSITIVEY = 0x1000,

        /// <summary>
        /// Required when these surfaces are stored in a cube map.
        /// <para>The DDS_CUBEMAP_NEGATIVEY flag, which is defined in Dds.h, is a bitwise-OR combination of the DDSCAPS2_CUBEMAP and DDSCAPS2_CUBEMAP_NEGATIVEY flags.</para>
        /// </summary>
        DDSCAPS2_CUBEMAP_NEGATIVEY = 0x2000,

        /// <summary>
        /// Required when these surfaces are stored in a cube map.
        /// <para>The DDS_CUBEMAP_POSITIVEZ flag, which is defined in Dds.h, is a bitwise-OR combination of the DDSCAPS2_CUBEMAP and DDSCAPS2_CUBEMAP_POSITIVEZ flags.</para>
        /// </summary>
        DDSCAPS2_CUBEMAP_POSITIVEZ = 0x4000,

        /// <summary>
        /// Required when these surfaces are stored in a cube map.
        /// <para>The DDS_CUBEMAP_NEGATIVEZ flag, which is defined in Dds.h, is a bitwise-OR combination of the DDSCAPS2_CUBEMAP and DDSCAPS2_CUBEMAP_NEGATIVEZ flags.</para>
        /// </summary>
        DDSCAPS2_CUBEMAP_NEGATIVEZ = 0x8000,

        /// <summary>
        /// Required for a volume texture.
        /// <para>The DDS_FLAGS_VOLUME flag, which is defined in Dds.h, is equal to the DDSCAPS2_VOLUME flag.</para>
        /// </summary>
        DDSCAPS2_VOLUME = 0x200000
    }
}
