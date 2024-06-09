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

        byte[] buffer = new byte[length * 2]; //CORRECT
        byte[] buffer2 = new byte[16]; //CORRECT
        int num7 = width / blockSize; //CORRECT
        int num8 = height / blockSize; //CORRECT

        int num10 = 0; //CORRECT

        long offset = 0;
        while (true)
        {
            if (num10 >= ((num7 + 7) / 8))
            {
                return buffer;
            }
            int num11 = 0;
            while (true)
            {
                if (num11 >= ((num8 + 7) / 8))
                {
                    num10++;
                    break;
                }
                int t = 0;
                while (true)
                {
                    if (t >= 64)
                    {
                        num11++;
                        break;
                    }
                    int num13 = morton(t, 8, 8);
                    int num14 = num13 / 8;
                    int num15 = num13 % 8;

                    for (int i = 0; i < count; i++)
                    {
                        buffer2[i] = pixelData[offset + i];
                    }

                    offset += count;

                    if ((((num11 * 8) + num15) < num8) && (((num10 * 8) + num14) < num7))
                    {
                        Array.Copy(buffer2, 0, buffer, count * ((((num10 * 8) + num14) * num8) + (num11 * 8) + num15), count);
                    }
                    t++;
                }
            }
        }

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


