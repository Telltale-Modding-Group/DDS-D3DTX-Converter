﻿namespace D3DTX_Converter.TelltaleTypes
{
    public class RegionStreamHeader
    {
        public int mFaceIndex { get; set; }
        public int mMipIndex { get; set; }
        public int mMipCount { get; set; }
        public uint mDataSize { get; set; }
        public int mPitch { get; set; }
        public int mSlicePitch { get; set; }
    }
}