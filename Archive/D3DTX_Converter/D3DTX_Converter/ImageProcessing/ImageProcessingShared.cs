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
    /// <summary>
    /// Custom color struct that has singles/floats instead of bytes
    /// </summary>
    public struct ColorFloat
    {
        public float r;
        public float g;
        public float b;
        public float a;

        public ColorFloat(Color color)
        {
            r = (float)color.R / (float)byte.MaxValue;
            g = (float)color.G / (float)byte.MaxValue;
            b = (float)color.B / (float)byte.MaxValue;
            a = (float)color.A / (float)byte.MaxValue;
        }

        /// <summary>
        /// Returns a (System.Drawing.Color) Color object.
        /// </summary>
        /// <returns></returns>
        public Color GetColor()
        {
            byte scaledR = (byte)(r * (float)byte.MaxValue);
            byte scaledG = (byte)(g * (float)byte.MaxValue);
            byte scaledB = (byte)(b * (float)byte.MaxValue);
            byte scaledA = (byte)(a * (float)byte.MaxValue);

            return Color.FromArgb(scaledA, scaledR, scaledG, scaledB);
        }
    }

    public static class ImageProcessingShared
    {
        public static float ClampFloat(float value, float min, float max)
        {
            return value < min ? min : value > max ? max : value;
        }

        public static PixelFormat GetPixelFormat(Pfim.ImageFormat format)
        {
            PixelFormat newFormat = PixelFormat.Undefined;

            switch (format)
            {
                case Pfim.ImageFormat.Rgba32:
                    newFormat = PixelFormat.Format32bppArgb;
                    break;
                case Pfim.ImageFormat.Rgba16:
                    newFormat = PixelFormat.Format16bppArgb1555;
                    break;
                case Pfim.ImageFormat.R5g5b5a1:
                    newFormat = PixelFormat.Format16bppArgb1555;
                    break;
                case Pfim.ImageFormat.R5g5b5:
                    newFormat = PixelFormat.Format16bppRgb555;
                    break;
                case Pfim.ImageFormat.R5g6b5:
                    newFormat = PixelFormat.Format16bppRgb565;
                    break;
                case Pfim.ImageFormat.Rgb24:
                    newFormat = PixelFormat.Format24bppRgb;
                    break;
                case Pfim.ImageFormat.Rgb8:
                    newFormat = PixelFormat.Format8bppIndexed;
                    break;
            }

            return newFormat;
        }
    }
}
