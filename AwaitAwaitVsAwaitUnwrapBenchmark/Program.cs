using BenchmarkDotNet.Running;
using System.Diagnostics;

namespace AsyncAsyncVsUnwrap
{
    public static class Program
    {
        public static void Main()
        {
            _ = BenchmarkRunner.Run<Benchmark>();
        }
    }
}
