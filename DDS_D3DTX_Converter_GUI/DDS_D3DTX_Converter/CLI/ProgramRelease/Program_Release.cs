using System;
using System.IO;

namespace D3DTX_Converter.ProgramRelease;

public static class Program_Release
{
    public static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Please Write In Arguments");
            return;
        }

        ArgumentInputPath argumentInputPath;
        ArgumentMode argumentMode;
        ArgumentOutputPath argumentOutputPath;

        for (int i = 0; i < args.Length; i++)
        {
            string arg = args[i];

            if (arg.Equals(ArgumentInputPath.Keyword) && (i + 1) > args.Length)
            {
                argumentInputPath = new(args);
            }

            if (arg.Equals(ArgumentMode.Keyword) && (i + 1) > args.Length)
            {
                argumentMode = new(args);
            }
        }

        string inputDirectory = args[0];

        if (Directory.Exists(inputDirectory) == false)
        {
            Console.WriteLine("Invalid Input Directory");
            return;
        }
    }
}
