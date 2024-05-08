using System;
using System.IO;
using D3DTX_Converter.Utilities;

namespace D3DTX_Converter.DirectX;

// DDS Docs - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-header
// DDS PIXEL FORMAT - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-pixelformat
// DDS DDS_HEADER_DXT10 - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-header-dxt10
// DDS File Layout https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-file-layout-for-textures
// Texture Block Compression in D3D11 - https://docs.microsoft.com/en-us/windows/win32/direct3d11/texture-block-compression-in-direct3d-11
// DDS Programming Guide - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dx-graphics-dds-pguide

/// <summary>
/// Describes a DDS file header.
/// </summary>
public struct DDS_HEADER
{
    /// <summary>
    /// [4 bytes] Size of structure. This member must be set to 124.
    /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
    /// </summary>
    public uint dwSize;

    /// <summary>
    /// [4 bytes] Flags to indicate which members contain valid data.
    /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
    /// </summary>
    public uint dwFlags;

    /// <summary>
    /// [4 bytes] Surface height (in pixels).
    /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
    /// </summary>
    public uint dwHeight;

    /// <summary>
    /// [4 bytes] Surface width (in pixels).
    /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
    /// </summary>
    public uint dwWidth;

    /// <summary>
    /// [4 bytes] The pitch or number of bytes per scan line in an uncompressed texture.
    /// <para>The total number of bytes in the top level texture for a compressed texture.</para>
    /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
    /// </summary>
    public uint dwPitchOrLinearSize;

    /// <summary>
    /// [4 bytes] Depth of a volume texture (in pixels), otherwise unused.
    /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
    /// </summary>
    public uint dwDepth;

    /// <summary>
    /// [4 bytes] Number of mipmap levels, otherwise unused.
    /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
    /// </summary>
    public uint dwMipMapCount;

    /// <summary>
    /// [4 bytes] Unused according to DDS docs.
    /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
    /// </summary>
    public uint dwReserved1;

    /// <summary>
    /// [4 bytes] Unused according to DDS docs.
    /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
    /// </summary>
    public uint dwReserved2;

    /// <summary>
    /// [4 bytes] Unused according to DDS docs.
    /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
    /// </summary>
    public uint dwReserved3;

    /// <summary>
    /// [4 bytes] Unused according to DDS docs.
    /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
    /// </summary>
    public uint dwReserved4;

    /// <summary>
    /// [4 bytes] Unused according to DDS docs.
    /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
    /// </summary>
    public uint dwReserved5;

    /// <summary>
    /// [4 bytes] Unused according to DDS docs.
    /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
    /// </summary>
    public uint dwReserved6;

    /// <summary>
    /// [4 bytes] Unused according to DDS docs.
    /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
    /// </summary>
    public uint dwReserved7;

    /// <summary>
    /// [4 bytes] Unused according to DDS docs.
    /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
    /// </summary>
    public uint dwReserved8;

    /// <summary>
    /// [4 bytes] Unused according to DDS docs.
    /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
    /// </summary>
    public uint dwReserved9;

    /// <summary>
    /// [4 bytes] Unused according to DDS docs.
    /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
    /// </summary>
    public uint dwReserved10;

    /// <summary>
    /// [4 bytes] Unused according to DDS docs.
    /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
    /// </summary>
    public uint dwReserved11;

    /// <summary>
    /// [36 bytes] Surface pixel format.
    /// </summary>
    public DDS_PIXELFORMAT ddspf;

    /// <summary>
    /// [4 bytes] Specifies the complexity of the surfaces stored.
    /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
    /// </summary>
    public uint dwCaps;

    /// <summary>
    /// [4 bytes] Additional detail about the surfaces stored.
    /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
    /// </summary>
    public uint dwCaps2;

    /// <summary>
    /// [4 bytes] Unused according to DDS docs.
    /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
    /// </summary>
    public uint dwCaps3;

    /// <summary>
    /// [4 bytes] Unused according to DDS docs.
    /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
    /// </summary>
    public uint dwCaps4;

    /// <summary>
    /// [4 bytes] Unused according to DDS docs.
    /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
    /// </summary>
    public uint dwReserved12;

    public DDS_HEADER(BinaryReader reader)
    {
        dwSize = reader.ReadUInt32();
        dwFlags = reader.ReadUInt32();
        dwHeight = reader.ReadUInt32();
        dwWidth = reader.ReadUInt32();
        dwPitchOrLinearSize = reader.ReadUInt32();
        dwDepth = reader.ReadUInt32();
        dwMipMapCount = reader.ReadUInt32();
        dwReserved1 = reader.ReadUInt32();
        dwReserved2 = reader.ReadUInt32();
        dwReserved3 = reader.ReadUInt32();
        dwReserved4 = reader.ReadUInt32();
        dwReserved5 = reader.ReadUInt32();
        dwReserved6 = reader.ReadUInt32();
        dwReserved7 = reader.ReadUInt32();
        dwReserved8 = reader.ReadUInt32();
        dwReserved9 = reader.ReadUInt32();
        dwReserved10 = reader.ReadUInt32();
        dwReserved11 = reader.ReadUInt32();
        ddspf = new DDS_PIXELFORMAT(reader);
        dwCaps = reader.ReadUInt32();
        dwCaps2 = reader.ReadUInt32();
        dwCaps3 = reader.ReadUInt32();
        dwCaps4 = reader.ReadUInt32();
        dwReserved12 = reader.ReadUInt32();
    }

    public void Write(BinaryWriter writer)
    {
        writer.Write(dwSize);
        writer.Write(dwFlags);
        writer.Write(dwHeight);
        writer.Write(dwWidth);
        writer.Write(dwPitchOrLinearSize);
        writer.Write(dwDepth);
        writer.Write(dwMipMapCount);
        writer.Write(dwReserved1);
        writer.Write(dwReserved2);
        writer.Write(dwReserved3);
        writer.Write(dwReserved4);
        writer.Write(dwReserved5);
        writer.Write(dwReserved6);
        writer.Write(dwReserved7);
        writer.Write(dwReserved8);
        writer.Write(dwReserved9);
        writer.Write(dwReserved10);
        writer.Write(dwReserved11);
        ddspf.Write(writer);
        writer.Write(dwCaps);
        writer.Write(dwCaps2);
        writer.Write(dwCaps3);
        writer.Write(dwCaps4);
        writer.Write(dwReserved12);
    }

    public static DDS_HEADER GetHeaderFromBytes(byte[] byteArray)
    {
        using MemoryStream stream = new(byteArray);
        using BinaryReader reader = new(stream);

        return new DDS_HEADER(reader);
    }

    /// <summary>
    /// Returns a preset DDS_Header. We modify it later when we need it.
    /// </summary>
    /// <returns></returns>
    public static DDS_HEADER GetPresetHeader() => new()
    {
        dwSize = 124,
        dwFlags = (uint)DDSD.CAPS | (uint)DDSD.HEIGHT | (uint)DDSD.WIDTH | (uint)DDSD.PIXELFORMAT | (uint)DDSD.MIPMAPCOUNT,
        dwHeight = 1024,
        dwWidth = 1024,
        dwPitchOrLinearSize = 8192,
        dwDepth = 0,
        dwMipMapCount = 0,
        ddspf = new()
        {
            dwSize = 32,
            dwFlags = 4,
            dwFourCC = (uint)D3DFORMAT.DXT1,
            dwRGBBitCount = 0,
        },
        dwCaps = 4096,
        dwCaps2 = 0,
        dwCaps3 = 0,
        dwCaps4 = 0,
    };

    public void Print()
    {
        Console.WriteLine("DDS Header:");
        Console.WriteLine($"dwSize: {dwSize}");
        Console.WriteLine($"dwFlags: {dwFlags}");
        Console.WriteLine($"dwHeight: {dwHeight}");
        Console.WriteLine($"dwWidth: {dwWidth}");
        Console.WriteLine($"dwPitchOrLinearSize: {dwPitchOrLinearSize}");
        Console.WriteLine($"dwDepth: {dwDepth}");
        Console.WriteLine($"dwMipMapCount: {dwMipMapCount}");
        Console.WriteLine($"dwReserved1: {dwReserved1}");
        Console.WriteLine($"dwReserved2: {dwReserved2}");
        Console.WriteLine($"dwReserved3: {dwReserved3}");
        Console.WriteLine($"dwReserved4: {dwReserved4}");
        Console.WriteLine($"dwReserved5: {dwReserved5}");
        Console.WriteLine($"dwReserved6: {dwReserved6}");
        Console.WriteLine($"dwReserved7: {dwReserved7}");
        Console.WriteLine($"dwReserved8: {dwReserved8}");
        Console.WriteLine($"dwReserved9: {dwReserved9}");
        Console.WriteLine($"dwReserved10: {dwReserved10}");
        Console.WriteLine($"dwReserved11: {dwReserved11}");
        ddspf.Print();
        Console.WriteLine($"dwCaps: {dwCaps}");
        Console.WriteLine($"dwCaps2: {dwCaps2}");
        Console.WriteLine($"dwCaps3: {dwCaps3}");
        Console.WriteLine($"dwCaps4: {dwCaps4}");
        Console.WriteLine($"dwReserved12: {dwReserved12}");
    }
}
