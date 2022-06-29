using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D3DTX_TextureConverter.DirectX
{
    //DDS Docs - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-header
    //DDS PIXEL FORMAT - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-pixelformat
    //DDS DDS_HEADER_DXT10 - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-header-dxt10
    //DDS File Layout https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-file-layout-for-textures
    //Texutre Block Compression in D3D11 - https://docs.microsoft.com/en-us/windows/win32/direct3d11/texture-block-compression-in-direct3d-11
    //DDS Programming Guide - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dx-graphics-dds-pguide

    public enum DDS_ALPHA
    {
        /// <summary>
        /// Alpha channel content is unknown. This is the value for legacy files, which typically is assumed to be 'straight' alpha.
        /// </summary>
        DDS_ALPHA_MODE_UNKNOWN = 0x0,

        /// <summary>
        /// Any alpha channel content is presumed to use straight alpha.
        /// </summary>
        DDS_ALPHA_MODE_STRAIGHT = 0x1,

        /// <summary>
        /// Any alpha channel content is using premultiplied alpha. The only legacy file formats that indicate this information are 'DX2' and 'DX4'.
        /// </summary>
        DDS_ALPHA_MODE_PREMULTIPLIED = 0x2,

        /// <summary>
        /// Any alpha channel content is all set to fully opaque.
        /// </summary>
        DDS_ALPHA_MODE_OPAQUE = 0x3,

        /// <summary>
        /// Any alpha channel content is being used as a 4th channel and is not intended to represent transparency (straight or premultiplied).
        /// </summary>
        DDS_ALPHA_MODE_CUSTOM = 0x4
    }
}
