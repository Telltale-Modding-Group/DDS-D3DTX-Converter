using System;

namespace D3DTX_Converter.DirectX;

// DDS Docs - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-header
// DDS PIXEL FORMAT - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-pixelformat
// DDS DDS_HEADER_DXT10 - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-header-dxt10
// DDS File Layout https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-file-layout-for-textures
// Texture Block Compression in D3D11 - https://docs.microsoft.com/en-us/windows/win32/direct3d11/texture-block-compression-in-direct3d-11
// DDS Programming Guide - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dx-graphics-dds-pguide

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
