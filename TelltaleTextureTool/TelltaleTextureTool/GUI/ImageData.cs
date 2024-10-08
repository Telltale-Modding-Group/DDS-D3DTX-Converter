using System;
using System.IO;
using TelltaleTextureTool.DirectX;
using TelltaleTextureTool.Main;
using TelltaleTextureTool.Telltale.FileTypes.D3DTX;
using Avalonia.Media.Imaging;
using SkiaSharp;
using System.Runtime.InteropServices;
using TelltaleTextureTool.TelltaleEnums;

namespace TelltaleTextureTool;

public class ImageData
{
    public ImageProperties ImageProperties { get; set; } = new ImageProperties();
    public Texture DDSImage { get; set; } = new Texture();

    public uint MaxMip { get; set; }
    public uint MaxFace { get; set; }

    private string CurrentFilePath { get; set; } = string.Empty;
    private bool IsSamePath { get; set; }
    private bool HasPixelData { get; set; }
    private TextureType CurrentTextureType { get; set; }

    private TelltaleToolGame Game { get; set; }
    private bool IsLegacyConsole { get; set; }

    public void Initialize(string filePath, TextureType textureType, TelltaleToolGame game = TelltaleToolGame.DEFAULT, bool isLegacyConsole = false)
    {
        IsSamePath = CurrentFilePath == filePath;

        if (!IsSamePath && CurrentFilePath != string.Empty && HasPixelData)
        {
            HasPixelData = false;
            DDSImage.Release();
        }

        CurrentFilePath = filePath;
        CurrentTextureType = textureType;
        Game = game;
        IsLegacyConsole = isLegacyConsole;

        GetImageData(out ImageProperties imageProperties);

        ImageProperties = imageProperties;
    }

    public void Reset()
    {
        if (HasPixelData)
        {
            HasPixelData = false;
            DDSImage.Release();
        }

        ImageProperties = new ImageProperties();
        MaxMip = 0;
        MaxFace = 0;
        CurrentFilePath = string.Empty;
        CurrentTextureType = TextureType.Unknown;
    }

    /// <summary>
    /// Applies the effects to the image.
    /// </summary>
    /// <param name="options"></param>
    public void ApplyEffects(ImageAdvancedOptions options)
    {
        try
        {
            DDSImage.ChangePreviewImage(options);

            DDSImage.GetBounds(out uint maxMip, out uint maxFace);
            MaxMip = maxMip;
            MaxFace = maxFace;
        }
        catch (Exception)
        {
            HasPixelData = false;
            throw;
        }
    }

    private void GetImageData(out ImageProperties imageProperties)
    {
        switch (CurrentTextureType)
        {
            case TextureType.DDS:
            case TextureType.BMP:
            case TextureType.PNG:
            case TextureType.TGA:
            case TextureType.JPEG:
            case TextureType.TIFF:
            case TextureType.HDR:
                GetImageDataFromCommon(out imageProperties);
                HasPixelData = true;
                break;
            case TextureType.D3DTX:
                GetImageDataFromD3DTX(Game, out imageProperties);
                HasPixelData = true;
                break;
            default:
                HasPixelData = false;
                GetImageDataFromInvalid(out imageProperties);
                break;
        }
    }

    private void GetImageDataFromD3DTX(TelltaleToolGame game, out ImageProperties imageProperties)
    {
        var d3dtx = new D3DTX_Master();
        d3dtx.ReadD3DTXFile(CurrentFilePath, game, IsLegacyConsole);

        // Initialize image properties
        D3DTXMetadata metadata = d3dtx.GetMetadata();

        imageProperties = new ImageProperties()
        {
            Name = metadata.TextureName,
            Width = metadata.Width.ToString(),
            Height = metadata.Height.ToString(),
            HasAlpha = d3dtx.GetHasAlpha(),
            SurfaceFormat = d3dtx.GetSurfaceFormat(),
            SurfaceGamma = metadata.SurfaceGamma.ToString(),
            MipMapCount = metadata.MipLevels.ToString(),
            TextureLayout = metadata.Dimension.ToString(),
            AlphaMode = metadata.AlphaMode.ToString(),
            ArraySize = metadata.ArraySize.ToString(),
        };

        if (!IsSamePath)
        {
            // Initialize image bitmap
            DDS_Master ddsFile = new(d3dtx);
            var array = ddsFile.GetData(d3dtx);

            DDSImage = new Texture(array, TextureType.D3DTX);
        }
    }

    /// <summary>
    /// Gets the pre-defined advanced options for the image.
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    public ImageAdvancedOptions GetImageAdvancedOptions(ImageAdvancedOptions options)
    {
        if (CurrentTextureType != TextureType.D3DTX)
        {
            return options;
        }

        var d3dtx = new D3DTX_Master();
        d3dtx.ReadD3DTXFile(CurrentFilePath, Game, options.IsLegacyConsole);

        try
        {
            d3dtx.ReadD3DTXJSON(Path.Combine(Path.GetDirectoryName(CurrentFilePath), Path.GetFileNameWithoutExtension(CurrentFilePath) + ".json"));
        }
        catch (Exception)
        {
            // Silent Error if the JSON file is not found or it's invalid.
        }

        if (d3dtx.IsLegacyConsole())
        {
            options.IsLegacyConsole = true;
        }

        return options;
    }

    private void GetImageDataFromCommon(out ImageProperties imageProperties)
    {
        if (!IsSamePath)
        {
            DDSImage = new Texture(CurrentFilePath, CurrentTextureType);
        }

        imageProperties = TextureManager.GetDDSProperties(CurrentFilePath, DDSImage.Metadata);
    }

    /// <summary>
    /// Converts the data from the scratch image to a bitmap.
    /// </summary>
    /// <param name="mip"></param>
    /// <param name="face"></param>
    /// <returns>The bitmap from the mip and face. </returns>
    public Bitmap GetBitmapFromScratchImage(uint mip = 0, uint face = 0)
    {
        if (TextureType.Unknown == CurrentTextureType)
        {
            return new Bitmap(MemoryStream.Null);
        }

        DDSImage.GetBounds(out uint maxMip, out uint maxFace);
        MaxMip = maxMip;
        MaxFace = maxFace;

        if (mip > maxMip)
        {
            mip = MaxMip;
        }
        if (face > maxFace)
        {
            face = MaxFace;
        }

        DDSImage.GetData(mip, face, out ulong width, out ulong height, out ulong pitch, out ulong length, out byte[] pixelData);

        // Converts the data into writeableBitmap. (TODO Insert a link to the code)
        var imageInfo = new SKImageInfo((int)width, (int)height, SKColorType.Rgba8888);
        var handle = GCHandle.Alloc(pixelData, GCHandleType.Pinned);
        var ptr = Marshal.UnsafeAddrOfPinnedArrayElement(pixelData, 0);
        using var data = SKData.Create(ptr, (int)length, (_, _) => handle.Free());
        using var skImage = SKImage.FromPixels(imageInfo, data, (int)pitch);
        using var bitmap = SKBitmap.FromImage(skImage);

        // Create a memory stream to hold the PNG data
        var memoryStream = new MemoryStream();

        // Encode the bitmap to PNG and write it to the memory stream
        var wstream = new SKManagedWStream(memoryStream);

        var success = bitmap.Encode(wstream, SKEncodedImageFormat.Png, 95);
        Console.WriteLine(success ? "Image converted successfully" : "Image conversion failed");

        memoryStream.Position = 0;

        return new Bitmap(memoryStream);
    }

    private static void GetImageDataFromInvalid(out ImageProperties imageProperties)
    {
        imageProperties = new();
    }
}