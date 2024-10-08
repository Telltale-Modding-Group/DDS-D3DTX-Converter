using TelltaleTextureTool.DirectX.Enums;
using TelltaleTextureTool.Utilities;
using Hexa.NET.DirectXTex;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using TelltaleTextureTool.Telltale.FileTypes.D3DTX;
using System.Numerics;
using System.Text;
using static TelltaleTextureTool.DirectX.TextureManager;
using TelltaleTextureTool.TelltaleEnums;
using System.ComponentModel.DataAnnotations;
using System.Linq;

using Image = Hexa.NET.DirectXTex.Image;

namespace TelltaleTextureTool.DirectX;

/// <summary>
/// Image section of texture file. Contains width, height, format, slice pitch, row pitch and the pixels.
/// </summary>
public class ImageSection
{
    public nuint Width { get; set; }
    public nuint Height { get; set; }
    public DXGIFormat Format { get; set; }
    public nuint SlicePitch { get; set; }
    public nuint RowPitch { get; set; }
    public byte[] Pixels { get; set; } = [];
};

public enum ImageEffect
{
    [Display(Name = "Default Mode")]
    DEFAULT,

    [Display(Name = "Swizzle ABGR")]
    SWIZZLE_ABGR,

    [Display(Name = "Restore Z")]
    RESTORE_Z,

    [Display(Name = "Remove Z")]
    REMOVE_Z
}

/// <summary>
/// A class that provides methods to interact with DirectXTexNet. Mainly used for loading and saving DDS files.
/// </summary>
public unsafe static partial class TextureManager
{

    /// <summary>
    /// Get the DDS image from DirectXTexNet.
    /// </summary>
    /// <param name="ddsFilePath">The file path of the .dds file.</param>
    /// <param name="flags">(Optional) The mode in which the DirectXTexNet will load the .dds file. If not provided, it defaults to NONE.</param>
    /// <returns>ScratchImage instance of the DDS file.</returns>
    public static void GetDDSImage(string ddsFilePath, out ScratchImage image, out TexMetadata metadata, DDSFlags flags = DDSFlags.None)
    {
        image = GetDDSImage(ByteFunctions.LoadTexture(ddsFilePath), flags);
        metadata = image.GetMetadata();
    }

    public static string GetDDSDebugInfo(string ddsFilePath)
    {
        ScratchImage image = GetDDSImage(ByteFunctions.LoadTexture(ddsFilePath));

        string debugInfo = GetDDSDebugInfo(image.GetMetadata());

        image.Release();

        return debugInfo;
    }

    public static ImageProperties GetDDSProperties(string ddsFilePath, TexMetadata ddsMetadata)
    {
        DXGIFormat dxgiFormat = (DXGIFormat)ddsMetadata.Format;

        ImageProperties properties = new()
        {
            Name = Path.GetFileNameWithoutExtension(ddsFilePath),
            Extension = ".dds",
            Height = ddsMetadata.Height.ToString(),
            Width = ddsMetadata.Width.ToString(),
            SurfaceFormat = dxgiFormat.ToString(),
            HasAlpha = DirectXTex.HasAlpha((int)dxgiFormat) ? "True" : "False",
            SurfaceGamma = DirectXTex.IsSRGB((int)dxgiFormat) ? "sRGB" : "Linear",
            MipMapCount = ddsMetadata.MipLevels.ToString(),
            ArraySize = ddsMetadata.ArraySize.ToString(),
            TextureLayout = ddsMetadata.Dimension.ToString(),
            AlphaMode = ddsMetadata.GetAlphaMode().ToString(),
        };

        return properties;
    }

    /// <summary>
    /// Compute the pitch given the Direct3D10/DXGI format, the width and the height.
    /// </summary>
    /// <param name="dxgiFormat">The Direct3D10/DXGI format.</param>
    /// <param name="width">The width of the DDS image.</param>
    /// <param name="height">(Optional) The height of the DDS image. If not provided, it defaults to 0.</param>
    /// <returns>The pitch.</returns>
    public static uint ComputePitch(DXGIFormat dxgiFormat, ulong width, ulong height = 1)
    {
        nuint rowPitch;
        nuint slicePitch;

        DirectXTex.ComputePitch((int)dxgiFormat, width, height, (ulong*)&rowPitch, (ulong*)&slicePitch, CPFlags.None).ThrowIf();
        return (uint)rowPitch;
    }

    /// <summary>
    /// Returns a DirectXTexNet DDS image from a byte array.
    /// </summary>
    /// <param name="array">The byte array containing the DDS data.</param>
    /// <param name="flags">(Optional) The mode in which the DirectXTexNet will load the .dds file. If not provided, it defaults to NONE.</param>
    /// <returns>The DirectXTexNet DDS image.</returns>
    unsafe public static ScratchImage GetDDSImage(byte[] array, DDSFlags flags = DDSFlags.None)
    {
        GCHandle handle = GCHandle.Alloc(array, GCHandleType.Pinned);
        try
        {
            ScratchImage image = DirectXTex.CreateScratchImage();
            TexMetadata metadata;

            // Obtain a pointer to the data
            IntPtr ptr = handle.AddrOfPinnedObject();
            DirectXTex.LoadFromDDSMemory((void*)ptr, (nuint)array.Length, flags, &metadata, ref image).ThrowIf();
            return image;
        }
        finally
        {
            // Release the handle to allow the garbage collector to reclaim the memory
            handle.Free();
        }
    }

    /// <summary>
    /// Returns a byte array from a DirectXTexNet DDS image.
    /// </summary>
    /// <param name="image">The DirectXTexNet DDS image.</param>
    /// <param name="flags">(Optional) The mode in which the DirectXTexNet will load the .dds file. If not provided, it defaults to NONE.</param>
    /// <returns>The byte array containing the DDS data.</returns>
    public static byte[] GetDDSByteArray(ScratchImage image, DDSFlags flags = DDSFlags.None)
    {
        Blob blob = DirectXTex.CreateBlob();
        try
        {
            Console.WriteLine("Image count: " + image.GetImageCount());
            Console.WriteLine("Image format: " + image.GetMetadata().Format);
            TexMetadata metadata = image.GetMetadata();
            DirectXTex.SaveToDDSMemory2(image.GetImages(), image.GetImageCount(), ref metadata, flags, ref blob);
            // Create a byte array to hold the data
            Console.WriteLine(blob.GetBufferSize());
            Console.WriteLine((nint)blob.GetBufferPointer());

            byte[] ddsArray = new byte[blob.GetBufferSize()];

            Console.WriteLine("BLOB BUFFER SIZE: " + blob.GetBufferSize());

            // Read the data from the Blob into the byte array
            Marshal.Copy((nint)blob.GetBufferPointer(), ddsArray, 0, ddsArray.Length);
            return ddsArray;
        }
        finally
        {
            blob.Release();
        }
    }

    public static void GetDDSInformation(string ddsFilePath, out D3DTXMetadata metadata, out ImageSection[] sections, DDSFlags flags = DDSFlags.None)
    {
        ScratchImage image = GetDDSImage(ByteFunctions.LoadTexture(ddsFilePath), flags);

        metadata = GetDDSInformation(image.GetMetadata());
        sections = GetDDSImageSections(image, flags);

        image.Release();
    }

    public static D3DTXMetadata GetDDSInformation(TexMetadata metadata)
    {
        return new D3DTXMetadata
        {
            Width = (uint)metadata.Width,
            Height = (uint)metadata.Height,
            Depth = (uint)metadata.Depth,
            ArraySize = (uint)metadata.ArraySize,
            MipLevels = (uint)metadata.MipLevels,
            Format = DDSHelper.GetTelltaleSurfaceFormat((DXGIFormat)metadata.Format),
            SurfaceGamma = DirectXTex.IsSRGB(metadata.Format) ? T3SurfaceGamma.sRGB : T3SurfaceGamma.Linear,
            D3DFormat = DDSHelper.GetD3DFORMAT((DXGIFormat)metadata.Format, metadata),
            Dimension = DDSHelper.GetDimensionFromDDS(metadata),
        };
    }

    /// <summary>
    /// Returns a byte array List containing the pixel data from a DDS_DirectXTexNet_ImageSection array.
    /// </summary>
    /// <param name="sections">The sections of the DDS image.</param>
    /// <returns></returns>
    public static List<byte[]> GetPixelDataListFromSections(ImageSection[] sections)
    {
        List<byte[]> textureData = [];

        foreach (ImageSection imageSection in sections)
        {
            textureData.Add(imageSection.Pixels);
        }

        return textureData;
    }

    /// <summary>
    /// Returns a byte array List containing the pixel data from a DDS_DirectXTexNet_ImageSection array.
    /// </summary>
    /// <param name="sections">The sections of the DDS image.</param>
    /// <returns></returns>
    public static byte[] GetPixelDataArrayFromSections(ImageSection[] sections)
    {
        byte[] textureData = [];

        foreach (ImageSection imageSection in sections)
        {
            textureData = ByteFunctions.Combine(textureData, imageSection.Pixels);
        }

        return textureData;
    }

    /// <summary>
    /// Returns the image sections of the DDS image. Each mipmap and slice is a section on its own. 
    /// </summary>
    /// <param name="ddsImage">The DirectXTexNet DDS image.</param>
    /// <param name="flags">(Optional) The mode in which the DirectXTexNet will load the .dds file. If not provided, it defaults to NONE.</param>
    /// <returns>The DDS sections</returns>
    public static ImageSection[] GetDDSImageSections(ScratchImage ddsImage, DDSFlags flags = DDSFlags.None)
    {
        List<ImageSection> sections = [];

        if (flags == DDSFlags.ForceDx9Legacy)
        {
            sections.Add(new()
            {
                Pixels = GetDDSHeaderBytes(ddsImage)
            });
        }

        Image[] images = GetImages(ddsImage);

        for (int i = 0; i < images.Length; i++)
        {
            byte[] pixels = new byte[images[i].SlicePitch];

            Marshal.Copy((nint)images[i].Pixels, pixels, 0, pixels.Length);

            sections.Add(new()
            {
                Width = (nuint)images[i].Width,
                Height = (nuint)images[i].Height,
                Format = (DXGIFormat)images[i].Format,
                SlicePitch = (nuint)images[i].SlicePitch,
                RowPitch = (nuint)images[i].RowPitch,
                Pixels = pixels
            });
        }

        for (int i = 0; i < sections.Count; i++)
        {
            Console.WriteLine($"Image {i} - Width: {sections[i].Width}, Height: {sections[i].Height}, Format: {sections[i].Format}, SlicePitch: {sections[i].SlicePitch}, RowPitch: {sections[i].RowPitch}");
            Console.WriteLine($"Image {i} - Pixels: {sections[i].Pixels.Length}");
        }

        return sections.ToArray();
    }

    public static byte[] GetDDSHeaderBytes(ScratchImage image, DDSFlags flags = DDSFlags.None)
    {
        Blob blob = DirectXTex.CreateBlob();
        TexMetadata metadata = image.GetMetadata();
        DirectXTex.SaveToDDSMemory2(image.GetImages(), image.GetImageCount(), ref metadata, flags, ref blob).ThrowIf();

        blob.GetBufferPointer();

        // Extract the DDS header from the blob
        byte[] ddsHeaderCheckDX10 = new byte[148];
        Marshal.Copy((nint)blob.GetBufferPointer(), ddsHeaderCheckDX10, 0, ddsHeaderCheckDX10.Length);
        blob.Release();

        byte[] ddsHeader;
        int size;
        if (ddsHeaderCheckDX10[84] == 0x44 && ddsHeaderCheckDX10[85] == 0x58 && ddsHeaderCheckDX10[86] == 0x31 && ddsHeaderCheckDX10[87] == 0x30)
        {
            size = 148;
        }
        else
        {
            size = 128;
        }

        ddsHeader = new byte[size];
        Array.Copy(ddsHeaderCheckDX10, 0, ddsHeader, 0, size);

        return ddsHeader;
    }

    public static void SetDDSHeaderFourCC(ref byte[] header, char c1, char c2, char c3, char c4)
    {
        if (header == null || header.Length < 88)
        {
            throw new Exception("Invalid DDS Header length!");
        }
        header[84] = (byte)c1;
        header[85] = (byte)c2;
        header[86] = (byte)c3;
        header[87] = (byte)c4;
    }

    private static Image[] GetImages(ScratchImage image)
    {
        Image* pointerImages = DirectXTex.GetImages(image);

        int imageCount = (int)image.GetImageCount();

        Image[] images = new Image[imageCount];

        for (int i = 0; i < imageCount; i++)
        {
            images[i] = pointerImages[i];
        }

        return images;
    }

    public static byte[] AsBytes(Blob blob)
    {
        byte[] bytes = new byte[blob.GetBufferSize()];
        Marshal.Copy((nint)blob.GetBufferPointer(), bytes, 0, bytes.Length);
        return bytes;
    }

    /// <summary>
    /// Returns a boolean if a Direct3D10/DXGI format is sRGB.
    /// </summary>
    /// <param name="dxgiFormat">The Direct3D10/DXGI format.</param>
    /// <returns>True, if it is SRGB. Otherwise - false.</returns>
    public static bool IsSRGB(DXGIFormat dxgiFormat)
    {
        return DirectXTex.IsSRGB((int)dxgiFormat);
    }

    /// <summary>
    /// Returns information about the DDS image.
    /// </summary>
    /// <param name="metadata">The metadata of the DDS image.</param>
    /// <returns>The string containing the DDS metadata information</returns>
    public static string GetDDSDebugInfo(TexMetadata metadata)
    {
        StringBuilder information = new();

        information.AppendLine("||||||||||| DDS Debug Information |||||||||||");
        information.AppendLine($"Width: {metadata.Width}");
        information.AppendLine($"Height: {metadata.Height}");
        information.AppendLine($"Depth: {metadata.Depth}");
        information.AppendLine($"Format: {Enum.GetName((DXGIFormat)metadata.Format)} " + "(" + metadata.Format + ")");
        information.AppendLine($"Mips: {metadata.MipLevels}");
        information.AppendLine($"Dimension: {metadata.Dimension}");
        information.AppendLine($"Array Elements: {metadata.ArraySize}");
        information.AppendLine($"Volumemap: {metadata.IsVolumemap()}");
        information.AppendLine($"Cubemap: {metadata.IsCubemap()}");
        information.AppendLine($"Alpha mode: {metadata.GetAlphaMode()}");
        information.AppendLine($"Premultiplied alpha: {metadata.IsPMAlpha()}");
        information.AppendLine($"Misc Flags: {metadata.MiscFlags}");
        information.AppendLine($"Misc Flags2: {metadata.MiscFlags2}");

        return information.ToString();
    }

    unsafe public static void ReverseChannels(Vector4* outPixels, Vector4* inPixels, ulong width, ulong y)
    {
        for (ulong j = 0; j < width; ++j)
        {
            Vector4 value = inPixels[j];

            outPixels[j].X = value.W;
            outPixels[j].Y = value.Z;
            outPixels[j].Z = value.Y;
            outPixels[j].W = value.X;
        }
    }

    unsafe public static void RemoveZ(Vector4* outPixels, Vector4* inPixels, ulong width, ulong y)
    {
        for (ulong j = 0; j < width; ++j)
        {
            Vector2 NormalXY = new(inPixels[j].X, inPixels[j].Y);

            outPixels[j] = new Vector4(inPixels[j].X, inPixels[j].Y, 0, 0);
        }
    }

    unsafe public static void RestoreZ(Vector4* outPixels, Vector4* inPixels, ulong width, ulong y)
    {
        for (ulong j = 0; j < width; ++j)
        {
            Vector2 NormalXY = new(inPixels[j].X, inPixels[j].Y);

            NormalXY = NormalXY * 2.0f - Vector2.One;
            float NormalZ = (float)Math.Sqrt(Math.Clamp(1.0f - Vector2.Dot(NormalXY, NormalXY), 0, 1));

            outPixels[j] = new Vector4(inPixels[j].X, inPixels[j].Y, NormalZ, 1.0f);
        }
    }

    unsafe public static void DefaultCopy(Vector4* outPixels, Vector4* inPixels, ulong width, ulong y)
    {
        for (ulong j = 0; j < width; ++j)
        {
            outPixels[j] = inPixels[j];
        }
    }
}

public unsafe partial class Texture
{
    public string FilePath { get; set; } = string.Empty;
    private ScratchImage Image { get; set; }
    public TexMetadata Metadata { get; set; }

    private ScratchImage OriginalImage { get; set; }
    private TexMetadata OriginalMetadata { get; set; }

    public ImageAdvancedOptions CurrentOptions { get; set; } = new ImageAdvancedOptions();
    public TextureType TextureType { get; set; } = TextureType.Unknown;
    private DXGIFormat PreviewFormat { get; set; }
    private ulong PreviewWidth { get; set; }
    private ulong PreviewHeight { get; set; }
    private uint PreviewMip { get; set; } = 0;

    public Texture() { }
    public Texture(string filePath, DDSFlags flags = DDSFlags.None)
    {
        Initialize(filePath, flags);
        FilePath = filePath;
    }

    public Texture(byte[] data, TextureType textureType = TextureType.D3DTX, DDSFlags flags = DDSFlags.None)
    {
        Initialize(data, flags);
        TextureType = textureType;
    }

    public Texture(string filePath, TextureType textureType, DDSFlags flags = DDSFlags.None)
    {
        Initialize(filePath, textureType, flags);
        FilePath = filePath;
        TextureType = textureType;
    }

    private void Initialize(byte[] ddsData, DDSFlags flags = DDSFlags.None)
    {
        InitializeSingleScratchImage(ddsData, false, flags);
        InitializeSingleScratchImage(ddsData, true, flags);
    }

    private void InitializeSingleScratchImage(byte[] ddsData, bool isCopy, DDSFlags flags = DDSFlags.None)
    {
        ScratchImage Image = DirectXTex.CreateScratchImage();

        Span<byte> src = new(ddsData);
        Blob blob = DirectXTex.CreateBlob();
        TexMetadata meta = new();

        Console.WriteLine("DDS LENGTH: " + ddsData.Length);
        fixed (byte* srcPtr = src)
        {
            DirectXTex.LoadFromDDSMemory(srcPtr, (nuint)src.Length, flags, ref meta, ref Image).ThrowIf();
        }

        if (isCopy)
        {
            this.Image = Image;
            Metadata = meta;
        }
        else
        {
            OriginalImage = Image;
            OriginalMetadata = meta;
        }

        blob.Release();
    }

    private void Initialize(string filePath, DDSFlags flags = DDSFlags.None)
    {
        ScratchImage Image = DirectXTex.CreateScratchImage();
        Blob blob = DirectXTex.CreateBlob();
        TexMetadata meta = new();

        DirectXTex.LoadFromDDSFile(filePath, flags, ref meta, ref Image).ThrowIf();

        this.Image = Image;
        Metadata = meta;

        blob.Release();
    }

    public string GetDDSDebugInfo()
    {
        return TextureManager.GetDDSDebugInfo(Metadata);
    }

    public void ConvertToRGBA()
    {
        ScratchImage destImage = DirectXTex.CreateScratchImage();

        TexMetadata ogMeta = Image.GetMetadata();
        Decompress(DXGIFormat.R8G8B8A8_UNORM);

        if (Image.GetMetadata().Format != (uint)DXGIFormat.R8G8B8A8_UNORM)
        {
            DirectXTex.Convert2(Image.GetImages(), Image.GetImageCount(), ref ogMeta, (int)DXGIFormat.R8G8B8A8_UNORM, TexFilterFlags.Default, 0.5f, ref destImage).ThrowIf();

            Image.Release();
            Image = destImage;
        }
        else
        {
            destImage.Release();
        }

        Metadata = Image.GetMetadata();
    }

    public byte[] GetSectionPixelData(uint mip, uint face)
    {
        if ((uint)Image.GetMetadata().Format != (uint)DXGIFormat.R8G8B8A8_UNORM)
        {
            ConvertToRGBA();
        }

        uint slice = 0;

        // Swap slice and face if it's a 3D texture, because they don't have faces.
        if (Metadata.Dimension == TexDimension.Texture3D)
        {
            slice = face;
            face = 0;
        }

        var image = DirectXTex.GetImage(Image, (ulong)mip, (ulong)face, (ulong)slice);

        PreviewFormat = (DXGIFormat)image->Format;
        PreviewWidth = image->Width;
        PreviewHeight = image->Height;
        PreviewMip = mip;

        byte[] pixels = new byte[image->SlicePitch];

        Marshal.Copy((nint)image->Pixels, pixels, 0, pixels.Length);

        return pixels;
    }

    public void GetData(uint mip, uint face, out ulong width, out ulong height, out ulong pitch, out ulong length, out byte[] pixels)
    {
        pixels = GetSectionPixelData(mip, face);
        width = PreviewWidth;
        height = PreviewHeight;
        pitch = ComputePitch(PreviewFormat, PreviewWidth, PreviewHeight);
        length = width * height * (DirectXTex.BitsPerPixel((int)PreviewFormat) / 8);
    }

    public void GetBounds(out uint mip, out uint face)
    {
        mip = (uint)Image.GetMetadata().MipLevels - 1;

        if (Image.GetMetadata().IsVolumemap())
        {
            uint mipDiff = (uint)(Image.GetMetadata().MipLevels - PreviewMip);
            uint depth = (uint)Image.GetMetadata().Depth;

            while (mipDiff > 0)
            {
                depth = Math.Max(1, depth / 2);
                mipDiff--;
            }

            face = depth - 1;
        }
        else
        {
            face = (uint)Image.GetMetadata().ArraySize - 1;
        }
    }

    public void Compress(DXGIFormat format = DXGIFormat.UNKNOWN)
    {
        if (!DirectXTex.IsCompressed((int)format))
        {
            return;
        }

        ScratchImage transformedImage = DirectXTex.CreateScratchImage();

        if (!DirectXTex.IsCompressed(Image.GetMetadata().Format) && (int)format != Image.GetMetadata().Format)
        {
            TexMetadata originalMetadata = Image.GetMetadata();

            DirectXTex.Compress2(Image.GetImages(), Image.GetImageCount(), ref originalMetadata, (int)format, TexCompressFlags.Default, 0.5f, ref transformedImage).ThrowIf();

            Image.Release();
            Image = transformedImage;
        }
        else
        {
            transformedImage.Release();
        }
    }

    public void Decompress(DXGIFormat format = DXGIFormat.UNKNOWN)
    {
        if (DirectXTex.IsCompressed((int)format))
        {
            throw new Exception("Invalid format!");
        }

        ScratchImage transformedImage = DirectXTex.CreateScratchImage();

        if (DirectXTex.IsCompressed(Image.GetMetadata().Format) && (int)format != Image.GetMetadata().Format)
        {
            TexMetadata originalMetadata = Image.GetMetadata();

            DirectXTex.Decompress2(Image.GetImages(), Image.GetImageCount(), ref originalMetadata, (int)format, ref transformedImage).ThrowIf();

            Image.Release();
            Image = transformedImage;
        }
        else
        {
            transformedImage.Release();
        }
    }

    public void TransformImage(ImageEffect conversionMode = ImageEffect.DEFAULT)
    {
        Decompress();

        TransformImageFunc transformFunction = DefaultCopy;

        if (conversionMode == ImageEffect.RESTORE_Z)
        {
            transformFunction = RestoreZ;
        }
        else if (conversionMode == ImageEffect.REMOVE_Z)
        {
            transformFunction = RemoveZ;
        }
        else if (conversionMode == ImageEffect.SWIZZLE_ABGR)
        {
            transformFunction = ReverseChannels;
        }

        ScratchImage transformedImage = DirectXTex.CreateScratchImage();
        TexMetadata texMetadata = Image.GetMetadata();

        DirectXTex.TransformImage2(Image.GetImages(), Image.GetImageCount(), ref texMetadata, transformFunction, ref transformedImage).ThrowIf();

        Console.WriteLine("Transforming image" + Image.GetImageCount());

        Image.Release();

        Image = transformedImage;
    }

    public void Initialize(string filePath, TextureType textureType, DDSFlags flags = DDSFlags.None)
    {
        TextureType = textureType;
        Initialize(filePath, textureType, false, flags);
        Initialize(filePath, textureType, true, flags);
    }

    private void Initialize(string filePath, TextureType textureType, bool isCopy = false, DDSFlags flags = DDSFlags.None)
    {
        ScratchImage scratchImage = DirectXTex.CreateScratchImage();
        TexMetadata texMetadata = new();

        try
        {
            switch (textureType)
            {
                case TextureType.DDS: DirectXTex.LoadFromDDSFile(filePath, flags, ref texMetadata, ref scratchImage).ThrowIf(); break;
                case TextureType.PNG:
                    if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                    {
                        DirectXTex.LoadFromWICFile(filePath, WICFlags.AllFrames, ref texMetadata, ref scratchImage, default).ThrowIf(); break;
                    }
                    else if (Environment.OSVersion.Platform == PlatformID.Unix)
                    {
                        DirectXTex.LoadFromPNGFile(filePath, ref texMetadata, ref scratchImage).ThrowIf();
                    }
                    break;
                case TextureType.HDR: DirectXTex.LoadFromHDRFile(filePath, ref texMetadata, ref scratchImage).ThrowIf(); break;
                case TextureType.JPEG: DirectXTex.LoadFromJPEGFile(filePath, ref texMetadata, ref scratchImage).ThrowIf(); break;
                case TextureType.TGA: DirectXTex.LoadFromTGAFile2(filePath, ref texMetadata, ref scratchImage).ThrowIf(); break;
                case TextureType.TIFF: DirectXTex.LoadFromWICFile(filePath, WICFlags.AllFrames, ref texMetadata, ref scratchImage, default).ThrowIf(); break;
                case TextureType.BMP: DirectXTex.LoadFromWICFile(filePath, WICFlags.AllFrames, ref texMetadata, ref scratchImage, default).ThrowIf(); break;
                default: break;
            }

            if (isCopy)
            {
                if (!Image.IsNull)
                {
                    Image.Release();
                }
                Image = scratchImage;
                Metadata = texMetadata;
            }
            else
            {
                if (!OriginalImage.IsNull)
                {
                    OriginalImage.Release();
                }
                OriginalImage = scratchImage;
                OriginalMetadata = texMetadata;
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            scratchImage.Release();
            throw new Exception("Failed to load image!");
        }
    }

    private void ResetImageToOriginal()
    {
        Metadata = OriginalMetadata;

        if (TextureType != TextureType.D3DTX)
        {
            Initialize(FilePath, TextureType, true, DDSFlags.None);
        }
        else
        {
            var memory = GetDDSByteArray(OriginalImage);
            InitializeSingleScratchImage(memory, true);
        }
    }

    public void ChangePreviewImage(ImageAdvancedOptions options, bool keepOriginal = false)
    {
        ResetImageToOriginal();

        if (options.EnableSwizzle && options.IsDeswizzle)
        {
            Deswizzle(options.PlatformType);
        }

        Decompress(DXGIFormat.R8G8B8A8_UNORM);

        if (options.EnableNormalMap)
        {
            GenerateNormalMap();
        }

        if (options.EnableEditing)
        {
            if (options.ImageEffect != ImageEffect.DEFAULT)
            {
                TransformImage(options.ImageEffect);
            }
        }

        if (options.EnableTelltaleNormalMap && options.IsTelltaleNormalMap)
        {
            TransformImage(ImageEffect.SWIZZLE_ABGR);
        }

        if (CurrentOptions.EnableNormalMap != options.EnableNormalMap)
        {
            if (!CurrentOptions.EnableNormalMap)
            { GenerateNormalMap(); }
        }

        if (options.EnableMips)
        {
            if (options.AutoGenerateMips)
            {
                GenerateMipMaps(0);
            }
            else if (options.ManualGenerateMips && options.SetMips > 1)
            {
                GenerateMipMaps(Math.Min(options.SetMips, GetMaxMipLevels()));
            }
        }

        if (options.EnableAutomaticCompression)
        {
            if (options.EnableNormalMap && options.IsTelltaleXYNormalMap)
            {
                Compress(DXGIFormat.BC5_UNORM);
            }
            else if (OriginalImage.IsAlphaAllOpaque())
            {
                if (options.IsSRGB)
                {
                    Compress(DXGIFormat.BC1_UNORM_SRGB);
                }
                else
                {
                    Compress(DXGIFormat.BC1_UNORM);
                }
            }
            else
            {
                if (options.IsSRGB)
                {
                    Compress(DXGIFormat.BC3_UNORM_SRGB);
                }
                else
                {
                    Compress(DXGIFormat.BC3_UNORM);
                }
            }
        }
        else if (keepOriginal)
        {
            Compress((DXGIFormat)OriginalImage.GetMetadata().Format);
        }

        if (options.EnableSwizzle && options.IsSwizzle)
        {
            Swizzle(options.PlatformType);
        }

        CurrentOptions = new(options);
    }

    public uint GetMaxMipLevels()
    {
        uint width = (uint)Metadata.Width;
        uint height = (uint)Metadata.Height;
        uint depth = (uint)Metadata.Depth;

        uint mipLevels = 1;

        while (width > 1 || height > 1 || depth > 1)
        {
            width = Math.Max(1, width / 2);
            height = Math.Max(1, height / 2);
            depth = Math.Max(1, depth / 2);

            mipLevels++;
        }

        return mipLevels;
    }

    public void GenerateNormalMap()
    {
        if (DirectXTex.IsCompressed(Image.GetMetadata().Format))
            Decompress();

        ScratchImage destImage = DirectXTex.CreateScratchImage();
        TexMetadata metadata = Image.GetMetadata();

        DirectXTex.ComputeNormalMap2(Image.GetImages(), Image.GetImageCount(), ref metadata, CNMAPFlags.Default, 7, metadata.Format, ref destImage).ThrowIf();

        Image.Release();
        Image = destImage;
    }

    public void Swizzle(T3PlatformType platform = T3PlatformType.ePlatform_None)
    {
        TexMetadata metadata = Image.GetMetadata();

        var images = GetDDSImageSections(Image);

        foreach (var image in images)
        {
            image.Pixels = platform switch
            {
                T3PlatformType.ePlatform_NX => DrSwizzler.Swizzler.SwitchSwizzle(image.Pixels, (int)image.Width, (int)image.Height, (DrSwizzler.DDS.DXEnums.DXGIFormat)image.Format),
                T3PlatformType.ePlatform_PS3 => DrSwizzler.Swizzler.PS3Swizzle(image.Pixels, (int)image.Width, (int)image.Height, (DrSwizzler.DDS.DXEnums.DXGIFormat)image.Format),
                T3PlatformType.ePlatform_PS4 => DrSwizzler.Swizzler.PS4Swizzle(image.Pixels, (int)image.Width, (int)image.Height, (DrSwizzler.DDS.DXEnums.DXGIFormat)image.Format),
                T3PlatformType.ePlatform_Xbox => DrSwizzler.Swizzler.Xbox360Swizzle(image.Pixels, (int)image.Width, (int)image.Height, (DrSwizzler.DDS.DXEnums.DXGIFormat)image.Format),
                T3PlatformType.ePlatform_Vita => DrSwizzler.Swizzler.VitaSwizzle(image.Pixels, (int)image.Width, (int)image.Height, (DrSwizzler.DDS.DXEnums.DXGIFormat)image.Format),
                _ => image.Pixels
            };
        }

        byte[] header = GetDDSHeaderBytes(Image);
        byte[] pixels = images.SelectMany(x => x.Pixels).ToArray();

        byte[] newDDS = ByteFunctions.Combine(header, pixels);

        Image.Release();

        InitializeSingleScratchImage(newDDS, true);
    }

    public void Deswizzle(T3PlatformType platform = T3PlatformType.ePlatform_None)
    {
        TexMetadata metadata = Image.GetMetadata();

        var images = GetDDSImageSections(Image);

        foreach (var image in images)
        {
            image.Pixels = platform switch
            {
                T3PlatformType.ePlatform_NX => DrSwizzler.Deswizzler.SwitchDeswizzle(image.Pixels, (int)image.Width, (int)image.Height, (DrSwizzler.DDS.DXEnums.DXGIFormat)image.Format),
                T3PlatformType.ePlatform_PS3 => DrSwizzler.Deswizzler.PS3Deswizzle(image.Pixels, (int)image.Width, (int)image.Height, (DrSwizzler.DDS.DXEnums.DXGIFormat)image.Format),
                T3PlatformType.ePlatform_PS4 => DrSwizzler.Deswizzler.PS4Deswizzle(image.Pixels, (int)image.Width, (int)image.Height, (DrSwizzler.DDS.DXEnums.DXGIFormat)image.Format),
                T3PlatformType.ePlatform_Xbox => DrSwizzler.Deswizzler.Xbox360Deswizzle(image.Pixels, (int)image.Width, (int)image.Height, (DrSwizzler.DDS.DXEnums.DXGIFormat)image.Format),
                T3PlatformType.ePlatform_Vita => DrSwizzler.Deswizzler.VitaDeswizzle(image.Pixels, (int)image.Width, (int)image.Height, (DrSwizzler.DDS.DXEnums.DXGIFormat)image.Format),
                _ => image.Pixels
            };
        }

        byte[] header = GetDDSHeaderBytes(Image);
        byte[] pixels = images.SelectMany(x => x.Pixels).ToArray();

        byte[] newDDS = ByteFunctions.Combine(header, pixels);

        Image.Release();

        InitializeSingleScratchImage(newDDS, true);
    }

    public void GenerateMipMaps(uint mipLevels = 0)
    {
        Decompress();

        ScratchImage destImage = DirectXTex.CreateScratchImage();
        TexMetadata metadata = Image.GetMetadata();

        if (metadata.IsVolumemap())
        {
            DirectXTex.GenerateMipMaps3D2(Image.GetImages(), Image.GetImageCount(), TexFilterFlags.Default, (ulong)mipLevels, ref destImage).ThrowIf();
        }
        else
        {
            DirectXTex.GenerateMipMaps2(Image.GetImages(), Image.GetImageCount(), ref metadata, TexFilterFlags.Default, (ulong)mipLevels, ref destImage).ThrowIf();
        }

        Console.WriteLine("Mipmaps generated" + destImage.GetImageCount());
        Image.Release();
        Image = destImage;
    }

    public void SaveAsPNG(string filePath)
    {
        Decompress();

        if (Image.GetMetadata().Format != (int)DXGIFormat.R8_UNORM &&
            Image.GetMetadata().Format != (int)DXGIFormat.B8G8R8A8_UNORM &&
            Image.GetMetadata().Format != (int)DXGIFormat.B8G8R8X8_UNORM &&
            Image.GetMetadata().Format != (int)DXGIFormat.R8G8B8A8_UNORM)
        {
            Convert(DXGIFormat.R8G8B8A8_UNORM);
        }

        if (Metadata.ArraySize == 1)
        {
            if (Metadata.Depth == 1)
            {
                DirectXTex.SaveToPNGFile(Image.GetImage(0, 0, 0), filePath).ThrowIf();
            }
            else
            {
                for (uint i = 0; i < Metadata.Depth; i++)
                {
                    StringBuilder sb = new();
                    sb.Append(filePath).Append('_').Append(i);
                    DirectXTex.SaveToPNGFile(Image.GetImage(0, 0, i), sb.ToString()).ThrowIf();
                }
            }
        }
        else
        {
            for (uint i = 0; i < Metadata.ArraySize; i++)
            {
                StringBuilder sb = new();
                sb.Append(filePath).Append('_').Append(i);
                DirectXTex.SaveToPNGFile(Image.GetImage(0, i, 0), sb.ToString()).ThrowIf();
            }
        }
    }

    public void SaveAsJPEG(string filePath)
    {
        Decompress();

        if (Image.GetMetadata().Format != (int)DXGIFormat.R8_UNORM &&
            Image.GetMetadata().Format != (int)DXGIFormat.R8G8B8A8_UNORM)
        {
            Convert(DXGIFormat.R8G8B8A8_UNORM);
        }

        if (Metadata.ArraySize == 1)
        {
            if (Metadata.Depth == 1)
            {
                DirectXTex.SaveToJPEGFile(Image.GetImage(0, 0, 0), filePath).ThrowIf();
            }
            else
            {
                for (uint i = 0; i < Metadata.Depth; i++)
                {
                    StringBuilder sb = new();
                    sb.Append(filePath).Append('_').Append(i);
                    DirectXTex.SaveToJPEGFile(Image.GetImage(0, 0, i), sb.ToString()).ThrowIf();
                }
            }
        }
        else
        {
            for (uint i = 0; i < Metadata.ArraySize; i++)
            {
                StringBuilder sb = new();
                sb.Append(filePath).Append('_').Append(i);
                DirectXTex.SaveToJPEGFile(Image.GetImage(0, i, 0), sb.ToString()).ThrowIf();
            }
        }
    }

    public void SaveAsTGA(string filePath)
    {
        Decompress();

        if (Image.GetMetadata().Format != (int)DXGIFormat.R8_UNORM &&
            Image.GetMetadata().Format != (int)DXGIFormat.A8_UNORM &&
            Image.GetMetadata().Format != (int)DXGIFormat.R8G8B8A8_UNORM &&
            Image.GetMetadata().Format != (int)DXGIFormat.R8G8B8A8_UNORM_SRGB &&
            Image.GetMetadata().Format != (int)DXGIFormat.B8G8R8A8_UNORM &&
            Image.GetMetadata().Format != (int)DXGIFormat.B8G8R8A8_UNORM_SRGB &&
            Image.GetMetadata().Format != (int)DXGIFormat.B8G8R8X8_UNORM &&
            Image.GetMetadata().Format != (int)DXGIFormat.B8G8R8X8_UNORM_SRGB &&
            Image.GetMetadata().Format != (int)DXGIFormat.B8G8R8X8_UNORM &&
            Image.GetMetadata().Format != (int)DXGIFormat.B5G5R5A1_UNORM)
        {
            Convert(DXGIFormat.R8G8B8A8_UNORM);
        }

        TexMetadata tgaMetadata = Image.GetMetadata();

        if (Metadata.ArraySize == 1)
        {
            if (Metadata.Depth == 1)
            {
                DirectXTex.SaveToTGAFile2(Image.GetImage(0, 0, 0), filePath, ref tgaMetadata).ThrowIf();
            }
            else
            {
                for (uint i = 0; i < Metadata.Depth; i++)
                {
                    StringBuilder sb = new();
                    sb.Append(filePath).Append('_').Append(i);
                    DirectXTex.SaveToTGAFile2(Image.GetImage(0, 0, i), filePath, ref tgaMetadata).ThrowIf();
                }
            }
        }
        else
        {
            for (uint i = 0; i < Metadata.ArraySize; i++)
            {
                StringBuilder sb = new();
                sb.Append(filePath).Append('_').Append(i);
                DirectXTex.SaveToTGAFile2(Image.GetImage(0, i, 0), filePath, ref tgaMetadata).ThrowIf();
            }
        }
    }

    public void SaveAsHDR(string filePath)
    {
        Decompress();

        if (Image.GetMetadata().Format != (int)DXGIFormat.R32G32B32A32_FLOAT)
        {
            Convert(DXGIFormat.R32G32B32A32_FLOAT);
        }

        if (Metadata.ArraySize == 1)
        {
            if (Metadata.Depth == 1)
            {
                DirectXTex.SaveToHDRFile(Image.GetImage(0, 0, 0), filePath);
            }
            else
            {
                for (uint i = 0; i < Metadata.Depth; i++)
                {
                    StringBuilder sb = new();
                    sb.Append(filePath).Append('_').Append(i);
                    DirectXTex.SaveToHDRFile(Image.GetImage(0, 0, i), sb.ToString()).ThrowIf();
                }
            }
        }
        else
        {
            for (uint i = 0; i < Metadata.ArraySize; i++)
            {
                StringBuilder sb = new();
                sb.Append(filePath).Append('_').Append(i);
                DirectXTex.SaveToHDRFile(Image.GetImage(0, i, 0), sb.ToString()).ThrowIf();
            }
        }
    }

    public void SaveAsWIC(string filePath)
    {
        Decompress();

        if (Image.GetMetadata().Format == (int)DXGIFormat.R8G8_UNORM || Image.GetMetadata().Format == (int)DXGIFormat.R8G8_SNORM)
        {
            Convert(DXGIFormat.R8G8B8A8_UNORM);
        }

        if (Metadata.ArraySize == 1)
        {
            if (Metadata.Depth == 1)
            {
                DirectXTex.SaveToWICFile(Image.GetImage(0, 0, 0), WICFlags.None, DirectXTex.GetWICCodec(WICCodecs.CodecBmp), filePath, null, default).ThrowIf();
            }
            else
            {
                for (uint i = 0; i < Metadata.Depth; i++)
                {
                    StringBuilder sb = new();
                    sb.Append(filePath).Append('_').Append(i);
                    DirectXTex.SaveToWICFile(Image.GetImage(0, 0, i), WICFlags.None, DirectXTex.GetWICCodec(WICCodecs.CodecBmp), filePath, null, default).ThrowIf();
                }
            }
        }
        else
        {
            for (uint i = 0; i < Metadata.ArraySize; i++)
            {
                StringBuilder sb = new();
                sb.Append(filePath).Append('_').Append(i);
                DirectXTex.SaveToWICFile(Image.GetImage(0, i, 0), WICFlags.None, DirectXTex.GetWICCodec(WICCodecs.CodecBmp), filePath, null, default).ThrowIf();
            }
        }
    }

    public void SaveAsDDS(string filePath)
    {
        TexMetadata metadata = Image.GetMetadata();
        DirectXTex.SaveToDDSFile2(Image.GetImages(), Image.GetImageCount(), ref metadata, DDSFlags.None, filePath);
    }

    public void SaveTexture(string filePath, TextureType textureType = TextureType.Unknown)
    {
        switch (textureType)
        {
            case TextureType.DDS:
                filePath += ".dds";
                SaveAsDDS(filePath); break;
            case TextureType.BMP:
                filePath += ".bmp";
                SaveAsWIC(filePath); break;
            case TextureType.PNG:
                filePath += ".png";
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    SaveAsWIC(filePath);
                }
                else if (Environment.OSVersion.Platform == PlatformID.Unix)
                {
                    SaveAsPNG(filePath);
                }
                break;
            case TextureType.JPEG:
                filePath += ".jpeg";
                SaveAsJPEG(filePath); break;
            case TextureType.HDR:
                filePath += ".hdr";
                SaveAsHDR(filePath); break;
            case TextureType.TGA:
                filePath += ".tga";
                SaveAsTGA(filePath); break;
            case TextureType.TIFF:
                filePath += ".tiff";
                SaveAsWIC(filePath); break;
            default:
                filePath += ".dds";
                SaveAsDDS(filePath); break;
        }
    }

    public void Convert(DXGIFormat format)
    {
        TexMetadata texMetadata = Image.GetMetadata();

        if ((int)format != texMetadata.Format)
        {
            ScratchImage transformedImage = DirectXTex.CreateScratchImage();

            DirectXTex.Convert2(Image.GetImages(), Image.GetImageCount(), ref texMetadata, (int)format, TexFilterFlags.Default, 0.5f, ref transformedImage).ThrowIf();

            Image.Release();
            Image = transformedImage;
        }
    }

    public void ApplyTexFilter(TexFilterFlags filter = TexFilterFlags.Default)
    {
        Decompress();

        TexMetadata texMetadata = Image.GetMetadata();

        ScratchImage transformedImage = DirectXTex.CreateScratchImage();

        DirectXTex.Convert2(Image.GetImages(), Image.GetImageCount(), ref texMetadata, (int)DXGIFormat.R8G8B8A8_UNORM, filter, 0.5f, ref transformedImage).ThrowIf();

        Image.Release();
        Image = transformedImage;
    }

    public void GetDDSInformation(out D3DTXMetadata metadata, out ImageSection[] sections, DDSFlags flags = DDSFlags.None)
    {
        metadata = TextureManager.GetDDSInformation(Image.GetMetadata());
        sections = GetDDSImageSections(Image, flags);
    }

    public void Release()
    {
        if (!Image.IsNull)
        {
            Image.Release();
        }
        if (!OriginalImage.IsNull)
        {
            OriginalImage.Release();
        }
    }
}