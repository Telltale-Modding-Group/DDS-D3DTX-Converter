namespace D3DTX_Converter.TexconvOptions;

/// <summary>
/// Text string to attach to the front of the resulting texture's name.
/// </summary>
public class OutputPrefixString
{
    public string? prefix;

    public string GetArgumentOutput() => $"-px {prefix}";
}
