namespace D3DTX_Converter.TexconvOptions
{
    /// <summary>
    /// Provides a colorkey/chromakey value which is used to replace a specific RGB color in hexadecimal (within a tolerance) with alpha 0.0. 
    /// <para>Any existing color channel is overwritten. For example, -c 0000FF for a blue colorkey, -c 00FF00 for a green colorkey, -c 0CFF5D for the typical 'green-screen' in a video.</para>
    /// </summary>
    public class OutputChromaKey
    {
        public string hexColor;

        public string GetArgumentOutput() => string.Format("-c {0}", hexColor);
    }
}
