using System.Diagnostics;
using System.Threading.Tasks;

namespace AsyncBreakfast
{
    public static class Program
    {
        static async Task Main()
        {
            var breakfast = new Breakfast();
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            await breakfast.HaveBreakfastAsync().ConfigureAwait(false);
            stopWatch.Stop();
            Helpers.WriteLine($"Asynchronous Breakfast is ready in {stopWatch.ElapsedMilliseconds}ms!");
            Helpers.WriteLine("---");
            stopWatch.Restart();
#pragma warning disable CA1849 // Call async methods when in an async method
            breakfast.HaveBreakfast();
#pragma warning restore CA1849 // Call async methods when in an async method
            stopWatch.Stop();
            Helpers.WriteLine($"Synchronous Breakfast is ready in {stopWatch.ElapsedMilliseconds}ms!");
            Helpers.WriteLine("---");
        }
    }
}
