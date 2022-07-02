namespace D3DTX_Converter.TexconvOptions
{
    /// <summary>
    /// Sets the alpha threshold value for 1-bit alpha such as BC1 or RGBA5551. Defaults to 0.5.
    /// </summary>
    public class OutputAlphaThreshold
    {
        public float threshold;

        public string GetArgumentOutput() => string.Format("-at {0}", threshold);
    }
}
