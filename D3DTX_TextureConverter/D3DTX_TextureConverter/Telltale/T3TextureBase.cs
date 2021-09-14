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
        public T3TextureLayout TextureLayout;
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
    }
}
