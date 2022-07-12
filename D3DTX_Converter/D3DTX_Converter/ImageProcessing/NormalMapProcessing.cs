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
    public static class NormalMapProcessing
    {
        /// <summary>
        /// Reconstructs the Blue/Z channel on a normal map image and saves it to the disk.
        /// </summary>
        /// <param name="sourceFilePath"></param>
        public static void FromDDS_NormalMapReconstructZ(string sourceFilePath, System.Drawing.Imaging.ImageFormat savedFormat, string outputPath)
        {
            /*
             * Note: Yes texconv.exe does have a -reconstructz option...
             * HOWEVER for some reason its only for when converting a normal map FROM a BC5 compression.
             * Yes I did try to save the DDS file as a different compression and then see if that triggered the reconstructz option but wasn't able to.
             * So this is my way of getting around that, doing it by hand.
            */

            //open up the image
            using (var image = Pfim.Pfim.FromFile(sourceFilePath))
            {
                PixelFormat format = ImageProcessingShared.GetPixelFormat(image.Format);

                var handle = GCHandle.Alloc(image.Data, GCHandleType.Pinned);

                try
                {
                    var data = Marshal.UnsafeAddrOfPinnedArrayElement(image.Data, 0);
                    var bitmap = new Bitmap(image.Width, image.Height, image.Stride, format, data);

                    //get the original width/height
                    int imageWidth = bitmap.Width;
                    int imageHeight = bitmap.Height;

                    //loop through each pixel (x and y)
                    for (int x = 0; x < imageWidth; x++)
                    {
                        for (int y = 0; y < imageHeight; y++)
                        {
                            //get the current pixel value at the current position
                            Color currentPixel = bitmap.GetPixel(x, y); //8 bit pixel color value... (System.Drawing.Color)
                            ColorFloat currentFloatColor = new(currentPixel); //conver it to a float so its 0..1

                            //references for normal z channel calculation
                            //https://bitinn.github.io/ScriptableRenderPipeline/ShaderGraph/Normal-Reconstruct-Z-Node/
                            //https://i.stack.imgur.com/F6DKE.jpg

                            //convert the color to a vector and omit the blue channel (because we are calculating a new blue channel
                            Vector3 initalNormalValue = new(currentFloatColor.r, currentFloatColor.g, 0.0f);

                            //scale the normal
                            Vector3 scaledInitalNormalValue = initalNormalValue;
                            scaledInitalNormalValue *= 2.0f;
                            scaledInitalNormalValue -= Vector3.One;

                            //compute the z value
                            float reconstructZ = MathF.Sqrt(1.0f - (scaledInitalNormalValue.X * scaledInitalNormalValue.X + scaledInitalNormalValue.Y * scaledInitalNormalValue.Y));

                            //scale it back
                            reconstructZ *= 0.5f;
                            reconstructZ += 0.5f;

                            //build our new color value
                            ColorFloat newFloatColor = new()
                            {
                                r = currentFloatColor.r,
                                g = currentFloatColor.g,
                                b = reconstructZ,
                                a = currentFloatColor.a
                            };

                            //convert it back to a System.Drawing.Color (which is back to 8 bit...)
                            Color newPixel = newFloatColor.GetColor();

                            //set the new color value to the coresponding pixel
                            bitmap.SetPixel(x, y, newPixel);
                        }
                    }

                    //save the new image
                    bitmap.Save(outputPath, savedFormat);
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
