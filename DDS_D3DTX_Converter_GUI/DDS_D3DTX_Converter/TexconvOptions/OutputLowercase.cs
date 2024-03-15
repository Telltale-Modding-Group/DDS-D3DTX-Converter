namespace D3DTX_Converter.TexconvOptions
{
    /// <summary>
    /// Forces the output path & filename to all lower-case.
    /// <para>Windows file system is case-insensitive by default, but some programs like git are case-sensitive.</para>
    /// </summary>
    public class OutputLowercase
    {
        public string GetArgumentOutput() => "-l";
    }
}
