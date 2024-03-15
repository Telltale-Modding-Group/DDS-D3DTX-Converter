namespace D3DTX_Converter.TexconvOptions
{
    /// <summary>
    /// For older DDS files that incorrectly encode the DXTn block compression mipchain surface blocks smaller than 4x4. 
    /// <para>This switch causes the loader to tolerate the slightly too short file length and to copy the 4x4 blocks to the smaller ones.</para> 
    /// <para>The result is not identical to re-computing computing the mipchain fully, but does provide non-corrupted data.</para>
    /// </summary>
    public class OutputBadTails
    {
        public string GetArgumentOutput() => "-badtails";
    }
}
