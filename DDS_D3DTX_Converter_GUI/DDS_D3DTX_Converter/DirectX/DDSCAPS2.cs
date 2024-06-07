using System;

namespace D3DTX_Converter.DirectX;

// DDS Docs - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-header
// DDS PIXEL FORMAT - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-pixelformat
// DDS DDS_HEADER_DXT10 - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-header-dxt10
// DDS File Layout https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-file-layout-for-textures
// Texture Block Compression in D3D11 - https://docs.microsoft.com/en-us/windows/win32/direct3d11/texture-block-compression-in-direct3d-11
// DDS Programming Guide - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dx-graphics-dds-pguide

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
