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
    }
}
