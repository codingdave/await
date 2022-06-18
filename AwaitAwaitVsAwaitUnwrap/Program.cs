using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AsyncAsyncVsUnwrap
{
    public static class Program
    {
        private const bool _continueOnCapturedContext = true;
        public static async Task Main()
        {
            var taskRunBehavior = new TaskRunBehavior(_continueOnCapturedContext);

            var name = $"{nameof(taskRunBehavior.RunShortTaskAsync)}";
            Helpers.WriteLine($"<===== running {name} =====>");
            var analyzeTask1 = Analyze(taskRunBehavior, taskRunBehavior.RunShortTaskAsync(), name);
            Helpers.TreadTaskInfo($"{nameof(analyzeTask1)}", analyzeTask1);
            await analyzeTask1.ConfigureAwait(_continueOnCapturedContext);
            Helpers.TreadTaskInfo($"awaited {nameof(analyzeTask1)}", analyzeTask1);

            name = $"{nameof(taskRunBehavior.RunLongTaskAsync)}";
            Helpers.WriteLine($"<===== running {name} =====>");
            var analyzeTask2 = Analyze(taskRunBehavior, taskRunBehavior.RunLongTaskAsync(), name);
            Helpers.TreadTaskInfo($"{nameof(analyzeTask2)}", analyzeTask2);
            await analyzeTask2.ConfigureAwait(_continueOnCapturedContext);
            Helpers.TreadTaskInfo($"awaited {nameof(analyzeTask2)}", analyzeTask2);
        }

        private static async Task Analyze(TaskRunBehavior taskRunningBehavior, Task task, string name)
        {
            if (taskRunningBehavior is null)
            {
                throw new ArgumentNullException(nameof(taskRunningBehavior));
            }

            if (task is null)
            {
                throw new ArgumentNullException(nameof(task));
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or empty.", nameof(name));
            }

            Helpers.TreadTaskInfo($"{name}", task);
            await task.ConfigureAwait(_continueOnCapturedContext);
            Helpers.TreadTaskInfo($"awaited {name}", task);
            var whenAllTask = Task.WhenAll(
                taskRunningBehavior.TaskForAwait,
                taskRunningBehavior.AwaitedTask,
                taskRunningBehavior.TaskForUnwrap,
                taskRunningBehavior.UnwrappedTask);
            Helpers.TreadTaskInfo($"{nameof(whenAllTask)}", whenAllTask);
            await whenAllTask.ConfigureAwait(_continueOnCapturedContext);
            Helpers.TreadTaskInfo($"awaited {nameof(whenAllTask)}", whenAllTask);
            Helpers.Summary(taskRunningBehavior);
        }
    }
}
