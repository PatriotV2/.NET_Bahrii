using System;
using System.Collections.Generic;
using System.Linq;
using YehorBahrii.TaskPlanner.Domain.Models;
using YehorBahrii.TaskPlanner.DataAccess.Abstractions;

namespace YehorBahrii.TaskPlanner.Domain.Logic
{
    public class SimpleTaskPlanner
    {
        private readonly IWorkItemsRepository _repository;

        public SimpleTaskPlanner(IWorkItemsRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public WorkItem[] CreatePlan()
        {
            // Беремо всі завдання з репозиторію
            var items = _repository.GetAll();

            // Ігноруємо виконані завдання
            var pendingItems = items.Where(item => !item.IsCompleted).ToList();

            // Сортуємо за пріоритетом, датою та назвою
            pendingItems.Sort(CompareWorkItems);

            return pendingItems.ToArray();
        }

        private static int CompareWorkItems(WorkItem firstItem, WorkItem secondItem)
        {
            // 1. Priority (спаданням)
            int priorityCompare = secondItem.Priority.CompareTo(firstItem.Priority);
            if (priorityCompare != 0) return priorityCompare;

            // 2. DueDate (за зростанням)
            int dateCompare = firstItem.DueDate.CompareTo(secondItem.DueDate);
            if (dateCompare != 0) return dateCompare;

            // 3. Title (алфавітно)
            return string.Compare(firstItem.Title, secondItem.Title, StringComparison.OrdinalIgnoreCase);
        }
    }
}
