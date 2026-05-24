using TaskHub.Models;
using TaskHub.Storage;

namespace TaskHub.Services;

public class TaskManager {
    private List<TaskItem> Tasks_ = new List<TaskItem>();
    private TaskStorage Storage_;
    
    public delegate void TaskChangedDelegate(string message);
    public event TaskChangedDelegate? TaskChanged;
    
    public TaskManager(TaskStorage storage) {
        Storage_ = storage;
    }
    
    public void AddTask(TaskItem task) {
        Tasks_.Add(task);
        TaskChanged?.Invoke($"Задача '{task.Name}' добавлена");
    }
    
    public bool RemoveTask(int id) {
        var task = Tasks_.FirstOrDefault(t => t.Id == id);
        if (task != null) {
            Tasks_.Remove(task);
            TaskChanged?.Invoke($"Задача '{task.Name}' удалена");
            return true;
        }
        return false;
    }
    
    public TaskItem? GetTask(int id) {
        return Tasks_.FirstOrDefault(t => t.Id == id);
    }
    
    public List<TaskItem> GetAllTasks() {
        return new List<TaskItem>(Tasks_);
    }
    
    public List<TaskItem> GetCompletedTasks() {
        return Tasks_.Where(t => t.Status == Status.Done).ToList();
    }
    
    public List<TaskItem> GetIncompleteTasks() {
        return Tasks_.Where(t => t.Status != Status.Done).ToList();
    }
    
    public List<TaskItem> GetHighPriorityTasks() {
        return Tasks_.Where(t => t.Priority == Priority.High).ToList();
    }
    
    public List<TaskItem> SearchByName(string name) {
        return Tasks_.Where(t => t.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
    }
    
    public List<TaskItem> SearchByStatus(Status status) {
        return Tasks_.Where(t => t.Status == status).ToList();
    }
    
    public List<TaskItem> SearchByPriority(Priority priority) {
        return Tasks_.Where(t => t.Priority == priority).ToList();
    }
    
    public async Task SaveAsync() {
        await Storage_.SaveTasksAsync(Tasks_);
    }
    
    public async Task LoadAsync() {
        Tasks_ = await Storage_.LoadTasksAsync();
    }
    
    public int GetTotalCount() {
        return Tasks_.Count;
    }
    
    public int GetCompletedCount() {
        return Tasks_.Count(t => t.Status == Status.Done);
    }
    
    public int GetOverdueCount() {
        return Tasks_.Count(t => t.IsOverdue());
    }
    
    public Dictionary<Priority, int> GetPriorityStats() {
        return Tasks_.GroupBy(t => t.Priority)
                       .ToDictionary(g => g.Key, g => g.Count());
    }
}
