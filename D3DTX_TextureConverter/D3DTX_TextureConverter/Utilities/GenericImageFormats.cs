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
        public static void ConvertDDS_To_JPEG(string path, bool removeOriginalFile = false)
        {
            ConvertFromDDS_Master(path, ".jpeg", ImageFormat.JPEG, removeOriginalFile);
        }

        public static void ConvertDDS_To_PNG(string path, bool removeOriginalFile = false)
        {
            ConvertFromDDS_Master(path, ".png", ImageFormat.PNG, removeOriginalFile);
        }

        public static void ConvertDDS_To_PSD(string path, bool removeOriginalFile = false)
        {
            ConvertFromDDS_Master(path, ".psd", ImageFormat.PSD, removeOriginalFile);
        }

        public static void ConvertDDS_To_TGA(string path, bool removeOriginalFile = false)
        {
            ConvertFromDDS_Master(path, ".tga", ImageFormat.TARGA, removeOriginalFile);
        }

        public static void ConvertDDS_To_TIFF(string path, bool removeOriginalFile = false)
        {
            ConvertFromDDS_Master(path, ".tiff", ImageFormat.TIFF, removeOriginalFile);
        }

        public static void ConvertDDS_To_BMP(string path, bool removeOriginalFile = false)
        {
            ConvertFromDDS_Master(path, ".bmp", ImageFormat.BMP, removeOriginalFile);
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

        private static void ConvertFromDDS_Master(string path, string extension, ImageFormat format, bool removeOriginalFile = false)
        {
            string newFilePath = string.Format("{0}{1}", path.Remove(path.Length - 4, 4), extension);

            Surface surface = Surface.LoadFromFile(path);

            if (surface != null)
            {
                surface.SaveToFile(format, newFilePath);

                if (removeOriginalFile)
                    File.Delete(path);
            }
            else
            {
                ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Red);
                Console.WriteLine("ERROR! Can't convert '{0}' to a '{1}'!", Path.GetFileName(path), extension);
                ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            }
        }
    }
}
