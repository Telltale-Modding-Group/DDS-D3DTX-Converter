using System;

namespace D3DTX_Converter.DirectX;

// DDS Docs - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-header
// DDS PIXEL FORMAT - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-pixelformat
// DDS DDS_HEADER_DXT10 - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-header-dxt10
// DDS File Layout https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-file-layout-for-textures
// Texture Block Compression in D3D11 - https://docs.microsoft.com/en-us/windows/win32/direct3d11/texture-block-compression-in-direct3d-11
// DDS Programming Guide - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dx-graphics-dds-pguide

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
