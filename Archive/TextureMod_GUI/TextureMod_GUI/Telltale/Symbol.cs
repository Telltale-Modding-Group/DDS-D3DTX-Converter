using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D3DTX_TextureConverter.Telltale
{
    public struct Symbol
    {
        public ulong mCrc64;

        public override string ToString()
        {
            return string.Format("[Symbol] mCrc64: {0}", mCrc64);
        }
    }
}
