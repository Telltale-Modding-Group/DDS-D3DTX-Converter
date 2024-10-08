namespace D3DTX_Converter.TexconvOptions
{
    /// <summary>
    /// Swizzles image channels using an HLSL-style mask (rgba, rrra, r, a, xy, etc.).
    /// <para>The mask is 1 to 4 characters in length. A 0 indicates setting that channel to zero, a 1 indicates setting that channel to maximum.</para>
    /// </summary>
    public class OutputSwizzle
    {
        public string mask;

        public string GetArgumentOutput() => string.Format("-swizzle {0}", mask);
    }
}
