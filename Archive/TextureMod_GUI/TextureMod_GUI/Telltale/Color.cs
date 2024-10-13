using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D3DTX_TextureConverter.Telltale
{
    public struct Color
    {
        public float r;
        public float g;
        public float b;
        public float a;

        public override string ToString()
        {
            return string.Format("[Color] r: {0} g: {1} b: {2} a: {3}", r, g, b, a);
        }
    }
}
