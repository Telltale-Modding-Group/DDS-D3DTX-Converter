namespace D3DTX_Converter.TexconvOptions;

/// <summary>
/// Converts the final texture data to use premultiplied alpha.
/// <para>This sets an alpha mode of DDS_ALPHA_MODE_PREMULTIPLIED unless the entire alpha channel is fully opaque.</para>
/// </summary>
public class OutputPremultiplyAlpha
{
    public string GetArgumentOutput() => "-pmalpha";
}
