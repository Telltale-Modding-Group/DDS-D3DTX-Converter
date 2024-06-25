using System;
using D3DTX_Converter.DirectX.Enums;
using Hexa.NET.DirectXTex;

public static class PS4TextureDecoder
{

    public static byte[] UnswizzlePS4(byte[] pixelData, DXGIFormat format, int width, int height)
    {
        int blockSize = DirectXTex.IsCompressed((int)format) ? 4 : 1; //CORRECT

        int count = (int)(DirectXTex.BitsPerPixel((int)format) * 2); //CORRECT

        if (blockSize == 1) //CORRECT
        {
            count = (int)(DirectXTex.BitsPerPixel((int)format) / 8);
        }

        long length = width * height * (int)DirectXTex.BitsPerPixel((int)format) / 8; //CORRECT

        byte[] destinationArray = new byte[length * 2L];
        byte[] buffer = new byte[0x10];
        int num7 = height / blockSize;
        int num8 = width / blockSize;

        long offset = 0;

        for (int i = 0; i < ((num7 + 7) / 8); i++)
        {
            for (int j = 0; j < ((num8 + 7) / 8); j++)
            {
                for (int k = 0; k < 64; k++)
                {
                    int num13 = Morton(k, 8, 8);
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

    private static int Morton(int t, int sx, int sy)
    {
        int num1;
        int num2 = num1 = 1;
        int num3 = t;
        int num4 = sx;
        int num5 = sy;
        int num6 = 0;
        int num7 = 0;

        while (num4 > 1 || num5 > 1)
        {
            if (num4 > 1)
            {
                num6 += num2 * (num3 & 1);
                num3 >>= 1;
                num2 *= 2;
                num4 >>= 1;
            }
            if (num5 > 1)
            {
                num7 += num1 * (num3 & 1);
                num3 >>= 1;
                num1 *= 2;
                num5 >>= 1;
            }
        }

        return num7 * sx + num6;
    }

    public static byte[] UnswizzlePS3(byte[] pixelData, DXGIFormat format, int width, int height)
    {

        if (!DirectXTex.IsCompressed((int)format))
        {
            int count = (int)(DirectXTex.BitsPerPixel((int)format) / 8);

            long length = width * height * (int)DirectXTex.BitsPerPixel((int)format) / 8; //CORRECT

            byte[] destinationArray = new byte[length * 4L];
            byte[] buffer = new byte[0x10];
            int sy = height;
            int sx = width;

            long offset = 0;

            for (int i = 0; i < (sx * sy); i++)
            {
                int num40 = Morton(i, sx, sy);

                for (int f = 0; f < count; f++)
                {
                    buffer[f] = pixelData[offset + f];
                }

                offset += count;

                int destinationIndex = count * num40;
                Array.Copy(buffer, 0, destinationArray, destinationIndex, count);
            }

            return destinationArray;
        }

        return pixelData;
    }

    // public static byte[] UnswizzlePS3(byte[] data, DXGI_FORMAT format, int width, int height)
    // {
    //     int blockSize = TexHelper.Instance.IsCompressed(format) ? 4 : 1; //CORRECT

    //     // if (TexHelper.Instance.IsCompressed(format))
    //     // {

    //     // }
    //     if (format == DXGI_FORMAT.BC1_UNORM || format == DXGI_FORMAT.BC4_UNORM)
    //     {
    //         blockSize = 8;
    //     }
    //     else
    //     {
    //         blockSize = 16;
    //     }

    //     if (!TexHelper.Instance.IsCompressed(format))
    //     {
    //         blockSize = TexHelper.Instance.BitsPerPixel(format) / 8;
    //     }
    //     blockSize = 1;

    //     var unswizzled = new byte[data.Length];
    //     var dataIndex = 0;
    //     int heightTexels = height / 4;
    //     int widthTexels = width / 4;
    //     var texelCount = widthTexels * heightTexels;

    //     for (int texel = 0; texel < texelCount; ++texel)
    //     {
    //         int pixelIndex = Morton(texel, widthTexels, heightTexels);
    //         int destIndex = blockSize * pixelIndex;
    //         Array.Copy(data, dataIndex, unswizzled, destIndex, blockSize);
    //         dataIndex += blockSize;
    //     }

    //     return unswizzled;
    // }
}

// ```private void DoConvert()
// {
//     if (this.basename != "")
//     {
//         FileStream input = new FileStream(this.fname, FileMode.Open, FileAccess.Read);
//         new BinaryReader(input);
//         FileStream output = new FileStream(this.basedir + this.basename + ".dds", FileMode.Create);
//         BinaryWriter writer = new BinaryWriter(output);
//         long offset = 0L;
//         try
//         {
//             offset = Convert.ToInt64(this.tbOffset.Text, 0x10);
//         }
//         catch
//         {
//         }
//         input.Seek(offset, SeekOrigin.Begin);
//         long num2 = input.Length - offset;
//         if (num2 < 0L)
//         {
//             num2 = 0L;
//         }
//         int num3 = 1;
//         int num4 = 0x3031_5844;
//         int num5 = pixbl[this.ddDXGI.SelectedIndex];
//         int count = bpp[this.ddDXGI.SelectedIndex] * 2;
//         if (num5 == 1)
//         {
//             count = bpp[this.ddDXGI.SelectedIndex] / 8;
//         }
//         if (this.ddDXGI.SelectedIndex == 0x47)
//         {
//             num4 = 0x3154_5844;
//         }
//         if (this.ddDXGI.SelectedIndex == 0x4a)
//         {
//             num4 = 0x3354_5844;
//         }
//         if (this.ddDXGI.SelectedIndex == 0x4d)
//         {
//             num4 = 0x3554_5844;
//         }
//         if (this.ddDXGI.SelectedIndex == 80)
//         {
//             num4 = 0x3149_5441;
//         }
//         if (this.ddDXGI.SelectedIndex == 0x53)
//         {
//             num4 = 0x3249_5441;
//         }
//         num2 = ((this.sx * this.sy) * bpp[this.ddDXGI.SelectedIndex]) / 8;
//         writer.Write((long) 0x7c_2053_4444L);
//         writer.Write(0x1007);
//         writer.Write(this.sy);
//         writer.Write(this.sx);
//         writer.Write((int) num2);
//         writer.Write(0);
//         writer.Write(num3);
//         output.Seek(0x2cL, SeekOrigin.Current);
//         writer.Write(0x20);
//         writer.Write(4);
//         writer.Write(num4);
//         output.Seek(40L, SeekOrigin.Current);
//         if (num4 == 0x3031_5844)
//         {
//             writer.Write(this.ddDXGI.SelectedIndex);
//             writer.Write(3);
//             writer.Write(0);
//             writer.Write(1);
//             writer.Write(0);
//         }
//         if (this.cbPS4swiz.Checked)
//         {
//             byte[] destinationArray = new byte[num2 * 2L];
//             byte[] buffer = new byte[0x10];
//             int num7 = this.sy / num5;
//             int num8 = this.sx / num5;
//             for (int i = 0; i < ((num7 + 7) / 8); i++)
//             {
//                 for (int j = 0; j < ((num8 + 7) / 8); j++)
//                 {
//                     for (int k = 0; k < 0x40; k++)
//                     {
//                         int num13 = morton(k, 8, 8);
//                         int num14 = num13 / 8;
//                         int num15 = num13 % 8;
//                         input.Read(buffer, 0, count);
//                         if ((((j * 8) + num15) < num8) && (((i * 8) + num14) < num7))
//                         {
//                             int destinationIndex = count * (((((i * 8) + num14) * num8) + (j * 8)) + num15);
//                             Array.Copy(buffer, 0, destinationArray, destinationIndex, count);
//                         }
//                     }
//                 }
//             }
//             output.Write(destinationArray, 0, (int) num2);
//         }
//         else if (this.cbPS5swiz.Checked)
//         {
//             int num18;
//             int num19;
//             int num20;
//             int num21;
//             byte[] destinationArray = new byte[num2 * 2L];
//             byte[] buffer = new byte[0x40];
//             int num16 = this.sy / num5;
//             int num17 = this.sx / num5;
//             int num22 = 1;
//             switch (count)
//             {
//                 case 0x10:
//                     num22 = 1;
//                     break;

//                 case 8:
//                     num22 = 2;
//                     break;

//                 case 4:
//                     num22 = 4;
//                     break;
//             }
//             if (num5 == 1)
//             {
//                 for (int i = 0; i < ((num16 + 0x7f) / 0x80); i++)
//                 {
//                     for (int j = 0; j < ((num17 + 0x7f) / 0x80); j++)
//                     {
//                         for (int k = 0; k < 0x200; k++)
//                         {
//                             num19 = morton(k, 0x20, 0x10);
//                             int num26 = num19 % 0x20;
//                             int num27 = num19 / 0x20;
//                             for (int m = 0; m < 0x20; m++)
//                             {
//                                 input.Read(buffer, 0, count);
//                                 num20 = ((j * 0x80) + (num26 * 4)) + (m % 4);
//                                 num21 = (i * 0x80) + ((num27 * 8) + (m / 4));
//                                 if ((num20 < num17) && (num21 < num16))
//                                 {
//                                     num18 = count * ((num21 * num17) + num20);
//                                     Array.Copy(buffer, 0, destinationArray, num18, count);
//                                 }
//                             }
//                         }
//                     }
//                 }
//             }
//             else
//             {
//                 for (int i = 0; i < ((num16 + 0x3f) / 0x40); i++)
//                 {
//                     for (int j = 0; j < ((num17 + 0x3f) / 0x40); j++)
//                     {
//                         for (int k = 0; k < (0x100 / num22); k++)
//                         {
//                             num19 = morton(k, 0x10, 0x10 / num22);
//                             int num32 = num19 / 0x10;
//                             int num33 = num19 % 0x10;
//                             for (int m = 0; m < 0x10; m++)
//                             {
//                                 for (int n = 0; n < num22; n++)
//                                 {
//                                     input.Read(buffer, 0, count);
//                                     num20 = ((j * 0x40) + (((num32 * 4) + (m / 4)) * num22)) + n;
//                                     num21 = ((i * 0x40) + (num33 * 4)) + (m % 4);
//                                     if ((num20 < num17) && (num21 < num16))
//                                     {
//                                         num18 = count * ((num21 * num17) + num20);
//                                         Array.Copy(buffer, 0, destinationArray, num18, count);
//                                     }
//                                 }
//                             }
//                         }
//                     }
//                 }
//             }
//             output.Write(destinationArray, 0, (int) num2);
//         }
//         else if (this.cbPS3swiz.Checked)
//         {
//             byte[] destinationArray = new byte[num2 * 4L];
//             byte[] buffer = new byte[0x10];
//             int sy = this.sy / num5;
//             int sx = this.sx / num5;
//             for (int i = 0; i < (sx * sy); i++)
//             {
//                 int num40 = morton(i, sx, sy);
//                 input.Read(buffer, 0, count);
//                 int destinationIndex = count * num40;
//                 Array.Copy(buffer, 0, destinationArray, destinationIndex, count);
//             }
//             output.Write(destinationArray, 0, (int) num2);
//         }
//         else if (this.cbNINswiz.Checked)
//         {
//             byte[] destinationArray = new byte[num2 * 4L];
//             byte[] buffer = new byte[0x10];
//             int num42 = this.sy / num5;
//             int num43 = this.sx / num5;
//             int[,] numArray = new int[num43 * 2, num42 * 2];
//             int num44 = 0;
//             int num45 = 0;
//             int num48 = num42 / 8;
//             if (num48 > 0x10)
//             {
//                 num48 = 0x10;
//             }
//             int num49 = 0;
//             int num50 = 1;
//             switch (count)
//             {
//                 case 0x10:
//                     num50 = 1;
//                     break;

//                 case 8:
//                     num50 = 2;
//                     break;

//                 case 4:
//                     num50 = 4;
//                     break;
//             }
//             for (int i = 0; i < ((num42 / 8) / num48); i++)
//             {
//                 for (int j = 0; j < ((num43 / 4) / num50); j++)
//                 {
//                     for (int k = 0; k < num48; k++)
//                     {
//                         for (int m = 0; m < 0x20; m++)
//                         {
//                             for (int n = 0; n < num50; n++)
//                             {
//                                 int num56 = swi[m];
//                                 num45 = num56 / 4;
//                                 num44 = num56 % 4;
//                                 input.Read(buffer, 0, count);
//                                 int num47 = (((i * num48) + k) * 8) + num45;
//                                 int num46 = (((j * 4) + num44) * num50) + n;
//                                 int destinationIndex = count * ((num47 * num43) + num46);
//                                 Array.Copy(buffer, 0, destinationArray, destinationIndex, count);
//                                 numArray[num46, num47] = num49;
//                                 num49++;
//                             }
//                         }
//                     }
//                 }
//             }
//             output.Write(destinationArray, 0, (int) num2);
//         }
//         else
//         {
//             byte[] buffer = new byte[num2];
//             input.Read(buffer, 0, (int) num2);
//             output.Write(buffer, 0, (int) num2);
//         }
//         input.Close();
//         writer.Close();
//         output.Close();
//         this.pictureBox1.ImageLocation = "";
//         if (File.Exists(this.exepath + @"\texconv.exe"))
//         {
//             File.Delete(this.basedir + this.basename + ".png");
//             Process process = new Process {
//                 StartInfo = { 
//                     WindowStyle = ProcessWindowStyle.Hidden,
//                     Arguments = "-y -f R8G8B8A8_UNORM -ft png \"" + this.basedir + this.basename + ".dds\" -o \"" + this.basedir,
//                     FileName = this.exepath + @"\texconv.exe"
//                 }
//             };
//             process.Start();
//             process.WaitForExit();
//             this.pictureBox1.ImageLocation = this.basedir + this.basename + ".png";
//         }
//     }
// }




// private void DoConvert()
// {
//     if (this.basename != "")
//     {
//         FileStream input = new FileStream(this.fname, FileMode.Open, FileAccess.Read);
//         new BinaryReader(input);
//         FileStream output = new FileStream(this.basedir + this.basename + ".dds", FileMode.Create);
//         BinaryWriter writer = new BinaryWriter(output);
//         long offset = 0L;
//         try
//         {
//             offset = Convert.ToInt64(this.tbOffset.Text, 0x10);
//         }
//         catch
//         {
//         }
//         input.Seek(offset, SeekOrigin.Begin);
//         long num2 = input.Length - offset;
//         if (num2 < 0L)
//         {
//             num2 = 0L;
//         }
//         int num3 = 1;
//         int num4 = 0x3031_5844;
//         int num5 = pixbl[this.ddDXGI.SelectedIndex];
//         int count = bpp[this.ddDXGI.SelectedIndex] * 2;
//         if (num5 == 1)
//         {
//             count = bpp[this.ddDXGI.SelectedIndex] / 8;
//         }
//         if (this.ddDXGI.SelectedIndex == 0x47)
//         {
//             num4 = 0x3154_5844;
//         }
//         if (this.ddDXGI.SelectedIndex == 0x4a)
//         {
//             num4 = 0x3354_5844;
//         }
//         if (this.ddDXGI.SelectedIndex == 0x4d)
//         {
//             num4 = 0x3554_5844;
//         }
//         if (this.ddDXGI.SelectedIndex == 80)
//         {
//             num4 = 0x3149_5441;
//         }
//         if (this.ddDXGI.SelectedIndex == 0x53)
//         {
//             num4 = 0x3249_5441;
//         }
//         num2 = ((this.sx * this.sy) * bpp[this.ddDXGI.SelectedIndex]) / 8;
//         writer.Write((long) 0x7c_2053_4444L);
//         writer.Write(0x1007);
//         writer.Write(this.sy);
//         writer.Write(this.sx);
//         writer.Write((int) num2);
//         writer.Write(0);
//         writer.Write(num3);
//         output.Seek(0x2cL, SeekOrigin.Current);
//         writer.Write(0x20);
//         writer.Write(4);
//         writer.Write(num4);
//         output.Seek(40L, SeekOrigin.Current);
//         if (num4 == 0x3031_5844)
//         {
//             writer.Write(this.ddDXGI.SelectedIndex);
//             writer.Write(3);
//             writer.Write(0);
//             writer.Write(1);
//             writer.Write(0);
//         }
//         if (this.cbPS4swiz.Checked)
//         {
//             byte[] destinationArray = new byte[num2 * 2L];
//             byte[] buffer = new byte[0x10];
//             int num7 = this.sy / num5;
//             int num8 = this.sx / num5;
//             for (int i = 0; i < ((num7 + 7) / 8); i++)
//             {
//                 for (int j = 0; j < ((num8 + 7) / 8); j++)
//                 {
//                     for (int k = 0; k < 0x40; k++)
//                     {
//                         int num13 = morton(k, 8, 8);
//                         int num14 = num13 / 8;
//                         int num15 = num13 % 8;
//                         input.Read(buffer, 0, count);
//                         if ((((j * 8) + num15) < num8) && (((i * 8) + num14) < num7))
//                         {
//                             int destinationIndex = count * (((((i * 8) + num14) * num8) + (j * 8)) + num15);
//                             Array.Copy(buffer, 0, destinationArray, destinationIndex, count);
//                         }
//                     }
//                 }
//             }
//             output.Write(destinationArray, 0, (int) num2);
//         }
//         else if (this.cbPS5swiz.Checked)
//         {
//             int num18;
//             int num19;
//             int num20;
//             int num21;
//             byte[] destinationArray = new byte[num2 * 2L];
//             byte[] buffer = new byte[0x40];
//             int num16 = this.sy / num5;
//             int num17 = this.sx / num5;
//             int num22 = 1;
//             switch (count)
//             {
//                 case 0x10:
//                     num22 = 1;
//                     break;

//                 case 8:
//                     num22 = 2;
//                     break;

//                 case 4:
//                     num22 = 4;
//                     break;
//             }
//             if (num5 == 1)
//             {
//                 for (int i = 0; i < ((num16 + 0x7f) / 0x80); i++)
//                 {
//                     for (int j = 0; j < ((num17 + 0x7f) / 0x80); j++)
//                     {
//                         for (int k = 0; k < 0x200; k++)
//                         {
//                             num19 = morton(k, 0x20, 0x10);
//                             int num26 = num19 % 0x20;
//                             int num27 = num19 / 0x20;
//                             for (int m = 0; m < 0x20; m++)
//                             {
//                                 input.Read(buffer, 0, count);
//                                 num20 = ((j * 0x80) + (num26 * 4)) + (m % 4);
//                                 num21 = (i * 0x80) + ((num27 * 8) + (m / 4));
//                                 if ((num20 < num17) && (num21 < num16))
//                                 {
//                                     num18 = count * ((num21 * num17) + num20);
//                                     Array.Copy(buffer, 0, destinationArray, num18, count);
//                                 }
//                             }
//                         }
//                     }
//                 }
//             }
//             else
//             {
//                 for (int i = 0; i < ((num16 + 0x3f) / 0x40); i++)
//                 {
//                     for (int j = 0; j < ((num17 + 0x3f) / 0x40); j++)
//                     {
//                         for (int k = 0; k < (0x100 / num22); k++)
//                         {
//                             num19 = morton(k, 0x10, 0x10 / num22);
//                             int num32 = num19 / 0x10;
//                             int num33 = num19 % 0x10;
//                             for (int m = 0; m < 0x10; m++)
//                             {
//                                 for (int n = 0; n < num22; n++)
//                                 {
//                                     input.Read(buffer, 0, count);
//                                     num20 = ((j * 0x40) + (((num32 * 4) + (m / 4)) * num22)) + n;
//                                     num21 = ((i * 0x40) + (num33 * 4)) + (m % 4);
//                                     if ((num20 < num17) && (num21 < num16))
//                                     {
//                                         num18 = count * ((num21 * num17) + num20);
//                                         Array.Copy(buffer, 0, destinationArray, num18, count);
//                                     }
//                                 }
//                             }
//                         }
//                     }
//                 }
//             }
//             output.Write(destinationArray, 0, (int) num2);
//         }
//         else if (this.cbPS3swiz.Checked)
//         {
//             byte[] destinationArray = new byte[num2 * 4L];
//             byte[] buffer = new byte[0x10];
//             int sy = this.sy / num5;
//             int sx = this.sx / num5;
//             for (int i = 0; i < (sx * sy); i++)
//             {
//                 int num40 = morton(i, sx, sy);
//                 input.Read(buffer, 0, count);
//                 int destinationIndex = count * num40;
//                 Array.Copy(buffer, 0, destinationArray, destinationIndex, count);
//             }
//             output.Write(destinationArray, 0, (int) num2);
//         }
//         else if (this.cbNINswiz.Checked)
//         {
//             byte[] destinationArray = new byte[num2 * 4L];
//             byte[] buffer = new byte[0x10];
//             int num42 = this.sy / num5;
//             int num43 = this.sx / num5;
//             int[,] numArray = new int[num43 * 2, num42 * 2];
//             int num44 = 0;
//             int num45 = 0;
//             int num48 = num42 / 8;
//             if (num48 > 0x10)
//             {
//                 num48 = 0x10;
//             }
//             int num49 = 0;
//             int num50 = 1;
//             switch (count)
//             {
//                 case 0x10:
//                     num50 = 1;
//                     break;

//                 case 8:
//                     num50 = 2;
//                     break;

//                 case 4:
//                     num50 = 4;
//                     break;
//             }
//             for (int i = 0; i < ((num42 / 8) / num48); i++)
//             {
//                 for (int j = 0; j < ((num43 / 4) / num50); j++)
//                 {
//                     for (int k = 0; k < num48; k++)
//                     {
//                         for (int m = 0; m < 0x20; m++)
//                         {
//                             for (int n = 0; n < num50; n++)
//                             {
//                                 int num56 = swi[m];
//                                 num45 = num56 / 4;
//                                 num44 = num56 % 4;
//                                 input.Read(buffer, 0, count);
//                                 int num47 = (((i * num48) + k) * 8) + num45;
//                                 int num46 = (((j * 4) + num44) * num50) + n;
//                                 int destinationIndex = count * ((num47 * num43) + num46);
//                                 Array.Copy(buffer, 0, destinationArray, destinationIndex, count);
//                                 numArray[num46, num47] = num49;
//                                 num49++;
//                             }
//                         }
//                     }
//                 }
//             }
//             output.Write(destinationArray, 0, (int) num2);
//         }
//         else
//         {
//             byte[] buffer = new byte[num2];
//             input.Read(buffer, 0, (int) num2);
//             output.Write(buffer, 0, (int) num2);
//         }
//         input.Close();
//         writer.Close();
//         output.Close();
//         this.pictureBox1.ImageLocation = "";
//         if (File.Exists(this.exepath + @"\texconv.exe"))
//         {
//             File.Delete(this.basedir + this.basename + ".png");
//             Process process = new Process {
//                 StartInfo = { 
//                     WindowStyle = ProcessWindowStyle.Hidden,
//                     Arguments = "-y -f R8G8B8A8_UNORM -ft png \"" + this.basedir + this.basename + ".dds\" -o \"" + this.basedir,
//                     FileName = this.exepath + @"\texconv.exe"
//                 }
//             };
//             process.Start();
//             process.WaitForExit();
//             this.pictureBox1.ImageLocation = this.basedir + this.basename + ".png";
//         }
//     }
// }


// private void DoConvert()
// {
//     if (this.basename != "")
//     {
//         FileStream input = new FileStream(this.fname, FileMode.Open, FileAccess.Read);
//         new BinaryReader(input);
//         FileStream output = new FileStream(this.basedir + this.basename + ".dds", FileMode.Create);
//         BinaryWriter writer = new BinaryWriter(output);
//         long offset = 0L;
//         try
//         {
//             offset = Convert.ToInt64(this.tbOffset.Text, 0x10);
//         }
//         catch
//         {
//         }
//         input.Seek(offset, SeekOrigin.Begin);
//         long num2 = input.Length - offset;
//         if (num2 < 0L)
//         {
//             num2 = 0L;
//         }
//         int num3 = 1;
//         int num4 = 0x3031_5844;
//         int num5 = pixbl[this.ddDXGI.SelectedIndex];
//         int count = bpp[this.ddDXGI.SelectedIndex] * 2;
//         if (num5 == 1)
//         {
//             count = bpp[this.ddDXGI.SelectedIndex] / 8;
//         }
//         if (this.ddDXGI.SelectedIndex == 0x47)
//         {
//             num4 = 0x3154_5844;
//         }
//         if (this.ddDXGI.SelectedIndex == 0x4a)
//         {
//             num4 = 0x3354_5844;
//         }
//         if (this.ddDXGI.SelectedIndex == 0x4d)
//         {
//             num4 = 0x3554_5844;
//         }
//         if (this.ddDXGI.SelectedIndex == 80)
//         {
//             num4 = 0x3149_5441;
//         }
//         if (this.ddDXGI.SelectedIndex == 0x53)
//         {
//             num4 = 0x3249_5441;
//         }
//         num2 = ((this.sx * this.sy) * bpp[this.ddDXGI.SelectedIndex]) / 8;
//         writer.Write((long) 0x7c_2053_4444L);
//         writer.Write(0x1007);
//         writer.Write(this.sy);
//         writer.Write(this.sx);
//         writer.Write((int) num2);
//         writer.Write(0);
//         writer.Write(num3);
//         output.Seek(0x2cL, SeekOrigin.Current);
//         writer.Write(0x20);
//         writer.Write(4);
//         writer.Write(num4);
//         output.Seek(40L, SeekOrigin.Current);
//         if (num4 == 0x3031_5844)
//         {
//             writer.Write(this.ddDXGI.SelectedIndex);
//             writer.Write(3);
//             writer.Write(0);
//             writer.Write(1);
//             writer.Write(0);
//         }
//         if (this.cbPS4swiz.Checked)
//         {
//             byte[] destinationArray = new byte[num2 * 2L];
//             byte[] buffer = new byte[0x10];
//             int num7 = this.sy / num5;
//             int num8 = this.sx / num5;
//             for (int i = 0; i < ((num7 + 7) / 8); i++)
//             {
//                 for (int j = 0; j < ((num8 + 7) / 8); j++)
//                 {
//                     for (int k = 0; k < 0x40; k++)
//                     {
//                         int num13 = morton(k, 8, 8);
//                         int num14 = num13 / 8;
//                         int num15 = num13 % 8;
//                         input.Read(buffer, 0, count);
//                         if ((((j * 8) + num15) < num8) && (((i * 8) + num14) < num7))
//                         {
//                             int destinationIndex = count * (((((i * 8) + num14) * num8) + (j * 8)) + num15);
//                             Array.Copy(buffer, 0, destinationArray, destinationIndex, count);
//                         }
//                     }
//                 }
//             }
//             output.Write(destinationArray, 0, (int) num2);
//         }
//         else if (this.cbPS5swiz.Checked)
//         {
//             int num18;
//             int num19;
//             int num20;
//             int num21;
//             byte[] destinationArray = new byte[num2 * 2L];
//             byte[] buffer = new byte[0x40];
//             int num16 = this.sy / num5;
//             int num17 = this.sx / num5;
//             int num22 = 1;
//             switch (count)
//             {
//                 case 0x10:
//                     num22 = 1;
//                     break;

//                 case 8:
//                     num22 = 2;
//                     break;

//                 case 4:
//                     num22 = 4;
//                     break;
//             }
//             if (num5 == 1)
//             {
//                 for (int i = 0; i < ((num16 + 0x7f) / 0x80); i++)
//                 {
//                     for (int j = 0; j < ((num17 + 0x7f) / 0x80); j++)
//                     {
//                         for (int k = 0; k < 0x200; k++)
//                         {
//                             num19 = morton(k, 0x20, 0x10);
//                             int num26 = num19 % 0x20;
//                             int num27 = num19 / 0x20;
//                             for (int m = 0; m < 0x20; m++)
//                             {
//                                 input.Read(buffer, 0, count);
//                                 num20 = ((j * 0x80) + (num26 * 4)) + (m % 4);
//                                 num21 = (i * 0x80) + ((num27 * 8) + (m / 4));
//                                 if ((num20 < num17) && (num21 < num16))
//                                 {
//                                     num18 = count * ((num21 * num17) + num20);
//                                     Array.Copy(buffer, 0, destinationArray, num18, count);
//                                 }
//                             }
//                         }
//                     }
//                 }
//             }
//             else
//             {
//                 for (int i = 0; i < ((num16 + 0x3f) / 0x40); i++)
//                 {
//                     for (int j = 0; j < ((num17 + 0x3f) / 0x40); j++)
//                     {
//                         for (int k = 0; k < (0x100 / num22); k++)
//                         {
//                             num19 = morton(k, 0x10, 0x10 / num22);
//                             int num32 = num19 / 0x10;
//                             int num33 = num19 % 0x10;
//                             for (int m = 0; m < 0x10; m++)
//                             {
//                                 for (int n = 0; n < num22; n++)
//                                 {
//                                     input.Read(buffer, 0, count);
//                                     num20 = ((j * 0x40) + (((num32 * 4) + (m / 4)) * num22)) + n;
//                                     num21 = ((i * 0x40) + (num33 * 4)) + (m % 4);
//                                     if ((num20 < num17) && (num21 < num16))
//                                     {
//                                         num18 = count * ((num21 * num17) + num20);
//                                         Array.Copy(buffer, 0, destinationArray, num18, count);
//                                     }
//                                 }
//                             }
//                         }
//                     }
//                 }
//             }
//             output.Write(destinationArray, 0, (int) num2);
//         }
//         else if (this.cbPS3swiz.Checked)
//         {
//             byte[] destinationArray = new byte[num2 * 4L];
//             byte[] buffer = new byte[0x10];
//             int sy = this.sy / num5;
//             int sx = this.sx / num5;
//             for (int i = 0; i < (sx * sy); i++)
//             {
//                 int num40 = morton(i, sx, sy);
//                 input.Read(buffer, 0, count);
//                 int destinationIndex = count * num40;
//                 Array.Copy(buffer, 0, destinationArray, destinationIndex, count);
//             }
//             output.Write(destinationArray, 0, (int) num2);
//         }
//         else if (this.cbNINswiz.Checked)
//         {
//             byte[] destinationArray = new byte[num2 * 4L];
//             byte[] buffer = new byte[0x10];
//             int num42 = this.sy / num5;
//             int num43 = this.sx / num5;
//             int[,] numArray = new int[num43 * 2, num42 * 2];
//             int num44 = 0;
//             int num45 = 0;
//             int num48 = num42 / 8;
//             if (num48 > 0x10)
//             {
//                 num48 = 0x10;
//             }
//             int num49 = 0;
//             int num50 = 1;
//             switch (count)
//             {
//                 case 0x10:
//                     num50 = 1;
//                     break;

//                 case 8:
//                     num50 = 2;
//                     break;

//                 case 4:
//                     num50 = 4;
//                     break;
//             }
//             for (int i = 0; i < ((num42 / 8) / num48); i++)
//             {
//                 for (int j = 0; j < ((num43 / 4) / num50); j++)
//                 {
//                     for (int k = 0; k < num48; k++)
//                     {
//                         for (int m = 0; m < 0x20; m++)
//                         {
//                             for (int n = 0; n < num50; n++)
//                             {
//                                 int num56 = swi[m];
//                                 num45 = num56 / 4;
//                                 num44 = num56 % 4;
//                                 input.Read(buffer, 0, count);
//                                 int num47 = (((i * num48) + k) * 8) + num45;
//                                 int num46 = (((j * 4) + num44) * num50) + n;
//                                 int destinationIndex = count * ((num47 * num43) + num46);
//                                 Array.Copy(buffer, 0, destinationArray, destinationIndex, count);
//                                 numArray[num46, num47] = num49;
//                                 num49++;
//                             }
//                         }
//                     }
//                 }
//             }
//             output.Write(destinationArray, 0, (int) num2);
//         }
//         else
//         {
//             byte[] buffer = new byte[num2];
//             input.Read(buffer, 0, (int) num2);
//             output.Write(buffer, 0, (int) num2);
//         }
//         input.Close();
//         writer.Close();
//         output.Close();
//         this.pictureBox1.ImageLocation = "";
//         if (File.Exists(this.exepath + @"\texconv.exe"))
//         {
//             File.Delete(this.basedir + this.basename + ".png");
//             Process process = new Process {
//                 StartInfo = { 
//                     WindowStyle = ProcessWindowStyle.Hidden,
//                     Arguments = "-y -f R8G8B8A8_UNORM -ft png \"" + this.basedir + this.basename + ".dds\" -o \"" + this.basedir,
//                     FileName = this.exepath + @"\texconv.exe"
//                 }
//             };
//             process.Start();
//             process.WaitForExit();
//             this.pictureBox1.ImageLocation = this.basedir + this.basename + ".png";
//         }
//     }
// }



