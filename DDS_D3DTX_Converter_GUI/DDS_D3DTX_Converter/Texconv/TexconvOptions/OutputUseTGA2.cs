namespace D3DTX_Converter.TexconvOptions;

/// <summary>
/// Forces TGA file output to include the TGA 2.0 extension area which includes gamma, alpha mode, and a time stamp.
/// </summary>
public class OutputUseTGA2
{
    public string GetArgumentOutput() => "-tga20";
}
