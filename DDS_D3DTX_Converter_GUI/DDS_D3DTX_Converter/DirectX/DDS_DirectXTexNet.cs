
using D3DTX_Converter.Utilities;
using HexaEngine.DirectXTex;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace D3DTX_Converter.DirectX;

/// <summary>
/// A class that provides methods to interact with DirectXTexNet. Mainly used for loading and saving DDS files.
/// </summary>
public static class DDS_DirectXTexNet
{

    /// <summary>
    /// Get the DDS image from DirectXTexNet.
    /// </summary>
    /// <param name="ddsFilePath">The file path of the .dds file.</param>
    /// <param name="flags">(Optional) The mode in which the DirectXTexNet will load the .dds file. If not provided, it defaults to NONE.</param>
    /// <returns>ScratchImage instance of the DDS file.</returns>
    unsafe public static ScratchImage GetDDSImage(string ddsFilePath, DDSFlags flags = DDSFlags.None)
    {
        ScratchImage image = DirectXTex.CreateScratchImage();
        TexMetadata metadata;

        DirectXTex.LoadFromDDSFile(ddsFilePath, DDSFlags.None, &metadata, image);

        return image;
    }


    /// <summary>
    /// Compute the pitch given the Direct3D10/DXGI format, the width and the height.
    /// </summary>
    /// <param name="dxgiFormat">The Direct3D10/DXGI format.</param>
    /// <param name="width">The width of the DDS image.</param>
    /// <param name="height">(Optional) The height of the DDS image. If not provided, it defaults to 0.</param>
    /// <returns>The pitch.</returns>
    unsafe public static uint ComputePitch(DXGIFormat dxgiFormat, nuint width, nuint height = 1)
    {
        nuint rowPitch;
        nuint slicePitch;

        DirectXTex.ComputePitch((int)dxgiFormat, width, height, &rowPitch, &slicePitch, CPFlags.None);
        return (uint)rowPitch;
    }

    /// <summary>
    /// Returns the channel count of a Direct3D10/DXGI format. It is used in the image properties display.
    /// </summary>
    /// <param name="dxgiFormat">The Direct3D10/DXGI format</param>
    /// <returns>The channel count.</returns>
    public static uint GetChannelCount(DXGIFormat dxgiFormat) => (uint)Math.Ceiling((double)DirectXTex.BitsPerPixel((int)dxgiFormat) / Math.Max(1, DirectXTex.BitsPerColor((int)dxgiFormat)));

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
            DirectXTex.LoadFromDDSMemory((void*)ptr, (nuint)array.Length, flags, &metadata, image);
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
    unsafe public static byte[] GetDDSByteArray(ScratchImage image, DDSFlags flags = DDSFlags.None)
    {
        Blob blob = DirectXTex.CreateBlob();
        try
        {
            DirectXTex.SaveToDDSMemory2(image.GetImages(), image.GetImageCount(), image.GetMetadata(), flags, blob);
            // Create a byte array to hold the data
            byte[] ddsArray = new byte[blob.GetBufferSize()];

            // Read the data from the Blob into the byte array
            Marshal.Copy((nint)blob.GetBufferPointer(), ddsArray, 0, ddsArray.Length);

            return ddsArray;
        }
        finally
        {
            blob.Release();
        }
    }

    // /// <summary>
    // /// Returns a byte array from a DirectXTexNet DDS image.
    // /// </summary>
    // /// <param name="image">The DirectXTexNet DDS image.</param>
    // /// <param name="flags">(Optional) The mode in which the DirectXTexNet will load the .dds file. If not provided, it defaults to NONE.</param>
    // /// <returns>The byte array containing the DDS data.</returns>
    // unsafe public static byte[] GetDDSByteArray(string filePath, DDSFlags flags = DDSFlags.None)
    // {

    //     ScratchImage image = DirectXTex.CreateScratchImage();
    //     Span<byte> src = LoadTexture(DDSFilename);
    //     Blob blob = DirectXTex.CreateBlob();

    //     TexMetadata metadata;
    //     fixed (byte* srcPtr = src)
    //     {
    //         DirectXTex.LoadFromDDSMemory(srcPtr, (nuint)src.Length, DDSFlags.None, &metadata, image);
    //     }

    //     DirectXTex.SaveToDDSMemory2(image.GetImages(), image.GetImageCount(), image.GetMetadata(), DDSFlags.None, blob);

    //     Span<byte> dest = blob.AsBytes();
    //     Assert.True(src.SequenceEqual(dest));

    //     blob.Release();
    //     image.Release();


    //     Blob blob = DirectXTex.CreateBlob();
    //     try
    //     {
    //         DirectXTex.SaveToDDSMemory2(image.GetImages(), image.GetImageCount(), image.GetMetadata(), flags, blob);
    //         // Create a byte array to hold the data
    //         byte[] ddsArray = new byte[blob.GetBufferSize()];

    //         // Read the data from the Blob into the byte array
    //         Marshal.Copy((nint)blob.GetBufferPointer(), ddsArray, 0, ddsArray.Length);

    //         return ddsArray;
    //     }
    //     finally
    //     {
    //         blob.Release();
    //     }
    // }

    /// <summary>
    /// Returns a byte array List containing the pixel data from a DDS_DirectXTexNet_ImageSection array.
    /// </summary>
    /// <param name="sections">The sections of the DDS image.</param>
    /// <returns></returns>
    public static List<byte[]> GetPixelDataFromSections(DDS_DirectXTexNet_ImageSection[] sections)
    {
        List<byte[]> textureData = [];

        foreach (DDS_DirectXTexNet_ImageSection imageSection in sections)
        {
            textureData.Add(imageSection.Pixels);
        }

        return textureData;
    }

    unsafe public static UnmanagedMemoryStream GetUnmanagedMemoryStreamFromMemory(byte[] array)
    {
        ScratchImage image = DirectXTex.CreateScratchImage();
        Blob blob = DirectXTex.CreateBlob();
        Span<byte> src = array;

        TexMetadata metadata;
        fixed (byte* srcPtr = src)
        {
            DirectXTex.LoadFromDDSMemory(srcPtr, (nuint)src.Length, DDSFlags.None, &metadata, image);
        }

        if (image.GetImageCount() == 0)
        {
            throw new Exception("Invalid DDS file!");
        }

        var ddsMainImage = image.GetImage(0, 0, 0);
        bool isCompressed = DirectXTex.IsCompressed(ddsMainImage.Format);

        // If the image is compressed, decompress it to RGBA32. Otherwise, convert it to RGBA32.
        if (isCompressed)
        {
            DirectXTex.SaveToDDSMemory(ddsMainImage, DDSFlags.None, blob);
        }
        else
        {
            // Convert the image to RGBA32 format. 
            //  (TODO Insert a link to the code)
            ScratchImage image1 = DirectXTex.CreateScratchImage();
            if (ddsMainImage.Format != (int)DXGIFormat.R8G8B8A8_UNORM)
                DirectXTex.Convert(ddsMainImage, (int)DXGIFormat.R8G8B8A8_UNORM, TexFilterFlags.Default, 0.5f, image1);

            DirectXTex.SaveToDDSMemory(ddsMainImage, DDSFlags.None, blob);
            image1.Release();
        }

        Span<byte> dest = new(blob.GetBufferPointer(), (int)blob.GetBufferSize());

        // Convert Span<byte> to byte[]
        byte[] byteArray = dest.ToArray();

        blob.Release();
        image.Release();

        // Allocate unmanaged memory for the byte array
        IntPtr ptr = Marshal.AllocHGlobal(byteArray.Length);

        // Copy the byte array to unmanaged memory
        Marshal.Copy(byteArray, 0, ptr, byteArray.Length);

        return new UnmanagedMemoryStream((byte*)ptr.ToPointer(), byteArray.Length);
    }

    /// <summary>
    /// Returns the image sections of the DDS image. Each mipmap and slice is a section on its own. 
    /// </summary>
    /// <param name="ddsImage">The DirectXTexNet DDS image.</param>
    /// <returns>The DDS sections</returns>
    unsafe public static DDS_DirectXTexNet_ImageSection[] GetDDSImageSections(ScratchImage ddsImage)
    {
        DDS_DirectXTexNet_ImageSection[] section = new DDS_DirectXTexNet_ImageSection[ddsImage.GetImageCount()];

        Image[] images = GetImages(ddsImage);

        for (int i = 0; i < images.Length; i++)
        {
            byte[] pixels = new byte[images[i].SlicePitch];

            Marshal.Copy((nint)images[i].Pixels, pixels, 0, pixels.Length);

            section[i] = new()
            {
                Width = images[i].Width,
                Height = images[i].Height,
                Format = (DXGIFormat)images[i].Format,
                SlicePitch = images[i].SlicePitch,
                RowPitch = images[i].RowPitch,
                Pixels = pixels
            };

            Console.WriteLine($"Image {i} - Width: {section[i].Width}, Height: {section[i].Height}, Format: {section[i].Format}, SlicePitch: {section[i].SlicePitch}, RowPitch: {section[i].RowPitch}");
            Console.WriteLine($"Image {i} - Pixels: {section[i].Pixels.Length}");
        }

        return section;
    }

    unsafe private static Image[] GetImages(ScratchImage image)
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

    /// <summary>
    /// Returns the metadata of the DDS metadata.
    /// </summary>
    /// <param name="scratchImage">The DirectXTexNet DDS image.</param>
    /// <returns>The metadata of the DDS image.</returns>
    public static TexMetadata GetDDSMetaData(ScratchImage scratchImage)
    {
        return scratchImage.GetMetadata();
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
        string information = "";

        information += "||||||||||| DDS Debug Information |||||||||||\n";
        information += $"Width: {metadata.Width}\n";
        information += $"Height: {metadata.Height}\n";
        information += $"Depth: {metadata.Depth}\n";
        information += $"Format: {Enum.GetName((DXGIFormat)metadata.Format)} " + "(" + metadata.Format + ")" + Environment.NewLine;
        information += $"Mips: {metadata.MipLevels}\n";
        information += $"Dimension: {metadata.Dimension}\n";
        information += $"Array Elements: {metadata.ArraySize}\n";
        information += $"Volumemap: {metadata.IsVolumemap()}\n";
        information += $"Cubemap: {metadata.IsCubemap()}\n";
        information += $"Alpha mode: {metadata.GetAlphaMode()}\n";
        information += $"Premultiplied alpha: {metadata.IsPMAlpha()}\n";
        information += $"Misc Flags: {metadata.MiscFlags}\n";
        information += $"Misc Flags2: {metadata.MiscFlags2}\n";

        return information;
    }
}
