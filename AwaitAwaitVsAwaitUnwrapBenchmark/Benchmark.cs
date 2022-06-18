using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using System.Threading.Tasks;

namespace AsyncAsyncVsUnwrap
{
    [MemoryDiagnoser]
    [SimpleJob(RuntimeMoniker.Net60, baseline: true)]
    [SimpleJob(RuntimeMoniker.Net50)]
    [SimpleJob(RuntimeMoniker.Net48)]
    [SimpleJob(RuntimeMoniker.Net472)]
    [SimpleJob(RuntimeMoniker.Net461)]
    public class Benchmark
    {
        [Benchmark(Description = "await + Unwrap()")]
        public void AwaitUnwrap() => _ = RunAwaitUnwrap().GetAwaiter().GetResult();

        private async Task<int> RunAwaitUnwrap() => await GetTaskOfTask().Unwrap().ConfigureAwait(false);

        [Benchmark(Description = "await await")]
        public void AwaitAwait() => _ = RunAwaitAwait().GetAwaiter().GetResult();
        private async Task<int> RunAwaitAwait() => await (await GetTaskOfTask().ConfigureAwait(false)).ConfigureAwait(false);

        private Task<Task<int>> GetTaskOfTask() => Task.Run(() => 1).ContinueWith((m) => m, TaskScheduler.Default);
    }
}