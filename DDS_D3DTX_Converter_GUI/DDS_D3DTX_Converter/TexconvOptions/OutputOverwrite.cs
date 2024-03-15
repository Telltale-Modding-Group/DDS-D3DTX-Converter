namespace D3DTX_Converter.TexconvOptions
{
    /// <summary>
    /// Overwrite existing output file if any.
    /// <para>By default, the tool will skip the write if the output file already exists.</para>
    /// </summary>
    public class OutputOverwrite
    {
        public string GetArgumentOutput() => "-y";
    }
}
