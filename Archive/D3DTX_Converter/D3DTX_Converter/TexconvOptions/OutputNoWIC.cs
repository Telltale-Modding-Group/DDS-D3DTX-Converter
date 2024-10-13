namespace D3DTX_Converter.TexconvOptions
{
    /// <summary>
    /// Forces filtering to use non-WIC-based code paths.
    /// <para>This is useful for working around known issues with WIC depending on which operating system you are using.</para>
    /// </summary>
    public class OutputNoWIC
    {
        public string GetArgumentOutput() => "-nowic";
    }
}
