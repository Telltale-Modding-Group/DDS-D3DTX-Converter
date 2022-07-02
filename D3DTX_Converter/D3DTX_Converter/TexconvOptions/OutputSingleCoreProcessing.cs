namespace D3DTX_Converter.TexconvOptions
{
    /// <summary>
    /// If the DirectXTex library and the texconv utility are built with OpenMP enabled, by default the tool will use multi-threading for CPU-based compression of BC6H and BC7 formats to spread the compression work across multiple cores.
    /// <para>This flag disables this behavior forcing it to remain on a single core.</para>
    /// </summary>
    public class OutputSingleCoreProcessing
    {
        public string GetArgumentOutput() => "-singleproc";
    }
}
