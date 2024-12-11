
using ClinicalTrials.Application.ClinicalTrials;
using ClinicalTrials.Domain;
using FluentAssertions;

namespace ClinicalTrials.Application.Tests.ClinicalTrials
{
    [TestClass]

    public class ClinicalTrialsProcessingHandlerTests
    {
        private ClinicalTrialsProcessingHandler? _testee;

        [TestInitialize]
        public void TestInitialize()
        {
            _testee = new ClinicalTrialsProcessingHandler();
        }

        [TestMethod]
        public void ProcessClinicalTrial_ShouldSetDefaultEndDateAndCalculateDuration()
        {
            // Arrange
            var clinicalTrial = new ClinicalTrial
            {
                StartDate = DateTime.Now.Date,
                Status = "Ongoing"
            };
            var clinicalTrialresult = new ClinicalTrial
            {
                StartDate = DateTime.Now.Date,
                EndDate = DateTime.Now.Date.AddMonths(1),
                DurationInDays = (DateTime.Now.Date.AddMonths(1) - DateTime.Now.Date).Days,
                Status = "Ongoing"
            };

            // Act
            var result = _testee!.Process(clinicalTrial);

            // Assert
            result.EndDate.Should().Be(clinicalTrialresult.EndDate);
            result.DurationInDays.Should().Be(clinicalTrialresult.DurationInDays);
            result.Status.Should().Be(clinicalTrialresult.Status);
        }

        [TestMethod]
        public void ProcessClinicalTrial_ShouldNotSetDefaultEndDat()
        {
            // Arrange
            var clinicalTrial = new ClinicalTrial
            {
                StartDate = DateTime.Now.Date,
                Status = "Complete"
            };

            // Act
            var result = _testee!.Process(clinicalTrial);

            // Assert
            result.EndDate.Should().Be(clinicalTrial.EndDate);
            result.DurationInDays.Should().Be(clinicalTrial.DurationInDays);
            result.Status.Should().Be(clinicalTrial.Status);
        }
    }
}
