using System;
using System.Text;

namespace D3DTX_Converter.Utilities;

public static class FlagHelpers
{
    public static bool IsSet<T>(T flags, T flag) where T : struct
    {
        int flagsValue = (int)(object)flags;
        int flagValue = (int)(object)flag;

        return (flagsValue & flagValue) != 0;
    }

    public static void Set<T>(ref T flags, T flag) where T : struct
    {
        int flagsValue = (int)(object)flags;
        int flagValue = (int)(object)flag;

        flags = (T)(object)(flagsValue | flagValue);
    }

    public static void Unset<T>(ref T flags, T flag) where T : struct
    {
        int flagsValue = (int)(object)flags;
        int flagValue = (int)(object)flag;

        flags = (T)(object)(flagsValue & (~flagValue));
    }

    const string Separator = ", ";

    public static string FlagsEnumToString<T>(Enum e)
    {
        var str = new StringBuilder();

        foreach (object i in Enum.GetValues(typeof(T)))
        {
            if (IsExactlyOneBitSet((int)i) &&
                e.HasFlag((Enum)i))
            {
                str.Append((T)i + Separator);
            }
        }

        if (str.Length > 0)
        {
            str.Length -= Separator.Length;
        }

        return str.ToString();
    }

    static bool IsExactlyOneBitSet(int i)
    {
        return i != 0 && (i & (i - 1)) == 0;
    }
}
