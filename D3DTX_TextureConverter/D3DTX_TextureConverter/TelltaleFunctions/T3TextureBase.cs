using D3DTX_TextureConverter.TelltaleEnums;

namespace D3DTX_TextureConverter.TelltaleFunctions
{
    public static class T3TextureBase
    {
        public static bool IsCompressed(int mSurfaceFormat)
        {
            return (mSurfaceFormat - 64) <= 5 || mSurfaceFormat >= 64 && (mSurfaceFormat <= 68 || (mSurfaceFormat - 70) <= 1) || (mSurfaceFormat - 80) <= 3 || mSurfaceFormat == 128;
        }

        public static T3ResourceUsage GetResourceUsage(int value)
        {
            switch (value)
            {
                default: return (T3ResourceUsage)value;
                case 0: return T3ResourceUsage.eResourceUsage_Static;
                case 1: return T3ResourceUsage.eResourceUsage_Dynamic;
                case 2: return T3ResourceUsage.eResourceUsage_System;
            }
        }

        public static T3SurfaceGamma GetSurfaceGamma(int value)
        {
            switch (value)
            {
                default: return (T3SurfaceGamma)value;
                case -1: return T3SurfaceGamma.eSurfaceGamma_Unknown;
                case 0: return T3SurfaceGamma.eSurfaceGamma_Linear;
                case 1: return T3SurfaceGamma.eSurfaceGamma_sRGB;
            }
        }

        public static T3SurfaceMultisample GetSurfaceMultisample(int value)
        {
            switch (value)
            {
                default: return (T3SurfaceMultisample)value;
                case 0: return T3SurfaceMultisample.eSurfaceMultisample_None;
                case 1: return T3SurfaceMultisample.eSurfaceMultisample_2x;
                case 2: return T3SurfaceMultisample.eSurfaceMultisample_4x;
                case 3: return T3SurfaceMultisample.eSurfaceMultisample_8x;
                case 4: return T3SurfaceMultisample.eSurfaceMultisample_16x;
            }
        }

        public static T3TextureLayout GetTextureLayout(int value)
        {
            switch (value)
            {
                default: return (T3TextureLayout)value;
                case -1: return T3TextureLayout.eTextureLayout_Unknown;
                case 0: return T3TextureLayout.eTextureLayout_2D;
                case 1: return T3TextureLayout.eTextureLayout_Cube;
                case 2: return T3TextureLayout.eTextureLayout_3D;
                case 3: return T3TextureLayout.eTextureLayout_2DArray;
                case 4: return T3TextureLayout.eTextureLayout_CubeArray;
            }
        }

        public static T3SurfaceFormat GetSurfaceFormat(int value)
        {
            switch(value)
            {
                default: return (T3SurfaceFormat)value;
                case -1: return T3SurfaceFormat.eSurface_Unknown;
                case 0: return T3SurfaceFormat.eSurface_ARGB8;
                case 10: return T3SurfaceFormat.eSurface_RGBA8;
                case 1: return T3SurfaceFormat.eSurface_ARGB16;
                case 2: return T3SurfaceFormat.eSurface_RGB565;
                case 3: return T3SurfaceFormat.eSurface_ARGB1555;
                case 4: return T3SurfaceFormat.eSurface_ARGB4;
                case 5: return T3SurfaceFormat.eSurface_ARGB2101010;
                case 16: return T3SurfaceFormat.eSurface_A8;
                case 17: return T3SurfaceFormat.eSurface_L8;
                case 18: return T3SurfaceFormat.eSurface_AL8;
                case 19: return T3SurfaceFormat.eSurface_L16;
                case 6: return T3SurfaceFormat.eSurface_R16;
                case 7: return T3SurfaceFormat.eSurface_RG16;
                case 8: return T3SurfaceFormat.eSurface_RGBA16;
                case 9: return T3SurfaceFormat.eSurface_RG8;
                case 32: return T3SurfaceFormat.eSurface_R16F;
                case 33: return T3SurfaceFormat.eSurface_RG16F;
                case 34: return T3SurfaceFormat.eSurface_RGBA16F;
                case 35: return T3SurfaceFormat.eSurface_R32F;
                case 36: return T3SurfaceFormat.eSurface_RG32F;
                case 37: return T3SurfaceFormat.eSurface_RGBA32F;
                case 48: return T3SurfaceFormat.eSurface_DepthPCF16;
                case 49: return T3SurfaceFormat.eSurface_DepthPCF24;
                case 50: return T3SurfaceFormat.eSurface_Depth16;
                case 51: return T3SurfaceFormat.eSurface_Depth24;
                case 52: return T3SurfaceFormat.eSurface_DepthStencil32;
                case 53: return T3SurfaceFormat.eSurface_Depth32F;
                case 54: return T3SurfaceFormat.eSurface_Depth32F_Stencil8;
                case 64: return T3SurfaceFormat.eSurface_DXT1;
                case 65: return T3SurfaceFormat.eSurface_DXT3;
                case 66: return T3SurfaceFormat.eSurface_DXT5;
                case 67: return T3SurfaceFormat.eSurface_DXT5A;
                case 68: return T3SurfaceFormat.eSurface_DXN;
                case 69: return T3SurfaceFormat.eSurface_CTX1;
                case 70: return T3SurfaceFormat.eSurface_BC6;
                case 71: return T3SurfaceFormat.eSurface_BC7;
                case 80: return T3SurfaceFormat.eSurface_PVRTC2;
                case 81: return T3SurfaceFormat.eSurface_PVRTC4;
                case 82: return T3SurfaceFormat.eSurface_PVRTC2a;
                case 83: return T3SurfaceFormat.eSurface_PVRTC4a;
                case 96: return T3SurfaceFormat.eSurface_ATC_RGB;
                case 97: return T3SurfaceFormat.eSurface_ATC_RGB1A;
                case 98: return T3SurfaceFormat.eSurface_ATC_RGBA;
                case 112: return T3SurfaceFormat.eSurface_ETC1_RGB;
                case 113: return T3SurfaceFormat.eSurface_ETC2_RGB;
                case 114: return T3SurfaceFormat.eSurface_ETC2_RGB1A;
                case 115: return T3SurfaceFormat.eSurface_ETC2_RGBA;
                case 116: return T3SurfaceFormat.eSurface_ETC2_R;
                case 117: return T3SurfaceFormat.eSurface_ETC2_RG;
                case 128: return T3SurfaceFormat.eSurface_ATSC_RGBA_4x4;
                case 14: return T3SurfaceFormat.eSurface_R8;
            }
        }

        public static long D3D9Format_FromSurfaceFormat(T3SurfaceFormat format)
        {
            switch (format)
            {
                case 0u:
                case (T3SurfaceFormat)0xAu:
                    return 21;
                case (T3SurfaceFormat)5u:
                    return 35;
                case (T3SurfaceFormat)2u:
                    return 23;
                case (T3SurfaceFormat)3u:
                    return 25;
                case (T3SurfaceFormat)4u:
                    return 26;
                case (T3SurfaceFormat)0x10u:
                    return 28;
                case (T3SurfaceFormat)0x11u:
                    return 50;
                case (T3SurfaceFormat)9u:
                case (T3SurfaceFormat)0x12u:
                    return 51;
                case (T3SurfaceFormat)0xBu:
                    return 42;
                case (T3SurfaceFormat)0xCu:
                    return 17;
                case (T3SurfaceFormat)0xDu:
                    return 3;
                case (T3SurfaceFormat)6u:
                case (T3SurfaceFormat)0x13u:
                    return 81;
                case (T3SurfaceFormat)7u:
                    return 34;
                case (T3SurfaceFormat)1u:
                case (T3SurfaceFormat)8u:
                    return 36;
                case (T3SurfaceFormat)0x22u:
                    return 113;
                case (T3SurfaceFormat)0x20u:
                    return 111;
                case (T3SurfaceFormat)0x23u:
                    return 114;
                case (T3SurfaceFormat)0x21u:
                    return 112;
                case (T3SurfaceFormat)0x24u:
                    return 115;
                case (T3SurfaceFormat)0x25u:
                    return 116;
                case (T3SurfaceFormat)0x30u:
                case (T3SurfaceFormat)0x32u:
                    return 80;
                case (T3SurfaceFormat)0x31u:
                case (T3SurfaceFormat)0x33u:
                    return 77;
                case (T3SurfaceFormat)0x34u:
                    return 75;
                case (T3SurfaceFormat)0x40u:
                    return 827611204;
                case (T3SurfaceFormat)0x41u:
                    return 861165636;
                case (T3SurfaceFormat)0x42u:
                    return 894720068;
                case (T3SurfaceFormat)0x90u:
                    return 22;
                case (T3SurfaceFormat)0x43u:
                    return 826889281;
                case (T3SurfaceFormat)0x44u:
                    return 843666497;
                default:
                    return 0;
            }
        }
    }
}
