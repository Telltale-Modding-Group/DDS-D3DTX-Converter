namespace D3DTX_Converter.TelltaleEnums
{
    public enum GFXPlatformAllocationType
    {
        eGFXPlatformAllocation_Unknown = 0,
        eGFXPlatformAllocation_RenderTarget = 1,
        eGFXPlatformAllocation_ShadowMap = 2,
        eGFXPlatformAllocation_DiffuseTexture = 3,
        eGFXPlatformAllocation_NormalmapTexture = 4,
        eGFXPlatformAllocation_LightmapTexture = 5,
        eGFXPlatformAllocation_DetailTexture = 6,
        eGFXPlatformAllocation_AmbientOcclusionTexture = 7,
        eGFXPlatformAllocation_FontTexture = 8,
        eGFXPlatformAllocation_ParticleTexture = 9,
        eGFXPlatformAllocation_MiscTexture = 10, //0Ah
        eGFXPlatformAllocation_StaticMesh = 11, //0Bh
        eGFXPlatformAllocation_TextMesh = 12, //0Ch
        eGFXPlatformAllocation_NPRLineMesh = 13, //0Dh
        eGFXPlatformAllocation_BokehMesh = 14, //0Eh
        eGFXPlatformAllocation_DynamicMesh = 15, //0Fh
        eGFXPlatformAllocation_GenericBuffer = 16, //10h
        eGFXPlatformAllocation_ParticleMesh = 17, //11h
        eGFXPlatformAllocation_Effect = 18, //12h
        eGFXPlatformAllocation_EffectShader = 19, //13h
        eGFXPlatformAllocation_Uniform = 20, //14h
        eGFXPlatformAllocation_StreamingUniform = 21, //15h
        eGFXPlatformAllocation_AmbientOcclusion = 22, //16h
        eGFXPlatformAllocation_Count = 23, //17h
    }
}
