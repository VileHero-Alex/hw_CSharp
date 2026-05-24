using System.Text.Json;
using TaskHub.Models;

namespace TaskHub.Storage;

public class TaskStorage : IDisposable {
    private string FilePath_;
    private bool Disposed_;
    
    public TaskStorage(string filePath) {
        FilePath_ = filePath;
    }
    
    public async Task SaveTasksAsync(List<TaskItem> tasks) {
        try {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(tasks, options);
            await File.WriteAllTextAsync(FilePath_, json);
        }
        catch (Exception ex) {
            Console.WriteLine($"Ошибка сохранения: {ex.Message}");
            throw;
        }
    }
    
    public async Task<List<TaskItem>> LoadTasksAsync() {
        try {
            if (!File.Exists(FilePath_)) {
                return new List<TaskItem>();
            }
            
            string json = await File.ReadAllTextAsync(FilePath_);
            var tasks = JsonSerializer.Deserialize<List<TaskItem>>(json);
            return tasks ?? new List<TaskItem>();
        }
        catch (Exception ex) {
            Console.WriteLine($"Ошибка загрузки: {ex.Message}");
            throw;
        }
    }
    
    public void Dispose() {
        if (!Disposed_) {
            Disposed_ = true;
            GC.SuppressFinalize(this);
        }
    }
}
