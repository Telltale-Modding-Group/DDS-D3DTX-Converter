using System;
using System.IO;
using Pfim;

namespace D3DTX_Converter.DirectX;

// DDS Docs - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-header
// DDS PIXEL FORMAT - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-pixelformat
// DDS DDS_HEADER_DXT10 - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-header-dxt10
// DDS File Layout https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-file-layout-for-textures
// Texture Block Compression in D3D11 - https://docs.microsoft.com/en-us/windows/win32/direct3d11/texture-block-compression-in-direct3d-11
// DDS Programming Guide - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dx-graphics-dds-pguide

/// <summary>
/// DDS header extension to handle resource arrays, DXGI pixel formats that don't map to the legacy Microsoft DirectDraw pixel format structures, and additional metadata.
/// </summary>
public struct DDS_HEADER_DXT10
{
    /// <summary>
    /// [4 bytes] The surface pixel format.
    /// </summary>
    public DXGIFormat dxgiFormat;

    /// <summary>
    /// [4 bytes] Identifies the type of resource.
    /// </summary>
    public D3D10_RESOURCE_DIMENSION resourceDimension;

    /// <summary>
    /// [4 bytes] Identifies other, less common options for resources.
    /// </summary>
    public DDS_RESOURCE miscFlag;

    /// <summary>
    /// [4 bytes] The number of elements in the array.
    /// <para>For a 2D texture that is also a cube-map texture, this number represents the number of cubes.</para>
    /// </summary>
    public uint arraySize;

    /// <summary>
    /// [4 bytes] Contains additional metadata (formerly was reserved). 
    /// </summary>
    public uint miscFlags2;

    public DDS_HEADER_DXT10(BinaryReader reader)
    {
        dxgiFormat = (DXGIFormat)reader.ReadUInt32();
        resourceDimension = (D3D10_RESOURCE_DIMENSION)reader.ReadUInt32();
        miscFlag = (DDS_RESOURCE)reader.ReadUInt32();
        arraySize = reader.ReadUInt32();
        miscFlags2 = reader.ReadUInt32();
    }

    public void Write(BinaryWriter writer)
    {
        writer.Write((uint)dxgiFormat);
        writer.Write((uint)resourceDimension);
        writer.Write((uint)miscFlag);
        writer.Write(arraySize);
        writer.Write(miscFlags2);
    }

    public static DDS_HEADER_DXT10 GetHeaderFromBytes(byte[] byteArray)
    {
        using MemoryStream stream = new(byteArray);
        using BinaryReader reader = new(stream);

        return new DDS_HEADER_DXT10(reader);
    }

    /// <summary>
    /// Returns a preset DDS_Header_DXT10, when the compression format is DXT10. We modify it later when we need it.
    /// </summary>
    /// <returns></returns>
    public static DDS_HEADER_DXT10 GetPresetDXT10Header() => new()
    {
        dxgiFormat = DXGIFormat.R8G8B8A8_UNORM,
        resourceDimension = D3D10_RESOURCE_DIMENSION.TEXTURE2D,
        arraySize = 1,
    };

    public void Print(){
        Console.WriteLine("DDS_HEADER_DXT10");
        Console.WriteLine($"\tdxgiFormat: {dxgiFormat}");
        Console.WriteLine($"\tresourceDimension: {resourceDimension}");
        Console.WriteLine($"\tmiscFlag: {miscFlag}");
        Console.WriteLine($"\tarraySize: {arraySize}");
        Console.WriteLine($"\tmiscFlags2: {miscFlags2}");
    }
}
