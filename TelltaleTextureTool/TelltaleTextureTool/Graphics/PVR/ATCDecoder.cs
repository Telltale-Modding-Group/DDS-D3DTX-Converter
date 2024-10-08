using BCnEncoder.Decoder;
using BCnEncoder.Shared.ImageFiles;
using System.Collections.Generic;
using System.IO;

namespace TelltaleTextureTool.Graphics.PVR
{
    internal class ATC_Master
    {
        public static byte[] Decode(byte[] data)
        {
            var decoder = new BcDecoder();
            
            DdsFile file = DdsFile.Load(new MemoryStream(data));
            var images = decoder.DecodeAllMipMaps(file);

            List<byte> ddsData = [];

            foreach (var image in images) {

                foreach (var pixel in image)
                {
                    ddsData.Add(pixel.r);
                    ddsData.Add(pixel.g);
                    ddsData.Add(pixel.b);
                    ddsData.Add(pixel.a);
                }
                
            }
            return ddsData.ToArray();
        }
    }
}
