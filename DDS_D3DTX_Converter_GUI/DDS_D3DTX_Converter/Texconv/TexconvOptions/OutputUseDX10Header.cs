namespace D3DTX_Converter.TexconvOptions;

/// <summary>
/// Forces DDS file output to always use the "DX10" header extension, and allows the writing of alpha mode metadata information.
/// <para>The resulting file may not be compatible with the legacy D3DX10 or D3DX11 libraries or older DDS readers.</para>
/// </summary>
public class OutputUseDX10Header
{
    public string GetArgumentOutput() => "-dx10";
}
