using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D3DTX_TextureConverter.Telltale
{
    /// <summary>
    /// This is a class name struct used in a meta header.
    /// This contains a CRC64'd string of a class name used in the file.
    /// The CRC64 string of a classname is usually all lowercase, and uses 
    /// </summary>
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
