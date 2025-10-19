using NUnit.Framework;
using System;
using System.Linq;
using UniPlanner.Models;
using UniPlanner.Services;

namespace UniPlanner.Tests
{
    /// <summary>
    /// NUnit tests for TaskService
    /// </summary>
    [TestFixture]
    public class TaskServiceTests
    {
        private TaskService _service;

        [SetUp]
        public void Setup()
        {
            _service = new TaskService();
        }

        [Test]
        public void GetAll_ShouldReturnList()
        {
            // Arrange & Act
            var result = _service.GetAll();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<System.Collections.Generic.IReadOnlyList<TaskItem>>(result);
        }

        [Test]
        public void Add_ShouldIncreaseCount()
        {
            // Arrange
            int initialCount = _service.GetAll().Count;
            var newTask = new TaskItem
            {
                Title = "Test Task",
                DueDate = DateTime.Today.AddDays(1),
                Priority = "High",
                SubjectCode = "TEST",
                Description = "Test Description"
            };

            // Act
            _service.Add(newTask);
            int finalCount = _service.GetAll().Count;

            // Assert
            Assert.AreEqual(initialCount + 1, finalCount);
        }

        [Test]
        public void GetUpcomingOrdered_ShouldReturnOnlyFutureTasks()
        {
            // Act
            var result = _service.GetUpcomingOrdered();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.All(t => t.DueDate >= DateTime.Today));
            Assert.IsTrue(result.All(t => !t.IsCompleted));
        }

        [Test]
        public void GetOverdue_ShouldReturnOnlyOverdueTasks()
        {
            // Act
            var result = _service.GetOverdue();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.All(t => t.IsOverdue()));
        }

        [Test]
        public void MarkComplete_ShouldSetIsCompletedToTrue()
        {
            // Arrange
            var task = new TaskItem
            {
                Title = "Mark Complete Test",
                DueDate = DateTime.Today,
                Priority = "Medium"
            };
            _service.Add(task);
            
            var addedTask = _service.GetAll().FirstOrDefault(t => t.Title == "Mark Complete Test");
            Assert.IsNotNull(addedTask);

            // Act
            _service.MarkComplete(addedTask.Id);

            // Assert
            var completedTask = _service.GetById(addedTask.Id);
            Assert.IsTrue(completedTask.IsCompleted);
        }

        [Test]
        public void TaskItem_Validate_ShouldReturnTrueForValidTask()
        {
            // Arrange
            var task = new TaskItem
            {
                Title = "Valid Task",
                DueDate = DateTime.Today,
                Priority = "High"
            };

            // Act
            bool isValid = task.Validate();

            // Assert
            Assert.IsTrue(isValid);
        }

        [Test]
        public void TaskItem_Validate_ShouldReturnFalseForInvalidTitle()
        {
            // Arrange
            var task = new TaskItem
            {
                Title = "",
                DueDate = DateTime.Today,
                Priority = "High"
            };

            // Act
            bool isValid = task.Validate();

            // Assert
            Assert.IsFalse(isValid);
        }

        [Test]
        public void TaskItem_GetDisplayInfo_ShouldReturnFormattedString()
        {
            // Arrange
            var task = new TaskItem
            {
                Title = "Test Display",
                DueDate = DateTime.Today,
                Priority = "Medium",
                IsCompleted = false
            };

            // Act
            string displayInfo = task.GetDisplayInfo();

            // Assert
            Assert.IsNotNull(displayInfo);
            Assert.IsTrue(displayInfo.Contains("Test Display"));
            Assert.IsTrue(displayInfo.Contains("Medium"));
        }

        [TearDown]
        public void TearDown()
        {
            // Cleanup: Remove test tasks if needed
            // In a real scenario, you might want to use a test database
        }
    }
}

