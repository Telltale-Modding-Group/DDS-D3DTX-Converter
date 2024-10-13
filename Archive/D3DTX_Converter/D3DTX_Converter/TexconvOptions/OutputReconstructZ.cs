namespace D3DTX_Converter.TexconvOptions
{
    /// <summary>
    /// Rebuilds the Z (blue) channel assuming X/Y are normals (primarily for converting from the BC5 format)
    /// </summary>
    public class OutputReconstructZ
    {
        public string GetArgumentOutput() => "-reconstructz";
    }
}
