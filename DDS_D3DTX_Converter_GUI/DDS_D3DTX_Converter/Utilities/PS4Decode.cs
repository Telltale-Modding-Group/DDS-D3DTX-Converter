using System;
using DirectXTexNet;

public static class PS4TextureDecoder
{

    public static byte[] UnswizzlePS4(byte[] pixelData, DXGI_FORMAT format, int width, int height)
    {
        int blockSize = TexHelper.Instance.IsCompressed(format) ? 4 : 1; //CORRECT

        int count = TexHelper.Instance.BitsPerPixel(format) * 2; //CORRECT

        if (blockSize == 1) //CORRECT
        {
            count = TexHelper.Instance.BitsPerPixel(format) / 8;
        }

        long length = width * height * TexHelper.Instance.BitsPerPixel(format) / 8; //CORRECT


        byte[] destinationArray = new byte[length * 2L];
        byte[] buffer = new byte[0x10];
        int num7 = height / blockSize;
        int num8 = width / blockSize;


        int num10 = 0; //CORRECT

        long offset = 0;

        for (int i = 0; i < ((num7 + 7) / 8); i++)
        {
            for (int j = 0; j < ((num8 + 7) / 8); j++)
            {
                for (int k = 0; k < 64; k++)
                {
                    int num13 = morton(k, 8, 8);
                    int num14 = num13 / 8;
                    int num15 = num13 % 8;

                    for (int f = 0; f < count; f++)
                    {
                        buffer[f] = pixelData[offset + f];
                    }

                    offset += count;

                    if ((((j * 8) + num15) < num8) && (((i * 8) + num14) < num7))
                    {
                        int destinationIndex = count * (((((i * 8) + num14) * num8) + (j * 8)) + num15);
                        Array.Copy(buffer, 0, destinationArray, destinationIndex, count);
                    }
                }
            }
        }
        return destinationArray;
    }

    private static int morton(int t, int sx, int sy)
    {
        int num4;
        int num = 0;
        int num2 = 0;
        int num3 = num4 = 1;
        int num7 = t;
        int num5 = sx;
        int num6 = sy;
        num = 0;
        num2 = 0;
        while ((num5 > 1) || (num6 > 1))
        {
            if (num5 > 1)
            {
                num += num3 * (num7 & 1);
                num7 = num7 >> 1;
                num3 *= 2;
                num5 = num5 >> 1;
            }
            if (num6 > 1)
            {
                num2 += num4 * (num7 & 1);
                num7 = num7 >> 1;
                num4 *= 2;
                num6 = num6 >> 1;
            }
        }
        return ((num2 * sx) + num);

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


