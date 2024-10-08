namespace D3DTX_Converter.TexconvOptions
{
    /// <summary>
    /// Output directory.
    /// </summary>
    public class OutputDirectory
    {
        public string directory;

        public string GetArgumentOutput() => string.Format("-o {0}", directory);
    }
}
