using PVRTexLib;
using System;
using System.Runtime.InteropServices;
using TelltaleTextureTool.Telltale.FileTypes.D3DTX;
using TelltaleTextureTool.TelltaleEnums;

namespace TelltaleTextureTool.Graphics.PVR;

internal class PVR_Main
{
    public static byte[] DecodeTexture(D3DTXMetadata d3dtxMetadata, byte[] data)
    {
        var format = GetPVRFormat(d3dtxMetadata.Format);
        ulong RGBA8888 = PVRDefine.PVRTGENPIXELID4('r', 'g', 'b', 'a', 8, 8, 8, 8);
        uint width = d3dtxMetadata.Width;
        uint height = d3dtxMetadata.Height;
        uint depth = d3dtxMetadata.Depth;
        uint numMipMaps = d3dtxMetadata.MipLevels;
        uint numArrayMembers = d3dtxMetadata.ArraySize;
        uint numFaces = d3dtxMetadata.IsCubemap() ? 6U : 1U;
        using PVRTextureHeader textureHeader = new((ulong)format, width, height, depth, numMipMaps, numArrayMembers, numFaces);
        ulong textureSize = textureHeader.GetTextureDataSize();

        if (textureSize == 0)
        {
            throw new Exception("Could not create PVR header!");
        }

        unsafe
        {
            fixed (byte* ptr = &data[0])
            {
                using PVRTexture texture = new PVRTexture(textureHeader, ptr);

                var colorSpace = d3dtxMetadata.SurfaceGamma == TelltaleEnums.T3SurfaceGamma.sRGB ? PVRTexLibColourSpace.sRGB : PVRTexLibColourSpace.Linear;
                texture.Transcode(RGBA8888, PVRTexLibVariableType.UnsignedByteNorm, colorSpace);

                void* ddsPtr = null;
                try
                {
                    ddsPtr = texture.SaveTextureToMemory(PVRTexLibFileContainerType.DDS, out ulong size);

                    byte[] ddsData = new byte[size];

                    Marshal.Copy(new IntPtr(ddsPtr), ddsData, 0, (int)size);

                    return ddsData;
                }
                finally
                {
                    texture.Dispose();
                }

            }
        }
    }

    public static PVRTexLibPixelFormat GetPVRFormat(T3SurfaceFormat format)
    {
        return format switch
        {
            T3SurfaceFormat.ETC1_RGB => PVRTexLibPixelFormat.ETC1,
            T3SurfaceFormat.ETC2_RGB => PVRTexLibPixelFormat.ETC2_RGB,
            T3SurfaceFormat.ETC2_RGBA => PVRTexLibPixelFormat.ETC2_RGBA,
            T3SurfaceFormat.ETC2_RGB1A => PVRTexLibPixelFormat.ETC2_RGB_A1,
            T3SurfaceFormat.ETC2_R => PVRTexLibPixelFormat.EAC_R11,
            T3SurfaceFormat.ETC2_RG => PVRTexLibPixelFormat.EAC_RG11,
            T3SurfaceFormat.ATSC_RGBA_4x4 => PVRTexLibPixelFormat.ASTC_4x4,
            T3SurfaceFormat.PVRTC2 => PVRTexLibPixelFormat.PVRTCI_2bpp_RGB,
            T3SurfaceFormat.PVRTC4 => PVRTexLibPixelFormat.PVRTCI_4bpp_RGB,
            T3SurfaceFormat.PVRTC2a => PVRTexLibPixelFormat.PVRTCI_2bpp_RGBA,
            T3SurfaceFormat.PVRTC4a => PVRTexLibPixelFormat.PVRTCI_4bpp_RGBA,
            _ => 0
        };
    }
}
