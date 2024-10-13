
## [Home](/Docs/home.md)

Many people know that images are usually stored in `RGB` or `RGBA` format. While this is true, the pixel data can take in a lot of different forms for a lot of reasons, the main 2 being: to reduce the amount resources (compression) or the program requiring more precise calculations (`HDR`).

This article has a table containing useful information regarding the formats..

Telltale surface formats have their equivalent **OpenGL formats** for games newer than `The Walking Dead`. For older ones, the directly use the ones from the original container (XENOS, Xbox 360 for example). If you are unsure in which format to save, this is the place that has a l

Softwares such as `Paint.NET`, `GIMP`, `NVIDIA Texture Tool` or `DirectX SDK (2010)` have labeled their options well enough. The download links are [here](/Docs/articles/tutorial_prelude.md#what-software-do-i-need-to-edit-d3dtx). `Paint.NET` or `GIMP` will be more than enough.
More information about different types of textures can be found [here](/Docs/articles/textures.md#what-types-of-textures-usually-exist).

| Telltale Surface Format | Equivalent Format | Notes 
| ----------------------- | ----------- | -----------  
| RGBA8 | R8G8B8A8_UNORM or R8G8B8A8_UNORM_SRGB[^1] | <ul><li>Used for **diffuse** textures.</li><li>Use this save option if you do not want to lose quality.</li></ul> | 
| ARGB8 | R8G8B8A8_UNORM or R8G8B8A8_UNORM_SRGB[^1] | <ul><li>Used for **diffuse** textures.</li></li><li>Use this save option if you do not want to lose quality.</li></ul> |
| BC1[^2] | BC1_UNORM or BC1_UNORM_SRGB[^1] |<ul><li>Used for **diffuse** textures **without alpha/transparency**.</li><li>Use this save option if you want your texture compressed.</li><li>Also known as **DXT1**.</li></ul>
| BC2[^2] | BC2_UNORM or BC2_UNORM_SRGB[^1] |<ul><li>Used for textures **with alpha/transparency**.</li><li>Use this save option if you want your texture **compressed**.</li><li>Also known as **DXT2** (pre-multiplied alpha) or **DXT3**.</li></ul>
| BC3[^2] | BC3_UNORM or BC3_UNORM_SRGB[^1] |<ul><li>Used for various textures **with alpha/transparency**.</li><li>Use this save option if you want your texture **compressed**.</li><li>Also known as **DXT4** (pre-multiplied alpha) or **DXT5**.</li></ul>
| BC4[^2] | BC4_UNORM | <ul><li>Used for ???</li><li>Also known as **ATI1**.</li><li>Use this save option if you want your texture **compressed**.</li></ul>
| BC5[^2] | BC5_UNORM |<ul><li>Used for **normal maps**.</li><li>Also known as **ATI2**.<li>Use this save option if you want your texture **compressed**.</li></ul>
| BC6[^2] | BC6H_UF16 | <ul><li>Use this save option if you want your texture **compressed**.</li></ul>
| BC7[^2] | BC7_UNORM or BC7_UNORM_SRGB[^1] | <ul><li>Use this save option if you want your texture **compressed**.</li></ul>
| ARGB16 | R16B16G16A16_UNORM |  <ul><li>Used for big detailed textures.</li><li> Only 1 such texture has been found. **(Poker Night 2 - ui_endofdemo_items.d3dtx)**</li></ul>
| RGB565 | R5G6B5_UNORM | <ul><li>Used for **diffuse** textures.</li></ul>
| ARGB1555 | B5G5R5A1_UNORM | 
| ARGB4 | R4G4B4A4_UNORM | <ul><li>Used for **diffuse** textures.</li></ul>
| ARGB2101010 | R10G10B10A2_UNORM | Requires swizzling the red and blue channels.
| R16 | R16_UNORM | 
| RG16 | R16G16_UNORM |
| RGBA16 | R16G16B16A16_UNORM | 
| RG8 | R8G8_UNORM |  
| R32 | R32_FLOAT | 
| RG32 | R32G32_FLOAT |
| RGBA32 | R32G32B32A32_FLOAT |
| R8 | R8_UNORM |  <ul><li>Equivalent to L8.</li></ul>
| RGBA8S | R8G8B8A8_SNORM |
| A8 | A8_UNORM |<ul><li>Used for detail or 1 color textures.</li></ul>
| L8 | R8_UNORM | <ul><li>Equivalent to R8.</li><li>Used for old lookup textures.</li></ul>
| AL8 | R8G8_UNORM | Used for look ups.
| L16 | R16_UNORM | Use DirectX SDK (2010 version).
| RG16S | R16G16_SNORM | 
| RGBA16S | R16G16B16A16_SNORM | 
| R16UI | R16_UINT|
| RG16UI | R16G16 _UINT |
| R16F | R16_FLOAT |
| RG16F | R16G16_FLOAT | 
| RGBA16F | R16G16B16A16_FLOAT | 
| R32F | R32_FLOAT | 
| RG32F | R32G32_FLOAT |  |
| RGBA32F | R32G32B32A32_FLOAT | <ul><li>Used for light maps.</li><li>Use NVIDIA Texture Tool or DirectX SDK.</li></ul>
| CTX1[^5]  |
| RGBA1010102F 
| RGB111110F 
| RGB9E5F | R9G9B9E5_SHAREDEXP
| PVRTC2[^3] | PVRTC 1 4bpp RGB
| PVRTC4[^3] | PVRTC 1 2bpp RGBA
| PVRTC2a[^3] | PVRTC 1 2bpp RGBA
| PVRTC4a[^3] | PVRTC 1 4bpp RGBA       
| ATC_RGB[^4] | ATC RGB
| ATC_RGB1A[^4] | ATC Explicit Alpha
| ATC_RGBA[^4] | ATC Interpolated Alpha
| ETC1_RGB[^4] | ETC1 RGB
| ETC2_RGB[^4] | ETC2_RGB
| ETC2_RGB1A[^4] | ETC2 R8G8B8A1
| ETC2_RGBA[^4] | ETC2 EAC RGBA
| ETC2_R[^4] | ETC2 EAC R11
| ETC2_RG[^4] | ETC2 EAC RG11
| ASTC_RGBA_4x4[^3] | ASTC 4x4
| FrontBuffer | X8R8G8B8

[^1]: The output depends on the D3DTX's surface gamma value. 
[^2]: BC means Block Compression, which are basically image compression algorithms. They are **lossy**.
[^3]: Used for iOS games.
[^4]: Used for Android games.
[^5]: Used for Xbox 360. (No texture found yet.)


| Telltale Surface Gamma | Color Space |
| --- | --- | 
| Non-existent | Non-sRGB (Linear) |
| Unknown | Non-sRGB (Linear) |
| Linear | Non-sRGB (Linear) | 
| sRGB | sRGB |
> [!NOTE]
> Non-existent means that there is no such field in the relative `D3DTX` version.

### Reading Debug Information

A typical `D3DTX` debug information will contain a `Meta` header and a `D3DTX` header. Other image files will use the `DirectXTex` metadata information.

The structures, types and fields can be found in the converter source code.
