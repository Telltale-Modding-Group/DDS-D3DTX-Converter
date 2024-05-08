using System.Collections.Generic;

namespace D3DTX_Converter.TexconvOptions
{
    /*
     * TEXCONV DOCS - https://github.com/Microsoft/DirectXTex/wiki/Texconv 
    */

    public class MasterOptions
    {
        public OutputAlphaThreshold? outputAlphaThreshold;
        public OutputAlphaWeighting? outputAlphaWeighting;
        public OutputApplyTonemap? outputApplyTonemap;
        public OutputBadTails? outputBadTails;
        public OutputChromaKey? outputChromaKey;
        public OutputCompressionOptionsForBC? outputCompressionOptionsForBC;
        public OutputDirectory? outputDirectory;
        public OutputDisplayCompressionTiming? outputDisplayCompressionTiming;
        public OutputDoXLUM? outputDoXLUM;
        public OutputFeatureLevel? outputFeatureLevel;
        public OutputFileType? outputFileType;
        public OutputFilter? outputFilter;
        public OutputFixBC4x4? outputFixBC4X4;
        public OutputFormat? outputFormat;
        public OutputHDRNits? outputHDRNits;
        public OutputHeight? outputHeight;
        public OutputHorizontalFlip? outputHorizontalFlip;
        public OutputInvertY? outputInvertY;
        public OutputKeepCoverage? outputKeepCoverage;
        public OutputLowercase? outputLowercase;
        public OutputMipMaps? outputMipMaps;
        public OutputNormalFromHeight? outputNormalFromHeight;
        public OutputNormalFromHeightAmplitude? outputNormalFromHeightAmplitude;
        public OutputNoWIC? outputNoWIC;
        public OutputOverwrite? outputOverwrite;
        public OutputPowerOfTwo? outputPowerOfTwo;
        public OutputPrefixString? outputPrefixString;
        public OutputPremultiplyAlpha? outputPremultiplyAlpha;
        public OutputReconstructZ? outputReconstructZ;
        public OutputRotateColor? outputRotateColor;
        public OutputSeperateAlpha? outputSeperateAlpha;
        public OutputSingleCoreProcessing? outputSingleCoreProcessing;
        public OutputSpecialNormalConversion? outputSpecialNormalConversion;
        public OutputSRGB? outputSRGB;
        public OutputStraightAlpha? outputStraightAlpha;
        public OutputSuffixString? outputSuffixString;
        public OutputSupressCopyrightMessage? outputSupressCopyrightMessage;
        public OutputSwizzle? outputSwizzle;
        public OutputTreatTypelessAsFLOAT? outputTreatTypelessAsFLOAT;
        public OutputTreatTypelessAsUNORM? outputTreatTypelessAsUNORM;
        public OutputUseDWORD? outputUseDWORD;
        public OutputUseDX10Header? outputUseDX10Header;
        public OutputUseDX9Header? outputUseDX9Header;
        public OutputUseGPUAdapter? outputUseGPUAdapter;
        public OutputUseSoftwareEncoding? outputUseSoftwareEncoding;
        public OutputUseTGA2? outputUseTGA2;
        public OutputVerticalFlip? outputVerticalFlip;
        public OutputWICCompressionLevel? outputWICCompressionLevel;
        public OutputWICLossless? outputWICLossless;
        public OutputWICMultiFrameEncoding? outputWICMultiFrameEncoding;
        public OutputWidth? outputWidth;
        public OutputWrapMode? outputWrapMode;

        public string GetArguments(string inputFilePath)
        {
            List<string> arguments = new();

            if (inputFilePath.Contains(" "))
            {
                inputFilePath = $"\"{inputFilePath}\"";
            }

            arguments.Add(inputFilePath);
            arguments.Add("-r");

            if (outputOverwrite != null) arguments.Add(outputOverwrite.GetArgumentOutput());

            if (outputWidth != null) arguments.Add(outputWidth.GetArgumentOutput());
            if (outputHeight != null) arguments.Add(outputHeight.GetArgumentOutput());
            if (outputMipMaps != null) arguments.Add(outputMipMaps.GetArgumentOutput());

            if (outputAlphaThreshold != null) arguments.Add(outputAlphaThreshold.GetArgumentOutput());
            if (outputAlphaWeighting != null) arguments.Add(outputAlphaWeighting.GetArgumentOutput());
            if (outputApplyTonemap != null) arguments.Add(outputApplyTonemap.GetArgumentOutput());
            if (outputBadTails != null) arguments.Add(outputBadTails.GetArgumentOutput());
            if (outputChromaKey != null) arguments.Add(outputChromaKey.GetArgumentOutput());
            if (outputCompressionOptionsForBC != null) arguments.Add(outputCompressionOptionsForBC.GetArgumentOutput());
            if (outputDisplayCompressionTiming != null) arguments.Add(outputDisplayCompressionTiming.GetArgumentOutput());
            if (outputDoXLUM != null) arguments.Add(outputDoXLUM.GetArgumentOutput());
            if (outputFeatureLevel != null) arguments.Add(outputFeatureLevel.GetArgumentOutput());
            if (outputFilter != null) arguments.Add(outputFilter.GetArgumentOutput());
            if (outputFixBC4X4 != null) arguments.Add(outputFixBC4X4.GetArgumentOutput());
            if (outputFormat != null) arguments.Add(outputFormat.GetArgumentOutput());
            if (outputHDRNits != null) arguments.Add(outputHDRNits.GetArgumentOutput());
            if (outputHorizontalFlip != null) arguments.Add(outputHorizontalFlip.GetArgumentOutput());
            if (outputInvertY != null) arguments.Add(outputInvertY.GetArgumentOutput());
            if (outputKeepCoverage != null) arguments.Add(outputKeepCoverage.GetArgumentOutput());
            if (outputLowercase != null) arguments.Add(outputLowercase.GetArgumentOutput());
            if (outputNormalFromHeight != null) arguments.Add(outputNormalFromHeight.GetArgumentOutput());
            if (outputNormalFromHeightAmplitude != null) arguments.Add(outputNormalFromHeightAmplitude.GetArgumentOutput());
            if (outputNoWIC != null) arguments.Add(outputNoWIC.GetArgumentOutput());
            if (outputPowerOfTwo != null) arguments.Add(outputPowerOfTwo.GetArgumentOutput());
            if (outputPrefixString != null) arguments.Add(outputPrefixString.GetArgumentOutput());
            if (outputPremultiplyAlpha != null) arguments.Add(outputPremultiplyAlpha.GetArgumentOutput());
            if (outputReconstructZ != null) arguments.Add(outputReconstructZ.GetArgumentOutput());
            if (outputRotateColor != null) arguments.Add(outputRotateColor.GetArgumentOutput());
            if (outputSeperateAlpha != null) arguments.Add(outputSeperateAlpha.GetArgumentOutput());
            if (outputSingleCoreProcessing != null) arguments.Add(outputSingleCoreProcessing.GetArgumentOutput());
            if (outputSpecialNormalConversion != null) arguments.Add(outputSpecialNormalConversion.GetArgumentOutput());
            if (outputSRGB != null) arguments.Add(outputSRGB.GetArgumentOutput());
            if (outputStraightAlpha != null) arguments.Add(outputStraightAlpha.GetArgumentOutput());
            if (outputSuffixString != null) arguments.Add(outputSuffixString.GetArgumentOutput());
            if (outputSupressCopyrightMessage != null) arguments.Add(outputSupressCopyrightMessage.GetArgumentOutput());
            if (outputSwizzle != null) arguments.Add(outputSwizzle.GetArgumentOutput());
            if (outputTreatTypelessAsFLOAT != null) arguments.Add(outputTreatTypelessAsFLOAT.GetArgumentOutput());
            if (outputTreatTypelessAsUNORM != null) arguments.Add(outputTreatTypelessAsUNORM.GetArgumentOutput());
            if (outputUseDWORD != null) arguments.Add(outputUseDWORD.GetArgumentOutput());
            if (outputUseDX10Header != null) arguments.Add(outputUseDX10Header.GetArgumentOutput());
            if (outputUseDX9Header != null) arguments.Add(outputUseDX9Header.GetArgumentOutput());
            if (outputUseGPUAdapter != null) arguments.Add(outputUseGPUAdapter.GetArgumentOutput());
            if (outputUseSoftwareEncoding != null) arguments.Add(outputUseSoftwareEncoding.GetArgumentOutput());
            if (outputUseTGA2 != null) arguments.Add(outputUseTGA2.GetArgumentOutput());
            if (outputVerticalFlip != null) arguments.Add(outputVerticalFlip.GetArgumentOutput());
            if (outputWICCompressionLevel != null) arguments.Add(outputWICCompressionLevel.GetArgumentOutput());
            if (outputWICLossless != null) arguments.Add(outputWICLossless.GetArgumentOutput());
            if (outputWICMultiFrameEncoding != null) arguments.Add(outputWICMultiFrameEncoding.GetArgumentOutput());
            if (outputWrapMode != null) arguments.Add(outputWrapMode.GetArgumentOutput());

            if (outputFileType != null) arguments.Add(outputFileType.GetArgumentOutput());

            if (outputDirectory != null) arguments.Add(outputDirectory.GetArgumentOutput());

            return string.Join(" ", arguments.ToArray());
        }
    }
}
