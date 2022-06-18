using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AsyncAsyncVsUnwrap
{
    public class TaskRunBehavior
    {
        private readonly bool _continueOnCapturedContext = true;
        public TaskRunBehavior(bool continueOnCapturedContext) => _continueOnCapturedContext = continueOnCapturedContext;

        public Task TaskForAwait { get; private set; }
        public Task AwaitedTask { get; private set; }
        public Task TaskForUnwrap { get; private set; }
        public Task UnwrappedTask { get; private set; }

        public async Task RunLongTaskAsync()
        {
            ClearTasks();
            var longRunnningTask = RunAsync(() => Task.Delay(100));
            Helpers.TreadTaskInfo($"{nameof(longRunnningTask)}", longRunnningTask);
            await longRunnningTask.ConfigureAwait(_continueOnCapturedContext);
            Helpers.TreadTaskInfo($"awaited {nameof(longRunnningTask)}", longRunnningTask);
        }

        public async Task RunShortTaskAsync()
        {
            var shortRunningTask = RunAsync(() => Task.Run(() => { }));
            Helpers.TreadTaskInfo($"{nameof(shortRunningTask)}", shortRunningTask);
            await shortRunningTask.ConfigureAwait(_continueOnCapturedContext);
            Helpers.TreadTaskInfo($"awaited {nameof(shortRunningTask)}", shortRunningTask);
        }

        private void ClearTasks()
        {
            TaskForAwait = null;
            TaskForUnwrap = null;
            AwaitedTask = null;
            UnwrappedTask = null;
        }

        private async Task RunAsync(Func<Task> GetTask)
        {
            Helpers.WriteLine($" - create task for await - ");
            TaskForAwait = GetTask();
            Helpers.TreadTaskInfo($"{nameof(TaskForAwait)}", TaskForAwait);
            var taskOfTaskForAwait = GetTaskOfTask(TaskForAwait);
            Helpers.TreadTaskInfo($"{nameof(taskOfTaskForAwait)} (continueWithTask)", taskOfTaskForAwait);
            AwaitedTask = await taskOfTaskForAwait.ConfigureAwait(_continueOnCapturedContext);
            Helpers.TreadTaskInfo($"{nameof(AwaitedTask)}", AwaitedTask);

            Helpers.WriteLine($" - create tasks for unwrap - ");

            TaskForUnwrap = GetTask();
            Helpers.TreadTaskInfo($"{nameof(TaskForUnwrap)}", TaskForUnwrap);
            var taskOfTaskForUnwrap = GetTaskOfTask(TaskForUnwrap);
            Helpers.TreadTaskInfo($"{nameof(taskOfTaskForUnwrap)} (continueWithTask)", taskOfTaskForUnwrap);
            UnwrappedTask = taskOfTaskForUnwrap.Unwrap();
            Helpers.TreadTaskInfo($"{nameof(UnwrappedTask)}", UnwrappedTask);

            Helpers.WriteLine($" - tasks created - ");
        }

        private Task<Task> GetTaskOfTask(Task originalTask)
        {
            Task<Task> continueWithTask = null;
            continueWithTask = originalTask.ContinueWith((previousTask) =>
            {
                Helpers.TreadTaskInfo($"{nameof(previousTask)} (in ContinueWith)", previousTask);
                Helpers.TreadTaskInfo($"{nameof(continueWithTask)} (in ContinueWith)", continueWithTask);
                return previousTask;
            }, TaskScheduler.Current);
            return continueWithTask;
        }
    }
}
