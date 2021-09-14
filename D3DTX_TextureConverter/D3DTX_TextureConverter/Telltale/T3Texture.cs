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
        public int mType;
        public float mSpecularGlossExponent; //tends to be 1090519040
        public RenderSwizzleParams mSwizzle;
        public eTxAlpha mAlphaMode;
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
    }
}
