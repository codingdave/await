using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AsyncAsyncVsUnwrap
{
    public static class Helpers
    {
        public static void WriteLine(object o)
        {
            Console.WriteLine(o);
            Debug.WriteLine(o);
        }

        internal static void Summary(Task taskForAwait, Task awaitedTask, Task taskForUnwrap, Task unwrappedTask)
        {
            if (taskForAwait is null)
            {
                throw new ArgumentNullException(nameof(taskForAwait));
            }

            if (awaitedTask is null)
            {
                throw new ArgumentNullException(nameof(awaitedTask));
            }

            if (taskForUnwrap is null)
            {
                throw new ArgumentNullException(nameof(taskForUnwrap));
            }

            if (unwrappedTask is null)
            {
                throw new ArgumentNullException(nameof(unwrappedTask));
            }

            WriteLine($"-> {nameof(taskForAwait)},  id: {taskForAwait.Id,2 } == {nameof(awaitedTask)},   id: {awaitedTask.Id,2  }: {ReferenceEquals(taskForAwait, awaitedTask)}");
            WriteLine($"-> {nameof(taskForUnwrap)}, id: {taskForUnwrap.Id,2} == {nameof(unwrappedTask)}, id: {unwrappedTask.Id,2}: {ReferenceEquals(taskForUnwrap, unwrappedTask)}");
        }

        internal static void TreadTaskInfo(string postfix, Task task)
        {
            if (task == null)
            {
                Debug.Assert(Task.CurrentId == null);
            }
            if (Task.CurrentId != null && !task.IsCompleted)
            {
                Debug.Assert(task.Id == Task.CurrentId);
            }

            var syncstate = Task.CurrentId == null ? " sync" : $"async";
            var taskInfo = task == null ? string.Empty : $" (task {task.Id}, status: {task.Status})";
            WriteLine($"{syncstate} on Thread {Environment.CurrentManagedThreadId}{taskInfo}: {postfix}");
        }

        internal static void Summary(TaskRunBehavior taskRunningBehavior)
        {
            if (taskRunningBehavior is null)
            {
                throw new ArgumentNullException(nameof(taskRunningBehavior));
            }

            Summary(
                taskRunningBehavior.TaskForAwait,
                taskRunningBehavior.AwaitedTask,
                taskRunningBehavior.TaskForUnwrap,
                taskRunningBehavior.UnwrappedTask);
        }
    }
}
