using NUnit.Framework;
using System;
using System.Linq;
using UniPlanner.Models;
using UniPlanner.Services;

namespace UniPlanner.Tests
{
    /// <summary>
    /// NUnit tests for ScheduleService
    /// </summary>
    [TestFixture]
    public class ScheduleServiceTests
    {
        private ScheduleService _service;

        [SetUp]
        public void Setup()
        {
            _service = new ScheduleService();
        }

        [Test]
        public void GetAll_ShouldReturnList()
        {
            // Act
            var result = _service.GetAll();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<System.Collections.Generic.IReadOnlyList<ScheduleItem>>(result);
        }

        [Test]
        public void GetByDay_ShouldReturnOnlySpecifiedDay()
        {
            // Arrange
            int monday = 1;

            // Act
            var result = _service.GetByDay(monday);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.All(s => s.DayOfWeek == monday));
        }

        [Test]
        public void GetAllOrdered_ShouldReturnOrderedList()
        {
            // Act
            var result = _service.GetAllOrdered();

            // Assert
            Assert.IsNotNull(result);
            // Verify ordering (day first, then time)
            for (int i = 0; i < result.Count - 1; i++)
            {
                var current = result[i];
                var next = result[i + 1];
                
                if (current.DayOfWeek == next.DayOfWeek)
                {
                    // If same day, check time ordering
                    if (TimeSpan.TryParse(current.StartTime, out var currentTime) &&
                        TimeSpan.TryParse(next.StartTime, out var nextTime))
                    {
                        Assert.LessOrEqual(currentTime, nextTime);
                    }
                }
                else
                {
                    // Different days should be in order
                    Assert.LessOrEqual(current.DayOfWeek, next.DayOfWeek);
                }
            }
        }

        [Test]
        public void ScheduleItem_Validate_ShouldReturnTrueForValidSchedule()
        {
            // Arrange
            var schedule = new ScheduleItem
            {
                DayOfWeek = 1,
                SubjectCode = "COMP101",
                StartTime = "09:00",
                EndTime = "10:00",
                Location = "CB11.07.115"
            };

            // Act
            bool isValid = schedule.Validate();

            // Assert
            Assert.IsTrue(isValid);
        }

        [Test]
        public void ScheduleItem_Validate_ShouldReturnFalseForInvalidTimeRange()
        {
            // Arrange
            var schedule = new ScheduleItem
            {
                DayOfWeek = 1,
                SubjectCode = "COMP101",
                StartTime = "10:00",
                EndTime = "09:00", // End before start
                Location = "CB11.07.115"
            };

            // Act
            bool isValid = schedule.Validate();

            // Assert
            Assert.IsFalse(isValid);
        }

        [Test]
        public void ScheduleItem_GetDurationHours_ShouldCalculateCorrectDuration()
        {
            // Arrange
            var schedule = new ScheduleItem
            {
                StartTime = "09:00",
                EndTime = "11:00"
            };

            // Act
            double duration = schedule.GetDurationHours();

            // Assert
            Assert.AreEqual(2.0, duration);
        }

        [Test]
        public void ScheduleItem_GetDisplayInfo_ShouldReturnFormattedString()
        {
            // Arrange
            var schedule = new ScheduleItem
            {
                DayOfWeek = 1, // Monday
                SubjectCode = "COMP101",
                SubjectName = "Introduction to Programming",
                StartTime = "09:00",
                EndTime = "10:00",
                Location = "CB11.07.115"
            };

            // Act
            string displayInfo = schedule.GetDisplayInfo();

            // Assert
            Assert.IsNotNull(displayInfo);
            Assert.IsTrue(displayInfo.Contains("Monday"));
            Assert.IsTrue(displayInfo.Contains("09:00"));
            Assert.IsTrue(displayInfo.Contains("CB11.07.115"));
        }

        [TearDown]
        public void TearDown()
        {
            // Cleanup if needed
        }
    }
}

