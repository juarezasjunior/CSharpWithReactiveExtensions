using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpWithReactiveExtensionsTests.RxMoreSamples
{
    [TestFixture]
    public class GettingASequenceTests
    {
        [Test]
        public void GetSimpleObservableTest()
        {
            List<string> Log = new List<string>();

            // It will return a simple pipeline
            var sequence = Observable.Return(42);

            sequence.Subscribe(
                x => Log.Add("Called OnNext - Value: " + x),
                exception => Log.Add("Called OnError - Exception: " + exception),
                () => Log.Add("Called OnCompleted"));

            // The code above will call only OnNext and OnCompleted
            Assert.That(Log.Any(x => x.StartsWith("Called OnNext")), Is.True);
            Assert.That(Log.Any(x => x.StartsWith("Called OnError")), Is.False);
            Assert.That(Log.Any(x => x.StartsWith("Called OnCompleted")), Is.True);
        }

        [Test]
        public void GetThrowObservableTest()
        {
            List<string> Log = new List<string>();

            // It will return a throw in the sequence
            var sequence = Observable.Throw<int>(new Exception("Error"));

            sequence.Subscribe(
                x => Log.Add("Called OnNext - Value: " + x),
                exception => Log.Add("Called OnError - Exception: " + exception),
                () => Log.Add("Called OnCompleted"));

            // The code above will call only OnError
            Assert.That(Log.Any(x => x.StartsWith("Called OnNext")), Is.False);
            Assert.That(Log.Any(x => x.StartsWith("Called OnError")), Is.True);
            Assert.That(Log.Any(x => x.StartsWith("Called OnCompleted")), Is.False);
        }

        [Test]
        public void GetEmptyObservableTest()
        {
            List<string> Log = new List<string>();

            // It will return an empty value in the sequence
            var sequence = Observable.Empty<int>();

            sequence.Subscribe(
                x => Log.Add("Called OnNext - Value: " + x),
                exception => Log.Add("Called OnError - Exception: " + exception),
                () => Log.Add("Called OnCompleted"));

            // The code above will call only OnCompleted
            Assert.That(Log.Any(x => x.StartsWith("Called OnNext")), Is.False);
            Assert.That(Log.Any(x => x.StartsWith("Called OnError")), Is.False);
            Assert.That(Log.Any(x => x.StartsWith("Called OnCompleted")), Is.True);
        }

        [Test]
        public void GetTaskObservableTest()
        {
            List<string> Log = new List<string>();

            var task = new Task<int>(() => 50);

            task.RunSynchronously();

            // It will return a task in the sequence
            var sequence = task.ToObservable();

            sequence.Subscribe(
                x => Log.Add("Called OnNext - Value: " + x),
                exception => Log.Add("Called OnError - Exception: " + exception),
                () => Log.Add("Called OnCompleted"));

            // The code above will call only OnNext and OnCompleted
            Assert.That(Log.Any(x => x.StartsWith("Called OnNext")), Is.True);
            Assert.That(Log.Any(x => x.StartsWith("Called OnError")), Is.False);
            Assert.That(Log.Any(x => x.StartsWith("Called OnCompleted")), Is.True);
        }

        [Test]
        public void GetRangeObservableTest()
        {
            List<string> Log = new List<string>();

            // It will return a sequencial numbers in the sequence
            var sequence = Observable.Range(1, 10);

            sequence.Subscribe(
                x => Log.Add("Called OnNext - Value: " + x),
                exception => Log.Add("Called OnError - Exception: " + exception),
                () => Log.Add("Called OnCompleted"));

            // The code above will call OnNext 10 times and OnCompleted only once
            Assert.That(Log.Count(x => x.StartsWith("Called OnNext")), Is.EqualTo(10));
            Assert.That(Log.Any(x => x.StartsWith("Called OnError")), Is.False);
            Assert.That(Log.Any(x => x.StartsWith("Called OnCompleted")), Is.True);
        }

        [Test]
        public void GetIntervalObservableTest()
        {
            List<string> Log = new List<string>();

            // It will return a sequencial numbers every 500 milliseconds in the sequence
            var sequence = Observable.Interval(TimeSpan.FromMilliseconds(500));

            sequence.Subscribe(
                x => Log.Add("Called OnNext - Value: " + x),
                exception => Log.Add("Called OnError - Exception: " + exception),
                () => Log.Add("Called OnCompleted"));

            // It will generate a sequence every 500 milliseconds.
        }

        [Test]
        public void GetCreateObservableTest()
        {
            List<string> Log = new List<string>();

            // It will return a sequencial numbers in the sequence
            var sequence = Observable.Create<int>(
                x => {
                    x.OnNext(1);
                    x.OnNext(2);
                    x.OnNext(3);
                    x.OnCompleted();

                    return Disposable.Empty;
                });

            sequence.Subscribe(
                x => Log.Add("Called OnNext - Value: " + x),
                exception => Log.Add("Called OnError - Exception: " + exception),
                () => Log.Add("Called OnCompleted"));

            // The code above will call OnNext 10 times and OnCompleted only once
            Assert.That(Log.Count(x => x.StartsWith("Called OnNext")), Is.EqualTo(3));
            Assert.That(Log.Any(x => x.StartsWith("Called OnError")), Is.False);
            Assert.That(Log.Any(x => x.StartsWith("Called OnCompleted")), Is.True);
        }

        [Test]
        public void GetGenerateObservableTest()
        {
            List<string> Log = new List<string>();

            // It will return a sequencial numbers in the sequence
            var sequence = Observable.Generate(
                1,
                x => x < 5,
                x => x + 1,
                x => x);

            sequence.Subscribe(
                x => Log.Add("Called OnNext - Value: " + x),
                exception => Log.Add("Called OnError - Exception: " + exception),
                () => Log.Add("Called OnCompleted"));

            // The code above will call OnNext 4 times and OnCompleted only once
            Assert.That(Log.Count(x => x.StartsWith("Called OnNext")), Is.EqualTo(4));
            Assert.That(Log.Any(x => x.StartsWith("Called OnError")), Is.False);
            Assert.That(Log.Any(x => x.StartsWith("Called OnCompleted")), Is.True);
        }
    }
}
