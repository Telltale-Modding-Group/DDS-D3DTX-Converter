using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D3DTX_TextureConverter.Telltale
{
    public struct RenderSwizzleParams
    {
        public char mSwizzle1;
        public char mSwizzle2;
        public char mSwizzle3;
        public char mSwizzle4;

        public override string ToString()
        {
            return string.Format("[RenderSwizzleParams]: {0} {1} {2} {3}", (int)mSwizzle1, (int)mSwizzle2, (int)mSwizzle3, (int)mSwizzle4);
        }
    }
}
