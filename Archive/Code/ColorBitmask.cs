using System.IO;
using System.Runtime.InteropServices;

namespace TelltaleTextureTool.TelltaleTypes;

public struct ColorMask
{
    public int alpha;
    public int color;

    public ColorMask(BinaryReader reader)
    {
        alpha = reader.ReadInt32();
        color = reader.ReadInt32();
    }

    public readonly void WriteBinaryData(BinaryWriter writer)
    {
        writer.Write(alpha);
        writer.Write(color);
    }

    public readonly uint GetByteSize()
    {
        uint totalByteSize = 0;

        totalByteSize += (uint)Marshal.SizeOf(alpha);

        return totalByteSize;
    }

    public override readonly string ToString() => $"ColorBitmask: {{ alpha: {alpha}, color: {color} }}";
}
