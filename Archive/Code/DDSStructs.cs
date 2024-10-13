using System;
using System.IO;
using TelltaleTextureTool.DirectX.Enums;
using TelltaleTextureTool.Utilities;

namespace TelltaleTextureTool.DirectX;

// DDS Docs - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-header
// DDS PIXEL FORMAT - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-pixelformat
// DDS DDS_HEADER_DXT10 - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-header-dxt10
// DDS File Layout https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-file-layout-for-textures
// Texture Block Compression in D3D11 - https://docs.microsoft.com/en-us/windows/win32/direct3d11/texture-block-compression-in-direct3d-11
// DDS Programming Guide - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dx-graphics-dds-pguide
// DWORD is a 32-bit unsigned integer. 

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
    public DDSD dwFlags;

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
    public DDSPixelFormat ddspf;

    /// <summary>
    /// [4 bytes] Specifies the complexity of the surfaces stored.
    /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
    /// </summary>
    public DDSCAPS dwCaps;

    /// <summary>
    /// [4 bytes] Additional detail about the surfaces stored.
    /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
    /// </summary>
    public DDSCAPS2 dwCaps2;

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

    public DDS_HEADER(BinaryReader reader, bool skipDWORD = false)
    {
        if (skipDWORD)
        {
            uint word = reader.ReadUInt32();

            if (ByteFunctions.ConvertStringToUInt32("DDS ") != word)
            {
               // throw new Exception("Invalid DDS Header");
            }
        }

        dwSize = reader.ReadUInt32();
        dwFlags = (DDSD)reader.ReadUInt32();
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
        ddspf = new DDSPixelFormat(reader);
        dwCaps = (DDSCAPS)reader.ReadUInt32();
        dwCaps2 = (DDSCAPS2)reader.ReadUInt32();
        dwCaps3 = reader.ReadUInt32();
        dwCaps4 = reader.ReadUInt32();
        dwReserved12 = reader.ReadUInt32();
    }

    public void Write(BinaryWriter writer)
    {
        writer.Write(dwSize);
        writer.Write((uint)dwFlags);
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
        writer.Write((uint)dwCaps);
        writer.Write((uint)dwCaps2);
        writer.Write(dwCaps3);
        writer.Write(dwCaps4);
        writer.Write(dwReserved12);
    }

    public static DDS_HEADER GetHeaderFromBytes(byte[] byteArray, bool skipDWORD = false)
    {
        using MemoryStream stream = new(byteArray);
        using BinaryReader reader = new(stream);

        return new DDS_HEADER(reader, skipDWORD);
    }

    /// <summary>
    /// Returns a preset DDS_Header. We modify it later when we need it.
    /// </summary>
    /// <returns></returns>
    public static DDS_HEADER GetPresetHeader() => new()
    {
        dwSize = 124,
        dwFlags = DDSD.CAPS | DDSD.HEIGHT | DDSD.WIDTH | DDSD.PIXELFORMAT | DDSD.MIPMAPCOUNT,
        dwHeight = 1024,
        dwWidth = 1024,
        dwPitchOrLinearSize = 8192,
        dwDepth = 0,
        dwMipMapCount = 0,
        ddspf = new()
        {
            dwSize = 32,
            dwFlags = 4,
            dwFourCC = (uint)D3DFormat.DXT1,
            dwRGBBitCount = 0,
        },
        dwCaps = DDSCAPS.TEXTURE,
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

    public override string ToString()
    {
        string result = $"DDS Header:" + Environment.NewLine;
        result += $"dwSize: {dwSize}" + Environment.NewLine;
        result += $"dwFlags: {dwFlags}" + Environment.NewLine;
        result += $"dwHeight: {dwHeight}" + Environment.NewLine;
        result += $"dwWidth: {dwWidth}" + Environment.NewLine;
        result += $"dwPitchOrLinearSize: {dwPitchOrLinearSize}" + Environment.NewLine;
        result += $"dwDepth: {dwDepth}" + Environment.NewLine;
        result += $"dwMipMapCount: {dwMipMapCount}" + Environment.NewLine;
        result += $"dwReserved1: {dwReserved1}" + Environment.NewLine;
        result += $"dwReserved2: {dwReserved2}" + Environment.NewLine;
        result += $"dwReserved3: {dwReserved3}" + Environment.NewLine;
        result += $"dwReserved4: {dwReserved4}" + Environment.NewLine;
        result += $"dwReserved5: {dwReserved5}" + Environment.NewLine;
        result += $"dwReserved6: {dwReserved6}" + Environment.NewLine;
        result += $"dwReserved7: {dwReserved7}" + Environment.NewLine;
        result += $"dwReserved8: {dwReserved8}" + Environment.NewLine;
        result += $"dwReserved9: {dwReserved9}" + Environment.NewLine;
        result += $"dwReserved10: {dwReserved10}" + Environment.NewLine;
        result += $"dwReserved11: {dwReserved11}" + Environment.NewLine;
        result += ddspf.ToString();
        result += $"dwCaps: {dwCaps}" + Environment.NewLine;
        result += $"dwCaps2: {dwCaps2}" + Environment.NewLine;
        result += $"dwCaps3: {dwCaps3}" + Environment.NewLine;
        result += $"dwCaps4: {dwCaps4}" + Environment.NewLine;
        result += $"dwReserved12: {dwReserved12}";

        return result;
    }
}

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

/// <summary>
/// Surface pixel format.
/// </summary>
public struct DDSPixelFormat
{
    /// <summary>
    /// [4 bytes] Structure size; set to 32 (bytes).
    /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
    /// </summary>
    public uint dwSize;

    /// <summary>
    /// [4 bytes] Values which indicate what type of data is in the surface.
    /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
    /// </summary>
    public uint dwFlags;

    /// <summary>
    /// [4 bytes] Four-character codes for specifying compressed or custom formats.
    /// <para>Possible values include: DXT1, DXT2, DXT3, DXT4, or DXT5.</para>
    /// <para>A FourCC of DX10 indicates the presence of the DDS_HEADER_DXT10 extended header, and the dxgiFormat member of that structure indicates the true format.</para>
    /// <para>When using a four-character code, dwFlags must include DDPF_FOURCC.</para>
    /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
    /// </summary>
    public uint dwFourCC;

    /// <summary>
    /// [4 bytes] Number of bits in an RGB (possibly including alpha) format.
    /// <para>Valid when dwFlags includes DDPF_RGB, DDPF_LUMINANCE, or DDPF_YUV.</para>
    /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
    /// </summary>
    public uint dwRGBBitCount;

    /// <summary>
    /// [4 bytes] Red (or luminance or Y) mask for reading color data.
    /// <para>For instance, given the A8R8G8B8 format, the red mask would be 0x00ff0000.</para>
    /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
    /// </summary>
    public uint dwRBitMask;

    /// <summary>
    /// [4 bytes] Green (or U) mask for reading color data.
    /// <para>For instance, given the A8R8G8B8 format, the green mask would be 0x0000ff00.</para>
    /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
    /// </summary>
    public uint dwGBitMask;

    /// <summary>
    /// [4 bytes] Blue (or V) mask for reading color data.
    /// <para>For instance, given the A8R8G8B8 format, the blue mask would be 0x000000ff.</para>
    /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
    /// </summary>
    public uint dwBBitMask;

    /// <summary>
    /// [4 bytes] Alpha mask for reading alpha data.
    /// <para>dwFlags must include DDPF_ALPHAPIXELS or DDPF_ALPHA. For instance, given the A8R8G8B8 format, the alpha mask would be 0xff000000.</para>
    /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
    /// </summary>
    public uint dwABitMask;

    public DDSPixelFormat(BinaryReader reader)
    {
        dwSize = reader.ReadUInt32();
        dwFlags = reader.ReadUInt32();
        dwFourCC = reader.ReadUInt32();
        dwRGBBitCount = reader.ReadUInt32();
        dwRBitMask = reader.ReadUInt32();
        dwGBitMask = reader.ReadUInt32();
        dwBBitMask = reader.ReadUInt32();
        dwABitMask = reader.ReadUInt32();
    }

    public void Write(BinaryWriter writer)
    {
        writer.Write(dwSize);
        writer.Write(dwFlags);
        writer.Write(dwFourCC);
        writer.Write(dwRGBBitCount);
        writer.Write(dwRBitMask);
        writer.Write(dwGBitMask);
        writer.Write(dwBBitMask);
        writer.Write(dwABitMask);
    }

    public void Print()
    {
        Console.WriteLine("DDS_PIXELFORMAT");
        Console.WriteLine($"\tdwSize: {dwSize}");
        Console.WriteLine($"\tdwFlags: {dwFlags}");
        Console.WriteLine($"\tdwFourCC: {dwFourCC}");
        Console.WriteLine($"\tdwRGBBitCount: {dwRGBBitCount}");
        Console.WriteLine($"\tdwRBitMask: {dwRBitMask}");
        Console.WriteLine($"\tdwGBitMask: {dwGBitMask}");
        Console.WriteLine($"\tdwBBitMask: {dwBBitMask}");
        Console.WriteLine($"\tdwABitMask: {dwABitMask}");
    }

    public static DDSPixelFormat Of(uint dwSize, uint dwFlags, uint dwFourCC, uint dwRGBBitCount, uint dwRBitMask, uint dwGBitMask, uint dwBBitMask, uint dwABitMask)
    {
        return new DDSPixelFormat()
        {
            dwSize = dwSize,
            dwFlags = dwFlags,
            dwFourCC = dwFourCC,
            dwRGBBitCount = dwRGBBitCount,
            dwRBitMask = dwRBitMask,
            dwGBitMask = dwGBitMask,
            dwBBitMask = dwBBitMask,
            dwABitMask = dwABitMask
        };
    }

    public override string ToString()
    {
        return $"DDS_PIXELFORMAT" +
            $"{Environment.NewLine}\tdwSize: {dwSize}" +
            $"{Environment.NewLine}\tdwFlags: {dwFlags}" +
            $"{Environment.NewLine}\tdwFourCC: {dwFourCC}" +
            $"{Environment.NewLine}\tdwRGBBitCount: {dwRGBBitCount}" +
            $"{Environment.NewLine}\tdwRBitMask: {dwRBitMask}" +
            $"{Environment.NewLine}\tdwGBitMask: {dwGBitMask}" +
            $"{Environment.NewLine}\tdwBBitMask: {dwBBitMask}" +
            $"{Environment.NewLine}\tdwABitMask: {dwABitMask}" + Environment.NewLine;
    }
}
