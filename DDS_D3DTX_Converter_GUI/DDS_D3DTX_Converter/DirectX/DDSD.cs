using System;

namespace D3DTX_Converter.DirectX;

// DDS Docs - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-header
// DDS PIXEL FORMAT - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-pixelformat
// DDS DDS_HEADER_DXT10 - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-header-dxt10
// DDS File Layout https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-file-layout-for-textures
// Texture Block Compression in D3D11 - https://docs.microsoft.com/en-us/windows/win32/direct3d11/texture-block-compression-in-direct3d-11
// DDS Programming Guide - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dx-graphics-dds-pguide

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
