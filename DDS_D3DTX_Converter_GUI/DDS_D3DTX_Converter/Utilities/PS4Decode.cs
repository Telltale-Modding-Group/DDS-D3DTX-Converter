using System;
using DirectXTexNet;

public static class PS4TextureDecoder
{

    public static byte[] UnswizzlePS4(byte[] pixelData, DXGI_FORMAT format, int width, int height)
    {
        int blockSize = TexHelper.Instance.IsCompressed(format) ? 4 : 1;

        int num5 = TexHelper.Instance.BitsPerPixel(format) * 2;

        if (blockSize == 1)
        {
            num5 = TexHelper.Instance.BitsPerPixel(format) / 8;
        }

        int length = width * height * TexHelper.Instance.BitsPerPixel(format) / 8;

        byte[] destArr = new byte[length * 2];
        byte[] array2 = new byte[16];
        int num6 = width / blockSize;
        int num7 = height / blockSize;

        int offset = 0;

        for (int i = 0; i < (num6 + 7) / 8; i++)
        {
            for (int j = 0; j < (num7 + 7) / 8; j++)
            {
                for (int k = 0; k < 64; k++)
                {
                    int num8 = morton(k, 8, 8);
                    int num9 = num8 / 8;
                    int num10 = num8 % 8;

                    for (int h = 0; h < num5; h++)
                    {
                        array2[h] = pixelData[offset + h];
                    }

                    offset += num5;

                    if (j * 8 + num10 < num7 && i * 8 + num9 < num6)
                    {
                        int destinationIndex = num5 * ((i * 8 + num9) * num7 + j * 8 + num10);
                        Array.Copy(array2, 0, destArr, destinationIndex, num5);
                    }
                }
            }
        }

        return destArr;
    }

    private static int morton(int t, int sx, int sy)
    {
        int num3;
        int num4 = num3 = 1;
        int num5 = t;
        int num6 = sx;
        int num7 = sy;
        int num = 0;
        int num2 = 0;
        while (num6 > 1 || num7 > 1)
        {
            if (num6 > 1)
            {
                num += num4 * (num5 & 1);
                num5 >>= 1;
                num4 *= 2;
                num6 >>= 1;
            }
            if (num7 > 1)
            {
                num2 += num3 * (num5 & 1);
                num5 >>= 1;
                num3 *= 2;
                num7 >>= 1;
            }
        }
        return num2 * sx + num;
    }

    // public static byte[] UnswizzlePS3(byte[] pixelData, DXGI_FORMAT format, int width, int height)
    // {
    //     byte[] array3 = new byte[num2 * 4];
    //     byte[] array4 = new byte[16];
    //     int num11 = sy / num4;
    //     int num12 = sx / num4;
    //     for (int l = 0; l < num12 * num11; l++)
    //     {
    //         int num13 = morton(l, num12, num11);
    //         fileStream.Read(array4, 0, num5);
    //         int destinationIndex2 = num5 * num13;
    //         Array.Copy(array4, 0, array3, destinationIndex2, num5);
    //     }
    //     fileStream2.Write(array3, 0, (int)num2);
    // }

}


