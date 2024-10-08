using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TeximpNet;
using TeximpNet.DDS;
using TeximpNet.Compression;
using System.IO;

namespace D3DTX_TextureConverter.Utilities
{
    /// <summary>
    /// This is a plug-in-play class that converts a DDS into common image formats, and also can convert a common image format into a DDS
    /// </summary>
    public static class GenericImageFormats
    {
        public enum ConverterImageFormat
        {
            D3DTX,
            DDS,
            PNG,
            JPEG,
            JPG,
            PSD,
            TIFF,
            TGA,
            BMP
        }

        /// <summary>
        /// Converts a common image format into a DDS file.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="removeOriginalFile"></param>
        public static void ConvertImage_To_DDS(string path, bool removeOriginalFile = false)
        {
            string newExtension = ".dds";
            string originalExtension = Path.GetExtension(path);
            string newFilePath = string.Format("{0}{1}", path.Remove(path.Length - originalExtension.Length, originalExtension.Length), newExtension);

            Surface surface = Surface.LoadFromFile(path);

            if (surface != null)
            {
                surface.SaveToFile(ImageFormat.DDS, newFilePath);

                if (removeOriginalFile)
                    File.Delete(path);
            }
            else
            {
                ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Red);
                Console.WriteLine("ERROR! Can't convert '{0}' to a '{1}'!", Path.GetFileName(path), newExtension);
                ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            }
        }

        private static void ConvertFromDDS_ToGeneric(string path, ConverterImageFormat format, bool removeOriginalFile = false)
        {
            string formatExtension = Get_ImageFormat_Extension(format);
            string newFilePath = string.Format("{0}{1}", path.Remove(path.Length - 4, 4), formatExtension);

            Surface surface = Surface.LoadFromFile(path);

            if (surface != null)
            {
                ImageFormat impFormat = Get_Teximp_ImageFormat(format);

                surface.SaveToFile(impFormat, newFilePath, Get_Teximp_ImageSaveFlags(impFormat));

                if (removeOriginalFile)
                    File.Delete(path);
            }
            else
            {
                ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Red);
                Console.WriteLine("ERROR! Can't convert '{0}' to a '{1}'!", Path.GetFileName(path), formatExtension);
                ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            }
        }

        public static string Get_ImageFormat_Extension(ConverterImageFormat imageFormat)
        {
            switch (imageFormat)
            {
                default:
                    return "";
                case ConverterImageFormat.BMP:
                    return ".bmp";
                case ConverterImageFormat.D3DTX:
                    return ".d3dtx";
                case ConverterImageFormat.DDS:
                    return ".dds";
                case ConverterImageFormat.JPEG:
                    return ".jpeg";
                case ConverterImageFormat.JPG:
                    return ".jpg";
                case ConverterImageFormat.PNG:
                    return ".png";
                case ConverterImageFormat.PSD:
                    return ".psd";
                case ConverterImageFormat.TGA:
                    return ".tga";
                case ConverterImageFormat.TIFF:
                    return ".tiff";
            }
        }

        public static ConverterImageFormat Get_ImageFormat_Extension(string extension)
        {
            switch (extension)
            {
                default:
                    return ConverterImageFormat.PNG;
                case ".bmp":
                    return ConverterImageFormat.BMP;
                case ".d3dtx":
                    return ConverterImageFormat.D3DTX;
                case ".dds":
                    return ConverterImageFormat.DDS;
                case ".jpeg":
                    return ConverterImageFormat.JPEG;
                case ".jpg":
                    return ConverterImageFormat.JPG;
                case ".png":
                    return ConverterImageFormat.PNG;
                case ".psd":
                    return ConverterImageFormat.PSD;
                case ".tga":
                    return ConverterImageFormat.TGA;
                case ".tiff":
                    return ConverterImageFormat.TIFF;
            }
        }

        public static ImageFormat Get_Teximp_ImageFormat(ConverterImageFormat imageFormat)
        {
            switch (imageFormat)
            {
                default:
                    return ImageFormat.PNG;
                case ConverterImageFormat.BMP:
                    return ImageFormat.BMP;
                case ConverterImageFormat.DDS:
                    return ImageFormat.DDS;
                case ConverterImageFormat.JPEG:
                    return ImageFormat.JPEG;
                case ConverterImageFormat.JPG:
                    return ImageFormat.JPEG;
                case ConverterImageFormat.PNG:
                    return ImageFormat.PNG;
                case ConverterImageFormat.PSD:
                    return ImageFormat.PSD;
                case ConverterImageFormat.TGA:
                    return ImageFormat.TARGA;
                case ConverterImageFormat.TIFF:
                    return ImageFormat.TIFF;
            }
        }

        public static ImageSaveFlags Get_Teximp_ImageSaveFlags(ImageFormat imageFormat)
        {
            switch(imageFormat)
            {
                default:
                    return ImageSaveFlags.Default;
                case ImageFormat.JPEG:
                    return ImageSaveFlags.JPEG_QualitySuperb;
                case ImageFormat.PNG:
                    return ImageSaveFlags.PNG_Z_NoCompression;
                case ImageFormat.TIFF:
                    return ImageSaveFlags.TIFF_None;
            }
        }
    }
}
