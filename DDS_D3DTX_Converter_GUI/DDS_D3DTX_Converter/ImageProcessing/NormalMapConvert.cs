using System;
using System.IO;
using Pfim;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Tga;
using SixLabors.ImageSharp.Formats.Tiff;
using SixLabors.ImageSharp.PixelFormats;

namespace D3DTX_Converter.ImageProcessing
{
    public static class NormalMapConvert
    {
        public static void ConvertNormalMapToOthers(string sourceFilePathDds, string extension)
        {
            /*
             * Note: Yes texconv.exe does have the ability to convert dds to this format
             * However for specific compressions of DDS its not able to convert the normal maps.
             */

            //open up the image
            using var image = Pfimage.FromFile(sourceFilePathDds);

            byte[] newData;

            // Since image sharp can't handle data with line padding in a stride
            // we create an stripped down array if any padding is detected
            var tightStride = image.Width * image.BitsPerPixel / 8;
            if (image.Stride != tightStride)
            {
                newData = new byte[image.Height * tightStride];
                for (int i = 0; i < image.Height; i++)
                {
                    Buffer.BlockCopy(image.Data, i * image.Stride, newData, i * tightStride, tightStride);
                }
            }
            else
            {
                newData = image.Data;
            }

            SaveImage(image, image.Format, newData, extension, sourceFilePathDds);
        }

        /// <summary>
        /// Save the .dds into the desired image format.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="format"></param>
        /// <param name="newData"></param>
        /// <param name="extension"></param>
        /// <param name="ddsSourceFilePath"></param>
        /// <exception cref="Exception"></exception>
        private static void SaveImage(IImage image, ImageFormat format, byte[] newData, string extension,
            string ddsSourceFilePath)
        {
            //Get the required image encoder.
            IImageEncoder encoder = GetEncoder(extension);

            switch (format)
            {
                case ImageFormat.Rgba32:
                    Image.LoadPixelData<Rgba32>(image.Data, image.Width, image.Height).Save(
                        Path.ChangeExtension(ddsSourceFilePath, extension),
                        encoder);
                    break;
                case ImageFormat.Rgba16:
                    Image.LoadPixelData<Bgra4444>(image.Data, image.Width, image.Height).Save(
                        Path.ChangeExtension(ddsSourceFilePath, extension),
                        encoder);
                    break;
                case ImageFormat.R5g5b5a1:
                    Image.LoadPixelData<Bgra5551>(image.Data, image.Width, image.Height).Save(
                        Path.ChangeExtension(ddsSourceFilePath, extension),
                        encoder);
                    break;
                case ImageFormat.R5g5b5:
                    for (int i = 1; i < newData.Length; i += 2)
                    {
                        newData[i] |= 128;
                    }

                    Image.LoadPixelData<Bgra5551>(image.Data, image.Width, image.Height).Save(
                        Path.ChangeExtension(ddsSourceFilePath, extension),
                        encoder);
                    break;
                case ImageFormat.R5g6b5:
                    Image.LoadPixelData<Bgr565>(image.Data, image.Width, image.Height).Save(
                        Path.ChangeExtension(ddsSourceFilePath, extension),
                        encoder);
                    break;
                case ImageFormat.Rgb24:
                    Image.LoadPixelData<Rgb24>(image.Data, image.Width, image.Height).Save(
                        Path.ChangeExtension(ddsSourceFilePath, extension),
                        encoder);
                    break;
                case ImageFormat.Rgb8:
                    Image.LoadPixelData<L8>(image.Data, image.Width, image.Height).Save(
                        Path.ChangeExtension(ddsSourceFilePath, extension),
                        encoder);
                    break;
                default:
                    throw new Exception($"ImageSharp does not recognize image format: {image.Format}");
            }
        }

        private static IImageEncoder GetEncoder(string fileType)
        {
            return fileType switch
            {
                ".jpeg" => new JpegEncoder(),
                ".jpg" => new JpegEncoder(),
                ".png" => new PngEncoder(),
                ".tif" => new TiffEncoder(),
                ".tiff" => new TiffEncoder(),
                ".bmp" => new BmpEncoder(),
                ".tga" => new TgaEncoder(), //for the future
                _ => throw new ArgumentException("File type is not valid!")
            };
        }
    }
}