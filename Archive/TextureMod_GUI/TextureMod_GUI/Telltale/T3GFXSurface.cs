using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D3DTX_TextureConverter.Telltale
{
    public struct T3GFXSurface
    {
        public uint mpData;
        public int mLayout;
        public int mFormat;
        public int mWidthInBlocks;
        public int mHeightInBlocks;
        public int mDepth;
        public int mArraySize;
        public int mPitch;
        public int mSlicePitch;
    }

    /*
    T3GFXSurface    struc ; (sizeof=0x28, align=0x8, copyof_3269)
    00000000                                         ; XREF: T3GFXDynamicTextureResult/r
    00000000 mpData          dq ?                    ; offset
    00000008 mLayout         dd ?                    ; enum T3TextureLayout
    0000000C mFormat         dd ?                    ; enum T3SurfaceFormat
    00000010 mWidthInBlocks  dd ?
    00000014 mHeightInBlocks dd ?
    00000018 mDepth          dd ?
    0000001C mArraySize      dd ?
    00000020 mPitch          dd ?
    00000024 mSlicePitch     dd ?
    00000028 T3GFXSurface    ends 
    */
}
