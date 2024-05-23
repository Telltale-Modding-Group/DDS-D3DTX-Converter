using System.IO;
using D3DTX_Converter.Utilities;

namespace D3DTX_Converter.TelltaleTypes;
public struct TelltaleBoolean
{
    public bool mbTelltaleBoolean;

    public TelltaleBoolean(BinaryReader reader)
    {
        mbTelltaleBoolean = ByteFunctions.ReadBoolean(reader);
    }

    public TelltaleBoolean(bool value)
    {
        mbTelltaleBoolean = value;
    }

    public static uint GetByteSize()
    {
        uint totalByteSize = 0;

        totalByteSize += 1; //mbHasProps [1 bytes]

        return totalByteSize;
    }

    public override readonly string ToString() => string.Format("[TelltaleBoolean] : {0} ", mbTelltaleBoolean);
}