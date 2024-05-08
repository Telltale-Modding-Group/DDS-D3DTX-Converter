using System;
using System.IO;
using D3DTX_Converter.DirectX;
using DirectXTexNet;

namespace D3DTX_Converter_Avalonia_GUI.DirectX;

public static class DDS_DirectXTexNet
{
    public static DDS_DirectXTexNet_ImageSection[] GetDDSImageSections(string ddsFilePath)
    {
        string solutionDir = AppDomain.CurrentDomain.BaseDirectory;
        string texconvApplicationDirectoryPath =
            Path.Combine(solutionDir, "ExternalDependencies", "DirectXTexNetImpl.dll");

        TexHelper.LoadInstanceFrom(texconvApplicationDirectoryPath);

        ScratchImage ddsImage = TexHelper.Instance.LoadFromDDSFile(ddsFilePath, DDS_FLAGS.NONE);

        DDS_DirectXTexNet_ImageSection[] section = new DDS_DirectXTexNet_ImageSection[ddsImage.GetImageCount()];

        for (int i = 0; i < section.Length; i++)
        {
            Image image = ddsImage.GetImage(i);

            section[i] = new()
            {
                Width = image.Width,
                Height = image.Height,
                Format = image.Format,
                SlicePitch = image.SlicePitch,
                RowPitch = image.RowPitch
            };
        }

        return section;
    }
}
