using System;

public class UTEX
{
    private static byte[] _arr16 = new byte[16];
    private static byte[] _int8 = new byte[4];
    private static uint[] _int = new uint[1];

    public static uint readUintLE(byte[] buff, int p)
    {
        _int8[0] = buff[p + 0];
        _int8[1] = buff[p + 1];
        _int8[2] = buff[p + 2];
        _int8[3] = buff[p + 3];
        return _int[0];
    }

    public static void writeUintLE(byte[] buff, int p, uint n)
    {
        _int[0] = n;
        buff[p + 0] = _int8[0];
        buff[p + 1] = _int8[1];
        buff[p + 2] = _int8[2];
        buff[p + 3] = _int8[3];
    }

    public static string readASCII(byte[] buff, int p, int l)
    {
        char[] chars = new char[l];
        for (int i = 0; i < l; i++)
        {
            chars[i] = (char)buff[p + i];
        }
        return new string(chars);
    }

    public static void writeASCII(byte[] buff, int p, string s)
    {
        for (int i = 0; i < s.Length; i++)
        {
            buff[p + i] = (byte)s[i];
        }
    }

    public static byte[] readATC(byte[] data, int offset, byte[] img, int w, int h)
    {
        int posBoff = offset * 8;
        byte[] sqr = new byte[4 * 4 * 4];

        for (int y = 0; y < h; y += 4)
        {
            for (int x = 0; x < w; x += 4)
            {
                readATCcolor(data, offset, sqr);
                write4x4(img, w, h, x, y, sqr);
                offset += 8;
            }
        }
        return img;
    }

    public static byte[] readATA(byte[] data, int offset, byte[] img, int w, int h)
    {
        int posBoff = offset * 8;
        byte[] sqr = new byte[4 * 4 * 4];

        for (int y = 0; y < h; y += 4)
        {
            for (int x = 0; x < w; x += 4)
            {
                readATCcolor(data, offset + 8, sqr);
                write4x4(img, w, h, x, y, sqr);
                offset += 16;
                posBoff += 64;
            }
        }
        return img;
    }

    public static void readATCcolor(byte[] data, int offset, byte[] sqr)
    {
        ushort c0 = (ushort)((data[offset + 1] << 8) | data[offset]);
        ushort c1 = (ushort)((data[offset + 3] << 8) | data[offset + 2]);

        float c0b = (c0 & 31) * (255f / 31);
        float c0g = ((c0 >> 5) & 31) * (255f / 31);
        float c0r = (c0 >> 10) * (255f / 31);

        float c1b = (c1 & 31) * (255f / 31);
        float c1g = ((c1 >> 5) & 31) * (255f / 31);
        float c1r = (c1 >> 10) * (255f / 31);

        byte[] clr = _arr16;
        clr[0] = (byte)(c0r); clr[1] = (byte)(c0g); clr[2] = (byte)(c0b); clr[3] = 255;
        clr[12] = (byte)(c1r); clr[13] = (byte)(c1g); clr[14] = (byte)(c1b); clr[15] = 255;

        float fr = 2f / 3f, ifr = 1f - fr;
        clr[4] = (byte)(fr * c0r + ifr * c1r); clr[5] = (byte)(fr * c0g + ifr * c1g); clr[6] = (byte)(fr * c0b + ifr * c1b); clr[7] = 255;
        fr = 1f / 3f; ifr = 1f - fr;
        clr[8] = (byte)(fr * c0r + ifr * c1r); clr[9] = (byte)(fr * c0g + ifr * c1g); clr[10] = (byte)(fr * c0b + ifr * c1b); clr[11] = 255;

        toSquare(data, sqr, clr, offset);
    }

    public static void toSquare(byte[] data, byte[] sqr, byte[] clr, int offset)
    {
        int boff = (offset + 4) << 3;
        for (int i = 0; i < 64; i += 4)
        {
            int code = ((data[boff >> 3] >> (boff & 7)) & 3);
            boff += 2;
            code = (code << 2);
            sqr[i] = clr[code];
            sqr[i + 1] = clr[code + 1];
            sqr[i + 2] = clr[code + 2];
            sqr[i + 3] = clr[code + 3];
        }
    }

    public static void write4x4(byte[] a, int w, int h, int sx, int sy, byte[] b)
    {
        for (int y = 0; y < 4; y++)
        {
            int si = ((sy + y) * w + sx) << 2;
            int ti = y << 4;
            a[si + 0] = b[ti + 0]; a[si + 1] = b[ti + 1]; a[si + 2] = b[ti + 2]; a[si + 3] = b[ti + 3];
            a[si + 4] = b[ti + 4]; a[si + 5] = b[ti + 5]; a[si + 6] = b[ti + 6]; a[si + 7] = b[ti + 7];
            a[si + 8] = b[ti + 8]; a[si + 9] = b[ti + 9]; a[si + 10] = b[ti + 10]; a[si + 11] = b[ti + 11];
            a[si + 12] = b[ti + 12]; a[si + 13] = b[ti + 13]; a[si + 14] = b[ti + 14]; a[si + 15] = b[ti + 15];
        }
    }
    // Other methods like readBC1, readBC2, etc. would be translated similarly
}