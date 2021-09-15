using System.Collections.Generic;

namespace D3DTX_TextureConverter.Telltale
{
    public struct T3Texture
    {
        //baseclass_0 T3Texture_DX11 ?
        //mpHOI Ptr<HandleObjectInfo> ?
        //mDeletedCallbacks Callbacks<T3Texture> ?

        public bool mbLocked;
        public int mVersion;
        public string mImportName;
        public float mImportScale;
        public ToolProps mToolProps;
        public PlatformType mPlatform;
        public T3TextureType mType;
        public float mSpecularGlossExponent; //tends to be 1090519040
        public RenderSwizzleParams mSwizzle;
        public eTxAlpha mAlphaMode;
        public eTxColor mColorMode;
        public float mHDRLightmapScale; //tends to be 1086324736
        public float mToonGradientCutoff; //tends to be -1082130432
        public Vector2 mUVOffset; //tends to be vec2(0, 0)
        public Vector2 mUVScale; //tends to be vec2(1065353216, 1065353216)
        public List<T3ToonGradientRegion> mToonRegions; //this is an array of toon regions
        public List<Symbol> mArrayFrameNames; //this is an array of symbols
        //public List<AuxiliaryData> mAuxiliaryData; //this is a linked list of 'AuxiliaryData'
        public List<RegionStreamHeader> mRegionHeaders; //this is a list of region stream headers
        public int mNumRegionsLoaded;
        public LockContext mAsyncReadLock;
        public int mFrameUsedForRendering;
        public uint mAsyncStreamPos;
        //mpAsyncStream
        //mhAsyncRequest
        //mhTextureReadRequest
        public float mAbsTimeNeeded;
        public int mStreamingRefCount;
        //mpAsyncData
    }

    public static class T3Texture_Functions
    {
        public static string GetTextureTypeName(int mType)
        {
            switch (mType)
            {
                case 0:
                    return "Unknown";
                case 1:
                    return "LightMapV0";
                case 2:
                    return "BumpMap";
                case 3:
                    return "NormalMap";
                case 4:
                    return "UNUSED0";
                case 5:
                    return "UNUSED1";
                case 6:
                    return "SubsurfaceScatteringMapV0";
                case 7:
                    return "SubsurfaceScatteringMap";
                case 8:
                    return "DetailMap";
                case 9:
                    return "StaticShadowMap";
                case 10:
                    return "LightmapHDR";
                case 11:
                    return "SharpDetailMap";
                case 12:
                    return "EnvMap";
                case 13:
                    return "SpecularColorMap";
                default:
                    return "";
            }
        }


        public static T3TextureType GetTextureType(int value)
        {
            switch (value)
            {
                default:
                    return T3TextureType.eTxStandard;
                case 0:
                    return T3TextureType.eTxUnknown;
                case 15:
                    return T3TextureType.eTxStandard;
                case 1:
                    return T3TextureType.eTxLightmap_V0;
                case 2:
                    return T3TextureType.eTxBumpmap;
                case 3:
                    return T3TextureType.eTxNormalMap;
                case 6:
                    return T3TextureType.eTxSubsurfaceScatteringMap_V0;
                case 7:
                    return T3TextureType.eTxSubsurfaceScatteringMap;
                case 8:
                    return T3TextureType.eTxDetailMap;
                case 9:
                    return T3TextureType.eTxStaticShadowMap;
                case 10:
                    return T3TextureType.eTxLightmapHDR;
                case 17:
                    return T3TextureType.eTxLightmapHDRScaled;
                case 11:
                    return T3TextureType.eTxSDFDetailMap;
                case 12:
                    return T3TextureType.eTxEnvMap;
                case 13:
                    return T3TextureType.eTxSpecularColor;
                case 14:
                    return T3TextureType.eTxToonLookup;
                case 16:
                    return T3TextureType.eTxOutlineDiscontinuity;
                case 18:
                    return T3TextureType.eTxEmissiveMap;
                case 19:
                    return T3TextureType.eTxParticleProperties;
                case 22:
                    return T3TextureType.eTxNormalGlossMap;
                case 23:
                    return T3TextureType.eTxLookup;
                case 24:
                    return T3TextureType.eTxAmbientOcclusion;
                case 25:
                    return T3TextureType.eTxPrefilteredEnvCubeMapHDR;
                case 26:
                    return T3TextureType.eTxBrushLookupMap;
                case 27:
                    return T3TextureType.eTxVector2Map;
                case 28:
                    return T3TextureType.eTxNormalDxDyMap;
                case 29:
                    return T3TextureType.eTxPackedSDFDetailMap;
                case 30:
                    return T3TextureType.eTxSingleChannelSDFDetailMap;
                case 31:
                    return T3TextureType.eTxLightmapDirection;
                case 32:
                    return T3TextureType.eTxLightmapStaticShadows;
                case 33:
                    return T3TextureType.eTxLightStaticShadowMapAtlas;
                case 34:
                    return T3TextureType.eTxLightStaticShadowMap;
                case 35:
                    return T3TextureType.eTxPrefilteredEnvCubeMapHDRScaled;
                case 36:
                    return T3TextureType.eTxLightStaticShadowVolume;
                case 37:
                    return T3TextureType.eTxLightmapAtlas;
                case 38:
                    return T3TextureType.eTxNormalXYMap;
                case 39:
                    return T3TextureType.eTxLightmapFlatAtlas;
                case 40:
                    return T3TextureType.eTxLookupXY;
                case 41:
                    return T3TextureType.eTxObjectNormalMap;
            }
        }

        public static eTxAlpha GetAlphaMode(long value)
        {
            switch (value)
            {
                default:
                    return eTxAlpha.eTxNoAlpha;
                case -1:
                    return eTxAlpha.eTxAlphaUnkown;
                case 0:
                    return eTxAlpha.eTxNoAlpha;
                case 1:
                    return eTxAlpha.eTxAlphaTest;
                case 2:
                    return eTxAlpha.eTxAlphaBlend;
            }
        }

        public static eTxColor GetColorMode(long value)
        {
            switch (value)
            {
                default:
                    return eTxColor.eTxColorFull;
                case -1:
                    return eTxColor.eTxColorUnknown;
                case 0:
                    return eTxColor.eTxColorFull;
                case 1:
                    return eTxColor.eTxColorBlack;
                case 2:
                    return eTxColor.eTxColorGrayscale;
                case 3:
                    return eTxColor.eTxColorGrayscaleAlpha;
            }
        }
    }
}
