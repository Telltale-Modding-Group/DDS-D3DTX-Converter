using D3DTX_Converter.TexconvEnums;

namespace D3DTX_Converter.TexconvOptions;

/// <summary>
/// Sets the texture addressing mode for filtering to wrap or mirror, otherwise defaults to clamp.
/// </summary>
public class OutputWrapMode
{
    public TexconvEnumWrapMode wrapMode;

    public string GetArgumentOutput()
    {
        switch (wrapMode)
        {
            case TexconvEnumWrapMode.wrap:
                return "-wrap";
            case TexconvEnumWrapMode.mirror:
                return "-mirror";
            default:
                return "";
        }
    }
}
