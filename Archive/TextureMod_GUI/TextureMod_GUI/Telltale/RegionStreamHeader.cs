using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D3DTX_TextureConverter.Telltale
{
    public class RegionStreamHeader
    {
        public int mFaceIndex;
        public int mMipIndex;
        public int mMipCount;
        public uint mDataSize;
        public int mPitch;
        public int mSlicePitch;
    }
}
