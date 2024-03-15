using System;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.ComponentModel;
using D3DTX_Converter.DirectX;
using D3DTX_Converter.Main;
using D3DTX_Converter.Utilities;
using DDS_D3DTX_Converter_GUI.Utilities;
using MsBox.Avalonia;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace DDS_D3DTX_Converter;

public class ImageProperties : ObservableObject
{
    //<summary>
    //Image properties that are displayed on the panel.
    //</summary>
    public string? Name { get; set; }
    public string? Extension { get; set; }
    public string? Width { get; set; }
    public string? Height { get; set; }
    public string? CompressionType { get; set; }
    public string? HasAlpha { get; set; }
    public string? ChannelCount { get; set; }
    public string? MipMapCount { get; set; }

    public static ImageProperties GetImagePropertiesFromD3DTX(string filePath)
    {
        var master = new D3DTX_Master();
        master.Read_D3DTX_File(filePath);
        master.HasMipMaps();
        master.GetMipMapCount();

        return new ImageProperties()
        {
            Name = master.GetTextureName(),
            CompressionType = master.GetCompressionType(),
            Width = master.GetWidth().ToString(),
            Height = master.GetHeight().ToString(),
            HasAlpha = master.GetHasAlpha(),
            ChannelCount = master.GetChannelCount(),
            MipMapCount = master.GetMipMapCount().ToString()
        };
    }
    
    /// <summary>
    /// Gets the properties of the selected .dds file
    /// </summary>
    /// <param name="ddsFilePath"></param>
    public static ImageProperties GetDdsProperties(string ddsFilePath)
    {
        var sourceFileData = File.ReadAllBytes(ddsFilePath);

        byte[] headerBytes = ByteFunctions.AllocateBytes(124, sourceFileData, 4);

        var header = DDS.GetHeaderFromBytes(headerBytes);

        byte[] bytes = BitConverter.GetBytes(header.ddspf.dwFourCC);
        string result = System.Text.Encoding.UTF8.GetString(bytes);

        result.Reverse();
            
        string hasAlpha = "False";
        if (header.ddspf.dwABitMask  > 0)
        {
            hasAlpha = "True";
        }
            
        return new ImageProperties
        {
            Name = Path.GetFileNameWithoutExtension(ddsFilePath),
            Extension = ".dds",
            Height = header.dwHeight.ToString(),
            Width = header.dwWidth.ToString(),
            CompressionType = result,
            HasAlpha = hasAlpha,
            ChannelCount = (header.ddspf.dwRGBBitCount / 8).ToString(),
            MipMapCount = header.dwMipMapCount.ToString()
        };
    }
    
    /// <summary>
    /// Get the needed image properties from the selected file, excluding .dds and .d3dtx.
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static ImageProperties GetImagePropertiesFromOthers(string filePath)
    {
        try
        {
            var imageInfo = Image.Identify(filePath);

            var image = Image.Load<Rgba32>(filePath);

            bool hasAlpha = ImageUtilities.IsImageOpaque(image);

            string hasAlphaString = "N/A";
            hasAlphaString = hasAlpha ? "True" : "False";

            return new ImageProperties()
            {
                Name = Path.GetFileNameWithoutExtension(filePath),
                Extension = Path.GetExtension(filePath),
                CompressionType = imageInfo.Metadata.DecodedImageFormat.Name,
                ChannelCount = (imageInfo.PixelType.BitsPerPixel / 8).ToString(),
                Height = imageInfo.Height.ToString(),
                Width = imageInfo.Width.ToString(),
                HasAlpha = hasAlphaString,
                MipMapCount = "N/A"
            };
        }
        catch (Exception ex)
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime)
            {
                var mainWindow = lifetime.MainWindow;
                var messageBox =
                    MessageBoxes.GetErrorBox("Error during getting image properties. " + ex.Message);

                MessageBoxManager.GetMessageBoxStandard(messageBox).ShowWindowDialogAsync(mainWindow);
            }

            return GetImagePropertiesFromInvalid();
        }
    }

    /// <summary>
    /// Helper function. Creates new empty ImageProperties to display unsupported files.
    /// </summary>
    /// <returns></returns>
    public static ImageProperties GetImagePropertiesFromInvalid()
    {
        return new ImageProperties()
        {
            Name = "",
            CompressionType = "",
            ChannelCount = "",
            Height = "",
            Width = "",
            MipMapCount = ""
        };
    }
}