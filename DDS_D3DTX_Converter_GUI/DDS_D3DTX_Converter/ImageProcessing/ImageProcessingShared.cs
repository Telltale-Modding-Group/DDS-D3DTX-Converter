using SixLabors.ImageSharp.PixelFormats;


namespace D3DTX_Converter.ImageProcessing
{
    /// <summary>
    /// Custom color struct that has singles/floats instead of bytes
    /// </summary>
    public struct ColorFloat
    {
        public float R;
        public float G;
        public float B;
        public float A;

        /// <summary>
        /// Converts the color into a float number;
        /// </summary>
        /// <param name="color"></param>
        public ColorFloat(Rgba32 color)
        {
            R = color.R / (float)byte.MaxValue;
            G = color.G / (float)byte.MaxValue;
            B = color.B / (float)byte.MaxValue;
            A = color.A / (float)byte.MaxValue;
        }

        /// <summary>
        /// Returns a (System.Drawing.Color) Color object.
        /// </summary>
        /// <returns></returns>
        public Rgba32 GetColor()
        {
            byte scaledR = (byte)(R * byte.MaxValue);
            byte scaledG = (byte)(G * byte.MaxValue);
            byte scaledB = (byte)(B * byte.MaxValue);
            byte scaledA = (byte)(A * byte.MaxValue);

            Abgr32 abgrPixel = new Abgr32(scaledR, scaledG, scaledB, scaledA);

            Rgba32 nexPixel = new Rgba32();
            nexPixel.FromAbgr32(abgrPixel);
            return nexPixel;
        }
    }

    public static class ImageProcessingShared
    {
        public static float ClampFloat(float value, float min, float max)
        {
            return value < min ? min : value > max ? max : value;
        }
    }
}