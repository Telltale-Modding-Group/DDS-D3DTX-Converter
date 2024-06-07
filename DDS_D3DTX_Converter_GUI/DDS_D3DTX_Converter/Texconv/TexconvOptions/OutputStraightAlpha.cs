namespace D3DTX_Converter.TexconvOptions;

/// <summary>
/// Converts a premultiplied alpha image to non-premultiplied alpha (a.k.a. straight alpha).
/// </summary>
public class OutputStraightAlpha
{
    public string GetArgumentOutput() => "-alpha";
}
