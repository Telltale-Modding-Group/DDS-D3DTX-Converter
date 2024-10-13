namespace D3DTX_Converter.TexconvOptions
{
    /// <summary>
    /// Preserves alpha coverage in generate mipmaps for alpha test reference value (0 to 1).
    /// </summary>
    public class OutputKeepCoverage
    {
        public float coverage;

        public string GetArgumentOutput() => string.Format("-keepcoverage {0}", coverage);
    }
}
