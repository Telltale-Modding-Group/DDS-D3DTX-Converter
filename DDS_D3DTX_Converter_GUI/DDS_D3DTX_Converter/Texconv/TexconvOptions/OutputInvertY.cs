namespace D3DTX_Converter.TexconvOptions;

/// <summary>
/// Inverts the value of the green channel.
/// <para>This is typically used for normal maps to deal with OpenGL vs. Direct3D conventions for 'push in' vs. 'push out'.</para>
/// </summary>
public class OutputInvertY
{
    public string GetArgumentOutput() => "-inverty";
}
