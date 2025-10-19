using NUnit.Framework;
using System;
using UniPlanner.Utils;

namespace UniPlanner.Tests
{
    /// <summary>
    /// NUnit tests for ValidationHelper
    /// </summary>
    [TestFixture]
    public class ValidationHelperTests
    {
        [Test]
        public void IsValidString_ShouldReturnTrue_ForValidString()
        {
            // Arrange
            string validString = "Test String";

            // Act
            bool result = ValidationHelper.IsValidString(validString, 1, 100);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void IsValidString_ShouldReturnFalse_ForEmptyString()
        {
            // Arrange
            string emptyString = "";

            // Act
            bool result = ValidationHelper.IsValidString(emptyString);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void IsValidString_ShouldReturnFalse_ForTooLongString()
        {
            // Arrange
            string longString = new string('a', 501);

            // Act
            bool result = ValidationHelper.IsValidString(longString, 1, 500);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void IsValidTimeFormat_ShouldReturnTrue_ForValidTime()
        {
            // Arrange
            string validTime = "14:30";

            // Act
            bool result = ValidationHelper.IsValidTimeFormat(validTime);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void IsValidTimeFormat_ShouldReturnFalse_ForInvalidTime()
        {
            // Arrange
            string invalidTime = "25:99";

            // Act
            bool result = ValidationHelper.IsValidTimeFormat(invalidTime);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void IsValidTimeRange_ShouldReturnTrue_ForValidRange()
        {
            // Arrange
            string startTime = "09:00";
            string endTime = "10:00";

            // Act
            bool result = ValidationHelper.IsValidTimeRange(startTime, endTime);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void IsValidTimeRange_ShouldReturnFalse_ForInvalidRange()
        {
            // Arrange
            string startTime = "10:00";
            string endTime = "09:00"; // End before start

            // Act
            bool result = ValidationHelper.IsValidTimeRange(startTime, endTime);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void IsValidFutureDate_ShouldReturnTrue_ForFutureDate()
        {
            // Arrange
            DateTime futureDate = DateTime.Today.AddDays(1);

            // Act
            bool result = ValidationHelper.IsValidFutureDate(futureDate);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void IsValidFutureDate_ShouldReturnFalse_ForPastDate()
        {
            // Arrange
            DateTime pastDate = DateTime.Today.AddDays(-1);

            // Act
            bool result = ValidationHelper.IsValidFutureDate(pastDate, allowToday: false);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void IsValidPriority_ShouldReturnTrue_ForHighPriority()
        {
            // Act
            bool result = ValidationHelper.IsValidPriority("High");

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void IsValidPriority_ShouldReturnFalse_ForInvalidPriority()
        {
            // Act
            bool result = ValidationHelper.IsValidPriority("Invalid");

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void IsValidDayOfWeek_ShouldReturnTrue_ForValidDay()
        {
            // Act
            bool result = ValidationHelper.IsValidDayOfWeek(1); // Monday

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void IsValidDayOfWeek_ShouldReturnFalse_ForInvalidDay()
        {
            // Act
            bool result = ValidationHelper.IsValidDayOfWeek(7); // Invalid

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void IsInRange_ShouldReturnTrue_ForValueInRange()
        {
            // Act
            bool result = ValidationHelper.IsInRange(5, 1, 10);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void IsInRange_ShouldReturnFalse_ForValueOutOfRange()
        {
            // Act
            bool result = ValidationHelper.IsInRange(15, 1, 10);

            // Assert
            Assert.IsFalse(result);
        }
    }
}

