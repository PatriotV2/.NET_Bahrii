using System;
using System.Collections.Generic;
using Moq;
using Xunit;
using YehorBahrii.TaskPlanner.Domain.Logic;
using YehorBahrii.TaskPlanner.DataAccess.Abstractions;
using YehorBahrii.TaskPlanner.Domain.Models;
using YehorBahrii.TaskPlanner.Domain.Models.Enums;

namespace YehorBahrii.TaskPlanner.Domain.Logic.Tests
{
    public class SimpleTaskPlannerTests
    {
        [Fact]
        public void CreatePlan_ShouldSortTasksByPriorityDueDateAndTitle_AndIgnoreCompleted()
        {
            // 🔹 Arrange: створюємо мок репозиторію
            var mockRepo = new Mock<IWorkItemsRepository>();

            // Створюємо список завдань для тесту
            var tasks = new List<WorkItem>
            {
                new WorkItem { Title = "Task C", Priority = Priority.Medium, DueDate = new DateTime(2025,10,10), IsCompleted = false },
                new WorkItem { Title = "Task A", Priority = Priority.High, DueDate = new DateTime(2025,10,5), IsCompleted = false },
                new WorkItem { Title = "Task B", Priority = Priority.High, DueDate = new DateTime(2025,10,5), IsCompleted = true }, // має бути ігнорована
                new WorkItem { Title = "Task D", Priority = Priority.Low, DueDate = new DateTime(2025,10,12), IsCompleted = false },
            };

            // 🔹 Перетворюємо список у масив (щоб збігався тип)
            mockRepo.Setup(r => r.GetAll()).Returns(tasks.ToArray());

            // Створюємо SimpleTaskPlanner з мок-репозиторієм
            var planner = new SimpleTaskPlanner(mockRepo.Object);

            // 🔹 Act
            var plan = planner.CreatePlan();

            // 🔹 Assert
            // 1. Всі завершені завдання ігноруються
            Assert.DoesNotContain(plan, t => t.IsCompleted);

            // 2. Кількість завдань правильна (3 невиконані)
            Assert.Equal(3, plan.Length);

            // 3. Перевірка порядку сортування (Priority -> DueDate -> Title)
            Assert.Equal("Task A", plan[0].Title); // High, earliest date
            Assert.Equal("Task C", plan[1].Title); // Medium
            Assert.Equal("Task D", plan[2].Title); // Low
        }

        [Fact]
        public void CreatePlan_ShouldReturnEmptyArray_WhenAllTasksCompleted()
        {
            // 🔹 Arrange
            var mockRepo = new Mock<IWorkItemsRepository>();

            var tasks = new List<WorkItem>
            {
                new WorkItem { Title = "Task A", IsCompleted = true },
                new WorkItem { Title = "Task B", IsCompleted = true }
            };

            mockRepo.Setup(r => r.GetAll()).Returns(tasks.ToArray());

            var planner = new SimpleTaskPlanner(mockRepo.Object);

            // 🔹 Act
            var plan = planner.CreatePlan();

            // 🔹 Assert
            Assert.Empty(plan); // всі завдання завершені → план порожній
        }
    }
}
