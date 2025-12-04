using System;
using YehorBahrii.TaskPlanner.Domain.Models;
using YehorBahrii.TaskPlanner.Domain.Models.Enums;
using YehorBahrii.TaskPlanner.Domain.Logic;
using YehorBahrii.TaskPlanner.DataAccess;

internal static class Program
{
    public static void Main(string[] args)
    {
        var repository = new FileWorkItemsRepository();
        var planner = new SimpleTaskPlanner(repository);

        while (true)
        {
            Console.WriteLine("\n=== Task Planner ===");
            Console.WriteLine("[A]dd work item");
            Console.WriteLine("[B]uild a plan");
            Console.WriteLine("[M]ark work item as completed");
            Console.WriteLine("[R]emove a work item");
            Console.WriteLine("[Q]uit the app");
            Console.Write("Виберiть дiю: ");

            string choice = Console.ReadLine()?.ToUpper();

            switch (choice)
            {
                case "A":
                    AddWorkItem(repository);
                    break;
                case "B":
                    BuildPlan(planner);
                    break;
                case "M":
                    MarkCompleted(repository);
                    break;
                case "R":
                    RemoveWorkItem(repository);
                    break;
                case "Q":
                    repository.SaveChanges();
                    Console.WriteLine("Роботу завершено. Натисніть Enter для виходу.");
                    Console.ReadLine();
                    return;
                default:
                    Console.WriteLine("Невiдома команда. Спробуйте ще раз.");
                    break;
            }
        }
    }

    private static void AddWorkItem(FileWorkItemsRepository repo)
    {
        Console.Write("Назва завдання: ");
        string title = Console.ReadLine();

        Console.Write("Прiоритет (None, Low, Medium, High, Urgent): ");
        var priority = Enum.Parse<Priority>(Console.ReadLine()!, true);

        Console.Write("Складнiсть (None, Low, Medium, Hard, Impossible): ");
        var complexity = Enum.Parse<Complexity>(Console.ReadLine()!, true);

        Console.Write("Дата виконання (yyyy-MM-dd): ");
        DateTime dueDate = DateTime.Parse(Console.ReadLine()!);

        var workItem = new WorkItem
        {
            Title = title,
            Priority = priority,
            Complexity = complexity,
            CreationDate = DateTime.Now,
            DueDate = dueDate,
            IsCompleted = false
        };

        var id = repo.Add(workItem);
        repo.SaveChanges();

        Console.WriteLine($"Завдання додано! Id: {id}");
    }

    private static void BuildPlan(SimpleTaskPlanner planner)
    {
        var sorted = planner.CreatePlan();
        if (sorted.Length == 0)
        {
            Console.WriteLine("Список завдань порожнiй.");
            return;
        }

        Console.WriteLine("\n=== Вiдсортований план ===");
        foreach (var item in sorted)
        {
            string status = item.IsCompleted ? "Виконано" : "Не виконано";
            Console.WriteLine($"{item.Id} | {item.Title} | Прiоритет: {item.Priority} | Складнiсть: {item.Complexity} | Дата: {item.DueDate:yyyy-MM-dd} | {status}");
        }
    }

    private static void MarkCompleted(FileWorkItemsRepository repo)
    {
        Console.Write("Введiть Id завдання для позначення як виконаного: ");
        if (!Guid.TryParse(Console.ReadLine(), out var id))
        {
            Console.WriteLine("Невiрний формат Id.");
            return;
        }

        var item = repo.Get(id);
        if (item != null)
        {
            item.IsCompleted = true;
            repo.Update(item);
            Console.WriteLine("Завдання позначене як виконане");
        }
        else
        {
            Console.WriteLine("Завдання не знайдено");
        }
    }

    private static void RemoveWorkItem(FileWorkItemsRepository repo)
    {
        Console.Write("Введiть Id завдання для видалення: ");
        if (!Guid.TryParse(Console.ReadLine(), out var id))
        {
            Console.WriteLine("Невiрний формат Id.");
            return;
        }

        if (repo.Remove(id))
            Console.WriteLine("Завдання видалене");
        else
            Console.WriteLine("Завдання не знайдено");
    }
}
