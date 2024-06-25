using System;

namespace D3DTX_Converter.DirectX.Enums;

// DDS Docs - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-header
// DDS PIXEL FORMAT - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-pixelformat
// DDS DDS_HEADER_DXT10 - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-header-dxt10
// DDS File Layout https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-file-layout-for-textures
// Texture Block Compression in D3D11 - https://docs.microsoft.com/en-us/windows/win32/direct3d11/texture-block-compression-in-direct3d-11
// DDS Programming Guide - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dx-graphics-dds-pguide
// D3DFORMAT - https://learn.microsoft.com/en-us/windows/win32/direct3d9/d3dformat
// Map Direct3D 9 Formats to Direct3D 10 - https://learn.microsoft.com/en-gb/windows/win32/direct3d10/d3d10-graphics-programming-guide-resources-legacy-formats?redirectedfrom=MSDN

/// <summary>
/// Flags to indicate which members contain valid data.
/// <para>The DDS_HEADER_FLAGS_TEXTURE flag, which is defined in Dds.h, is a bitwise-OR combination of the DDSD_CAPS, DDSD_HEIGHT, DDSD_WIDTH, and DDSD_PIXELFORMAT flags.</para>
/// </summary>
[Flags]
public enum DDSD : uint
{
    /// <summary>
    /// Required in every .dds file.
    /// </summary>
    CAPS = 0x1,

    /// <summary>
    /// Required in every .dds file.
    /// </summary>
    HEIGHT = 0x2,

    /// <summary>
    /// Required in every .dds file.
    /// </summary>
    WIDTH = 0x4,

    /// <summary>
    /// Required when pitch is provided for an uncompressed texture.
    /// <para>The DDS_HEADER_FLAGS_PITCH flag, which is defined in Dds.h, is equal to the DDSD_PITCH flag.</para>
    /// </summary>
    PITCH = 0x8,

    /// <summary>
    /// Required in every .dds file.
    /// </summary>
    PIXELFORMAT = 0x1000,

    /// <summary>
    /// Required in a mipmapped texture.
    /// <para>The DDS_HEADER_FLAGS_MIPMAP flag, which is defined in Dds.h, is equal to the DDSD_MIPMAPCOUNT flag.</para>
    /// </summary>
    MIPMAPCOUNT = 0x20000,

    /// <summary>
    /// Required when pitch is provided for a compressed texture.
    /// <para>The DDS_HEADER_FLAGS_LINEARSIZE flag, which is defined in Dds.h, is equal to the DDSD_LINEARSIZE flag.</para>
    /// </summary>
    LINEARSIZE = 0x80000,

    /// <summary>
    /// Required in a depth texture.
    /// <para>The DDS_HEADER_FLAGS_VOLUME flag, which is defined in Dds.h, is equal to the DDSD_DEPTH flag.</para>
    /// </summary>
    DEPTH = 0x800000
}

/// <summary>
/// Defines the various types of Direct3D9 surface formats. It also include some DDS FourCC codes.
/// </summary>
public enum D3DFormat : uint
{
    UNKNOWN = 0, 

    R8G8B8 = 20, // Supported
    A8R8G8B8 = 21, // Supported
    X8R8G8B8 = 22, // Supported
    R5G6B5 = 23,
    X1R5G5B5 = 24,
    A1R5G5B5 = 25,
    A4R4G4B4 = 26,
    R3G3B2 = 27,
    A8 = 28,
    A8R3G3B2 = 29,
    X4R4G4B4 = 30,
    A2B10G10R10 = 31,
    A8B8G8R8 = 32,
    X8B8G8R8 = 33,
    G16R16 = 34,
    A2R10G10B10 = 35,
    A16B16G16R16 = 36,

    A8P8 = 40,
    P8 = 41,

    L8 = 50, // Supported
    A8L8 = 51,
    A4L4 = 52,

    V8U8 = 60,
    L6V5U5 = 61,
    X8L8V8U8 = 62, // Used by Telltale, Not Supported
    Q8W8V8U8 = 63,
    V16U16 = 64,
    A2W10V10U10 = 67,

    UYVY = 0x59565955, // 'UYVY'
    R8G8_B8G8 = 0x47424752, // 'RGBG'
    YUY2 = 0x32595559, // 'YUY2'
    G8R8_G8B8 = 0x42475247, // 'GRGB'
    DXT1 = 0x31545844, // 'DXT1' // Supported
    DXT2 = 0x32545844, // 'DXT2' // Supported
    DXT3 = 0x33545844, // 'DXT3' // Supported
    DXT4 = 0x34545844, // 'DXT4' // Supported
    DXT5 = 0x35545844, // 'DXT5' // Supported
    ATI1 = 0x31495441, // 'ATI1'
    ATI2 = 0x32495441, // 'ATI2'
    BC4S = 0x42433453, // 'BC4S'
    BC5S = 0x42433553, // 'BC4S'

    D16_LOCKABLE = 70,
    D32 = 71,
    D15S1 = 73,
    D24S8 = 75,
    D24X8 = 77,
    D24X4S4 = 79,
    D16 = 80,

    D32F_LOCKABLE = 82,
    D24FS8 = 83,

    D32_LOCKABLE = 84,
    S8_LOCKABLE = 85,

    L16 = 81,

    VERTEXDATA = 100,
    INDEX16 = 101,
    INDEX32 = 102,

    Q16W16V16U16 = 110,

    MULTI2_ARGB8 = 0x3145544D, // 'MET1'

    R16F = 111,
    G16R16F = 112,
    A16B16G16R16F = 113,

    R32F = 114,
    G32R32F = 115,
    A32B32G32R32F = 116,

    CxV8U8 = 117,

    A1 = 118,
    A2B10G10R10_XR_BIAS = 119,
    BINARYBUFFER = 199,

    AI44 = 0x34344941, // 'AI44'
    IA44 = 0x34344149, // 'IA44'
    YV12 = 0x32315659, // 'YV12'

    FORCE_DWORD = 0x7fffffff
}

/// <summary>
/// Values which indicate what type of data is in the surface.
/// </summary>
[Flags]
public enum DDPF
{
    /// <summary>
    /// Texture contains alpha data; dwRGBAlphaBitMask contains valid data.
    /// </summary>
    ALPHAPIXELS = 0x1,

    /// <summary>
    /// Used in some older DDS files for alpha channel only uncompressed data 
    /// (dwRGBBitCount contains the alpha channel bitcount; dwABitMask contains valid data).
    /// </summary>
    ALPHA = 0x2,

    /// <summary>
    /// Texture contains compressed RGB data; dwFourCC contains valid data.
    /// </summary>
    FOURCC = 0x4,

    /// <summary>
    /// Texture contains uncompressed RGB data; dwRGBBitCount and the RGB masks (dwRBitMask, dwGBitMask, dwBBitMask) contain valid data.
    /// </summary>
    RGB = 0x40,

    /// <summary>
    /// Used in some older DDS files for YUV uncompressed data 
    /// (dwRGBBitCount contains the YUV bit count; dwRBitMask contains the Y mask, dwGBitMask contains the U mask, dwBBitMask contains the V mask).
    /// </summary>
    YUV = 0x200,

    /// <summary>
    /// Used in some older DDS files for single channel uncompressed data 
    /// (dwRGBBitCount contains the luminance channel bit count; dwRBitMask contains the channel mask). 
    /// <para>Can be combined with DDPF_ALPHAPIXELS for a two channel DDS file.</para>
    /// </summary>
    LUMINANCE = 0x20000
};

/// <summary>
/// Specifies the complexity of the surfaces stored.
/// <para>The DDS_SURFACE_FLAGS_MIPMAP flag, which is defined in Dds.h, is a bitwise-OR combination of the DDSCAPS_COMPLEX and DDSCAPS_MIPMAP flags.</para>
/// </summary>
[Flags]
public enum DDSCAPS : uint
{
    /// <summary>
    /// Optional; must be used on any file that contains more than one surface (a mipmap, a cubic environment map, or mipmapped volume texture).
    /// <para>The DDS_SURFACE_FLAGS_CUBEMAP flag, which is defined in Dds.h, is equal to the DDSCAPS_COMPLEX flag.</para>
    /// </summary>
    COMPLEX = 0x8,

    /// <summary>
    /// Optional; should be used for a mipmap.
    /// </summary>
    MIPMAP = 0x400000,

    /// <summary>
    /// Required
    /// <para>The DDS_SURFACE_FLAGS_TEXTURE flag, which is defined in Dds.h, is equal to the DDSCAPS_TEXTURE flag.</para>
    /// </summary>
    TEXTURE = 0x1000
}

/// <summary>
/// Additional detail about the surfaces stored.
/// <para>The DDS_CUBEMAP_ALLFACES flag, which is defined in Dds.h, is a bitwise-OR combination of the DDS_CUBEMAP_POSITIVEX, DDS_CUBEMAP_NEGATIVEX, DDS_CUBEMAP_POSITIVEY, DDS_CUBEMAP_NEGATIVEY, DDS_CUBEMAP_POSITIVEZ, and DDSCAPS2_CUBEMAP_NEGATIVEZ flags.</para>
/// </summary>
[Flags]
public enum DDSCAPS2 : uint
{
    /// <summary>
    /// Required for a cube map.
    /// </summary>
    CUBEMAP = 0x200,

    /// <summary>
    /// Required when these surfaces are stored in a cube map.
    /// <para>The DDS_CUBEMAP_POSITIVEX flag, which is defined in Dds.h, is a bitwise-OR combination of the DDSCAPS2_CUBEMAP and DDSCAPS2_CUBEMAP_POSITIVEX flags.</para>
    /// </summary>
    CUBEMAP_POSITIVEX = 0x400,

    /// <summary>
    /// Required when these surfaces are stored in a cube map.
    /// <para>The DDS_CUBEMAP_NEGATIVEX flag, which is defined in Dds.h, is a bitwise-OR combination of the DDSCAPS2_CUBEMAP and DDSCAPS2_CUBEMAP_NEGATIVEX flags.</para>
    /// </summary>
    CUBEMAP_NEGATIVEX = 0x800,

    /// <summary>
    /// Required when these surfaces are stored in a cube map.
    /// <para>The DDS_CUBEMAP_POSITIVEY flag, which is defined in Dds.h, is a bitwise-OR combination of the DDSCAPS2_CUBEMAP and DDSCAPS2_CUBEMAP_POSITIVEY flags.</para>
    /// </summary>
    CUBEMAP_POSITIVEY = 0x1000,

    /// <summary>
    /// Required when these surfaces are stored in a cube map.
    /// <para>The DDS_CUBEMAP_NEGATIVEY flag, which is defined in Dds.h, is a bitwise-OR combination of the DDSCAPS2_CUBEMAP and DDSCAPS2_CUBEMAP_NEGATIVEY flags.</para>
    /// </summary>
    CUBEMAP_NEGATIVEY = 0x2000,

    /// <summary>
    /// Required when these surfaces are stored in a cube map.
    /// <para>The DDS_CUBEMAP_POSITIVEZ flag, which is defined in Dds.h, is a bitwise-OR combination of the DDSCAPS2_CUBEMAP and DDSCAPS2_CUBEMAP_POSITIVEZ flags.</para>
    /// </summary>
    CUBEMAP_POSITIVEZ = 0x4000,

    /// <summary>
    /// Required when these surfaces are stored in a cube map.
    /// <para>The DDS_CUBEMAP_NEGATIVEZ flag, which is defined in Dds.h, is a bitwise-OR combination of the DDSCAPS2_CUBEMAP and DDSCAPS2_CUBEMAP_NEGATIVEZ flags.</para>
    /// </summary>
    CUBEMAP_NEGATIVEZ = 0x8000,

    /// <summary>
    /// Required for a volume texture.
    /// <para>The DDS_FLAGS_VOLUME flag, which is defined in Dds.h, is equal to the DDSCAPS2_VOLUME flag.</para>
    /// </summary>
    VOLUME = 0x200000
}

public enum DXGIFormat
{
    UNKNOWN = 0,
    R32G32B32A32_TYPELESS = 1,
    R32G32B32A32_FLOAT = 2,
    R32G32B32A32_UINT = 3,
    R32G32B32A32_SINT = 4,
    R32G32B32_TYPELESS = 5,
    R32G32B32_FLOAT = 6,
    R32G32B32_UINT = 7,
    R32G32B32_SINT = 8,
    R16G16B16A16_TYPELESS = 9,
    R16G16B16A16_FLOAT = 10,
    R16G16B16A16_UNORM = 11,
    R16G16B16A16_UINT = 12,
    R16G16B16A16_SNORM = 13,
    R16G16B16A16_SINT = 14,
    R32G32_TYPELESS = 15,
    R32G32_FLOAT = 16,
    R32G32_UINT = 17,
    R32G32_SINT = 18,
    R32G8X24_TYPELESS = 19,
    D32_FLOAT_S8X24_UINT = 20,
    R32_FLOAT_X8X24_TYPELESS = 21,
    X32_TYPELESS_G8X24_UINT = 22,
    R10G10B10A2_TYPELESS = 23,
    R10G10B10A2_UNORM = 24,
    R10G10B10A2_UINT = 25,
    R11G11B10_FLOAT = 26,
    R8G8B8A8_TYPELESS = 27,
    R8G8B8A8_UNORM = 28,
    R8G8B8A8_UNORM_SRGB = 29,
    R8G8B8A8_UINT = 30,
    R8G8B8A8_SNORM = 31,
    R8G8B8A8_SINT = 32,
    R16G16_TYPELESS = 33,
    R16G16_FLOAT = 34,
    R16G16_UNORM = 35,
    R16G16_UINT = 36,
    R16G16_SNORM = 37,
    R16G16_SINT = 38,
    R32_TYPELESS = 39,
    D32_FLOAT = 40,
    R32_FLOAT = 41,
    R32_UINT = 42,
    R32_SINT = 43,
    R24G8_TYPELESS = 44,
    D24_UNORM_S8_UINT = 45,
    R24_UNORM_X8_TYPELESS = 46,
    X24_TYPELESS_G8_UINT = 47,
    R8G8_TYPELESS = 48,
    R8G8_UNORM = 49,
    R8G8_UINT = 50,
    R8G8_SNORM = 51,
    R8G8_SINT = 52,
    R16_TYPELESS = 53,
    R16_FLOAT = 54,
    D16_UNORM = 55,
    R16_UNORM = 56,
    R16_UINT = 57,
    R16_SNORM = 58,
    R16_SINT = 59,
    R8_TYPELESS = 60,
    R8_UNORM = 61,
    R8_UINT = 62,
    R8_SNORM = 63,
    R8_SINT = 64,
    A8_UNORM = 65,
    R1_UNORM = 66,
    R9G9B9E5_SHAREDEXP = 67,
    R8G8_B8G8_UNORM = 68,
    G8R8_G8B8_UNORM = 69,
    BC1_TYPELESS = 70,
    BC1_UNORM = 71,
    BC1_UNORM_SRGB = 72,
    BC2_TYPELESS = 73,
    BC2_UNORM = 74,
    BC2_UNORM_SRGB = 75,
    BC3_TYPELESS = 76,
    BC3_UNORM = 77,
    BC3_UNORM_SRGB = 78,
    BC4_TYPELESS = 79,
    BC4_UNORM = 80,
    BC4_SNORM = 81,
    BC5_TYPELESS = 82,
    BC5_UNORM = 83,
    BC5_SNORM = 84,
    B5G6R5_UNORM = 85,
    B5G5R5A1_UNORM = 86,
    B8G8R8A8_UNORM = 87,
    B8G8R8X8_UNORM = 88,
    R10G10B10_XR_BIAS_A2_UNORM = 89,
    B8G8R8A8_TYPELESS = 90,
    B8G8R8A8_UNORM_SRGB = 91,
    B8G8R8X8_TYPELESS = 92,
    B8G8R8X8_UNORM_SRGB = 93,
    BC6H_TYPELESS = 94,
    BC6H_UF16 = 95,
    BC6H_SF16 = 96,
    BC7_TYPELESS = 97,
    BC7_UNORM = 98,
    BC7_UNORM_SRGB = 99,
    AYUV = 100,
    Y410 = 101,
    Y416 = 102,
    NV12 = 103,
    P010 = 104,
    P016 = 105,
    OPAQUE_420 = 106,
    YUY2 = 107,
    Y210 = 108,
    Y216 = 109,
    NV11 = 110,
    AI44 = 111,
    IA44 = 112,
    P8 = 113,
    A8P8 = 114,
    B4G4R4A4_UNORM = 115,

    P208 = 130,
    V208 = 131,
    V408 = 132,


    SAMPLER_FEEDBACK_MIN_MIP_OPAQUE = 189,
    SAMPLER_FEEDBACK_MIP_REGION_USED_OPAQUE = 190,

    A4B4G4R4_UNORM = 191,
    //FORCE_UINT = 0xffffffff
}

/// <summary>
/// Identifies the type of resource.
/// </summary>
public enum D3D10_RESOURCE_DIMENSION
{
    UNKNOWN = 0,
    BUFFER = 1,

    /// <summary>
    /// Resource is a 1D texture. 
    /// <para>The dwWidth member of DDS_HEADER specifies the size of the texture. 
    /// Typically, you set the dwHeight member of DDS_HEADER to 1; 
    /// you also must set the DDSD_HEIGHT flag in the dwFlags member of DDS_HEADER.</para>
    /// </summary>
    TEXTURE1D = 2,

    /// <summary>
    /// Resource is a 2D texture with an area specified by the dwWidth and dwHeight members of DDS_HEADER. 
    /// <para>You can also use this type to identify a cube-map texture. 
    /// For more information about how to identify a cube-map texture, see miscFlag and arraySize members.</para>
    /// </summary>
    TEXTURE2D = 3,

    /// <summary>
    /// Resource is a 3D texture with a volume specified by the dwWidth, dwHeight, and dwDepth members of DDS_HEADER. 
    /// <para>You also must set the DDSD_DEPTH flag in the dwFlags member of DDS_HEADER.</para>
    /// </summary>
    TEXTURE3D = 4
};

/// <summary>
/// Identifies other, less common options for resources. 
/// </summary>
[Flags]
public enum DDS_RESOURCE
{
    /// <summary>
    /// Indicates a 2D texture is a cube-map texture.
    /// </summary>
    MISC_TEXTURECUBE = 0x4
}

/// <summary>
/// Contains additional metadata (formerly was reserved). The lower 3 bits indicate the alpha mode of the associated resource.
/// </summary>
[Flags]
public enum DDS_ALPHA_MODE
{
    /// <summary>
    /// Alpha channel content is unknown. This is the value for legacy files, which typically is assumed to be 'straight' alpha.
    /// </summary>
    UNKNOWN = 0x0,

    /// <summary>
    /// Any alpha channel content is presumed to use straight alpha.
    /// </summary>
    STRAIGHT = 0x1,

    /// <summary>
    /// Any alpha channel content is using premultiplied alpha. The only legacy file formats that indicate this information are 'DX2' and 'DX4'.
    /// </summary>
    PREMULTIPLIED = 0x2,

    /// <summary>
    /// Any alpha channel content is all set to fully opaque.
    /// </summary>
    OPAQUE = 0x3,

    /// <summary>
    /// Any alpha channel content is being used as a 4th channel and is not intended to represent transparency (straight or premultiplied).
    /// </summary>
    CUSTOM = 0x4
};
