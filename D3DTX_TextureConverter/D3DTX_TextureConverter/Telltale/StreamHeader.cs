using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D3DTX_TextureConverter.Telltale
{
    public struct StreamHeader
    {
        /// <summary>
        /// [4 bytes] Amount of mip map regions in the texture (matches the amount of mip maps).
        /// </summary>
        public int mRegionCount;
        public int mAuxDataCount;
        public int mTotalDataSize;
    }
}
