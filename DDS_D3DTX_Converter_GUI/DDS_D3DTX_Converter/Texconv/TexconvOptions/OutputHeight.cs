namespace D3DTX_Converter.TexconvOptions;

/// <summary>
/// Height of the output texture in pixels.
/// <para>If no size is given, the size is taken from the first input image.</para>
/// </summary>
public class OutputHeight
{
    public int height;

    public string GetArgumentOutput() => string.Format("-h {0}", height);
}
