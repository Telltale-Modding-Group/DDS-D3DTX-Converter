using System.IO;
using D3DTX_Converter.Utilities;

namespace D3DTX_Converter.TelltaleTypes;
public struct ToolProps
{
    public bool mbHasProps;

    public ToolProps(BinaryReader reader)
    {
        mbHasProps = ByteFunctions.ReadBoolean(reader);
    }

    public readonly uint GetByteSize()
    {
        uint totalByteSize = 0;

        totalByteSize += 1; //mbHasProps [1 bytes]

        return totalByteSize;
    }

    public override readonly string ToString() => string.Format("[ToolProps] mbHasProps: {0}", mbHasProps);
}