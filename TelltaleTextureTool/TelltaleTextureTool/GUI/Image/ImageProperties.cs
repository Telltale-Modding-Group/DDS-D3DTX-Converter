using CommunityToolkit.Mvvm.ComponentModel;

namespace TelltaleTextureTool;

public class ImageProperties : ObservableObject
{
    /// <summary>
    /// Image properties that are displayed on the panel.
    /// </summary>
    public string Name { get; set; } = string.Empty;
    public string Extension { get; set; } = string.Empty;
    public string Width { get; set; } = string.Empty;
    public string Height { get; set; } = string.Empty;
    public string SurfaceFormat { get; set; } = string.Empty;
    public string SurfaceGamma { get; set; } = string.Empty;
    public string MipMapCount { get; set; } = string.Empty;
    public string ArraySize { get; set; } = string.Empty;
    public string HasAlpha { get; set; } = string.Empty;
    public string AlphaMode { get; set; } = string.Empty;
    public string TextureLayout { get; set; } = string.Empty;
}
