namespace D3DTX_Converter.TexconvOptions;

/// <summary>
/// Fits each texture to a power-of-2 for width & height, minimizing changes to the aspect ratio.
/// <para>The maximum size for a dimension is based on the -fl switch (defaults to 16384).</para>
/// </summary>
public class OutputPowerOfTwo
{
    public string GetArgumentOutput() => "-pow2";
}
