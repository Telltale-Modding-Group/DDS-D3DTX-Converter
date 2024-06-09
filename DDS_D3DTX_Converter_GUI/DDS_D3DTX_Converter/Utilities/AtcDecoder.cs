using Avalonia.Media.Imaging;
using System;
using System.Runtime.CompilerServices;


// Credits: https://github.com/mafaca/Atc
namespace Decoders;

public class AtcDecoder
{
    public byte[] DecompressAtcRgb4(byte[] input, int width, int height)
    {
        byte[] output = new byte[width * height * 4];

        int bcw = (width + 3) / 4;
        int bch = (height + 3) / 4;
        int clen_last = (width + 3) % 4 + 1;
        int d = 0;
        for (int t = 0; t < bch; t++)
        {
            for (int s = 0; s < bcw; s++, d += 8)
            {
                DecodeAtcRgb4Block(input, d);
                int clen = (s < bcw - 1 ? 4 : clen_last) * 4;
                for (int i = 0, y = height - t * 4 - 1; i < 4 && y >= 0; i++, y--)
                {
                    System.Buffer.BlockCopy(m_buf, i * 4 * 4, output, y * 4 * width + s * 4 * 4, clen);
                }
            }
        }

        return output;
    }

    public byte[] DecompressAtcRgba8(byte[] input, int width, int height)
    {
        byte[] output = new byte[width * height * 4];

        int bcw = (width + 3) / 4;
        int bch = (height + 3) / 4;
        int clen_last = (width + 3) % 4 + 1;
        int d = 0;
        for (int t = 0; t < bch; t++)
        {
            for (int s = 0; s < bcw; s++, d += 16)
            {
                DecodeAtcRgba8Block(input, d);
                int clen = (s < bcw - 1 ? 4 : clen_last) * 4;
                for (int i = 0, y = height - t * 4 - 1; i < 4 && y >= 0; i++, y--)
                {
                    System.Buffer.BlockCopy(m_buf, i * 4 * 4, output, y * 4 * width + s * 4 * 4, clen);
                }
            }
        }

        return output;
    }

    private void DecodeAtcRgb4Block(byte[] data, int offset)
    {
        int c0 = BitConverter.ToUInt16(data, offset);
        int c1 = BitConverter.ToUInt16(data, offset + 2);
        uint cindex = BitConverter.ToUInt32(data, offset + 4);

        DecodeColors(c0, c1);

        for (int y = 0; y < 4; y++)
        {
            for (int x = 0; x < 4; x++)
            {
                uint cidx = cindex & 3;
                int cb = m_colors[cidx * 4 + 0];
                int cg = m_colors[cidx * 4 + 1];
                int cr = m_colors[cidx * 4 + 2];
                m_buf[y * 4 + x] = Color(cr, cg, cb, 255);
                cindex >>= 2;
            }
        }
    }

    void DecodeAtcRgba8Block(byte[] data, int offset)
    {
        ulong avalue = BitConverter.ToUInt64(data, offset);
        int a0 = unchecked((int)(avalue >> 0) & 0xFF);
        int a1 = unchecked((int)(avalue >> 8) & 0xFF);
        ulong aindex = avalue >> 16;
        int c0 = BitConverter.ToUInt16(data, offset + 8);
        int c1 = BitConverter.ToUInt16(data, offset + 10);
        uint cindex = BitConverter.ToUInt32(data, offset + 12);

        DecodeColors(c0, c1);
        DecodeAlphas(a0, a1);

        for (int y = 0; y < 4; y++)
        {
            for (int x = 0; x < 4; x++)
            {
                int cidx = unchecked((int)cindex & 3);
                int cb = m_colors[cidx * 4 + 0];
                int cg = m_colors[cidx * 4 + 1];
                int cr = m_colors[cidx * 4 + 2];
                int aidx = unchecked((int)aindex & 7);
                int ca = m_alphas[aidx];
                m_buf[y * 4 + x] = Color(cr, cg, cb, ca);
                cindex >>= 2;
                aindex >>= 3;
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void DecodeColors(int c0, int c1)
    {
        if ((c0 & 0x8000) == 0)
        {
            m_colors[0] = Extend((c0 >> 0) & 0x1F, 5, 8);
            m_colors[1] = Extend((c0 >> 5) & 0x1F, 5, 8);
            m_colors[2] = Extend((c0 >> 10) & 0x1F, 5, 8);

            m_colors[12] = Extend((c1 >> 0) & 0x1F, 5, 8);
            m_colors[13] = Extend((c1 >> 5) & 0x3f, 6, 8);
            m_colors[14] = Extend((c1 >> 11) & 0x1F, 5, 8);

            m_colors[4] = (5 * m_colors[0] + 3 * m_colors[12]) / 8;
            m_colors[5] = (5 * m_colors[1] + 3 * m_colors[13]) / 8;
            m_colors[6] = (5 * m_colors[2] + 3 * m_colors[14]) / 8;

            m_colors[8] = (3 * m_colors[0] + 5 * m_colors[12]) / 8;
            m_colors[9] = (3 * m_colors[1] + 5 * m_colors[13]) / 8;
            m_colors[10] = (3 * m_colors[2] + 5 * m_colors[14]) / 8;
        }
        else
        {
            m_colors[0] = 0;
            m_colors[1] = 0;
            m_colors[2] = 0;

            m_colors[8] = Extend((c0 >> 0) & 0x1F, 5, 8);
            m_colors[9] = Extend((c0 >> 5) & 0x1F, 5, 8);
            m_colors[10] = Extend((c0 >> 10) & 0x1F, 5, 8);

            m_colors[12] = Extend((c1 >> 0) & 0x1F, 5, 8);
            m_colors[13] = Extend((c1 >> 5) & 0x3F, 6, 8);
            m_colors[14] = Extend((c1 >> 11) & 0x1F, 5, 8);

            m_colors[4] = Math.Max(0, m_colors[8] - m_colors[12] / 4);
            m_colors[5] = Math.Max(0, m_colors[9] - m_colors[13] / 4);
            m_colors[6] = Math.Max(0, m_colors[10] - m_colors[14] / 4);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void DecodeAlphas(int a0, int a1)
    {
        m_alphas[0] = a0;
        m_alphas[1] = a1;
        if (a0 > a1)
        {
            m_alphas[2] = (a0 * 6 + a1 * 1) / 7;
            m_alphas[3] = (a0 * 5 + a1 * 2) / 7;
            m_alphas[4] = (a0 * 4 + a1 * 3) / 7;
            m_alphas[5] = (a0 * 3 + a1 * 4) / 7;
            m_alphas[6] = (a0 * 2 + a1 * 5) / 7;
            m_alphas[7] = (a0 * 1 + a1 * 6) / 7;
        }
        else
        {
            m_alphas[2] = (a0 * 4 + a1 * 1) / 5;
            m_alphas[3] = (a0 * 3 + a1 * 2) / 5;
            m_alphas[4] = (a0 * 2 + a1 * 3) / 5;
            m_alphas[5] = (a0 * 1 + a1 * 4) / 5;
            m_alphas[6] = 0;
            m_alphas[7] = 255;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int Extend(int value, int from, int to)
    {
        // bit-pattern replicating scaling (can at most double the bits)
        return (value << (to - from)) | (value >> (from * 2 - to));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint Color(int r, int g, int b, int a)
    {
        return unchecked((uint)(r << 16 | g << 8 | b | a << 24));
    }

    private readonly uint[] m_buf = new uint[16];
    private readonly int[] m_colors = new int[16];
    private readonly int[] m_alphas = new int[16];


    //public static Bitmap DecodeATC(byte[] atcData, int width, int height)
    //{
    //    // Create an OpenGL context using OpenTK
    

    //        // Generate a texture handle
    //        int textureHandle = GL.GenTexture();

    //        // Bind the texture
    //        GL.BindTexture(TextureTarget.Texture2D, textureHandle);

    //        // Load the ATC data into the texture
    //        GL.CompressedTexImage2D(TextureTarget.Texture2D, 0, (InternalFormat)All.ATC, width, height, 0, atcData.Length, atcData);

    //        // Set texture parameters if needed
    //        // For example:
    //        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
    //        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

    //        // Allocate memory for decompressed image
    //        byte[] rgbaData = new byte[width * height * 4];

    //        // Read the decompressed image data from OpenGL
    //        GL.GetTexImage(TextureTarget.Texture2D, 0, PixelFormat.Rgba, PixelType.UnsignedByte, rgbaData);

    //        // Create a Bitmap from the RGBA data
    //        Bitmap rgbaBitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
    //        BitmapData bmpData = rgbaBitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, rgbaBitmap.PixelFormat);
    //        System.Runtime.InteropServices.Marshal.Copy(rgbaData, 0, bmpData.Scan0, rgbaData.Length);
    //        rgbaBitmap.UnlockBits(bmpData);

    //        return rgbaBitmap;
        
    //}
}

