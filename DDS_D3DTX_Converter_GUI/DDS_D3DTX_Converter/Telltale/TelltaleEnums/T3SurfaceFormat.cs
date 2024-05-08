namespace D3DTX_Converter.TelltaleEnums
{

    /*In order to correctly build your file and avoid visual errors within the game, you need to properly save the .dds file after editing it.
    * Fist of all, you need to generate mip maps when saving the.dds file.
    * Secondly, the file has to be saved in the correct format.
    * To find out which format the .dds file has to be saved in, you need to check "mSurfaceFormat" parameter in the corresponding .json file or the Surface Format shown on the GUI.
    * The parameter will contain a number. In order to learn, which format you need to save your file in, check the following table:
    */
    public enum T3SurfaceFormat
    {

        #region DXGI_FORMATS

        //Eqivalent to DXGI_FORMAT_B8G8R8A8_UNORM or Direct3D9 D3DFMT_A8R8G8B8
        eSurface_ARGB8 = 0,

        //Eqivalent to DXGI_FORMAT_B16G16R16A16_UNORM or Direct3D9 D3DFMT_A16B16G16R16 with blue and red channel reversed
        // EDIT: The channels may not be reversed at all. Poker Knight 2 has a single texture with this format.
        // When it was converted, the colours were OK.
        eSurface_ARGB16 = 1,

        //Eqivalent to DXGI_FORMAT_B5G6R5_UNORM or Direct3D9 D3DFMT_R5G6B5
        eSurface_RGB565 = 2,

        //Eqivalent to DXGI_FORMAT_B5G5R5A1_UNORM or Direct3D9 D3DFMT_A1R5G5B5
        eSurface_ARGB1555 = 3,

        //Eqivalent to DXGI_FORMAT_B4G4R4A4_UNORM or Direct3D9 D3DFMT_A4R4G4B4
        eSurface_ARGB4 = 4,

        //Eqivalent to DXGI_FORMAT_B2G3R3A8_UNORM or Direct3D9 D3DFMT_A8R3G3B2
        eSurface_ARGB2101010 = 5,

        //Eqivalent to DXGI_FORMAT_R16_UNORM or Direct3D9 D3DFMT_R16F
        eSurface_R16 = 6,

        //Eqivalent to DXGI_FORMAT_R16G16_UNORM or Direct3D9 D3DFMT_G16R16
        eSurface_RG16 = 7,

        //Eqivalent to DXGI_FORMAT_R16G16B16A16_UNORM or Direct3D9 D3DFMT_A16B16G16R16
        eSurface_RGBA16 = 8,

        //Eqivalent to = DXGI_FORMAT_R8G8_UNORM or Direct3D9 D3DFMT_G8R8
        eSurface_RG8 = 9,

        //Eqivalent to DXGI_FORMAT_R8G8B8A8_UNORM or Direct3D9 D3DFMT_A8B8G8R8
        eSurface_RGBA8 = 10, //0Ah

        //Eqivalent to DXGI_FORMAT_R32_FLOAT or Direct3D9 D3DFMT_R32F
        eSurface_R32 = 11, //0Bh

        //Eqivalent to DXGI_FORMAT_R32G32_FLOAT or Direct3D9 D3DFMT_G32R32
        eSurface_RG32 = 12, //0Ch

        //Eqivalent to DXGI_FORMAT_R32G32B32A32_FLOAT or Direct3D9 D3DFMT_A32B32G32R32F
        eSurface_RGBA32 = 13, //0Dh

        //Eqivalent to DXGI_FORMAT_R8_UNORM or Direct3D9 D3DFMT_L8
        eSurface_R8 = 14, //0Eh

        //Eqivalent to DXGI_FORMAT_R8G8B8A8_UNORM or Direct3D9 D3DFMT_A8B8G8R8
        eSurface_RGBA8S = 15, //0Fh

        //Eqivalent to DXGI_FORMAT_A8_UNORM or Direct3D9 D3DFMT_A8
        eSurface_A8 = 16, //10h

        //L8, AL8 and L16 are luminance formats, which are not supported by Direct3D11. 
        //These will be mapped to DXGI_FORMAT_R8, DXGI_FORMAT_R8G8 and DXGI_FORMAT_R16_UNORM respectively.
        //For further accurracy, mapping them RGBA8 and RGBA16 would be more accurate, but it's not necessary.

        //Eqivalent to DXGI_FORMAT_R8 or Direct3D9 D3DFMT_L8
        eSurface_L8 = 17, //11h Only one file found in Borderlands - mlaa_lookup

        //Eqivalent to DXGI_FORMAT_R8G8 or Direct3D9 D3DFMT_A8L8
        eSurface_AL8 = 18, //12h

        //Eqivalent to DXGI_FORMAT_R16_UNORM or Direct3D9 D3DFMT_R16F
        eSurface_L16 = 19, //13h

        //Eqivalent to DXGI_FORMAT_R16G16_SINT or Direct3D9 D3DFMT_G16R16
        eSurface_RG16S = 20, //14h

        //Eqivalent to DXGI_FORMAT_R16G16B16A16_SINT or Direct3D9 D3DFMT_A16B16G16R16
        eSurface_RGBA16S = 21, //15h

        //Eqivalent to DXGI_FORMAT_R16_UINT or Direct3D9 D3DFMT_R16F
        eSurface_R16UI = 22, //16h

        //Eqivalent to DXGI_FORMAT_R16G16_UINT or Direct3D9 D3DFMT_G16R16
        eSurface_RG16UI = 23, //17h

        //Eqivalent to DXGI_FORMAT_R10G10B10A2_UNORM or Direct3D9 D3DFMT_A2B10G10R10
        eSurface_RGBA1010102F = 30, //26h

        //Eqivalent to DXGI_FORMAT_R11G11B10_FLOAT or Direct3D9 D3DFMT_R11G11B10F
        eSurface_RGB111110F = 31, //27h

        //Eqivalent to DXGI_FORMAT_R16G16B16A16_UINT or Direct3D9 D3DFMT_A16B16G16R16
        eSurface_R16F = 32, //20h

        //Eqivalent to DXGI_FORMAT_R16G16_FLOAT or Direct3D9 D3DFMT_G16R16
        eSurface_RG16F = 33, //21h

        //Eqivalent to DXGI_FORMAT_R16G16B16A16_FLOAT or Direct3D9 D3DFMT_A16B16G16R16
        eSurface_RGBA16F = 34, //22h

        //Eqivalent to DXGI_FORMAT_R32_FLOAT or Direct3D9 D3DFMT_R32F
        eSurface_R32F = 35, //23h

        //Eqivalent to DXGI_FORMAT_R32G32_FLOAT or Direct3D9 D3DFMT_G32R32
        eSurface_RG32F = 36, //24h

        //Eqivalent to DXGI_FORMAT_R32G32B32A32_FLOAT or Direct3D9 D3DFMT_A32B32G32R32F
        eSurface_RGBA32F = 37, //25h Used for lmaps, 12 of them Borderlands

        //Eqivalent to DXGI_FORMAT_R9G9B9E5_SHAREDEXP or Direct3D9 D3DFMT_R9G9B9E5
        eSurface_RGB9E5F = 200, //28h

        #endregion
        /*
        PCF probably stands for percentage-closer filtering, which is used for shadow mapping. No idea why it's different than Depth16 and Depth24.
        https://github.com/bkaradzic/bimg/blob/master/src/image.cpp
        These are usually created run-time in the engine, so it's unlikely that you will need to use these formats.
        */
        eSurface_DepthPCF16 = 48, //30h D3DFMT, probably used for PC versions
        eSurface_DepthPCF24 = 201, //31h D3DFMT probably used for PC versions
        eSurface_Depth16 = 50, //32h D3DFMT regular depth
        eSurface_Depth24 = 51, //33h D3DFMT regular depth
        eSurface_DepthStencil32 = 52, //34h D3DFMT
        eSurface_Depth32F = 53, //35h D3DFMT
        eSurface_Depth32F_Stencil8 = 54, //36h DepthF 32 + 8 stencil
        eSurface_Depth24F_Stencil8 = 40, //37h D3DFMT

        #region Texture Compression Formats

        //Eqivalent to DXGI_FORMAT_BC1_UNORM or Direct3D9 D3DFMT_DXT1
        eSurface_BC1 = 41, //40h

        //Eqivalent to DXGI_FORMAT_BC1_UNORM or Direct3D9 D3DFMT_DXT1
        eSurface_DXT1 = 64, //40h

        //Eqivalent to DXGI_FORMAT_BC2_UNORM or Direct3D9 D3DFMT_DXT3
        eSurface_BC2 = 43, //41h

        //Eqivalent to DXGI_FORMAT_BC2_UNORM or Direct3D9 D3DFMT_DXT3
        eSurface_DXT3 = 65, //41h

        //Eqivalent to DXGI_FORMAT_BC3_UNORM or Direct3D9 D3DFMT_DXT5
        eSurface_BC3 = 45, //42h

        //Eqivalent to DXGI_FORMAT_BC3_UNORM or Direct3D9 D3DFMT_DXT5
        eSurface_DXT5 = 66, //42h

        //Eqivalent to DXGI_FORMAT_BC4_UNORM or Direct3D9 D3DFMT_ATI1
        eSurface_BC4 = 47, //43h

        //Eqivalent to DXGI_FORMAT_BC4_UNORM or Direct3D9 D3DFMT_ATI1?
        eSurface_DXT5A = 67, //43h

        //Eqivalent to DXGI_FORMAT_BC5_UNORM or Direct3D9 D3DFMT_ATI2?
        eSurface_BC5 = 49, //44h

        /*
        Variant of BC5 compression
        */
        eSurface_DXN = 68, //44h 

        /*
        CTX1 is a format that according to the limited information that exists online, is specific to the Xbox360 platform. 
        CTX1 is similar to DXN format in that it is a two channel texture designed for tangent space normal maps, but it is lower quality. 
        Information for this format is scarce, and so are tools regarding compressing/decompressing the format. 
        We will ignore support for this format for the time being especially as Xbox360 also had support for DXT compressions which were likely more commonly used than this one.
        https://forum.xen-tax.com/viewtopic.php@p=83846.html 
        https://github.com/Xenomega/Alteration/blob/master/Alteration/Halo%203/Map%20File/Raw/BitmapRaw/DXTDecoder.cs 
        https://fileadmin.cs.lth.se/cs/Personal/Michael_Doggett/talks/unc-xenos-doggett.pdf)
        */
        eSurface_CTX1 = 69, //45h

        //Eqivalent to DXGI_FORMAT_BC6H_UF16
        eSurface_BC6 = 70, //46h

        //Eqivalent to DXGI_FORMAT_BC7_UNORM
        eSurface_BC7 = 71, //47h

        /*
         *The following formats are used for iOS/Android platforms only, which are not supported:
         * PVRTC2
         * PVRTC4
         * PVRTC2a
         * PVRTC4a
         * ATC_RGB 
         * ATC_RGB1A (Presumably 1 bit for the alpha) 
         * ATC_RGBA 
         * ETC1_RGB
         * ETC2_RGB  
         * ETC2_RGB1A 
         * ETC2_RGBA 
         * ETC2_R
         * ETC2_RG
         * ATSC_RGBA_4x4 
         */
        eSurface_PVRTC2 = 80, //50h
        eSurface_PVRTC4 = 81, //51h
        eSurface_PVRTC2a = 82, //52h
        eSurface_PVRTC4a = 83, //53h
        eSurface_ATC_RGB = 96, //60h
        eSurface_ATC_RGB1A = 97, //61h
        eSurface_ATC_RGBA = 98, //62h
        eSurface_ETC1_RGB = 112, //70h
        eSurface_ETC2_RGB = 113, //71h
        eSurface_ETC2_RGB1A = 114, //72h
        eSurface_ETC2_RGBA = 115, //73h
        eSurface_ETC2_R = 116, //74h
        eSurface_ETC2_RG = 117, //75h

        /*
        * Presumably eSurface_ETC2_RGBM exists, but it is not used. ETC2_R

        Aticct
        */

        eSurface_ATSC_RGBA_4x4 = 128, //80h

        #endregion
        eSurface_FrontBuffer = 202, //90h

        eSurface_Count = 203, //91h
        eSurface_Unknown = -1, //0FFFFFFFFh
    }
}
