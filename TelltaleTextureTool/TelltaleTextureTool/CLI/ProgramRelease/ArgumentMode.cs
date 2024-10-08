using System;

namespace TelltaleTextureTool.ProgramRelease;

public class ArgumentMode
{
    public static string Keyword { get { return "-Mode"; } }

    public ArgumentEnumModes Mode = ArgumentEnumModes.UNKNOWN;

    public ArgumentMode(string[] arguments)
    {
        string argument_keyword = arguments[0];
        string argument_modeEnum = arguments[1];

        foreach (string modeName in Enum.GetNames(typeof(ArgumentEnumModes)))
        {
            if (modeName.Equals(argument_modeEnum))
            {
                Mode = (ArgumentEnumModes)Enum.Parse(typeof(ArgumentEnumModes), argument_modeEnum);
            }
        }
    }
}
