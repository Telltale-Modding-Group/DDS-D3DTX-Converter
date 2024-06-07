namespace D3DTX_Converter.TexconvOptions;

/// <summary>
/// Provides a paper-white nits value for the HDR10 conversions above (defaults to 200.0) with an upper limit of 10000.
/// </summary>
public class OutputHDRNits
{
    public float nits = 200.0f;

    public string GetArgumentOutput() => string.Format("-nits {0}", nits);
}
