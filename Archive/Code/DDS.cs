using System;
using System.IO;
using TelltaleTextureTool.Utilities;

namespace TelltaleTextureTool.DirectX;

// DDS Docs - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-header
// DDS PIXEL FORMAT - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-pixelformat
// DDS DDS_HEADER_DXT10 - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-header-dxt10
// DDS File Layout https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-file-layout-for-textures
// Texture Block Compression in D3D11 - https://docs.microsoft.com/en-us/windows/win32/direct3d11/texture-block-compression-in-direct3d-11
// DDS Programming Guide - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dx-graphics-dds-pguide

/// <summary>
/// Describes a DDS file format.
/// </summary>
public partial class DDS
{
    /// <summary>
    /// [4 bytes] The magic number for DDS files.
    /// </summary>
    public const string MAGIC_WORD = "DDS ";

    /// <summary>
    /// The FourCC code for DX10 extended header.
    /// </summary>
    public const string DX10_FOURCC = "DX10";

    /// <summary>
    /// [124 bytes] Describes a DDS file header.
    /// </summary>
    public DDS_HEADER header;

    /// <summary>
    /// [20 bytes] (OPTIONAL) 
    /// DDS header extension to handle resource arrays, DXGI pixel formats that don't map to the legacy Microsoft DirectDraw pixel format structures, and additional metadata.
    /// </summary>
    public DDS_HEADER_DXT10 dxt10Header;

    /// <summary>
    /// The pixel data of the DDS file.
    /// </summary>
    public byte[] pixelData;

    public byte[] GetBytes()
    {
        Print();
        using MemoryStream stream = new();
        using BinaryWriter writer = new(stream);

        writer.Write(ByteFunctions.ConvertStringToUInt32(MAGIC_WORD));
        header.Write(writer);

        if (header.ddspf.dwFourCC == ByteFunctions.ConvertStringToUInt32(DX10_FOURCC))
        {
            dxt10Header.Write(writer);
        }

        writer.Write(pixelData);

        return stream.ToArray();
    }

    public void Print()
    {
        Console.WriteLine("DDS Header:");
        header.Print();

        if (header.ddspf.dwFourCC == ByteFunctions.ConvertStringToUInt32(DX10_FOURCC))
        {
            Console.WriteLine("DDS Header DXT10:");
            dxt10Header.Print();
        }
    }
}
