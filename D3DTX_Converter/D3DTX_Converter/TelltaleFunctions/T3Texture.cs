using System.Collections.Generic;
using D3DTX_Converter.TelltaleEnums;

namespace D3DTX_Converter.TelltaleFunctions
{
    public static class T3Texture
    {
        public static string GetTextureTypeName(int mType)
        {
            switch (mType)
            {
                case 0: return "Unknown";
                case 1: return "LightMapV0";
                case 2: return "BumpMap";
                case 3: return "NormalMap";
                case 4: return "UNUSED0";
                case 5: return "UNUSED1";
                case 6: return "SubsurfaceScatteringMapV0";
                case 7: return "SubsurfaceScatteringMap";
                case 8: return "DetailMap";
                case 9: return "StaticShadowMap";
                case 10: return "LightmapHDR";
                case 11: return "SharpDetailMap";
                case 12: return "EnvMap";
                case 13: return "SpecularColorMap";
                case 14: return "ToonLookupMap";
                case 15: return "DiffuseColorMap";
                case 16: return "OutlineMap";
                case 17: return "LightmapHDRScaled";
                case 18: return "EmissiveMap";
                case 19: return "ParticleProperties";
                case 20: return "BrushNormalMap";
                case 21: return "UNUSED2";
                case 22: return "NormalGlossMap";
                case 23: return "LookupMap";
                case 40: return "LookupXYMap";
                case 24: return "AmbientOcclusionMap";
                case 25: return "PrefilteredEnvCubeMapHDR";
                case 35: return "PrefilteredEnvCubeMapHDRScaled";
                case 26: return "BrushLookupMap";
                case 27: return "Vector2Map";
                case 28: return "NormalDxDyMap";
                case 29: return "PackedSDFMap";
                case 30: return "SingleChannelSDFMap";
                case 31: return "LightmapDirection";
                case 32: return "LightmapStaticShadows";
                case 33: return "LightStaticShadowMapAtlas";
                case 34: return "LightStaticShadowMap";
                case 38: return "NormalXYMap";
                case 41: return "ObjectNormalMap";
                default: return "InvalidMap";
            }
        }


        public static T3TextureType GetTextureType(int value)
        {
            switch (value)
            {
                default: return (T3TextureType)value;
                case 0: return T3TextureType.eTxUnknown;
                case 15: return T3TextureType.eTxStandard;
                case 1: return T3TextureType.eTxLightmap_V0;
                case 2: return T3TextureType.eTxBumpmap;
                case 3: return T3TextureType.eTxNormalMap;
                case 6: return T3TextureType.eTxSubsurfaceScatteringMap_V0;
                case 7: return T3TextureType.eTxSubsurfaceScatteringMap;
                case 8: return T3TextureType.eTxDetailMap;
                case 9: return T3TextureType.eTxStaticShadowMap;
                case 10: return T3TextureType.eTxLightmapHDR;
                case 17: return T3TextureType.eTxLightmapHDRScaled;
                case 11: return T3TextureType.eTxSDFDetailMap;
                case 12: return T3TextureType.eTxEnvMap;
                case 13: return T3TextureType.eTxSpecularColor;
                case 14: return T3TextureType.eTxToonLookup;
                case 16: return T3TextureType.eTxOutlineDiscontinuity;
                case 18: return T3TextureType.eTxEmissiveMap;
                case 19: return T3TextureType.eTxParticleProperties;
                case 22: return T3TextureType.eTxNormalGlossMap;
                case 23: return T3TextureType.eTxLookup;
                case 24: return T3TextureType.eTxAmbientOcclusion;
                case 25: return T3TextureType.eTxPrefilteredEnvCubeMapHDR;
                case 26: return T3TextureType.eTxBrushLookupMap;
                case 27: return T3TextureType.eTxVector2Map;
                case 28: return T3TextureType.eTxNormalDxDyMap;
                case 29: return T3TextureType.eTxPackedSDFDetailMap;
                case 30: return T3TextureType.eTxSingleChannelSDFDetailMap;
                case 31: return T3TextureType.eTxLightmapDirection;
                case 32: return T3TextureType.eTxLightmapStaticShadows;
                case 33: return T3TextureType.eTxLightStaticShadowMapAtlas;
                case 34: return T3TextureType.eTxLightStaticShadowMap;
                case 35: return T3TextureType.eTxPrefilteredEnvCubeMapHDRScaled;
                case 36: return T3TextureType.eTxLightStaticShadowVolume;
                case 37: return T3TextureType.eTxLightmapAtlas;
                case 38: return T3TextureType.eTxNormalXYMap;
                case 39: return T3TextureType.eTxLightmapFlatAtlas;
                case 40: return T3TextureType.eTxLookupXY;
                case 41: return T3TextureType.eTxObjectNormalMap;
            }
        }

        public static eTxAlpha GetAlphaMode(long value)
        {
            switch (value)
            {
                default: return (eTxAlpha)value;
                case -1: return eTxAlpha.eTxAlphaUnknown;
                case 0: return eTxAlpha.eTxNoAlpha;
                case 1: return eTxAlpha.eTxAlphaTest;
                case 2: return eTxAlpha.eTxAlphaBlend;
            }
        }

        public static eTxColor GetColorMode(long value)
        {
            switch (value)
            {
                default: return (eTxColor)value;
                case -1: return eTxColor.eTxColorUnknown;
                case 0: return eTxColor.eTxColorFull;
                case 1: return eTxColor.eTxColorBlack;
                case 2: return eTxColor.eTxColorGrayscale;
                case 3: return eTxColor.eTxColorGrayscaleAlpha;
            }
        }
    }
}
