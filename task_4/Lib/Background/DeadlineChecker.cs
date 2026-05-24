using System.Collections.Concurrent;
using TaskHub.Services;
using TaskHub.Models;

namespace TaskHub.Background;

public class DeadlineChecker : IDisposable {
    private TaskManager TaskManager_;
    private CancellationTokenSource Cts_;
    private Task? BackgroundTask_;
    private int CheckIntervalSeconds_;
    private HashSet<int> NotifiedTasks_ = new HashSet<int>();
    private ConcurrentQueue<string> Notifications_ = new ConcurrentQueue<string>();
    
    public DeadlineChecker(TaskManager taskManager, int checkIntervalSeconds = 10) {
        TaskManager_ = taskManager;
        CheckIntervalSeconds_ = checkIntervalSeconds;
        Cts_ = new CancellationTokenSource();
    }
    
    public void Start() {
        BackgroundTask_ = Task.Run(async () => {
            while (!Cts_.Token.IsCancellationRequested) {
                CheckDeadlines();
                await Task.Delay(TimeSpan.FromSeconds(CheckIntervalSeconds_), Cts_.Token);
            }
        }, Cts_.Token);
    }
    
    public void Stop() {
        Cts_.Cancel();
        try {
            BackgroundTask_?.Wait();
        }
        catch (AggregateException) {
        }
    }
    
    private void CheckDeadlines() {
        var tasks = TaskManager_.GetAllTasks();
        foreach (var task in tasks) {
            if (task.IsOverdue() && !NotifiedTasks_.Contains(task.Id)) {
                NotifiedTasks_.Add(task.Id);
                Notifications_.Enqueue($"Задача просрочена: {task.Name} (ID: {task.Id})");
            }
        }
    }
    
    public List<string> GetNotifications() {
        var result = new List<string>();
        while (Notifications_.TryDequeue(out string? notification)) {
            if (notification != null) {
                result.Add(notification);
            }
        }
        return result;
    }
    
    public void Dispose() {
        Stop();
        Cts_.Dispose();
        GC.SuppressFinalize(this);
    }
}
