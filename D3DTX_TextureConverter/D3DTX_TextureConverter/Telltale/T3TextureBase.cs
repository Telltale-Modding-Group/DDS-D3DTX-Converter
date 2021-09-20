namespace D3DTX_TextureConverter.Telltale
{
    public struct T3TextureBase
    {
        //baseclass T3RenderResource
        public string mName;
        public int mWidth;
        public int mHeight;
        public int mDepth;
        public int mArraySize;
        public int mNumMipLevels;
        public T3TextureLayout mTextureLayout;
        public T3SurfaceFormat mSurfaceFormat;
        public T3SurfaceMultisample mSurfaceMultisample;
        public T3SurfaceGamma mSurfaceGamma;
        public int mSurfaceAccess; //not sure of type
        public T3ResourceUsage mResourceUsage;
        public int mNumMipLevelsAllocated;
        public int mNumSurfacesRequested;
        public int mNumSurfacesRequired;
        public int mNumSurfacesLoaded;
        public int mSamplerState;
        public GFXPlatformAllocationType mGFXMemoryOwner;
    }

    public static class T3TextureBase_Functions
    {
        public static bool IsCompressed(T3TextureBase texture)
        {
            int value = (int)texture.mSurfaceFormat;
            return (value - 64) <= 5 || value >= 64 && (value <= 68 || (value - 70) <= 1) || (value - 80) <= 3 || value == 128;
        }

        public static T3ResourceUsage GetResourceUsage(int value)
        {
            switch (value)
            {
                default:
                    return T3ResourceUsage.eResourceUsage_Static;
                case 0:
                    return T3ResourceUsage.eResourceUsage_Static;
                case 1:
                    return T3ResourceUsage.eResourceUsage_Dynamic;
                case 2:
                    return T3ResourceUsage.eResourceUsage_System;
            }
        }

        public static T3SurfaceGamma GetSurfaceGamma(int value)
        {
            switch (value)
            {
                default:
                    return T3SurfaceGamma.eSurfaceGamma_sRGB;
                case -1:
                    return T3SurfaceGamma.eSurfaceGamma_Unknown;
                case 0:
                    return T3SurfaceGamma.eSurfaceGamma_Linear;
                case 1:
                    return T3SurfaceGamma.eSurfaceGamma_sRGB;
            }
        }

        public static T3SurfaceMultisample GetSurfaceMultisample(int value)
        {
            switch (value)
            {
                default:
                    return T3SurfaceMultisample.eSurfaceMultisample_None;
                case 0:
                    return T3SurfaceMultisample.eSurfaceMultisample_None;
                case 1:
                    return T3SurfaceMultisample.eSurfaceMultisample_2x;
                case 2:
                    return T3SurfaceMultisample.eSurfaceMultisample_4x;
                case 3:
                    return T3SurfaceMultisample.eSurfaceMultisample_8x;
                case 4:
                    return T3SurfaceMultisample.eSurfaceMultisample_16x;
            }
        }

        public static T3TextureLayout GetTextureLayout(int value)
        {
            switch (value)
            {
                default:
                    return T3TextureLayout.eTextureLayout_2D;
                case -1:
                    return T3TextureLayout.eTextureLayout_Unknown;
                case 0:
                    return T3TextureLayout.eTextureLayout_2D;
                case 1:
                    return T3TextureLayout.eTextureLayout_Cube;
                case 2:
                    return T3TextureLayout.eTextureLayout_3D;
                case 3:
                    return T3TextureLayout.eTextureLayout_2DArray;
                case 4:
                    return T3TextureLayout.eTextureLayout_CubeArray;
            }
        }

        public static T3SurfaceFormat GetSurfaceFormat(int value)
        {
            switch(value)
            {
                default:
                    return T3SurfaceFormat.eSurface_DXT1;
                case -1:
                    return T3SurfaceFormat.eSurface_Unknown;
                case 0:
                    return T3SurfaceFormat.eSurface_ARGB8;
                case 10:
                    return T3SurfaceFormat.eSurface_RGBA8;
                case 1:
                    return T3SurfaceFormat.eSurface_ARGB16;
                case 2:
                    return T3SurfaceFormat.eSurface_RGB565;
                case 3:
                    return T3SurfaceFormat.eSurface_ARGB1555;
                case 4:
                    return T3SurfaceFormat.eSurface_ARGB4;
                case 5:
                    return T3SurfaceFormat.eSurface_ARGB2101010;
                case 16:
                    return T3SurfaceFormat.eSurface_A8;
                case 17:
                    return T3SurfaceFormat.eSurface_L8;
                case 18:
                    return T3SurfaceFormat.eSurface_AL8;
                case 19:
                    return T3SurfaceFormat.eSurface_L16;
                case 6:
                    return T3SurfaceFormat.eSurface_R16;
                case 7:
                    return T3SurfaceFormat.eSurface_RG16;
                case 8:
                    return T3SurfaceFormat.eSurface_RGBA16;
                case 9:
                    return T3SurfaceFormat.eSurface_RG8;
                case 32:
                    return T3SurfaceFormat.eSurface_R16F;
                case 33:
                    return T3SurfaceFormat.eSurface_RG16F;
                case 34:
                    return T3SurfaceFormat.eSurface_RGBA16F;
                case 35:
                    return T3SurfaceFormat.eSurface_R32F;
                case 36:
                    return T3SurfaceFormat.eSurface_RG32F;
                case 37:
                    return T3SurfaceFormat.eSurface_RGBA32F;
                case 48:
                    return T3SurfaceFormat.eSurface_DepthPCF16;
                case 49:
                    return T3SurfaceFormat.eSurface_DepthPCF24;
                case 50:
                    return T3SurfaceFormat.eSurface_Depth16;
                case 51:
                    return T3SurfaceFormat.eSurface_Depth24;
                case 52:
                    return T3SurfaceFormat.eSurface_DepthStencil32;
                case 53:
                    return T3SurfaceFormat.eSurface_Depth32F;
                case 54:
                    return T3SurfaceFormat.eSurface_Depth32F_Stencil8;
                case 64:
                    return T3SurfaceFormat.eSurface_DXT1;
                case 65:
                    return T3SurfaceFormat.eSurface_DXT3;
                case 66:
                    return T3SurfaceFormat.eSurface_DXT5;
                case 67:
                    return T3SurfaceFormat.eSurface_DXT5A;
                case 68:
                    return T3SurfaceFormat.eSurface_DXN;
                case 69:
                    return T3SurfaceFormat.eSurface_CTX1;
                case 70:
                    return T3SurfaceFormat.eSurface_BC6;
                case 71:
                    return T3SurfaceFormat.eSurface_BC7;
                case 80:
                    return T3SurfaceFormat.eSurface_PVRTC2;
                case 81:
                    return T3SurfaceFormat.eSurface_PVRTC4;
                case 82:
                    return T3SurfaceFormat.eSurface_PVRTC2a;
                case 83:
                    return T3SurfaceFormat.eSurface_PVRTC4a;
                case 96:
                    return T3SurfaceFormat.eSurface_ATC_RGB;
                case 97:
                    return T3SurfaceFormat.eSurface_ATC_RGB1A;
                case 98:
                    return T3SurfaceFormat.eSurface_ATC_RGBA;
                case 112:
                    return T3SurfaceFormat.eSurface_ETC1_RGB;
                case 113:
                    return T3SurfaceFormat.eSurface_ETC2_RGB;
                case 114:
                    return T3SurfaceFormat.eSurface_ETC2_RGB1A;
                case 115:
                    return T3SurfaceFormat.eSurface_ETC2_RGBA;
                case 116:
                    return T3SurfaceFormat.eSurface_ETC2_R;
                case 117:
                    return T3SurfaceFormat.eSurface_ETC2_RG;
                case 128:
                    return T3SurfaceFormat.eSurface_ATSC_RGBA_4x4;
                case 14:
                    return T3SurfaceFormat.eSurface_R8;
            }
        }

        public static long D3D9Format_FromSurfaceFormat(T3SurfaceFormat format)
        {
            long result = 0;

            switch (format)
            {
                case 0u:
                case (T3SurfaceFormat)0xAu:
                    result = 21;
                    break;
                case (T3SurfaceFormat)5u:
                    result = 35;
                    break;
                case (T3SurfaceFormat)2u:
                    result = 23;
                    break;
                case (T3SurfaceFormat)3u:
                    result = 25;
                    break;
                case (T3SurfaceFormat)4u:
                    result = 26;
                    break;
                case (T3SurfaceFormat)0x10u:
                    result = 28;
                    break;
                case (T3SurfaceFormat)0x11u:
                    result = 50;
                    break;
                case (T3SurfaceFormat)9u:
                case (T3SurfaceFormat)0x12u:
                    result = 51;
                    break;
                case (T3SurfaceFormat)0xBu:
                    result = 42;
                    break;
                case (T3SurfaceFormat)0xCu:
                    result = 17;
                    break;
                case (T3SurfaceFormat)0xDu:
                    result = 3;
                    break;
                case (T3SurfaceFormat)6u:
                case (T3SurfaceFormat)0x13u:
                    result = 81;
                    break;
                case (T3SurfaceFormat)7u:
                    result = 34;
                    break;
                case (T3SurfaceFormat)1u:
                case (T3SurfaceFormat)8u:
                    result = 36;
                    break;
                case (T3SurfaceFormat)0x22u:
                    result = 113;
                    break;
                case (T3SurfaceFormat)0x20u:
                    result = 111;
                    break;
                case (T3SurfaceFormat)0x23u:
                    result = 114;
                    break;
                case (T3SurfaceFormat)0x21u:
                    result = 112;
                    break;
                case (T3SurfaceFormat)0x24u:
                    result = 115;
                    break;
                case (T3SurfaceFormat)0x25u:
                    result = 116;
                    break;
                case (T3SurfaceFormat)0x30u:
                case (T3SurfaceFormat)0x32u:
                    result = 80;
                    break;
                case (T3SurfaceFormat)0x31u:
                case (T3SurfaceFormat)0x33u:
                    result = 77;
                    break;
                case (T3SurfaceFormat)0x34u:
                    result = 75;
                    break;
                case (T3SurfaceFormat)0x40u:
                    result = 827611204;
                    break;
                case (T3SurfaceFormat)0x41u:
                    result = 861165636;
                    break;
                case (T3SurfaceFormat)0x42u:
                    result = 894720068;
                    break;
                case (T3SurfaceFormat)0x90u:
                    result = 22;
                    break;
                case (T3SurfaceFormat)0x43u:
                    result = 826889281;
                    break;
                case (T3SurfaceFormat)0x44u:
                    result = 843666497;
                    break;
                default:
                    result = 0;
                    break;
            }

            return result;
        }
    }
}
