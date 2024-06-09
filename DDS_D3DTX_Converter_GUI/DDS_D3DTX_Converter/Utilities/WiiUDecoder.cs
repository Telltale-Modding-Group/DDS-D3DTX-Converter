using System;

public class AddrLib
{
    private static uint m_banks = 4;
    private static uint m_banksBitcount = 2;
    private static uint m_pipes = 2;
    private static uint m_pipesBitcount = 1;
    private static uint m_pipeInterleaveBytes = 256;
    private static uint m_pipeInterleaveBytesBitcount = 8;
    private static uint m_rowSize = 2048;
    private static uint m_swapSize = 256;
    private static uint m_splitSize = 2048;
    private static uint m_chipFamily = 2;
    private static uint MicroTilePixels = 8 * 8;

    private static byte[] formatHwInfo = new byte[0x40 * 4]
    {
        // todo: Convert to struct
        // each entry is 4 bytes
        0x00,0x00,0x00,0x01,0x08,0x03,0x00,0x01,0x08,0x01,0x00,0x01,0x00,0x00,0x00,0x01,
        0x00,0x00,0x00,0x01,0x10,0x07,0x00,0x00,0x10,0x03,0x00,0x01,0x10,0x03,0x00,0x01,
        0x10,0x0B,0x00,0x01,0x10,0x01,0x00,0x01,0x10,0x03,0x00,0x01,0x10,0x03,0x00,0x01,
        0x10,0x03,0x00,0x01,0x20,0x03,0x00,0x00,0x20,0x07,0x00,0x00,0x20,0x03,0x00,0x00,
        0x20,0x03,0x00,0x01,0x20,0x05,0x00,0x00,0x00,0x00,0x00,0x00,0x20,0x03,0x00,0x00,
        0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x01,0x20,0x03,0x00,0x01,0x00,0x00,0x00,0x01,
        0x00,0x00,0x00,0x01,0x20,0x0B,0x00,0x01,0x20,0x0B,0x00,0x01,0x20,0x0B,0x00,0x01,
        0x40,0x05,0x00,0x00,0x40,0x03,0x00,0x00,0x40,0x03,0x00,0x00,0x40,0x03,0x00,0x00,
        0x40,0x03,0x00,0x01,0x00,0x00,0x00,0x00,0x80,0x03,0x00,0x00,0x80,0x03,0x00,0x00,
        0x00,0x00,0x00,0x01,0x00,0x00,0x00,0x01,0x00,0x00,0x00,0x01,0x10,0x01,0x00,0x00,
        0x10,0x01,0x00,0x00,0x20,0x01,0x00,0x00,0x20,0x01,0x00,0x00,0x20,0x01,0x00,0x00,
        0x00,0x01,0x00,0x01,0x00,0x01,0x00,0x00,0x00,0x01,0x00,0x00,0x60,0x01,0x00,0x00,
        0x60,0x01,0x00,0x00,0x40,0x01,0x00,0x01,0x80,0x01,0x00,0x01,0x80,0x01,0x00,0x01,
        0x40,0x01,0x00,0x01,0x80,0x01,0x00,0x01,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
        0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
        0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00
    };

    public static uint SurfaceGetBitsPerPixel(uint surfaceFormat)
    {
        uint hwFormat = surfaceFormat & 0x3F;
        uint bpp = formatHwInfo[hwFormat * 4];
        return bpp;
    }

    public static uint ComputeSurfaceThickness(uint tileMode)
    {
        uint thickness = 1;

        if (tileMode == 3 || tileMode == 7 || tileMode == 11 || tileMode == 13 || tileMode == 15)
            thickness = 4;

        else if (tileMode == 16 || tileMode == 17)
            thickness = 8;

        return thickness;
    }

    public static uint ComputePixelIndexWithinMicroTile(uint x, uint y, uint bpp, uint tileMode)
    {
        uint z = 0;
        uint thickness;
        uint pixelBit8;
        uint pixelBit7;
        uint pixelBit6;
        uint pixelBit5;
        uint pixelBit4;
        uint pixelBit3;
        uint pixelBit2;
        uint pixelBit1;
        uint pixelBit0;
        pixelBit6 = 0;
        pixelBit7 = 0;
        pixelBit8 = 0;
        thickness = ComputeSurfaceThickness(tileMode);

        if (bpp == 0x08)
        {
            pixelBit0 = x & 1;
            pixelBit1 = (x & 2) >> 1;
            pixelBit2 = (x & 4) >> 2;
            pixelBit3 = (y & 2) >> 1;
            pixelBit4 = y & 1;
            pixelBit5 = (y & 4) >> 2;
        }

        else if (bpp == 0x10)
        {
            pixelBit0 = x & 1;
            pixelBit1 = (x & 2) >> 1;
            pixelBit2 = (x & 4) >> 2;
            pixelBit3 = y & 1;
            pixelBit4 = (y & 2) >> 1;
            pixelBit5 = (y & 4) >> 2;
        }

        else if (bpp == 0x20 || bpp == 0x60)
        {
            pixelBit0 = x & 1;
            pixelBit1 = (x & 2) >> 1;
            pixelBit2 = y & 1;
            pixelBit3 = (x & 4) >> 2;
            pixelBit4 = (y & 2) >> 1;
            pixelBit5 = (y & 4) >> 2;
        }

        else if (bpp == 0x40)
        {
            pixelBit0 = x & 1;
            pixelBit1 = y & 1;
            pixelBit2 = (x & 2) >> 1;
            pixelBit3 = (x & 4) >> 2;
            pixelBit4 = (y & 2) >> 1;
            pixelBit5 = (y & 4) >> 2;
        }

        else if (bpp == 0x80)
        {
            pixelBit0 = y & 1;
            pixelBit1 = x & 1;
            pixelBit2 = (x & 2) >> 1;
            pixelBit3 = (x & 4) >> 2;
            pixelBit4 = (y & 2) >> 1;
            pixelBit5 = (y & 4) >> 2;
        }

        else
        {
            pixelBit0 = x & 1;
            pixelBit1 = (x & 2) >> 1;
            pixelBit2 = y & 1;
            pixelBit3 = (x & 4) >> 2;
            pixelBit4 = (y & 2) >> 1;
            pixelBit5 = (y & 4) >> 2;
        }

        if (thickness > 1)
        {
            pixelBit6 = z & 1;
            pixelBit7 = (z & 2) >> 1;
        }

        if (thickness == 8)
            pixelBit8 = (z & 4) >> 2;

        return ((pixelBit8 << 8) | (pixelBit7 << 7) | (pixelBit6 << 6) |
            32 * pixelBit5 | 16 * pixelBit4 | 8 * pixelBit3 |
            4 * pixelBit2 | pixelBit0 | 2 * pixelBit1);
    }

    public static uint ComputePipeFromCoordWoRotation(uint x, uint y)
    {
        // hardcoded to assume 2 pipes
        uint pipe = ((y >> 3) ^ (x >> 3)) & 1;
        return pipe;
    }

    public static uint ComputeBankFromCoordWoRotation(uint x, uint y)
    {
        uint numPipes = m_pipes;
        uint numBanks = m_banks;
        uint bankBit0;
        uint bankBit0a;
        uint bank = 0;

        if (numBanks == 4)
        {
            bankBit0 = ((y / (16 * numPipes)) ^ (x >> 3)) & 1;
            bank = bankBit0 | 2 * (((y / (8 * numPipes)) ^ (x >> 4)) & 1);
        }

        else if (numBanks == 8)
        {
            bankBit0a = ((y / (32 * numPipes)) ^ (x >> 3)) & 1;
            bank = (bankBit0a | 2 * (((y / (32 * numPipes)) ^ (y / (16 * numPipes) ^ (x >> 4))) & 1) |
                4 * (((y / (8 * numPipes)) ^ (x >> 5)) & 1));
        }

        return bank;
    }

    public static uint IsThickMacroTiled(uint tileMode)
    {
        uint thickMacroTiled = 0;

        if (tileMode == 7 || tileMode == 11 || tileMode == 13 || tileMode == 15)
            thickMacroTiled = 1;

        return thickMacroTiled;
    }

    public static uint IsBankSwappedTileMode(uint tileMode)
    {
        uint bankSwapped = 0;

        if (tileMode == 8 || tileMode == 9 || tileMode == 10 || tileMode == 11 || tileMode == 14 || tileMode == 15)
            bankSwapped = 1;

        return bankSwapped;
    }

    public static uint ComputeMacroTileAspectRatio(uint tileMode)
    {
        uint ratio = 1;

        if (tileMode == 5 || tileMode == 9)
            ratio = 2;

        else if (tileMode == 6 || tileMode == 10)
            ratio = 4;

        return ratio;
    }

    public static uint ComputeSurfaceBankSwappedWidth(uint tileMode, uint bpp, uint pitch)
    {
        if (IsBankSwappedTileMode(tileMode) == 0)
            return 0;

        uint numSamples = 1;
        uint numBanks = m_banks;
        uint numPipes = m_pipes;
        uint swapSize = m_swapSize;
        uint rowSize = m_rowSize;
        uint splitSize = m_splitSize;
        uint groupSize = m_pipeInterleaveBytes;
        uint bytesPerSample = 8 * bpp;

        uint samplesPerTile = splitSize / bytesPerSample;
        uint slicesPerTile = Math.Max(1, numSamples / samplesPerTile);

        if (IsThickMacroTiled(tileMode) != 0)
            numSamples = 4;

        uint bytesPerTileSlice = numSamples * bytesPerSample / slicesPerTile;

        uint factor = ComputeMacroTileAspectRatio(tileMode);
        uint swapTiles = Math.Max(1, (swapSize >> 1) / bpp);

        uint swapWidth = swapTiles * 8 * numBanks;
        uint heightBytes = numSamples * factor * numPipes * bpp / slicesPerTile;
        uint swapMax = numPipes * numBanks * rowSize / heightBytes;
        uint swapMin = groupSize * 8 * numBanks / bytesPerTileSlice;

        uint bankSwapWidth = Math.Min(swapMax, Math.Max(swapMin, swapWidth));

        while (bankSwapWidth >= (2 * pitch))
            bankSwapWidth >>= 1;

        return bankSwapWidth;
    }

    public static ulong ComputeSurfaceAddrFromCoordLinear(uint x, uint y, uint bpp, uint pitch)
    {
        uint rowOffset = y * pitch;
        uint pixOffset = x;

        ulong addr = (rowOffset + pixOffset) * bpp;
        addr /= 8;

        return addr;
    }

    public static ulong ComputeSurfaceAddrFromCoordMicroTiled(uint x, uint y, uint bpp, uint pitch, uint tileMode)
    {
        ulong microTileThickness = 1;

        if (tileMode == 3)
            microTileThickness = 4;

        ulong microTileBytes = (MicroTilePixels * microTileThickness * bpp + 7) / 8;
        ulong microTilesPerRow = pitch >> 3;
        ulong microTileIndexX = x >> 3;
        ulong microTileIndexY = y >> 3;

        ulong microTileOffset = microTileBytes * (microTileIndexX + microTileIndexY * microTilesPerRow);

        ulong pixelIndex = ComputePixelIndexWithinMicroTile(x, y, bpp, tileMode);

        ulong pixelOffset = bpp * pixelIndex;

        pixelOffset >>= 3;

        return pixelOffset + microTileOffset;
    }

    public static ulong ComputeSurfaceAddrFromCoordMacroTiled(uint x, uint y, uint bpp, uint pitch, uint height, uint tileMode, uint pipeSwizzle, uint bankSwizzle)
    {
        ulong sampleSlice, numSamples;

        uint numPipes = m_pipes;
        uint numBanks = m_banks;
        uint numGroupBits = m_pipeInterleaveBytesBitcount;
        uint numPipeBits = m_pipesBitcount;
        uint numBankBits = m_banksBitcount;

        uint microTileThickness = ComputeSurfaceThickness(tileMode);

        ulong microTileBits = bpp * (microTileThickness * MicroTilePixels);
        ulong microTileBytes = (microTileBits + 7) / 8;

        ulong pixelIndex = ComputePixelIndexWithinMicroTile(x, y, bpp, tileMode);

        ulong pixelOffset = bpp * pixelIndex;

        ulong elemOffset = pixelOffset;

        ulong bytesPerSample = microTileBytes;
        if (microTileBytes <= m_splitSize)
        {
            numSamples = 1;
            sampleSlice = 0;
        }

        else
        {
            ulong samplesPerSlice = m_splitSize / bytesPerSample;
            ulong numSampleSplits = Math.Max(1, 1 / samplesPerSlice);
            numSamples = samplesPerSlice;
            sampleSlice = elemOffset / (microTileBits / numSampleSplits);
            elemOffset %= microTileBits / numSampleSplits;
        }

        ulong pixelSlice = pixelIndex / numSamples;

        ulong pipeBits = 0, bankBits = 0, groupBits = 0;

        for (int i = 0; i < numGroupBits; i++)
        {
            groupBits |= (pixelSlice & 1) << i;
            pixelSlice >>= 1;
        }

        for (int i = 0; i < numPipeBits; i++)
        {
            pipeBits |= (pixelIndex & 1) << i;
            pixelIndex >>= 1;
        }

        for (int i = 0; i < numBankBits; i++)
        {
            bankBits |= (pixelIndex & 1) << i;
            pixelIndex >>= 1;
        }

        ulong tileIndexX = x >> 3;
        ulong tileIndexY = y >> 3;

        ulong macroTileBits = microTileBits * 64;

        ulong macroTileBytes = (macroTileBits + 7) / 8;

        ulong macroTilesPerRow = pitch >> 6;

        ulong macroTileOffset = macroTileBytes * (tileIndexX + tileIndexY * macroTilesPerRow);

        ulong pipeOffset = (pipeBits << (int)numBankBits) | bankBits;

        ulong bankOffset = pipeOffset << (int)numGroupBits;

        ulong tileOffset = bankOffset << (int)numPipeBits;

        ulong macroTileIndexX = tileIndexX >> 3;
        ulong macroTileIndexY = tileIndexY >> 3;

        ulong macroTileSwappedX = macroTileIndexX;
        ulong macroTileSwappedY = macroTileIndexY;

        if (IsBankSwappedTileMode(tileMode) != 0)
        {
            ulong groupBytes = m_pipeInterleaveBytes * 8 * numBanks;
            ulong groupOffset = groupBits * 8;
            ulong macroGroupOffset = ((macroTileSwappedY * numPipes + macroTileSwappedX) * bpp * MicroTilePixels * microTileThickness + groupOffset) >> 3;

            ulong macroPipeBits = (ulong)pipeSwizzle << (int)numBankBits;
            ulong macroBankBits = (ulong)bankSwizzle << (int)numPipeBits;

            ulong macroPipeOffset = (macroPipeBits << (int)numBankBits) | macroBankBits;

            ulong macroBankOffset = macroPipeOffset << (int)numGroupBits;

            ulong macroOffset = macroBankOffset << (int)numPipeBits;

            ulong macroTileOffsetTmp = macroOffset + macroGroupOffset;

            macroTileOffset += macroTileOffsetTmp;
        }

        ulong elemOffsetTmp = elemOffset / numSamples;

        elemOffset = (elemOffset % numSamples) + sampleSlice * bytesPerSample;

        ulong addr = elemOffset + macroTileOffset;

        return addr;
    }

    public static ulong ComputeSurfaceAddrFromCoord(uint x, uint y, uint bpp, uint pitch, uint height, uint tileMode, uint microTileMode, uint pipeSwizzle, uint bankSwizzle)
    {
        if (tileMode == 0)
            return ComputeSurfaceAddrFromCoordLinear(x, y, bpp, pitch);

        else if (tileMode == 1)
            return ComputeSurfaceAddrFromCoordMicroTiled(x, y, bpp, pitch, microTileMode);

        else if (tileMode == 2 || tileMode == 18)
            return ComputeSurfaceAddrFromCoordMacroTiled(x, y, bpp, pitch, height, tileMode, pipeSwizzle, bankSwizzle);

        else
            return 0;
    }

     public static byte GetPixelData(uint x, uint y, uint bpp, uint pitch, uint height, uint tileMode, uint microTileMode, uint pipeSwizzle, uint bankSwizzle, byte[] imageData)
    {
        // Calculate the memory address corresponding to the given pixel coordinates
        ulong address = ComputeSurfaceAddrFromCoord(x, y, bpp, pitch, height, tileMode, microTileMode, pipeSwizzle, bankSwizzle);

        // Access the memory at the calculated address to retrieve the pixel data
        // Note: You need to ensure that the imageData array contains the swizzled pixel data
        byte pixelData = imageData[address];

        return pixelData;
    }
}