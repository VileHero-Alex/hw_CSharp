using TaskHub.Models;
using TaskHub.Services;
using TaskHub.Storage;

namespace TaskHub.Demo;

public class DemoRunner {
    private TaskManager Manager_;
    private TaskStorage Storage_;
    
    public DemoRunner(TaskManager manager, TaskStorage storage) {
        Manager_ = manager;
        Storage_ = storage;
    }
    
    public async Task RunDemo() {
        Console.Clear();
        Console.WriteLine("=== ДЕМОНСТРАЦИЯ ФУНКЦИОНАЛА ===");
        Console.WriteLine();
        
        Console.WriteLine("[ШАГ 1] Создание задач разных типов...");
        await Task.Delay(500);
        
        var Task1_ = new TaskItem("Изучить C#", "Пройти курс основ C#", Priority.High, DateTime.Now.AddHours(2));
        var Task2_ = new TaskItem("Сделать ДЗ", "Написать консольное приложение", Priority.High, DateTime.Now.AddDays(1));
        var Task3_ = new TaskItem("Купить продукты", "Молоко, хлеб, яйца", Priority.Low, DateTime.Now.AddDays(3));
        var Task4_ = new TaskItem("Позвонить маме", "Узнать как дела", Priority.Medium, DateTime.Now.AddHours(-1));
        var Task5_ = new TaskItem("Заплатить за интернет", "Оплата до 25 числа", Priority.High, DateTime.Now.AddDays(-2));
        
        Manager_.AddTask(Task1_);
        Manager_.AddTask(Task2_);
        Manager_.AddTask(Task3_);
        Manager_.AddTask(Task4_);
        Manager_.AddTask(Task5_);
        
        Console.WriteLine($"Создано задач: {Manager_.GetTotalCount()}");
        Console.WriteLine();
        await Task.Delay(1000);
        
        Console.WriteLine("[ШАГ 2] Вывод всех задач...");
        await Task.Delay(500);
        ShowAllTasks();
        await Task.Delay(1000);
        
        Console.WriteLine("[ШАГ 3] Пометим задачу 'Сделать ДЗ' как выполненную...");
        await Task.Delay(500);
        var HomeworkTask_ = Manager_.GetTask(2);
        if (HomeworkTask_ != null) {
            HomeworkTask_.Status = Status.Done;
            Console.WriteLine($"Задача '{HomeworkTask_.Name}' теперь со статусом: {HomeworkTask_.Status}");
        }
        Console.WriteLine();
        await Task.Delay(1000);
        
        Console.WriteLine("[ШАГ 4] Пометим задачу 'Позвонить маме' как в работе...");
        await Task.Delay(500);
        var CallTask_ = Manager_.GetTask(4);
        if (CallTask_ != null) {
            CallTask_.Status = Status.InProgress;
            Console.WriteLine($"Задача '{CallTask_.Name}' теперь со статусом: {CallTask_.Status}");
        }
        Console.WriteLine();
        await Task.Delay(1000);
        
        Console.WriteLine("[ШАГ 5] Поиск по названию 'C#'...");
        await Task.Delay(500);
        var SearchResults_ = Manager_.SearchByName("C#");
        Console.WriteLine($"Найдено: {SearchResults_.Count}");
        foreach (var Task_ in SearchResults_) {
            Console.WriteLine($"  - {Task_.Name}");
        }
        Console.WriteLine();
        await Task.Delay(1000);
        
        Console.WriteLine("[ШАГ 6] Поиск по приоритету High...");
        await Task.Delay(500);
        var HighPriorityTasks_ = Manager_.SearchByPriority(Priority.High);
        Console.WriteLine($"Найдено задач с высоким приоритетом: {HighPriorityTasks_.Count}");
        foreach (var Task_ in HighPriorityTasks_) {
            Console.WriteLine($"  - {Task_.Name}");
        }
        Console.WriteLine();
        await Task.Delay(1000);
        
        Console.WriteLine("[ШАГ 7] Поиск по статусу Done...");
        await Task.Delay(500);
        var DoneTasks_ = Manager_.SearchByStatus(Status.Done);
        Console.WriteLine($"Выполнено задач: {DoneTasks_.Count}");
        foreach (var Task_ in DoneTasks_) {
            Console.WriteLine($"  - {Task_.Name}");
        }
        Console.WriteLine();
        await Task.Delay(1000);
        
        Console.WriteLine("[ШАГ 8] Статистика...");
        await Task.Delay(500);
        ShowStatistics();
        await Task.Delay(1000);
        
        Console.WriteLine("[ШАГ 9] Фильтр: только невыполненные задачи...");
        await Task.Delay(500);
        var IncompleteTasks_ = Manager_.GetIncompleteTasks();
        Console.WriteLine($"Невыполненных задач: {IncompleteTasks_.Count}");
        foreach (var Task_ in IncompleteTasks_) {
            Console.WriteLine($"  - {Task_.Name} [{Task_.Status}]");
        }
        Console.WriteLine();
        await Task.Delay(1000);
        
        Console.WriteLine("[ШАГ 10] Фильтр: просроченные задачи...");
        await Task.Delay(500);
        var AllTasks_ = Manager_.GetAllTasks();
        var OverdueTasks_ = AllTasks_.Where(t => t.IsOverdue()).ToList();
        Console.WriteLine($"Просроченных задач: {OverdueTasks_.Count}");
        foreach (var Task_ in OverdueTasks_) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"  ! {Task_.Name} (дедлайн: {Task_.Deadline:dd.MM.yyyy HH:mm})");
            Console.ResetColor();
        }
        Console.WriteLine();
        await Task.Delay(1000);
        
        Console.WriteLine("[ШАГ 11] Удаление задачи 'Купить продукты'...");
        await Task.Delay(500);
        Manager_.RemoveTask(3);
        Console.WriteLine($"Осталось задач: {Manager_.GetTotalCount()}");
        Console.WriteLine();
        await Task.Delay(1000);
        
        Console.WriteLine("[ШАГ 12] Сохранение в файл...");
        await Task.Delay(500);
        await Manager_.SaveAsync();
        Console.WriteLine("Задачи сохранены в tasks.json");
        Console.WriteLine();
        await Task.Delay(1000);
        
        Console.WriteLine("[ШАГ 13] Очистка и загрузка из файла...");
        await Task.Delay(500);
        await Manager_.LoadAsync();
        Console.WriteLine($"Загружено задач из файла: {Manager_.GetTotalCount()}");
        Console.WriteLine();
        await Task.Delay(1000);
        
        Console.WriteLine("[ШАГ 14] Итоговый список...");
        await Task.Delay(500);
        ShowAllTasks();
        
        Console.WriteLine();
        Console.WriteLine("=== ДЕМОНСТРАЦИЯ ЗАВЕРШЕНА ===");
        Console.WriteLine();
        Console.WriteLine("Нажмите Enter для возврата в меню...");
        Console.ReadLine();
    }
    
    private void ShowAllTasks() {
        var Tasks_ = Manager_.GetAllTasks();
        if (Tasks_.Count == 0) {
            Console.WriteLine("Задач нет");
            return;
        }
        
        foreach (var Task_ in Tasks_) {
            Console.WriteLine(Task_);
        }
        Console.WriteLine($"\nВсего: {Tasks_.Count}");
        Console.WriteLine();
    }
    
    private void ShowStatistics() {
        Console.WriteLine($"Всего задач: {Manager_.GetTotalCount()}");
        Console.WriteLine($"Выполнено: {Manager_.GetCompletedCount()}");
        Console.WriteLine($"Просрочено: {Manager_.GetOverdueCount()}");
        
        Console.WriteLine("По приоритетам:");
        var PriorityStats_ = Manager_.GetPriorityStats();
        foreach (var Stat_ in PriorityStats_) {
            Console.WriteLine($"  {Stat_.Key}: {Stat_.Value}");
        }
        Console.WriteLine();
    }
}
