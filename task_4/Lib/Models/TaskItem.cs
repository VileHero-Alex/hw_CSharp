namespace TaskHub.Models;

public class TaskItem {
    private static int IdCounter_ = 0;
    
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Priority Priority { get; set; }
    public DateTime Deadline { get; set; }
    public Status Status { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public TaskItem() {
        Name = "";
        Description = "";
    }
    
    public TaskItem(string name, string description, Priority priority, DateTime deadline) {
        Id = ++IdCounter_;
        Name = name;
        Description = description;
        Priority = priority;
        Deadline = deadline;
        Status = Status.New;
        CreatedAt = DateTime.Now;
    }
    
    public bool IsOverdue() {
        return Deadline < DateTime.Now && Status != Status.Done;
    }
    
    public override string ToString() {
        string overdueMark = IsOverdue() ? " [ПРОСРОЧЕНО]" : "";
        return $"[{Id}] {Name} | Приоритет: {Priority} | Статус: {Status} | Дедлайн: {Deadline:dd.MM.yyyy HH:mm}{overdueMark}";
    }
}
