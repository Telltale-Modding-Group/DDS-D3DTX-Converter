namespace D3DTX_Converter.TexconvOptions
{
    /// <summary>
    /// When compressing BC6H / BC7 content, texconv will use DirectCompute on the GPU if available.
    /// <para>Use of this flag forces texconv to always use the software codec instead.</para>
    /// </summary>
    public class OutputUseSoftwareEncoding
    {
        public string GetArgumentOutput() => "-nogpu";
    }
}
