using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D3DTX_TextureConverter.Telltale
{
    public struct Vector2
    {
        public float x;
        public float y;

        public override string ToString()
        {
            return string.Format("[Vector2] x: {0} y: {1}", x, y);
        }
    }
}
