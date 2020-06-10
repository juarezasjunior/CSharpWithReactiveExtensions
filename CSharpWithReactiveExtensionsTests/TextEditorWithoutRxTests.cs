using CSharpWithReactiveExtensions;
using NUnit.Framework;

namespace CSharpWithReactiveExtensionsTests
{
    public class TextEditorWithoutRxTests
    {
        private int beforeWritingLineCount = 0;
        private int afterWritingLineCount = 0;

        [Test]
        public void UsingTextEditorWithoutRxTest()
        {
            var textEditor = new TextEditor();
            textEditor.AfterWritingLine += TextEditor_AfterWritingLine;
            textEditor.BeforeWritingLine += TextEditor_BeforeWritingLine;

            textEditor.WriteLine("My first line");
            textEditor.WriteLine("My second line");

            Assert.That(beforeWritingLineCount, Is.EqualTo(2));
            Assert.That(afterWritingLineCount, Is.EqualTo(2));
        }

        private void TextEditor_BeforeWritingLine(object sender, TextEditorLineArgs args)
        {
            beforeWritingLineCount++;
        }

        private void TextEditor_AfterWritingLine(object sender, TextEditorLineArgs args)
        {
            afterWritingLineCount++;
        }
    }
}