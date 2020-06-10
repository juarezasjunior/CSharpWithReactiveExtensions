using System;
using System.Diagnostics;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using CSharpWithReactiveExtensions;
using NUnit.Framework;

namespace CSharpWithReactiveExtensionsTests
{
    public class TextEditorWithRxTests
    {
        [Test]
        public void UsingTextEditorWithRxTest()
        {
            var beforeWritingLineCount = 0;
            var afterWritingLineCount = 0;

            var textEditor = new TextEditor();

            // Important: the event in TextEditor must follow the standard for event patterns.
            var beforeWritingLineObservable = Observable.FromEventPattern<TextEditorLineArgs>(
                textEditor,
                nameof(TextEditor.BeforeWritingLine));

            var afterWritingLineObservable = Observable.FromEventPattern<TextEditorLineArgs>(
                textEditor,
                nameof(TextEditor.AfterWritingLine));

            using var beforeWritingLineObservableDisposable = beforeWritingLineObservable.Subscribe(x => beforeWritingLineCount++);
            using var afterWritingLineObservableDisposable = afterWritingLineObservable.Subscribe(x => afterWritingLineCount++);

            textEditor.WriteLine("My first line");
            textEditor.WriteLine("My second line");

            beforeWritingLineObservableDisposable.Dispose();
            afterWritingLineObservableDisposable.Dispose();

            Assert.That(beforeWritingLineCount, Is.EqualTo(2));
            Assert.That(afterWritingLineCount, Is.EqualTo(2));
        }

        [Test]
        public void UsingRxWithCompositeDisposableTest()
        {
            var beforeWritingLineCount = 0;
            var afterWritingLineCount = 0;

            var textEditor = new TextEditor();

            var compositeDisposable = new CompositeDisposable();

            // It's a collection of disposable items.
            // I can use it to dispose all items later.
            compositeDisposable.Add(
                Observable.FromEventPattern<TextEditorLineArgs>(
                    textEditor,
                    nameof(TextEditor.BeforeWritingLine))
                .Subscribe(x => beforeWritingLineCount++));

            compositeDisposable.Add(
                Observable.FromEventPattern<TextEditorLineArgs>(
                    textEditor,
                    nameof(TextEditor.AfterWritingLine))
                .Subscribe(x => afterWritingLineCount++));

            textEditor.WriteLine("My first line");
            textEditor.WriteLine("My second line");

            foreach (var disposable in compositeDisposable)
            {
                disposable.Dispose();
            }

            // It will not be counted, because the observer was disposed before.
            textEditor.WriteLine("My third line");

            Assert.That(beforeWritingLineCount, Is.EqualTo(2));
            Assert.That(afterWritingLineCount, Is.EqualTo(2));
        }

        [TestCase("patterns", 2)]
        [TestCase("list", 1)]
        [TestCase(".NET", 2)]
        public void UsingRxWithLinqFiltersTest(string word, int linesCount)
        {
            var linesWithSpecificWord = 0;

            var textEditor = new TextEditor();

            var usageOfSpecificWordObservable = Observable.FromEventPattern<TextEditorLineArgs>(
                textEditor,
                nameof(TextEditor.BeforeWritingLine))
                .Where(x => x.EventArgs?.LineValue?.Contains(word, StringComparison.OrdinalIgnoreCase) == true);

            using var usageOfSpecificWordObservableDisposable = usageOfSpecificWordObservable.Subscribe(x => linesWithSpecificWord++);
            
            textEditor.WriteLine(".NET events generally follow a few known patterns.");
            textEditor.WriteLine("Standardizing on these patterns means that developers can leverage knowledge of those standard patterns, which can be applied to any .NET event program.");
            textEditor.WriteLine("The argument list contains two arguments: the sender, and the event arguments. ");
            textEditor.WriteLine("Using an event model provides some design advantages. ");

            usageOfSpecificWordObservableDisposable.Dispose();

            Assert.That(linesWithSpecificWord, Is.EqualTo(linesCount));
        }
    }
}
