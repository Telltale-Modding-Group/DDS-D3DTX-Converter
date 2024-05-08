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
/// Describes a DDS file format.
/// </summary>
public partial class DDS
{
    /// <summary>
    /// [4 bytes] The magic number for DDS files.
    /// </summary>
    public static string MAGIC_WORD = "DDS ";

    /// <summary>
    /// The FourCC code for DX10 extended header.
    /// </summary>
    public static string DX10_FOURCC = "DX10";

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
    public byte[]? pixelData;

    public void Read(BinaryReader reader)
    {
        if (reader.ReadUInt32() != ByteFunctions.Convert_String_To_UInt32(MAGIC_WORD))
            throw new Exception("Invalid DDS file.");

        header = new DDS_HEADER(reader);

        if (header.ddspf.dwFourCC == ByteFunctions.Convert_String_To_UInt32(DX10_FOURCC))
        {
            dxt10Header = new DDS_HEADER_DXT10(reader);
        }

        pixelData = reader.ReadBytes((int)(reader.BaseStream.Length - reader.BaseStream.Position));
    }

    public void Write(BinaryWriter writer)
    {
        header.Write(writer);

        if (header.ddspf.dwFourCC == ByteFunctions.Convert_String_To_UInt32(DX10_FOURCC))
        {
            dxt10Header.Write(writer);
        }

        writer.Write(pixelData);
    }

    public void Read(byte[] data)
    {
        using MemoryStream stream = new(data);
        using BinaryReader reader = new(stream);
        Read(reader);
    }

    public void Write(byte[] data)
    {
        using MemoryStream stream = new();
        using BinaryWriter writer = new(stream);
        Write(writer);
        data = stream.ToArray();
    }

    public byte[] GetBytes()
    {
        Print();
        using MemoryStream stream = new();
        using BinaryWriter writer = new(stream);

        writer.Write(ByteFunctions.Convert_String_To_UInt32(MAGIC_WORD));
        header.Write(writer);

        if (header.ddspf.dwFourCC == ByteFunctions.Convert_String_To_UInt32(DX10_FOURCC))
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

        if (header.ddspf.dwFourCC == ByteFunctions.Convert_String_To_UInt32(DX10_FOURCC))
        {
            Console.WriteLine("DDS Header DXT10:");
            dxt10Header.Print();
        }
    }
}
