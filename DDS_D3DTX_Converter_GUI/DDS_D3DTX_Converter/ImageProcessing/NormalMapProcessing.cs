using System;
using System.Numerics;
using Pfim;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;

namespace D3DTX_Converter.ImageProcessing
{
    public static class NormalMapProcessing
    {
        /// <summary>
        /// Reconstructs the Blue/Z channel on a normal map image and saves it to the disk.
        /// </summary>
        /// <param name="sourceFilePath"></param>
        /// <param name="outputPath"></param>
        public static void FromDDS_NormalMapReconstructZ(string sourceFilePath,
            string outputPath)
        {
            /*
             * Note: Yes texconv.exe does have a -reconstructz option...
             * HOWEVER for some reason its only for when converting a normal map FROM a BC5 compression.
             * Yes I did try to save the DDS file as a different compression and then see if that triggered the reconstructz option but wasn't able to.
             * So this is my way of getting around that, doing it by hand.
             */

            //open up the image
            using var image = Pfimage.FromFile(sourceFilePath);
            //PixelFormat format = ImageProcessingShared.GetPixelFormat(image.Format);

            var imgSharp = Image.LoadPixelData<Rgba32>(image.Data, image.Width, image.Height);
            // Loop through each pixel (x and y)
            for (int x = 0;
                 x < imgSharp.Width;
                 x++) //make this cycle in a function which returns an image and then saves it ig
            {
                for (int y = 0; y < imgSharp.Height; y++)
                {
                    // Get the current pixel value at the current position
                    Rgba32 currentPixel = imgSharp[x, y];
                    ColorFloat currentFloatColor = new(currentPixel);

                    // Convert the color to a vector and omit the blue channel (because we are calculating a new blue channel)
                    Vector3 initialNormalValue = new Vector3(currentPixel.R, currentPixel.G, 0.0f);

                    // Scale the normal
                    Vector3 scaledInitialNormalValue = initialNormalValue * 2.0f - Vector3.One;

                    // Compute the z value
                    float reconstructZ = MathF.Sqrt(1.0f -
                                                    (scaledInitialNormalValue.X * scaledInitialNormalValue.X +
                                                     scaledInitialNormalValue.Y * scaledInitialNormalValue.Y));

                    // Scale it back
                    reconstructZ = reconstructZ * 0.5f + 0.5f;

                    // Build our new color value
                    currentPixel.R = currentPixel.R;
                    currentPixel.G = currentPixel.G;
                    currentPixel.B = (byte)(reconstructZ * 255);
                    currentPixel.A = currentPixel.A;


                    ColorFloat newFloatColor = currentFloatColor with { B = reconstructZ };


                    Rgba32 newPixel = newFloatColor.GetColor();

                    // Set the new color value to the corresponding pixel
                    imgSharp[x, y] = newPixel;
                }

                // Save the new image
                imgSharp.Save(outputPath);
            }
        }
    }
}