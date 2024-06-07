namespace D3DTX_Converter.TexconvOptions;

/// <summary>
/// Sets the target feature level which determines the maximum supported texture size: 9.1, 9.2, 9.3, 10.0, 10.1, 11.0, 11.1, 12.0, or 12.1. 
/// <para>Defaults to 11.0 which is 16834, the limit for 11.x and 12.x Direct3D Hardware Feature Level.</para>
/// </summary>
public class OutputFeatureLevel
{
    public float featureLevel;

    public string GetArgumentOutput() => string.Format("-fl {0}", featureLevel);
}
