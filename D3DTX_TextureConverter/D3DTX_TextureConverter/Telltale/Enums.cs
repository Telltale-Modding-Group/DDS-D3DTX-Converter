using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D3DTX_TextureConverter.Telltale
{
    //note: can't find original name, so just using the prefix eTxAlpha
    public enum eTxAlpha
    {
        eTxAlphaUnkown = 0,
        eTxNoAlpha = 1,
        eTxAlphaTest = 2,
        eTxAlphaBlend = 3,
    }

    //note: can't find original name, so just using the prefix eTxColor
    public enum eTxColor
    {
        eTxColorUnknown = 0,
        eTxColorFull = 1,
        eTxColorBlack = 2,
        eTxColorGrayscale = 3,
        eTxColorGrayscaleAlpha = 4,
    }

    public enum T3ResourceUsage
    {
        eResourceUsage_Static = 0,
        eResourceUsage_Dynamic = 1,
        eResourceUsage_System = 2,
        eResourceUsage_RenderTarget = 3,
        eResourceUsage_ShaderWrite = 4
    }

    public enum T3SurfaceGamma
    {
        eSurfaceGamma_Linear = 0,
        eSurfaceGamma_sRGB = 1, 
        eSurfaceGamma_Unknown = 2, //0FFFFFFFFh
    }

    public enum T3SurfaceMultisample
    {
        eSurfaceMultisample_None = 0,
        eSurfaceMultisample_2x = 1,
        eSurfaceMultisample_4x = 2,
        eSurfaceMultisample_8x = 3,
        eSurfaceMultisample_16x = 4,
    }

    public enum PlatformType
    {
        ePlatform_None = 0,
        ePlatform_All = 1,
        ePlatform_PC = 2,
        ePlatform_Wii = 3,
        ePlatform_Xbox = 4,
        ePlatform_PS3 = 5,
        ePlatform_Mac = 6,
        ePlatform_iPhone = 7,
        ePlatform_Android = 8,
        ePlatform_Vita = 9,
        ePlatform_Linux = 10, //0Ah
        ePlatform_PS4 = 11, //0Bh
        ePlatform_XBOne = 12, //0Ch
        ePlatform_WiiU = 13, //0Dh
        ePlatform_Win10 = 14, //0Eh
        ePlatform_NX = 15, //0Fh
        ePlatform_Count = 16, //10h
    }

    public enum T3TextureLayout
    {
        eTextureLayout_2D = 0,
        eTextureLayout_Cube = 1,
        eTextureLayout_3D = 2,
        eTextureLayout_2DArray = 3,
        eTextureLayout_CubeArray = 4,
        eTextureLayout_Count = 5,
        eTextureLayout_Unknown = 6, //0FFFFFFFFh
    }

    public enum T3SurfaceAccess
    {
        eSurface_ReadOnly,
        eSurface_ReadWrite,
        eSurface_WriteOnly
    }

    //this name is not original, can't find the original name but the enum names are correct
    public enum T3TextureType
    {
        eTxUnknown,
        eTxStandard,
        eTxLightmap_V0,
        eTxBumpmap,
        eTxNormalMap,
        eTxSubsurfaceScatteringMap_V0,
        eTxSubsurfaceScatteringMap,
        eTxDetailMap,
        eTxStaticShadowMap,
        eTxLightmapHDR,
        eTxLightmapHDRScaled,
        eTxSDFDetailMap,
        eTxEnvMap,
        eTxSpecularColor,
        eTxToonLookup,
        eTxOutlineDiscontinuity,
        eTxEmissiveMap,
        eTxParticleProperties,
        eTxNormalGlossMap,
        eTxLookup,
        eTxAmbientOcclusion,
        eTxPrefilteredEnvCubeMapHDR,
        eTxBrushLookupMap,
        eTxVector2Map,
        eTxNormalDxDyMap,
        eTxPackedSDFDetailMap,
        eTxSingleChannelSDFDetailMap,
        eTxLightmapDirection,
        eTxLightmapStaticShadows,
        eTxLightStaticShadowMapAtlas,
        eTxLightStaticShadowMap,
        eTxPrefilteredEnvCubeMapHDRScaled,
        eTxLightStaticShadowVolume,
        eTxLightmapAtlas,
        eTxNormalXYMap,
        eTxLightmapFlatAtlas,
        eTxLookupXY,
        eTxObjectNormalMap,
    }

    public enum T3SurfaceFormat
    {
        eSurface_ARGB8 = 0,
        eSurface_ARGB16 = 1,
        eSurface_RGB565 = 2,
        eSurface_ARGB1555 = 3,
        eSurface_ARGB4 = 4,
        eSurface_ARGB2101010 = 5,
        eSurface_R16 = 6,
        eSurface_RG16 = 7,
        eSurface_RGBA16 = 8,
        eSurface_RG8 = 9,
        eSurface_RGBA8 = 10, //0Ah
        eSurface_R32 = 11, //0Bh
        eSurface_RG32 = 12, //0Ch
        eSurface_RGBA32 = 13, //0Dh
        eSurface_R8 = 14, //0Eh
        eSurface_RGBA8S = 15, //0Fh
        eSurface_A8 = 16, //10h
        eSurface_L8 = 17, //11h
        eSurface_AL8 = 18, //12h
        eSurface_L16 = 19, //13h
        eSurface_RG16S = 20, //14h
        eSurface_RGBA16S = 21, //15h
        eSurface_R16UI = 22, //16h
        eSurface_RG16UI = 23, //17h
        eSurface_R16F = 24, //20h
        eSurface_RG16F = 25, //21h
        eSurface_RGBA16F = 26, //22h
        eSurface_R32F = 27, //23h
        eSurface_RG32F = 28, //24h
        eSurface_RGBA32F = 29, //25h
        eSurface_RGBA1010102F = 30, //26h
        eSurface_RGB111110F = 31, //27h
        eSurface_RGB9E5F = 32, //28h
        eSurface_DepthPCF16 = 33, //30h
        eSurface_DepthPCF24 = 34, //31h
        eSurface_Depth16 = 35, //32h
        eSurface_Depth24 = 36, //33h
        eSurface_DepthStencil32 = 37, //34h
        eSurface_Depth32F = 38, //35h
        eSurface_Depth32F_Stencil8 = 39, //36h
        eSurface_Depth24F_Stencil8 = 40, //37h
        eSurface_BC1 = 41, //40h
        eSurface_DXT1 = 42, //40h
        eSurface_BC2 = 43, //41h
        eSurface_DXT3 = 44, //41h
        eSurface_BC3 = 45, //42h
        eSurface_DXT5 = 46, //42h
        eSurface_BC4 = 47, //43h
        eSurface_DXT5A = 48, //43h
        eSurface_BC5 = 49, //44h
        eSurface_DXN = 50, //44h
        eSurface_CTX1 = 51, //45h
        eSurface_BC6 = 52, //46h
        eSurface_BC7 = 53, //47h
        eSurface_PVRTC2 = 54, //50h
        eSurface_PVRTC4 = 55, //51h
        eSurface_PVRTC2a = 56, //52h
        eSurface_PVRTC4a = 57, //53h
        eSurface_ATC_RGB = 58, //60h
        eSurface_ATC_RGB1A = 59, //61h
        eSurface_ATC_RGBA = 60, //62h
        eSurface_ETC1_RGB = 61, //70h
        eSurface_ETC2_RGB = 62, //71h
        eSurface_ETC2_RGB1A = 63, //72h
        eSurface_ETC2_RGBA = 64, //73h
        eSurface_ETC2_R = 65, //74h
        eSurface_ETC2_RG = 66, //75h
        eSurface_ATSC_RGBA_4x4 = 67, //80h
        eSurface_FrontBuffer = 68, //90h
        eSurface_Count = 69, //91h
        eSurface_Unknown = 70, //0FFFFFFFFh
    }

    public enum GFXPlatformFastMemHeap
    {
        eGFXPlatformFastMemHeap_ShadowMap = 0,
        eGFXPlatformFastMemHeap_Other = 1,
        eGFXPlatformFastMemHeap_Count = 2,
        eGFXPlatformFastMemHeap_None = 3, //0FFFFFFFFh
    }

    public enum GFXPlatformAllocationType
    {
        eGFXPlatformAllocation_Unknown = 0,
        eGFXPlatformAllocation_RenderTarget = 1,
        eGFXPlatformAllocation_ShadowMap = 2,
        eGFXPlatformAllocation_DiffuseTexture = 3,
        eGFXPlatformAllocation_NormalmapTexture = 4,
        eGFXPlatformAllocation_LightmapTexture = 5,
        eGFXPlatformAllocation_DetailTexture = 6,
        eGFXPlatformAllocation_AmbientOcclusionTexture = 7,
        eGFXPlatformAllocation_FontTexture = 8,
        eGFXPlatformAllocation_ParticleTexture = 9,
        eGFXPlatformAllocation_MiscTexture = 10, //0Ah
        eGFXPlatformAllocation_StaticMesh = 11, //0Bh
        eGFXPlatformAllocation_TextMesh = 12, //0Ch
        eGFXPlatformAllocation_NPRLineMesh = 13, //0Dh
        eGFXPlatformAllocation_BokehMesh = 14, //0Eh
        eGFXPlatformAllocation_DynamicMesh = 15, //0Fh
        eGFXPlatformAllocation_GenericBuffer = 16, //10h
        eGFXPlatformAllocation_ParticleMesh = 17, //11h
        eGFXPlatformAllocation_Effect = 18, //12h
        eGFXPlatformAllocation_EffectShader = 19, //13h
        eGFXPlatformAllocation_Uniform = 20, //14h
        eGFXPlatformAllocation_StreamingUniform = 21, //15h
        eGFXPlatformAllocation_AmbientOcclusion = 22, //16h
        eGFXPlatformAllocation_Count = 23, //17h
    }
}
