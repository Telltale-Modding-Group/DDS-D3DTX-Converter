using System.IO;

namespace D3DTX_Converter.ProgramRelease
{
    public class ArgumentInputPath
    {
        public static string Keyword { get { return "-Input"; } }

        private string InputPath;
        public bool IsDirectory { get { return Directory.Exists(InputPath); } }
        public bool IsFile { get { return File.Exists(InputPath); } }

        public ArgumentInputPath(string[] arguments)
        {
            string argument_keyword = arguments[0];
            string argument_inputPath = arguments[1];

            InputPath = argument_inputPath;
        }
    }
}
