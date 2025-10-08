using System;
using System.Collections.Generic;
using System.Linq;
using YehorBahrii.TaskPlanner.Domain.Models; // Бачити WorkItem
using YehorBahrii.TaskPlanner.Domain.Models.Enums; // Бачити Priority та Complexity
namespace YehorBahrii.TaskPlanner.Domain.Logic
{
    public class SimpleTaskPlanner
    {
        public WorkItem[] CreatePlan(WorkItem[] items)
        {
            // Перетворюємо масив у список
            var itemsAsList = items.ToList();

            // Сортуємо
            itemsAsList.Sort(CompareWorkItems);

            // Повертаємо назад як масив
            return itemsAsList.ToArray();
        }

        private static int CompareWorkItems(WorkItem firstItem, WorkItem secondItem)
        {
            // 1️ Порівнюємо Priority (за спаданням: спершу важливіше)
            int priorityCompare = secondItem.Priority.CompareTo(firstItem.Priority);
            if (priorityCompare != 0)
                return priorityCompare;

            // 2️ Якщо Priority однаковий, порівнюємо DueDate (за зростанням)
            int dateCompare = firstItem.DueDate.CompareTo(secondItem.DueDate);
            if (dateCompare != 0)
                return dateCompare;

            // 3️ Якщо і дата однакова — порівнюємо Title (алфавітно)
            return string.Compare(firstItem.Title, secondItem.Title, StringComparison.OrdinalIgnoreCase);
        }
    }
}
