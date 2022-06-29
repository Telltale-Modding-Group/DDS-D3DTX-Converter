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

    /// <summary>
    /// Describes a DDS file header.
    /// </summary>
    public struct DDS_HEADER
    {
        /// <summary>
        /// [4 bytes] Size of structure. This member must be set to 124.
        /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
        /// </summary>
        public uint dwSize;

        /// <summary>
        /// [4 bytes] Flags to indicate which members contain valid data.
        /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
        /// </summary>
        public uint dwFlags;

        /// <summary>
        /// [4 bytes] Surface height (in pixels).
        /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
        /// </summary>
        public uint dwHeight;

        /// <summary>
        /// [4 bytes] Surface width (in pixels).
        /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
        /// </summary>
        public uint dwWidth;

        /// <summary>
        /// [4 bytes] The pitch or number of bytes per scan line in an uncompressed texture.
        /// <para>The total number of bytes in the top level texture for a compressed texture.</para>
        /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
        /// </summary>
        public uint dwPitchOrLinearSize;

        /// <summary>
        /// [4 bytes] Depth of a volume texture (in pixels), otherwise unused.
        /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
        /// </summary>
        public uint dwDepth;

        /// <summary>
        /// [4 bytes] Number of mipmap levels, otherwise unused.
        /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
        /// </summary>
        public uint dwMipMapCount;

        /// <summary>
        /// [4 bytes] Unused according to DDS docs.
        /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
        /// </summary>
        public uint dwReserved1;

        /// <summary>
        /// [4 bytes] Unused according to DDS docs.
        /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
        /// </summary>
        public uint dwReserved2;

        /// <summary>
        /// [4 bytes] Unused according to DDS docs.
        /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
        /// </summary>
        public uint dwReserved3;

        /// <summary>
        /// [4 bytes] Unused according to DDS docs.
        /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
        /// </summary>
        public uint dwReserved4;

        /// <summary>
        /// [4 bytes] Unused according to DDS docs.
        /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
        /// </summary>
        public uint dwReserved5;

        /// <summary>
        /// [4 bytes] Unused according to DDS docs.
        /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
        /// </summary>
        public uint dwReserved6;

        /// <summary>
        /// [4 bytes] Unused according to DDS docs.
        /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
        /// </summary>
        public uint dwReserved7;

        /// <summary>
        /// [4 bytes] Unused according to DDS docs.
        /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
        /// </summary>
        public uint dwReserved8;

        /// <summary>
        /// [4 bytes] Unused according to DDS docs.
        /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
        /// </summary>
        public uint dwReserved9;

        /// <summary>
        /// [4 bytes] Unused according to DDS docs.
        /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
        /// </summary>
        public uint dwReserved10;

        /// <summary>
        /// [4 bytes] Unused according to DDS docs.
        /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
        /// </summary>
        public uint dwReserved11;

        /// <summary>
        /// [36 bytes] Surface pixel format.
        /// </summary>
        public DDS_PIXELFORMAT ddspf;

        /// <summary>
        /// [4 bytes] Specifies the complexity of the surfaces stored.
        /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
        /// </summary>
        public uint dwCaps;

        /// <summary>
        /// [4 bytes] Additional detail about the surfaces stored.
        /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
        /// </summary>
        public uint dwCaps2;

        /// <summary>
        /// [4 bytes] Unused according to DDS docs.
        /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
        /// </summary>
        public uint dwCaps3;

        /// <summary>
        /// [4 bytes] Unused according to DDS docs.
        /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
        /// </summary>
        public uint dwCaps4;

        /// <summary>
        /// [4 bytes] Unused according to DDS docs.
        /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
        /// </summary>
        public uint dwReserved12;
    }
}
