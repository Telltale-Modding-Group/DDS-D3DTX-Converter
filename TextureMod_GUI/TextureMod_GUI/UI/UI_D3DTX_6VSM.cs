using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using D3DTX_TextureConverter.Telltale;

namespace TextureMod_GUI.UI
{
    public class UI_D3DTX_6VSM
    {
        //meta header
        public string MetaStreamVersion { get; }
        public uint DefaultSectionChunkSize { get; } 
        public uint DebugSectionChunkSize { get; }
        public uint AsyncSectionChunkSize { get; }

        //d3dtx header
        public int Version { get; }
        //public T3SamplerStateBlock mSamplerState { get; }
        public PlatformType Platform { get; set; }
        public string Name { get; set; }
        public uint NumMipLevels { get; set; }
        public uint Width { get; set; }
        public uint Height { get; set; }
        public uint Depth { get; set; }
        public uint ArraySize { get; }
        public T3SurfaceFormat SurfaceFormat { get; set; }
        public T3TextureLayout TextureLayout { get; set; }
        public T3SurfaceGamma SurfaceGamma { get; set; }
        public T3SurfaceMultisample SurfaceMultisample { get; set; }
        public T3ResourceUsage ResourceUsage { get; set; }
        public T3TextureType Type { get; set; }
        public float SpecularGlossExponent { get; set; }
        public float HDRLightmapScale { get; set; }
        public float ToonGradientCutoff { get; set; }
        public eTxAlpha AlphaMode { get; set; }
        public eTxColor ColorMode { get; set; }
        public Vector2 UVOffset { get; set; }
        public Vector2 UVScale { get; set; }
        public List<Symbol> ArrayFrameNames { get; set; }
        public List<T3ToonGradientRegion> ToonRegions { get; set; }
        public StreamHeader StreamHeader { get; set; }
        public List<RegionStreamHeader> RegionHeaders { get; }
    }
}
