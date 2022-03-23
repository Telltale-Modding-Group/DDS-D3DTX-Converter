using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using D3DTX_TextureConverter.Telltale;

namespace TextureMod_GUI.UI
{
    public struct DataGridObject_UINT
    {
        public ulong Offset { get; }
        public string VariableName { get; }
        public uint Value { get; set; }
    }

    public struct DataGridObject_STRING
    {
        public ulong Offset { get; }
        public string VariableName { get; }
        public string Value { get; set; }
    }

    public struct DataGridObject_BOOL
    {
        public ulong Offset { get; }
        public string VariableName { get; }
        public bool Value { get; set; }
    }

    public struct DataGridObject_ENUM
    {
        public ulong Offset { get; }
        public string VariableName { get; }
        public T3TextureType Value { get; set; }
    }
}
