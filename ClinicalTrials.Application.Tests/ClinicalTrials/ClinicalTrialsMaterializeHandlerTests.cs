using ClinicalTrials.Application.ClinicalTrials;
using FluentAssertions;

namespace ClinicalTrials.Application.Tests.ClinicalTrials
{
    [TestClass]
    public class ClinicalTrialsMaterializeHandlerTests
    {
        private ClinicalTrialsMaterializeHandler? _testee;

        [TestInitialize]
        public void TestInitialize()
        {
            _testee = new ClinicalTrialsMaterializeHandler();
        }

        [TestMethod]
        public void Materialized_ShouldReturnNone_WhenJsonIsInvalid()
        {
            // Arrange
            var invalidJson = "{\"invalid\": \"json\"}";

            // Act
            var result = _testee!.Materialized(invalidJson);

            // Assert
            result.IsNone.Should().BeTrue();
        }

        [TestMethod]
        public void Materialized_ShouldReturnNone_WhenJsonIsEmpty()
        {
            // Arrange
            var emptyJson = "";

            // Act
            var result = () => _testee!.Materialized(emptyJson);

            // Assert
            result.Should().Throw<Exception>();
        }

        [TestMethod]
        public void Materialized_ShouldReturnNone_WhenJsonIsNull()
        {
            // Act
            var result = () => _testee!.Materialized(null);

            // Assert
            result.Should().Throw<Exception>();
        }

        [TestMethod]
        public void Materialized_ShouldReturnNone_WhenJsonIsMalformed()
        {
            // Arrange
            var malformedJson = """
                {
                    "trialId": "12345",
                    "title": "Trial A",
                    "startDate": "2024-01-01",
                    "participants": 12
                    "status": "Completed"
                }
                """;

            // Act
            var result = () => _testee!.Materialized(malformedJson);

            // Assert
            result.Should().Throw<Exception>();
        }

        [TestMethod]
        public void Materialized_ShouldReturnClinicalTrial_WhenJsonIsValid()
        {
            // Arrange
            var validJson = """
                {
                    "trialId": "12345",
                    "title": "Trial A",
                    "startDate": "2024-01-01",
                    "participants": 12,
                    "status": "Completed"
                }
                """;

            // Act
            var result = _testee!.Materialized(validJson);

            // Assert
            result.IsSome.Should().BeTrue();
            result.Value.TrialId.Should().Be("12345");
            result.Value.Title.Should().Be("Trial A");
            result.Value.Participants.Should().Be(12);
            result.Value.Status.Should().Be("Completed");
        }
    }
}
