namespace D3DTX_Converter.TexconvOptions;

/// <summary>
/// Text string to attach to the end of the resulting texture's name.
/// </summary>
public class OutputSuffixString
{
    public string suffix;

    public string GetArgumentOutput() => string.Format("-sx {0}", suffix);
}
