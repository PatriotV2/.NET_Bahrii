using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using YehorBahrii.TaskPlanner.DataAccess.Abstractions;
using YehorBahrii.TaskPlanner.Domain.Models;

namespace YehorBahrii.TaskPlanner.DataAccess
{
    public class FileWorkItemsRepository : IWorkItemsRepository
    {
        private const string FileName = "work-items.json";

        // In-memory “таблиця”
        private readonly Dictionary<Guid, WorkItem> _items = new();

        // Конструктор: читає файл і завантажує у словник
        public FileWorkItemsRepository()
        {
            if (File.Exists(FileName))
            {
                string json = File.ReadAllText(FileName);

                if (!string.IsNullOrWhiteSpace(json))
                {
                    var itemsArray = JsonSerializer.Deserialize<WorkItem[]>(json);
                    if (itemsArray != null)
                    {
                        foreach (var item in itemsArray)
                        {
                            _items[item.Id] = item;
                        }
                    }
                }
            }
        }

        // Додає WorkItem, створюючи копію з новим Guid
        public Guid Add(WorkItem workItem)
        {
            // Створюємо копію об’єкта
            var copy = new WorkItem
            {
                Id = Guid.NewGuid(),
                Title = workItem.Title,
                Description = workItem.Description,
                CreationDate = workItem.CreationDate,
                DueDate = workItem.DueDate,
                Priority = workItem.Priority,
                Complexity = workItem.Complexity,
                IsCompleted = workItem.IsCompleted
            };

            _items[copy.Id] = copy;

            return copy.Id;
        }

        public WorkItem Get(Guid id)
        {
            _items.TryGetValue(id, out var item);
            return item;
        }

        public WorkItem[] GetAll()
        {
            return _items.Values.ToArray();
        }

        public bool Update(WorkItem workItem)
        {
            if (!_items.ContainsKey(workItem.Id))
                return false;

            _items[workItem.Id] = workItem;
            return true;
        }

        public bool Remove(Guid id)
        {
            return _items.Remove(id);
        }

        // Зберігає словник у JSON-файл
        public void SaveChanges()
        {
            var itemsArray = _items.Values.ToArray();
            string json = JsonSerializer.Serialize(itemsArray, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FileName, json);
        }
    }
}
