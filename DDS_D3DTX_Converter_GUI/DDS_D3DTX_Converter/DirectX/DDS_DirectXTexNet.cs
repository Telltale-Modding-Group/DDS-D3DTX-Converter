using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using DirectXTexNet;

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
    public static ScratchImage GetDDSImage(string ddsFilePath, DDS_FLAGS flags = DDS_FLAGS.NONE)
    {
        return TexHelper.Instance.LoadFromDDSFile(ddsFilePath, flags);
    }

    /// <summary>
    /// Compute the pitch given the Direct3D10/DXGI format, the width and the height.
    /// </summary>
    /// <param name="dxgiFormat">The Direct3D10/DXGI format.</param>
    /// <param name="width">The width of the DDS image.</param>
    /// <param name="height">(Optional) The height of the DDS image. If not provided, it defaults to 0.</param>
    /// <returns>The pitch.</returns>
    public static uint ComputePitch(DXGI_FORMAT dxgiFormat, int width, int height = 1)
    {
        TexHelper.Instance.ComputePitch(dxgiFormat, width, height, out long rowPitch, out _, CP_FLAGS.NONE);

        return (uint)rowPitch;
    }

    /// <summary>
    /// Returns the channel count of a Direct3D10/DXGI format. It is used in the image properties display.
    /// </summary>
    /// <param name="dxgiFormat">The Direct3D10/DXGI format</param>
    /// <returns>The channel count.</returns>
    public static uint GetChannelCount(DXGI_FORMAT dxgiFormat) => (uint)Math.Ceiling((double)TexHelper.Instance.BitsPerPixel(dxgiFormat) / Math.Max(1, TexHelper.Instance.BitsPerColor(dxgiFormat)));

    /// <summary>
    /// Returns a DirectXTexNet DDS image from a byte array.
    /// </summary>
    /// <param name="array">The byte array containing the DDS data.</param>
    /// <param name="flags">(Optional) The mode in which the DirectXTexNet will load the .dds file. If not provided, it defaults to NONE.</param>
    /// <returns>The DirectXTexNet DDS image.</returns>
    public static ScratchImage GetDDSImage(byte[] array, DDS_FLAGS flags = DDS_FLAGS.NONE)
    {
        GCHandle handle = GCHandle.Alloc(array, GCHandleType.Pinned);
        try
        {
            // Obtain a pointer to the data
            IntPtr ptr = handle.AddrOfPinnedObject();
            return TexHelper.Instance.LoadFromDDSMemory(ptr, array.Length, flags);
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
    public static byte[] GetDDSByteArray(ScratchImage image, DDS_FLAGS flags = DDS_FLAGS.NONE)
    {
        var ddsMemory = image.SaveToDDSMemory(flags);

        ddsMemory.Position = 0;

        // Create a byte array to hold the data
        byte[] ddsArray = new byte[ddsMemory.Length];

        // Read the data from the UnmanagedMemoryStream into the byte array
        ddsMemory.Read(ddsArray, 0, ddsArray.Length);
        ddsMemory.Close();
        return ddsArray;
    }

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

    /// <summary>
    /// Returns the image sections of the DDS image. Each mipmap and slice is a section on its own. 
    /// </summary>
    /// <param name="ddsImage">The DirectXTexNet DDS image.</param>
    /// <returns>The DDS sections</returns>
    public static DDS_DirectXTexNet_ImageSection[] GetDDSImageSections(ScratchImage ddsImage)
    {
        DDS_DirectXTexNet_ImageSection[] section = new DDS_DirectXTexNet_ImageSection[ddsImage.GetImageCount()];

        for (int i = 0; i < section.Length; i++)
        {
            Image image = ddsImage.GetImage(i);

            var pixelPointer = ddsImage.GetImage(i).Pixels;

            byte[] pixels = new byte[image.SlicePitch];

            Marshal.Copy(pixelPointer, pixels, 0, pixels.Length);

            section[i] = new()
            {
                Width = image.Width,
                Height = image.Height,
                Format = image.Format,
                SlicePitch = image.SlicePitch,
                RowPitch = image.RowPitch,
                Pixels = pixels
            };

            Console.WriteLine($"Image {i} - Width: {section[i].Width}, Height: {section[i].Height}, Format: {section[i].Format}, SlicePitch: {section[i].SlicePitch}, RowPitch: {section[i].RowPitch}");
            Console.WriteLine($"Image {i} - Pixels: {section[i].Pixels.Length}");
        }

        return section;
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
    public static bool IsSRGB(DXGI_FORMAT dxgiFormat)
    {
        return TexHelper.Instance.IsSRGB(dxgiFormat);
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
        information += $"Format: {metadata.Format} " + "(" + (int)metadata.Format + ")" + Environment.NewLine;
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
