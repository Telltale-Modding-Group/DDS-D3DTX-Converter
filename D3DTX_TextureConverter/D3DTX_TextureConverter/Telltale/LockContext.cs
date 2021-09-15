using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D3DTX_TextureConverter.Telltale
{
    public class LockContext
    {
        //public int mpTempAllocator;
        public char mpBits;
        public int mPitch;
        public int mSlicePitch;
        public T3SurfaceFormat mLockFormat;
        public int mFace;
        public int mWidth;
        public int mDepth;
        public bool mbRead;
        //public bool mbTexToTexCopy;
        //public int mpPlatformData;
    }
}
