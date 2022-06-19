using System.Threading.Tasks;
using Xunit;

namespace Tasks.Tests;

public class TaskTest
{
    [Fact]
    public void CompletedTaskShouldAlwaysBeTheSame()
    {
        // arrange
        var task1 = Task.CompletedTask;
        var task2 = Task.CompletedTask;
        // act
        // assert
        Assert.Same(task1, task2);
    }

    [Fact]
    public void TaskFromResultShouldAlwaysBeTheSame()
    {
        // arrange
        var task1 = Task.FromResult(true);
        var task2 = Task.FromResult(true);
        // act
        // assert
        Assert.Same(task1, task2);
    }

    [Fact]
    public void TaskRunShouldAlwaysCreateDifferentTask()
    {
        // arrange
        var task1 = Task.Run(() => { });
        var task2 = Task.Run(() => { });
        // act
        // assert
        Assert.NotSame(task1, task2);
        Assert.NotEqual(task1, task2);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task ContinueWithAndTaskSchedulerDefaultShouldHavePreviousTaskResult(bool continueOnCapturedContext)
    {
        // arrange
        var o = new object();
        Task<object> task1 = Task.Run(() => o);
        // act
        var continueWithTask = task1.ContinueWith((t) => t.Result, TaskScheduler.Default);
        var continueWithTaskResult = await continueWithTask.ConfigureAwait(continueOnCapturedContext);
        // assert
        Assert.Same(o, continueWithTaskResult);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task ContinueWithAndTaskSchedulerCurrentShouldHavePreviousTaskResult(bool continueOnCapturedContext)
    {
        // arrange
        var o = new object();
        Task<object> task1 = Task.Run(() => o);
        // act
        var continueWithTask = task1.ContinueWith((t) => t.Result, TaskScheduler.Current);
        var continueWithTaskResult = await continueWithTask.ConfigureAwait(continueOnCapturedContext);
        // assert
        Assert.Same(o, continueWithTaskResult);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task ContinueWithShouldReceivePreviousTask(bool continueOnCapturedContext)
    {
        // arrange
        Task task1 = Task.Run(() => { });
        // act
        var continueWithTask = task1.ContinueWith((t) => t, TaskScheduler.Default);
        var previousTask = await continueWithTask.ConfigureAwait(continueOnCapturedContext);
        // assert
        Assert.Same(task1, previousTask);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task TaskShouldReturnSameObject(bool continueOnCapturedContext)
    {
        // arrange
        var o = new object();
        Task<object> task1 = Task.Run(() => o);
        // act
        // assert
        Assert.Same(o, await task1.ConfigureAwait(continueOnCapturedContext));
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task AwaitTaskShouldReturnSameTask(bool continueOnCapturedContext)
    {
        // arrange
        Task task = Task.Run(() => { });
        Task<Task> taskOfTask = task.ContinueWith(t => t);
        // act
        var awaitedTask = await taskOfTask.ConfigureAwait(continueOnCapturedContext);
        // assert
        Assert.Same(task, awaitedTask);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task UnwrapTaskShouldReturnSameTask(bool continueOnCapturedContext)
    {
        // arrange
        Task task = Task.Run(() => { });
        Task<Task> taskOfTask = task.ContinueWith(t => t);
        // act
        var unwrappedTask = taskOfTask.Unwrap();
        // assert
        Assert.Same(task, await taskOfTask.ConfigureAwait(continueOnCapturedContext));
    }

    [Fact]
    public void UnwrapTaskShouldBeDifferentTask()
    {
        // arrange
        Task task = Task.Run(() => { });
        Task<Task> taskOfTask = task.ContinueWith(t => t);
        // act
        var unwrappedTask = taskOfTask.Unwrap();
        // assert
        Assert.NotEqual(task, unwrappedTask);
    }
}
