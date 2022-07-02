using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DirectXTexNet;

namespace D3DTX_Converter.DirectX
{
    //DDS Docs - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-header
    //DDS PIXEL FORMAT - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-pixelformat
    //DDS DDS_HEADER_DXT10 - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-header-dxt10
    //DDS File Layout https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-file-layout-for-textures
    //Texutre Block Compression in D3D11 - https://docs.microsoft.com/en-us/windows/win32/direct3d11/texture-block-compression-in-direct3d-11
    //DDS Programming Guide - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dx-graphics-dds-pguide

    /// <summary>
    /// DDS header extension to handle resource arrays, DXGI pixel formats that don't map to the legacy Microsoft DirectDraw pixel format structures, and additional metadata.
    /// </summary>
    public struct DDS_HEADER_DXT10
    {
        /// <summary>
        /// [4 bytes] The surface pixel format.
        /// </summary>
        public DXGI_FORMAT dxgiFormat;

        /// <summary>
        /// [4 bytes] Identifies the type of resource.
        /// </summary>
        public D3D10_RESOURCE_DIMENSION resourceDimension;

        /// <summary>
        /// [4 bytes] Identifies other, less common options for resources.
        /// </summary>
        public uint miscFlag;

        /// <summary>
        /// [4 bytes] The number of elements in the array.
        /// <para>For a 2D texture that is also a cube-map texture, this number represents the number of cubes.</para>
        /// </summary>
        public uint arraySize;

        /// <summary>
        /// [4 bytes] Contains additional metadata (formerly was reserved). 
        /// </summary>
        public uint miscFlags2;
    }
}
