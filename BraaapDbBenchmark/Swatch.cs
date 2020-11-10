using System;
using System.Diagnostics;

namespace BraaapDbBenchmark
{
    public class Swatch: IDisposable
    {
        private readonly string _message;
        private readonly Stopwatch _sw;

        public Swatch(string message)
        {
            _message = message;
            _sw = Stopwatch.StartNew();
        }
        
        public void Dispose()
        {
            Console.WriteLine($"{_message} {_sw.Elapsed}");
        }
    }
}