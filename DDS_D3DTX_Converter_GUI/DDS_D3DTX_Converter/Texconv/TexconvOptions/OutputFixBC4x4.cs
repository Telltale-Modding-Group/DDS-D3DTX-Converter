namespace D3DTX_Converter.TexconvOptions;

/// <summary>
/// DDS files can contain BC compressed DDS files of arbitrary size, but Direct3D can only create BC resources where the top-level width & height are multiples of 4. 
/// <para>This switch will resize the image to the proper size for loading as a texture, but any mipmaps will have to be regenerated.</para>
/// </summary>
public class OutputFixBC4x4
{
    public string GetArgumentOutput() => "-fixbc4x4";
}
