namespace D3DTX_Converter.TexconvOptions
{
    /// <summary>
    /// Forces DDS file output to always use legacy DX9 headers. This will fail for BC6, BC7, UINT, and SINT formats.
    /// <para>SRGB formats will be written using non-SRGB formats.</para>
    /// <para>It will also fail for texture 2D arrays and cubemap arrays.</para>
    /// <para>This is primarily of use with DX9 era DDS texture loaders.</para>
    /// </summary>
    public class OutputUseDX9Header
    {
        public string GetArgumentOutput() => "-dx9";
    }
}
