using System;
using System.Collections.Generic;
using System.IO;
using D3DTX_Converter.DirectX;
using System.Linq;
using static Ktx.Ktx2;
using Ktx;
using System.Runtime.InteropServices;

namespace D3DTX_Converter.Main
{
    /// <summary>
    /// Main class for managing D3DTX files and converting them to KTX2.
    /// </summary>
    public class KTX2_Master
    {
        Texture texture;

        /// <summary>
        /// Create a KTX2 file from a D3DTX.
        /// </summary>
        /// <param name="d3dtx">The D3DTX data that will be used.</param>
        unsafe public KTX2_Master(D3DTX_Master d3dtx)
        {
            if (d3dtx.IsLegacyD3DTX())
            {
                throw new NotImplementedException("KTX2 does not support legacy versions.");
            }

            // Initialize the KTX2 Header
            texture = new()
            {
                BaseHeight = (uint)d3dtx.GetHeight(),
                BaseWidth = (uint)d3dtx.GetWidth(),
                BaseDepth = (uint)d3dtx.GetDepth(),
                VkFormat = KTX2_HELPER.GetVkFormatFromTelltaleSurfaceFormat(d3dtx.GetCompressionType(), d3dtx.GetSurfaceGamma(), d3dtx.GetAlpha()),
                NumLevels = (uint)d3dtx.GetMipMapCount(),
                NumLayers = (uint)d3dtx.GetArraySize(),
                NumFaces = (uint)(d3dtx.IsCubeTexture() ? 6 : 1),
                IsArray = d3dtx.IsArrayTexture(),
                IsCubemap = d3dtx.IsCubeTexture(),
                IsCompressed = d3dtx.IsTextureCompressed(),
                NumDimensions = (uint)(d3dtx.IsVolumeTexture() ? 3 : 2)
            };

            List<byte[]> textureData = [];

            // Get all pixel data from the D3DTX
            var d3dtxTextureData = d3dtx.GetPixelData();

            int divideBy = 1;

            // Get the pixel data by mipmap levels and unswizzle depending on the Platform
            for (int i = (int)(texture.NumLevels - 1); i >= 0; i--)
            {
                textureData.Add(d3dtx.GetPixelDataByMipmapIndex(i, d3dtx.GetCompressionType(), (int)d3dtx.GetWidth() / divideBy, (int)d3dtx.GetHeight() / divideBy, d3dtx.GetPlatformType()));
                divideBy *= 2;
            }

            byte[] d3dtxTextureDataArray = textureData.SelectMany(b => b).ToArray();

            // // Attempt to put pixel data into the Texture
            IntPtr ptr = (nint)NativeMemory.Alloc(texture.DataSize);
            // Marshal.Copy(d3dtxTextureDataArray, 0, ptr, d3dtxTextureDataArray.Length);

            // texture.PData = (byte*)ptr;
            // texture.DataSize = (uint)d3dtxTextureDataArray.Length;

            string newKTX2Path = "C:\\Users\\ivani\\Downloads\\TWDS2\\KTX2" + Path.DirectorySeparatorChar + "LOL" +
                                                       Main_Shared.ktx2Extension;
//Ktx2.GetImageOffset()
            fixed (Texture* ptr1 = &texture)
            {
                Ktx2.WriteToNamedFile(ptr1, newKTX2Path);
            }

        }

        /// <summary>
        /// Writes a D3DTX into a KTX2 file on the disk.
        /// </summary>
        /// <param name="d3dtx"></param>
        /// <param name="destinationDirectory"></param>
        unsafe public void WriteD3DTXAsKTX2(D3DTX_Master d3dtx, string destinationDirectory)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            string d3dtxFilePath = d3dtx.filePath;
            string fileName = Path.GetFileNameWithoutExtension(d3dtxFilePath);

            string newKTX2Path = destinationDirectory + Path.DirectorySeparatorChar + fileName +
                                                       Main_Shared.ktx2Extension;

            fixed (Texture* ptr = &texture)
            {
                Ktx2.WriteToNamedFile(ptr, newKTX2Path);
            }

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine("Time to get data: {0}", elapsedMs);
        }

        // unsafe ~KTX2_Master()
        // {
        //     fixed (Texture* ptr = &texture)
        //     {
        //         Ktx2.Destroy(ptr);
        //     }
        // }
    }
}
