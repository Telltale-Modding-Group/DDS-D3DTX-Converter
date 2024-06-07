namespace D3DTX_Converter.TexconvOptions;

/// <summary>
/// flags Indicates conversion from a height-map to a normal-map. The flags is a combination of one or more of the following, and must have one of r, g, b, a, or l:
/// <para>r: Use the red channel of the input as the height</para>
/// <para>g: Use the green channel of the input as the height</para>
/// <para>b: Use the blue channel of the input as the height</para>
/// <para>a: Use the alpha channel of the input as the height</para>
/// <para>l: Use the luminance computed from red, green, and blue channels of the input as the height</para>
/// <para>m: Use mirroring in U & V.Defaults to wrap when doing the central difference computation.</para>
/// <para>u: Use mirroring in U.Defaults to wrap when doing the central difference computation.</para>
/// <para>v: Use mirroring in V.Defaults to wrap when doing the central difference computation.</para>
/// <para>i: Invert sign of the computed normal</para>
/// <para>o: Compute a rough occlusion term and encode it into the alpha channel of the output.</para>
/// </summary>
public class OutputNormalFromHeight
{
    public string flags;

    public string GetArgumentOutput() => string.Format("-nmap {0}", flags);
}
