using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D3DTX_TextureConverter.Telltale
{
    public struct ClassNames
    {
        public Symbol mTypeNameCRC;
        public uint mVersionCRC;

        public override string ToString()
        {
            return string.Format("[ClassNames] mTypeNameCRC: ({0}) mVersionCRC: {1}", mTypeNameCRC, mVersionCRC);
        }
    }
}
