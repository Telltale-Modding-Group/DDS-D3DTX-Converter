namespace D3DTX_Converter.TexconvOptions;

/// <summary>
/// When compressing BC6H / BC7 content, texconv will use DirectCompute on the GPU at the given index if available.
/// <para>The default is 0 which is the default adapter.</para>
/// </summary>
public class OutputUseGPUAdapter
{
    public int index;

    public string GetArgumentOutput() => string.Format("-gpu {0}", index);
}
