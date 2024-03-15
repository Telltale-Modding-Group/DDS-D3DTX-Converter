using Avalonia.Controls;
using System.IO;
using System.Text;

namespace DDS_D3DTX_Converter
{

    /// <summary>
    /// Potential console view panel for the app. 
    /// </summary>
    public class ConsoleWriter : TextWriter
    {
        // The control where we will write text.
        private TextBox TextBox;

        public ConsoleWriter(TextBox TextBox)
        {
            this.TextBox = TextBox;
        }

        public override void Write(char value)
        {
            TextBox.Text += value;
        }

        public override void Write(string value)
        {
            TextBox.Text += value;
        }

        public override Encoding Encoding
        {
            get { return Encoding.Unicode; }
        }
    }
}

