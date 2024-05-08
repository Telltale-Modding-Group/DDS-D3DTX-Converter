namespace D3DTX_Converter.TexconvOptions;

/// <summary>
/// DDS files with L8, A8L8, or L16 formats are expanded to 8:8:8:8 or 16:16:16:16. 
/// <para>Without this flag, they are converted to 1-channel (red) or 2-channel (red/green) formats.</para>
/// </summary>
public class OutputDoXLUM
{
    public string GetArgumentOutput() => "-xlum";
}
