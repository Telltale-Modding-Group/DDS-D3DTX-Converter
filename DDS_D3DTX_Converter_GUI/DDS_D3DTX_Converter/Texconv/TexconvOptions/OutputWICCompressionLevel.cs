namespace D3DTX_Converter.TexconvOptions;

/// <summary>
/// Provides an image quality setting to use when encoding WIC images ranging from 0.0 to 1.0.
/// <para>Applies to jpg, tiff, and jxr.</para>
/// </summary>
public class OutputWICCompressionLevel
{
    public float level;

    public string GetArgumentOutput() => string.Format("-wicq {0}", level);
}
