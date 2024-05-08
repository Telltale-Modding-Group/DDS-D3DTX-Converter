using System.IO;

namespace D3DTX_Converter.ProgramRelease
{
    public class ArgumentOutputPath
    {
        public static string Keyword { get { return "-Output"; } }

        private string OutputPath;
        public bool IsDirectory { get { return Directory.Exists(OutputPath); } }
        public bool IsFile { get { return File.Exists(OutputPath); } }

        public ArgumentOutputPath GetResult(string[] arguments)
        {
            try
            {
                string argument_keyword = arguments[0];
                string argument_inputPath = arguments[1];

                OutputPath = argument_inputPath;

                return this;
            }
            catch
            {
                return null;
            }
        }
    }
}
