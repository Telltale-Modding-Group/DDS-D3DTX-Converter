namespace D3DTX_Converter.TexconvOptions
{
    /// <summary>
    /// Indicates an amplitude for the normal map, which defaults to 1.
    /// </summary>
    public class OutputNormalFromHeightAmplitude
    {
        public float amplitude;

        public string GetArgumentOutput() => string.Format("-nmapamp {0}", amplitude);
    }
}
