namespace D3DTX_Converter.TexconvOptions;

/// <summary>
/// DDS files with TYPELESS formats are treated as UNORM
/// </summary>
public class OutputTreatTypelessAsUNORM
{
    public string GetArgumentOutput() => "-tu";
}
