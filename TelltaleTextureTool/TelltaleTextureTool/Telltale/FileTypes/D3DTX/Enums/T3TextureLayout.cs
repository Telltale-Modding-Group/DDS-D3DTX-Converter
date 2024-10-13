namespace TelltaleTextureTool.TelltaleEnums;

// In Telltale the enum is represented using Hungarian notation (eTextureLayout_layoutName).
public enum T3TextureLayout
{
    Unknown = -1, //0FFFFFFFFh
    Texture2D = 0,
    TextureCubemap = 1,
    Texture3D = 2,
    Texture2DArray = 3,
    TextureCubemapArray = 4,
    TextureCount = 5,
}
