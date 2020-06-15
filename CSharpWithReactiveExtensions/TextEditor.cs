using System;
using System.Text;

namespace CSharpWithReactiveExtensions
{
    public class TextEditor
    {
        private readonly StringBuilder stringBuilder = new StringBuilder();

        public event EventHandler<TextEditorLineArgs> BeforeWritingLine;

        public event EventHandler<TextEditorLineArgs> AfterWritingLine;

        public void WriteLine(string value)
        {
            BeforeWritingLine?.Invoke(this, new TextEditorLineArgs(value));

            stringBuilder.Append(value);

            AfterWritingLine?.Invoke(this, new TextEditorLineArgs(value));
        }

        public string GetText()
        {
            return stringBuilder.ToString();
        }
    }

    public class TextEditorLineArgs
    {
        public string LineValue { get; internal set; }

        public TextEditorLineArgs(string lineValue)
        {
            LineValue = lineValue;
        }
    }
}
