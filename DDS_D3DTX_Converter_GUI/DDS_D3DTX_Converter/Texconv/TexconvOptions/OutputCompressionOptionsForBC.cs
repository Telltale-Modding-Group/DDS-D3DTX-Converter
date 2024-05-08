namespace D3DTX_Converter.TexconvOptions;

/// <summary>
/// Sets options for BC compression. The flags is a combination of one or more of the following:
/// <para>u: to use uniform weighting rather than perceptual(BC1-BC3)</para>
/// <para>d: use dithering(BC1-BC3)</para>
/// <para>q: Uses minimal compression(BC7: uses just mode 6)</para>
/// <para>x: Uses maximum compression(BC7: enables mode 0 & 2 usage)</para>
/// </summary>
public class OutputCompressionOptionsForBC
{
    public string flags;

    public string GetArgumentOutput() => string.Format("-bc {0}", flags);
}
