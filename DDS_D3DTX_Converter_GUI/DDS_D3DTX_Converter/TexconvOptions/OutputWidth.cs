namespace D3DTX_Converter.TexconvOptions
{
    /// <summary>
    /// Width of the output texture in pixels.
    /// </summary>
    public class OutputWidth
    {
        public int width;

        public string GetArgumentOutput() => string.Format("-w {0}", width);
    }
}
