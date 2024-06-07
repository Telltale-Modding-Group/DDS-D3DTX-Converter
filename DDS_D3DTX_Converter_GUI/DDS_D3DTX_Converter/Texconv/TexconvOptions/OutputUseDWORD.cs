namespace D3DTX_Converter.TexconvOptions;

/// <summary>
/// For DDS files that use a DWORD alignment instead of BYTE alignment (used for some legacy files typically 24bpp).
/// </summary>
public class OutputUseDWORD
{
    public string GetArgumentOutput() => "-dword";
}