﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace D3DTX_TextureConverter.TelltaleTypes
{
    public struct Color
    {
        public float r;
        public float g;
        public float b;
        public float a;

        public Color(BinaryReader reader)
        {
            r = reader.ReadSingle();
            g = reader.ReadSingle();
            b = reader.ReadSingle();
            a = reader.ReadSingle();
        }

        public void WriteBinaryData(BinaryWriter writer)
        {
            writer.Write(r);
            writer.Write(g);
            writer.Write(b);
            writer.Write(a);
        }

        public uint GetByteSize()
        {
            uint totalByteSize = 0;

            totalByteSize += (uint)Marshal.SizeOf(r); //r [4 bytes]
            totalByteSize += (uint)Marshal.SizeOf(g); //g [4 bytes]
            totalByteSize += (uint)Marshal.SizeOf(b); //b [4 bytes]
            totalByteSize += (uint)Marshal.SizeOf(a); //a [4 bytes]

            return totalByteSize;
        }

        public override string ToString() => string.Format("[Color] r: {0} g: {1} b: {2} a: {3}", r, g, b, a);
    }
}
