using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace D3DTX_Converter.Utilities
{
    public static class ImageUtilities
    {
        public static bool IsImageOpaque(string imagePath)
        {
            Bitmap bitmap = new(imagePath);

            PixelFormat pixelFormat = bitmap.PixelFormat;

            switch(pixelFormat)
            {
                case PixelFormat.Alpha:
                case PixelFormat.PAlpha:
                case PixelFormat.Format16bppArgb1555:
                case PixelFormat.Format32bppArgb:
                case PixelFormat.Format32bppPArgb:
                case PixelFormat.Format64bppArgb:
                case PixelFormat.Format64bppPArgb:
                    return false;
                case PixelFormat.Format16bppGrayScale:
                case PixelFormat.Format16bppRgb555:
                case PixelFormat.Format1bppIndexed:
                case PixelFormat.Format24bppRgb:
                case PixelFormat.Format32bppRgb:
                case PixelFormat.Format48bppRgb:
                case PixelFormat.Format4bppIndexed:
                case PixelFormat.Format8bppIndexed:
                    return true;
                default:
                    return true;
            }
        }
    }
}
