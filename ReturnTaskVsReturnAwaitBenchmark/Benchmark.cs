using BenchmarkDotNet.Attributes;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReturnTaskVsReturnAwaitBenchmark
{
    [MemoryDiagnoser]
    [ThreadingDiagnoser]
    public class Benchmark
    {
        [Params(1, 10, 100, 1000)]
        public int Runs { get; set; }

        [Benchmark]
        public async Task<int> ReturnAwait() => await GetIntRecursivelyAsync(Runs).ConfigureAwait(false);

        [Benchmark(Baseline = true)]
        public int ReturnTask() => GetIntTaskRecursively(Runs).Result;

        public async Task<int> GetIntRecursivelyAsync(int runs)
        {
            Task<int> ret;
            if (runs > 0)
            {
                ret = GetIntRecursivelyAsync(runs - 1);
            }
            else
            {
                ret = Task.Run(() => Task.FromResult(1));

            }
            return await ret.ConfigureAwait(false);
        }

        public Task<int> GetIntTaskRecursively(int runs)
        {
            Task<int> ret;
            if (runs > 0)
            {
                ret = GetIntTaskRecursively(runs - 1);
            }
            else
            {
                ret = Task.Run(() => 1);
            }
            return ret;
        }
    }
}
