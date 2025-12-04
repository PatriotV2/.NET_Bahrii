using System;
using YehorBahrii.TaskPlanner.Domain.Models.Enums;

namespace YehorBahrii.TaskPlanner.Domain.Models
{
    public class WorkItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();   // Унікальний ID

        public DateTime CreationDate { get; set; }
        public DateTime DueDate { get; set; }
        public Priority Priority { get; set; }
        public Complexity Complexity { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }

        public WorkItem Clone()
        {
            return new WorkItem
            {
                Id = Guid.NewGuid(), // Новий унікальний ID для копії!
                CreationDate = this.CreationDate,
                DueDate = this.DueDate,
                Priority = this.Priority,
                Complexity = this.Complexity,
                Title = this.Title,
                Description = this.Description,
                IsCompleted = this.IsCompleted
            };
        }

        public override string ToString()
        {
            return $"{Title}: due {DueDate:dd.MM.yyyy}, {Priority.ToString().ToLower()} priority";
        }
    }
}
