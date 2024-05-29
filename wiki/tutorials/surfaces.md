
## [Home](/wiki/home.md)

Telltale surface formats have their equivalent DXGI format. If you are not sure in which format to save, this is a table containing useful information.

| Telltale Surface Format | DXGI Format | Notes |
| ----------------------- | ----------- | ----------- |
| ARGB8 | B8R8G8A8_UNORM or B8R8G8A8_UNORM_SRGB[^1] | |
| ARGB16 | R16B16G16A16_UNORM | 
| RGB565 | R5G6B5_UNORM |
| ARGB1555 | B5G5R5A1_UNORM | 
| ARGB4 | B4G4R4A4_UNORM |
| ARGB2101010 | R10G10B10A2_UNORM | Requires swizzling the red and blue channels.
| R16 | R16_UNORM | 
| RG16 | R16G16_UNORM |
| RGBA16 | R16G16B16A16_UNORM | 
| RG8 | R8G8_UNORM |  
| RGBA8 | R8G8B8A8_UNORM or R8G8B8A8_UNORM_SRGB[^1] |
| R32 | R32_FLOAT| 
| RG32 | R32G32_FLOAT |
| RGBA32 | R32G32B32A32 |
| R8 | R8_UNORM | Use R8 in Paint.Net or L8 in GIMP
| RGBA8S | R8G8B8A8_SNORM |
| A8 | A8_UNORM | Used in detail textures.
| L8 | R8_UNORM | 
| AL8 | R8G8_UNORM | 
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
| RGBA32F | R32G32B32A32_FLOAT | Used in light maps.
| BC1 | BC1_UNORM or BC1_UNORM_SRGB[^1] | <ul><li>Also known as DXT2 (pre-multiplied alpha) or DXT3</li><li>Commonly used in textures **without** alpha.</li></ul>
| BC2 | BC2_UNORM or BC2_UNORM_SRGB[^1] | <ul><li>Also known as DXT2 (pre-multiplied alpha) or DXT3</li><li>Commonly used in textures **with** alpha.</li></ul>
| BC3 | BC3_UNORM or BC3_UNORM_SRGB[^1] | <ul><li>Also known as DXT4 (pre-multiplied alpha) or DXT5</li><li>Commonly used in textures with alpha.</li></ul>
| BC4 | BC4_UNORM | <ul><li>Also known as ATI1</li><li>Commonly used in ???</li></ul>
| BC5 | BC5_UNORM | <ul><li>Also known as ATI2</li><li>Commonly used in normal maps.</li></ul>
| BC6 | BC6H_UF16 | 
| BC7 | BC7_UNORM or BC7_UNORM_SRGB[^1] | 
| [Any Format]_UNORM | [Any Format]_UNORM | You will find these formats in legacy Telltale titles.

> There are other existing Telltale formats, but they cannot be found on PC platforms.

> DDS editors do not have all of the save options presented above. Use DirectX SDK or NVIDIA Texture Tool for conversion.

| Telltale Surface Gamma | DXGI Format |
| --- | --- | 
| Non-existent | Non-SRGB (Linear) |
| Unknown | Non-SRGB (Linear) |
| Linear | Non-SRGB (Linear)| 
| sRGB | SRGB |
> Non-existent means that there is no such field in the relative D3DTX version.

[^1]: The output depends on the D3DTX's surface gamma value.



