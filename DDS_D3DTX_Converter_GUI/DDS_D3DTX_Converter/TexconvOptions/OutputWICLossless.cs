namespace D3DTX_Converter.TexconvOptions
{
    /// <summary>
    /// <para>Enable lossless encoding when encoding WIC images. Applies to jpg, tiff, and jxr.</para>
    /// </summary>
    public class OutputWICLossless
    {
        public string GetArgumentOutput() => "-wiclossless";
    }
}
