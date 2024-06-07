namespace D3DTX_Converter.TexconvOptions;

/// <summary>
/// Separates alpha channel for resize/mipmap generation. 
/// <para>This implies an alpha mode setting of DDS_ALPHA_MODE_CUSTOM as this is typically only used if the alpha channel doesn't contain transparency information.</para>
/// </summary>
public class OutputSeperateAlpha
{
    public string GetArgumentOutput() => "-sepalpha";
}
