using System;
using System.IO;
using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using BitMiracle.LibTiff.Classic;
using D3DTX_Converter.Main;
using Pfim;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SkiaSharp;

namespace D3DTX_Converter.Utilities
{
    public static class ImageUtilities
    {

        /// <summary>
        /// Checks if the image from a file path is transparent.
        /// </summary>
        /// <param name="imageFilePath"></param>
        /// <returns></returns>
        public static bool IsImageOpaque(string imageFilePath)
        {
            var image = Image.Load<Rgba32>(imageFilePath);

            bool hasAlpha = false;

            image.ProcessPixelRows(pixelAccessor =>
            {
                for (int y = 0; y < pixelAccessor.Height; y++)
                {
                    Span<Rgba32> pixelRow = pixelAccessor.GetRowSpan(y);

                    for (int x = 0; x < pixelRow.Length; x++)
                    {
                        if (pixelRow[x].A != 255)
                        {
                            hasAlpha = true;
                            break;
                        }
                    }

                    if (hasAlpha)
                    {
                        break;
                    }
                }
            });

            return hasAlpha;
        }

        /// <summary>
        /// Checks if the loaded image is transparent. Uses ImageSharp library.
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static bool IsImageOpaque(Image<Rgba32> image)
        {

            bool hasAlpha = false;

            image.ProcessPixelRows(pixelAccessor =>
            {
                for (int y = 0; y < pixelAccessor.Height; y++)
                {
                    Span<Rgba32> pixelRow = pixelAccessor.GetRowSpan(y);

                    for (int x = 0; x < pixelRow.Length; x++)
                    {
                        if (pixelRow[x].A != 255)
                        {
                            hasAlpha = true;
                            break;
                        }
                    }

                    if (hasAlpha)
                    {
                        break;
                    }
                }
            });

            return hasAlpha;
        }

        /// <summary>
        /// Converts .dds (and .tga) files to a bitmap. This is only used in the image preview.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static WriteableBitmap ConvertFileFromDdsToBitmap(string filePath)
        {
            //load the image
            using var image = Pfimage.FromFile(filePath);

            return GetDDSBitmap(image);
        }

        /// <summary>
        /// Converts .tiff to a bitmap.This is only used in the image preview.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static WriteableBitmap ConvertTiffToBitmap(string filePath)
        {
            byte[] fileBytes = File.ReadAllBytes(filePath);
            Stream tiffStream = new MemoryStream(fileBytes);
            // open a TIFF stored in the stream
            using var tifImg = Tiff.ClientOpen("in-memory", "r", tiffStream, new TiffStream());
            // read the dimensions
            var width = tifImg.GetField(TiffTag.IMAGEWIDTH)[0].ToInt();
            var height = tifImg.GetField(TiffTag.IMAGELENGTH)[0].ToInt();

            //Experimentation, ignore this
            //var smth = tifImg.GetField(TiffTag.COMPRESSION)[0].ToInt();

            // create the bitmap
            var bitmap = new SKBitmap();
            var info = new SKImageInfo(width, height, SKImageInfo.PlatformColorType, SKAlphaType.Premul,
                SKColorSpace.CreateSrgb());

            // create the buffer that will hold the pixels
            var raster = new int[width * height];

            // get a pointer to the buffer, and give it to the bitmap
            var ptr = GCHandle.Alloc(raster, GCHandleType.Pinned);
            bitmap.InstallPixels(info, ptr.AddrOfPinnedObject(), info.RowBytes);


            // read the image into the memory buffer
            if (!tifImg.ReadRGBAImageOriented(width, height, raster,
                    Orientation.TOPLEFT))
            {
                // not a valid TIF image.
                return null;
            }

            // swap the red and blue because SkiaSharp may differ from the tiff
            if (SKImageInfo.PlatformColorType == SKColorType.Bgra8888)
            {
                SKSwizzle.SwapRedBlue(ptr.AddrOfPinnedObject(), raster.Length);
            }

            var writeableBitmap = new WriteableBitmap(new PixelSize(width, height), new Vector(96, 96),
                PixelFormat.Bgra8888);

            using var lockedBitmap = writeableBitmap.Lock();
            // Copy the SKBitmap pixel data to the Avalonia WriteableBitmap
            Marshal.Copy(bitmap.Bytes, 0, lockedBitmap.Address, bitmap.Bytes.Length);

            return writeableBitmap;
        }

        /// <summary>
        /// Converts .d3dtx files to a bitmap (by converting to .dds first). This is only used in the image preview. 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static WriteableBitmap ConvertD3dtxToBitmap(string filePath)
        {
            var d3dtx = new D3DTX_Master();
            d3dtx.Read_D3DTX_File(filePath);
            DDS_Master ddsFile = new(d3dtx);

            var array = ddsFile.GetData(d3dtx);
            Stream stream = new MemoryStream(array);

            var image = Pfimage.FromStream(stream);

            // Can't display RGBA32F
            return GetDDSBitmap(image);
        }


        public static WriteableBitmap GetDDSBitmap(IImage image)
        {
            //get the data
            var newData = image.Data;
            var newDataLen = image.DataLen;
            var stride = image.Stride;
            var pixels = image.DataLen;

            //get the color type
            SKColorType colorType;
            switch (image.Format)
            {
                case ImageFormat.Rgb8:
                    colorType = SKColorType.Gray8;
                    break;
                case ImageFormat.R5g5b5: // Pfim doesn't support L16 and L8A8 formats. Images with these formats will be incorrectly interpreted as R5G5B5.
                    pixels /= 2;
                    newDataLen = pixels * 2;
                    newData = new byte[newDataLen];
                    for (var i = 0; i < pixels; i++)
                    {
                        ushort pixelData = BitConverter.ToUInt16(image.Data, i * 2);
                        byte r = (byte)((pixelData & 0x7C00) >> 10); // Red component
                        byte g = (byte)((pixelData & 0x03E0) >> 5); // Green component
                        byte b = (byte)(pixelData & 0x001F); // Blue component
                        ushort rgb565 = (ushort)((r << 11) | (g << 5) | b); // Combine components into RGB565 format
                        byte[] rgb565Bytes = BitConverter.GetBytes(rgb565);
                        newData[i * 2] = rgb565Bytes[0];
                        newData[i * 2 + 1] = rgb565Bytes[1];
                    }
                    stride = image.Width * 2;
                    colorType = SKColorType.Rgb565;
                    break;
                case ImageFormat.R5g5b5a1:
                    pixels /= 2; // Each pixel is 2 bytes in R5G5B5A1 format
                    newDataLen = pixels * 4; // Each pixel will be 4 bytes in RGBA8888 format
                    newData = new byte[newDataLen];
                    for (var i = 0; i < pixels; i++)
                    {
                        ushort pixelData = BitConverter.ToUInt16(image.Data, i * 2);
                        byte r = (byte)((pixelData & 0x7C00) >> 10); // Red component
                        byte g = (byte)((pixelData & 0x03E0) >> 5); // Green component
                        byte b = (byte)(pixelData & 0x001F); // Blue component
                        newData[i * 4] = r;
                        newData[i * 4 + 1] = g;
                        newData[i * 4 + 2] = b;
                        newData[i * 4 + 3] = 255; // Alpha channel set to 255 (fully opaque)
                    }
                    stride = image.Width * 4;
                    colorType = SKColorType.Rgba8888;
                    break;
                case ImageFormat.R5g6b5:
                    colorType = SKColorType.Rgb565;
                    break;
                case ImageFormat.Rgba16:
                    colorType = SKColorType.Argb4444;
                    break;
                case ImageFormat.Rgb24:
                    // Skia has no 24bit pixels, so we upscale to 32bit
                    pixels = image.DataLen / 3;
                    newDataLen = pixels * 4;
                    newData = new byte[newDataLen];
                    for (var i = 0; i < pixels; i++)
                    {
                        newData[i * 4] = image.Data[i * 3];
                        newData[i * 4 + 1] = image.Data[i * 3 + 1];
                        newData[i * 4 + 2] = image.Data[i * 3 + 2];
                        newData[i * 4 + 3] = 255;
                    }

                    stride = image.Width * 4;
                    colorType = SKColorType.Bgra8888;
                    break;
                case ImageFormat.Rgba32:
                    colorType = SKColorType.Bgra8888;
                    break;
                default:
                    throw new ArgumentException($"Skia unable to interpret pfim format: {image.Format}");
            }

            //Converts the data into writeableBitmap. (TODO Insert a link to the code)
            var imageInfo = new SKImageInfo(image.Width, image.Height, colorType);
            var handle = GCHandle.Alloc(newData, GCHandleType.Pinned);
            var ptr = Marshal.UnsafeAddrOfPinnedArrayElement(newData, 0);
            using var data = SKData.Create(ptr, newDataLen, (_, _) => handle.Free());
            using var skImage = SKImage.FromPixels(imageInfo, data, stride);
            using var bitmap = SKBitmap.FromImage(skImage);
            var writeableBitmap = new WriteableBitmap(new PixelSize(image.Width, image.Height), new Vector(96, 96),
                PixelFormat.Bgra8888);

            using var lockedBitmap = writeableBitmap.Lock();
            // Copy the SKBitmap pixel data to the Avalonia WriteableBitmap
            Marshal.Copy(bitmap.Bytes, 0, lockedBitmap.Address, bitmap.Bytes.Length);

            return writeableBitmap;
        }
    }
}