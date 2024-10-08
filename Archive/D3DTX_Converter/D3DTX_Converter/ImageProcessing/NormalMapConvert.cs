using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using Pfim;
using Pfim.dds;
using System.Runtime.InteropServices;

namespace D3DTX_Converter.ImageProcessing
{
    public static class NormalMapConvert
    {
        public static void ConvertNormalMapToPNG(string sourceFilePath_dds)
        {
            /*
            * Note: Yes texconv.exe does have the abillity to convert dds to this format
            * However for specific compressions of DDS its not able to convert the normal maps.
            */

            //open up the image
            using (var image = Pfim.Pfim.FromFile(sourceFilePath_dds))
            {
                PixelFormat format = ImageProcessingShared.GetPixelFormat(image.Format);

                var handle = GCHandle.Alloc(image.Data, GCHandleType.Pinned);

                try
                {
                    var data = Marshal.UnsafeAddrOfPinnedArrayElement(image.Data, 0);
                    var bitmap = new Bitmap(image.Width, image.Height, image.Stride, format, data);

                    //save the new image
                    bitmap.Save(Path.ChangeExtension(sourceFilePath_dds, ".png"), System.Drawing.Imaging.ImageFormat.Png);
                    bitmap.Dispose();
                }
                finally
                {
                    handle.Free();
                }
            }
        }

        public static void ConvertNormalMapToBMP(string sourceFilePath_dds)
        {
            /*
            * Note: Yes texconv.exe does have the abillity to convert dds to this format
            * However for specific compressions of DDS its not able to convert the normal maps.
            */

            //open up the image
            using (var image = Pfim.Pfim.FromFile(sourceFilePath_dds))
            {
                PixelFormat format = ImageProcessingShared.GetPixelFormat(image.Format);

                var handle = GCHandle.Alloc(image.Data, GCHandleType.Pinned);

                try
                {
                    var data = Marshal.UnsafeAddrOfPinnedArrayElement(image.Data, 0);
                    var bitmap = new Bitmap(image.Width, image.Height, image.Stride, format, data);

                    //save the new image
                    bitmap.Save(Path.ChangeExtension(sourceFilePath_dds, ".bmp"), System.Drawing.Imaging.ImageFormat.Bmp);
                    bitmap.Dispose();
                }
                finally
                {
                    handle.Free();
                }
            }
        }

        public static void ConvertNormalMapToJPEG(string sourceFilePath_dds)
        {
            /*
            * Note: Yes texconv.exe does have the abillity to convert dds to this format
            * However for specific compressions of DDS its not able to convert the normal maps.
            */

            //open up the image
            using (var image = Pfim.Pfim.FromFile(sourceFilePath_dds))
            {
                PixelFormat format = ImageProcessingShared.GetPixelFormat(image.Format);

                var handle = GCHandle.Alloc(image.Data, GCHandleType.Pinned);

                try
                {
                    var data = Marshal.UnsafeAddrOfPinnedArrayElement(image.Data, 0);
                    var bitmap = new Bitmap(image.Width, image.Height, image.Stride, format, data);

                    //save the new image
                    bitmap.Save(Path.ChangeExtension(sourceFilePath_dds, ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                    bitmap.Dispose();
                }
                finally
                {
                    handle.Free();
                }
            }
        }

        public static void ConvertNormalMapToTIFF(string sourceFilePath_dds)
        {
            /*
            * Note: Yes texconv.exe does have the abillity to convert dds to this format
            * However for specific compressions of DDS its not able to convert the normal maps.
            */

            //open up the image
            using (var image = Pfim.Pfim.FromFile(sourceFilePath_dds))
            {
                PixelFormat format = ImageProcessingShared.GetPixelFormat(image.Format);

                var handle = GCHandle.Alloc(image.Data, GCHandleType.Pinned);

                try
                {
                    var data = Marshal.UnsafeAddrOfPinnedArrayElement(image.Data, 0);
                    var bitmap = new Bitmap(image.Width, image.Height, image.Stride, format, data);

                    //save the new image
                    bitmap.Save(Path.ChangeExtension(sourceFilePath_dds, ".tiff"), System.Drawing.Imaging.ImageFormat.Tiff);
                    bitmap.Dispose();
                }
                finally
                {
                    handle.Free();
                }
            }
        }
    }
}
