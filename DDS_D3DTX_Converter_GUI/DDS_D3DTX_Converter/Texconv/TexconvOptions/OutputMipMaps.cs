namespace D3DTX_Converter.TexconvOptions;

/// <summary>
/// Number of mipmap levels to generate in the output texture.
/// <para>This setting only applies to DDS output, which defaults to 0 which is generate all mipmaps.</para>
/// <para>Use -m 1 to remove mipmaps.</para>
/// </summary>
public class OutputMipMaps
{
    public bool remove;
    public int mipmaps = 0; //default, which generates all mipmaps

    public string GetArgumentOutput()
    {
        if (remove)
            return string.Format("-m {0}", 1);
        else
            return string.Format("-m {0}", mipmaps);
    }
}
