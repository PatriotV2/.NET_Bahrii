using System;
using System.Collections.Generic;
using YehorBahrii.TaskPlanner.Domain.Models;
using YehorBahrii.TaskPlanner.Domain.Models.Enums;
using YehorBahrii.TaskPlanner.Domain.Logic;

internal static class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("=== Task Planner ===\n");

        var workItems = new List<WorkItem>();
        var planner = new SimpleTaskPlanner();

        while (true)
        {
            Console.WriteLine("Виберiть дiю:");
            Console.WriteLine("1 - Додати завдання");
            Console.WriteLine("2 - Показати вiдсортований план");
            Console.WriteLine("0 - Вийти");
            Console.Write("Ваш вибiр: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddWorkItem(workItems);
                    break;

                case "2":
                    ShowSortedPlan(workItems, planner);
                    break;

                case "0":
                    Console.WriteLine("Роботу завершено. Натиснiть Enter для виходу.");
                    Console.ReadLine();
                    return;

                default:
                    Console.WriteLine("Невiрний вибiр, спробуйте ще раз.\n");
                    break;
            }
        }
    }

    private static void AddWorkItem(List<WorkItem> workItems)
    {
        Console.Write("Назва завдання: ");
        string title = Console.ReadLine();

        Console.Write("Прiоритет (None, Low, Medium, High, Urgent): ");
        string priorityInput = Console.ReadLine();
        var priority = Enum.Parse<Priority>(priorityInput, ignoreCase: true);

        Console.Write("Складнiсть (None, Low, Medium, High): ");
        string complexityInput = Console.ReadLine();
        var complexity = Enum.Parse<Complexity>(complexityInput, ignoreCase: true);

        Console.Write("Дата виконання (yyyy-MM-dd): ");
        string dateInput = Console.ReadLine();
        DateTime dueDate = DateTime.Parse(dateInput);

        workItems.Add(new WorkItem
        {
            Title = title,
            Priority = priority,
            Complexity = complexity,
            DueDate = dueDate,
            CreationDate = DateTime.Now
        });

        Console.WriteLine("Завдання додано!\n");
    }

    private static void ShowSortedPlan(List<WorkItem> workItems, SimpleTaskPlanner planner)
    {
        if (workItems.Count == 0)
        {
            Console.WriteLine("Список завдань порожнiй.\n");
            return;
        }

        var sortedItems = planner.CreatePlan(workItems.ToArray());

        Console.WriteLine("\n=== Вiдсортований план ===");
        foreach (var item in sortedItems)
        {
            Console.WriteLine($"{item.Title} | Прiоритет: {item.Priority} | Складнiсть: {item.Complexity} | Дата: {item.DueDate:yyyy-MM-dd}");
        }
        Console.WriteLine();
    }
}
