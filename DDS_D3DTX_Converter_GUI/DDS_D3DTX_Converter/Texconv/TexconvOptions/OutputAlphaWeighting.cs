namespace D3DTX_Converter.TexconvOptions;

/// <summary>
/// Provides an alpha weighting to use with the error metric for the BC7 GPU compressor. Defaults to 1.0.
/// </summary>
public class OutputAlphaWeighting
{
    public float weight = 1.0f;

    public string GetArgumentOutput() => string.Format("-aw {0}", weight);
}
