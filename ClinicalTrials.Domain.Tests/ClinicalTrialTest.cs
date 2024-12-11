
using FakeItEasy;
using FluentAssertions;

namespace ClinicalTrials.Domain.Tests
{
    [TestClass]
    public class ClinicalTrialTest
    {
        [TestMethod]
        public void CalculateDuration_ShouldSetDurationInDays_WhenEndDateIsProvided()
        {
            // Arrange
            var trial = A.Fake<ClinicalTrial>();
            trial.StartDate = new DateTime(2024, 1, 1);
            trial.EndDate = new DateTime(2024, 1, 11);

            // Act
            trial.CalculateDuration();

            // Assert
            trial.DurationInDays.Should().Be(10);
        }

        [TestMethod]
        public void CalculateDuration_ShouldNotChangeDurationInDays_WhenEndDateIsNull()
        {
            // Arrange
            var trial = A.Fake<ClinicalTrial>();
            trial.StartDate = new DateTime(2024, 1, 1);
            trial.EndDate = null;

            // Act
            trial.CalculateDuration();

            // Assert
            trial.DurationInDays.Should().Be(0);
        }

        [TestMethod]
        public void SetDefaultEndDate_ShouldSetEndDateToOneMonthAfterStartDate_WhenStatusIsOngoingAndEndDateIsNull()
        {
            // Arrange
            var trial = A.Fake<ClinicalTrial>();
            trial.StartDate = new DateTime(2024, 1, 1);
            trial.Status = "Ongoing";
            trial.EndDate = null;

            // Act
            trial.SetDefaultEndDate();

            // Assert
            trial.EndDate.Should().Be(new DateTime(2024, 2, 1));
        }

        [TestMethod]
        public void SetDefaultEndDate_ShouldNotChangeEndDate_WhenEndDateIsAlreadySet()
        {
            // Arrange
            var trial = A.Fake<ClinicalTrial>();
            trial.StartDate = new DateTime(2024, 1, 1);
            trial.Status = "Ongoing";
            trial.EndDate = new DateTime(2024, 1, 15);

            // Act
            trial.SetDefaultEndDate();

            // Assert
            trial.EndDate.Should().Be(new DateTime(2024, 1, 15));
        }

        [TestMethod]
        public void SetDefaultEndDate_ShouldNotSetEndDate_WhenStatusIsNotOngoing()
        {
            // Arrange
            var trial = A.Fake<ClinicalTrial>();
            trial.StartDate = new DateTime(2023, 1, 1);
            trial.Status = "Completed";
            trial.EndDate = null;

            // Act
            trial.SetDefaultEndDate();

            // Assert
            trial.EndDate.Should().BeNull();
        }

        [TestMethod]
        public void IsDefaultObject_ShouldBeTrue()
        {
            // Arrange
            var trial = new ClinicalTrial();

            // Act
            var result = trial.IsDefaultObject();

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void IsDefaultObject_ShouldBeFalse()
        {
            // Arrange
            var trial = new ClinicalTrial();
            trial.StartDate = new DateTime(2023, 1, 1);
            trial.Status = "Completed";
            trial.Title = "Test";
            trial.TrialId = "1234";

            // Act
            var result = trial.IsDefaultObject();

            // Assert
            result.Should().BeFalse();
        }
    }
}
