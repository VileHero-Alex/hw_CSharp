using TaskHub.Models;
using TaskHub.Services;
using TaskHub.Storage;
using TaskHub.Background;
using TaskHub.Demo;

namespace TaskHub;

class Program {
    private static TaskManager? TaskManager_;
    private static DeadlineChecker? DeadlineChecker_;
    
    static async Task Main(string[] args) {
        Console.WriteLine("=== TaskHub - Менеджер задач ===");
        Console.WriteLine();
        
        using var storage = new TaskStorage("tasks.json");
        TaskManager_ = new TaskManager(storage);
        
        TaskManager_.TaskChanged += message => Console.WriteLine($"[Событие] {message}");
        
        try {
            await TaskManager_.LoadAsync();
            Console.WriteLine($"Загружено {TaskManager_.GetTotalCount()} задач");
        }
        catch {
            Console.WriteLine("Файл задач не найден или поврежден, начинаем с пустого списка");
        }
        
        DeadlineChecker_ = new DeadlineChecker(TaskManager_, 5);
        DeadlineChecker_.Start();
        
        bool Running_ = true;
        while (Running_) {
            ShowMenu();
            string? Choice_ = Console.ReadLine();
            
            try {
                switch (Choice_) {
                    case "1":
                        CreateTask();
                        break;
                    case "2":
                        ViewTasks();
                        break;
                    case "3":
                        EditTask();
                        break;
                    case "4":
                        DeleteTask();
                        break;
                    case "5":
                        SearchTasks();
                        break;
                    case "6":
                        ShowStatistics();
                        break;
                    case "7":
                        await TaskManager_.SaveAsync();
                        Console.WriteLine("Задачи сохранены!");
                        break;
                    case "8":
                        await TaskManager_.LoadAsync();
                        Console.WriteLine("Задачи загружены!");
                        break;
                    case "9":
                        var DemoRunner_ = new DemoRunner(TaskManager_!, storage);
                        await DemoRunner_.RunDemo();
                        break;
                    case "0":
                        Running_ = false;
                        break;
                    default:
                        Console.WriteLine("Неверный выбор, попробуйте снова");
                        break;
                }
            }
            catch (Exception ex) {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            
            ShowNotifications();
            
            Console.WriteLine();
            Console.WriteLine("Нажмите Enter для продолжения...");
            Console.ReadLine();
            Console.Clear();
        }
        
        DeadlineChecker_?.Dispose();
        
        if (File.Exists("tasks.json")) {
            File.Delete("tasks.json");
        }
        
        Console.WriteLine("Спасибо за использование TaskHub!");
    }
    
    static void ShowNotifications() {
        if (DeadlineChecker_ == null) return;
        
        var notifications = DeadlineChecker_.GetNotifications();
        if (notifications.Count > 0) {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            foreach (var notification in notifications) {
                Console.WriteLine($"[!] {notification}");
            }
            Console.ResetColor();
        }
    }
    
    static void ShowMenu() {
        Console.WriteLine("\n=== МЕНЮ ===");
        Console.WriteLine("1. Создать задачу");
        Console.WriteLine("2. Просмотр задач");
        Console.WriteLine("3. Редактировать задачу");
        Console.WriteLine("4. Удалить задачу");
        Console.WriteLine("5. Поиск задач");
        Console.WriteLine("6. Статистика");
        Console.WriteLine("7. Сохранить в файл");
        Console.WriteLine("8. Загрузить из файла");
        Console.WriteLine("9. Демонстрация");
        Console.WriteLine("0. Выход");
        Console.Write("Выберите действие: ");
    }
    
    static void CreateTask() {
        Console.Write("Введите название: ");
        string Name_ = Console.ReadLine() ?? "";
        if (string.IsNullOrWhiteSpace(Name_)) {
            Console.WriteLine("Название не может быть пустым");
            return;
        }
        
        Console.Write("Введите описание: ");
        string Description_ = Console.ReadLine() ?? "";
        
        Console.WriteLine("Выберите приоритет:");
        Console.WriteLine("1. Low (Низкий)");
        Console.WriteLine("2. Medium (Средний)");
        Console.WriteLine("3. High (Высокий)");
        Console.Write("Выбор: ");
        Priority Priority_ = Console.ReadLine() switch {
            "1" => Priority.Low,
            "3" => Priority.High,
            _ => Priority.Medium
        };
        
        Console.Write("Введите дедлайн (дд.мм.гггг чч:мм): ");
        if (!DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime Deadline_)) {
            Deadline_ = DateTime.Now.AddDays(1);
            Console.WriteLine("Неверный формат, установлен дедлайн на завтра");
        }
        
        var Task_ = new TaskItem(Name_, Description_, Priority_, Deadline_);
        TaskManager_?.AddTask(Task_);
        Console.WriteLine("Задача создана!");
    }
    
    static void ViewTasks() {
        Console.WriteLine("\n=== ПРОСМОТР ЗАДАЧ ===");
        Console.WriteLine("1. Все задачи");
        Console.WriteLine("2. Выполненные");
        Console.WriteLine("3. Невыполненные");
        Console.WriteLine("4. Высокий приоритет");
        Console.Write("Выбор: ");
        
        var Choice_ = Console.ReadLine();
        List<TaskItem> Tasks_ = Choice_ switch {
            "2" => TaskManager_?.GetCompletedTasks() ?? new List<TaskItem>(),
            "3" => TaskManager_?.GetIncompleteTasks() ?? new List<TaskItem>(),
            "4" => TaskManager_?.GetHighPriorityTasks() ?? new List<TaskItem>(),
            _ => TaskManager_?.GetAllTasks() ?? new List<TaskItem>()
        };
        
        if (Tasks_.Count == 0) {
            Console.WriteLine("Задачи не найдены");
            return;
        }
        
        foreach (var TaskItem_ in Tasks_) {
            Console.WriteLine(TaskItem_);
            Console.WriteLine($"   Описание: {TaskItem_.Description}");
            Console.WriteLine($"   Создана: {TaskItem_.CreatedAt:dd.MM.yyyy HH:mm}");
            if (TaskItem_.IsOverdue()) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("   ! ПРОСРОЧЕНА");
                Console.ResetColor();
            }
            Console.WriteLine();
        }
        Console.WriteLine($"Всего: {Tasks_.Count}");
    }
    
    static void EditTask() {
        Console.Write("Введите ID задачи для редактирования: ");
        if (!int.TryParse(Console.ReadLine(), out int Id_)) {
            Console.WriteLine("Неверный ID");
            return;
        }
        
        var Task_ = TaskManager_?.GetTask(Id_);
        if (Task_ == null) {
            Console.WriteLine("Задача не найдена");
            return;
        }
        
        Console.WriteLine($"Редактирование: {Task_.Name}");
        Console.WriteLine("1. Изменить название");
        Console.WriteLine("2. Изменить описание");
        Console.WriteLine("3. Изменить приоритет");
        Console.WriteLine("4. Изменить статус");
        Console.Write("Выбор: ");
        
        switch (Console.ReadLine()) {
            case "1":
                Console.Write("Новое название: ");
                Task_.Name = Console.ReadLine() ?? Task_.Name;
                break;
            case "2":
                Console.Write("Новое описание: ");
                Task_.Description = Console.ReadLine() ?? Task_.Description;
                break;
            case "3":
                Console.WriteLine("1. Low, 2. Medium, 3. High");
                Task_.Priority = Console.ReadLine() switch {
                    "1" => Priority.Low,
                    "3" => Priority.High,
                    _ => Priority.Medium
                };
                break;
            case "4":
                Console.WriteLine("1. New, 2. InProgress, 3. Done");
                Task_.Status = Console.ReadLine() switch {
                    "2" => Status.InProgress,
                    "3" => Status.Done,
                    _ => Status.New
                };
                break;
        }
        Console.WriteLine("Задача обновлена!");
    }
    
    static void DeleteTask() {
        Console.Write("Введите ID задачи для удаления: ");
        if (!int.TryParse(Console.ReadLine(), out int Id_)) {
            Console.WriteLine("Неверный ID");
            return;
        }
        
        if (TaskManager_?.RemoveTask(Id_) == true) {
            Console.WriteLine("Задача удалена!");
        }
        else {
            Console.WriteLine("Задача не найдена");
        }
    }
    
    static void SearchTasks() {
        Console.WriteLine("\n=== ПОИСК ===");
        Console.WriteLine("1. По названию");
        Console.WriteLine("2. По статусу");
        Console.WriteLine("3. По приоритету");
        Console.Write("Выбор: ");
        
        var Choice_ = Console.ReadLine();
        List<TaskItem> Results_ = new List<TaskItem>();
        
        switch (Choice_) {
            case "1":
                Console.Write("Введите название: ");
                string Name_ = Console.ReadLine() ?? "";
                Results_ = TaskManager_?.SearchByName(Name_) ?? new List<TaskItem>();
                break;
            case "2":
                Console.WriteLine("1. New, 2. InProgress, 3. Done");
                Status Status_ = Console.ReadLine() switch {
                    "2" => Status.InProgress,
                    "3" => Status.Done,
                    _ => Status.New
                };
                Results_ = TaskManager_?.SearchByStatus(Status_) ?? new List<TaskItem>();
                break;
            case "3":
                Console.WriteLine("1. Low, 2. Medium, 3. High");
                Priority Priority_ = Console.ReadLine() switch {
                    "1" => Priority.Low,
                    "3" => Priority.High,
                    _ => Priority.Medium
                };
                Results_ = TaskManager_?.SearchByPriority(Priority_) ?? new List<TaskItem>();
                break;
        }
        
        if (Results_.Count == 0) {
            Console.WriteLine("Ничего не найдено");
        }
        else {
            foreach (var Task_ in Results_) {
                Console.WriteLine(Task_);
            }
            Console.WriteLine($"Найдено: {Results_.Count}");
        }
    }
    
    static void ShowStatistics() {
        Console.WriteLine("\n=== СТАТИСТИКА ===");
        Console.WriteLine($"Всего задач: {TaskManager_?.GetTotalCount()}");
        Console.WriteLine($"Выполнено: {TaskManager_?.GetCompletedCount()}");
        Console.WriteLine($"Просрочено: {TaskManager_?.GetOverdueCount()}");
        
        Console.WriteLine("\nПо приоритетам:");
        var PriorityStats_ = TaskManager_?.GetPriorityStats() ?? new Dictionary<Priority, int>();
        foreach (var Stat_ in PriorityStats_) {
            Console.WriteLine($"  {Stat_.Key}: {Stat_.Value}");
        }
    }
}
