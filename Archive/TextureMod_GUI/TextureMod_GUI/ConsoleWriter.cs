using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace TextureMod_GUI
{
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
