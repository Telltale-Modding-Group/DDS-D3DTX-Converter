using System;
using D3DTX_Converter.DirectX;

namespace D3DTX_Converter.TexconvOptions;

/// <summary>
/// Output format. Specify the DXGI format without the DXGI_FORMAT_ prefix (i.e. -f R10G10B10A2_UNORM). 
/// <para>It also supports some common aliases (
/// -f DXT1 for DXGI_FORMAT_BC1_UNORM,
/// -f DXT5 for DXGI_FORMAT_BC3_UNORM,
/// -f BGRA for DXGI_FORMAT_B8G8R8A8_UNORM,
/// -f BGR for DXGI_FORMAT_B8G8R8X8_UNORM,
/// -f FP16 for DXGI_FORMAT_R16G16B16A16_FLOAT, etc.)</para>
/// </summary>
public class OutputFormat
{
    public DXGIFormat format;
    //public string GetFormat() => "DXGI_FORMAT_" + Enum.GetName(typeof(DXGI_FORMAT), format);
    public string GetFormat() => Enum.GetName(typeof(DXGIFormat), format);

    public string GetArgumentOutput() => string.Format("-f {0}", GetFormat());
}
