
using System.Text;
using System.Text.Json;
using ClinicalTrials.Application.ClinicalTrials;
using ClinicalTrials.Application.Helpers;
using ClinicalTrials.Application.Validations;
using ClinicalTrials.Domain;
using FakeItEasy;
using FluentAssertions;
using Newtonsoft.Json;

namespace ClinicalTrials.Application.Tests.ClinicalTrials
{
    [TestClass]
    public class ClinicalTrialsServicesTests
    {
        private  IClinicalTrialsRepository? _clinicalTrialsRepository;
        private  IFileValidation? _fileValidation;
        private  IJsonSchemValidation? _jsonSchemValidation;
        private  IClinicalTrialsProcessingHandler? _processingHandler;
        private IClinicalTrialsMaterializeHandler? _materializeHandler;
        private  TestLogger<ClinicalTrialsServices>? _logger;
        private  ClinicalTrialsServices? _testee;

        [TestInitialize]
        public void TestInitialize()
        {

            _clinicalTrialsRepository = A.Fake<IClinicalTrialsRepository>();
            _fileValidation = A.Fake<IFileValidation>();
            _jsonSchemValidation = A.Fake<IJsonSchemValidation>();
            _processingHandler = A.Fake<IClinicalTrialsProcessingHandler>();
            _materializeHandler = A.Fake<IClinicalTrialsMaterializeHandler>();
            _logger = new TestLogger<ClinicalTrialsServices>();

            _testee = new ClinicalTrialsServices(
                _clinicalTrialsRepository,
                _fileValidation,
                _jsonSchemValidation,
                _processingHandler,
                _materializeHandler,
                _logger
            );
        }

        [TestMethod]
        public async Task GetTrialByIdAsync_ShouldReturnTrial_WhenFound()
        {
            // Arrange
            var trialId = "trial123";
            var expectedTrial = new ClinicalTrial { TrialId = trialId };
            A.CallTo(() =>  _clinicalTrialsRepository!.RetrieveByIdAsync(trialId))
                .Returns(expectedTrial);

            // Act
            var result = await _testee!.GetTrialByIdAsync(trialId);

            // Assert
            result.Should().BeEquivalentTo(expectedTrial);
        }

        [TestMethod]
        public async Task GetTrialByIdAsync_ShouldLogErrorAndThrow_WhenExceptionOccurs()
        {
            // Arrange
            var trialId = "trial123";
            A.CallTo(() => _clinicalTrialsRepository!.RetrieveByIdAsync(trialId))
                .Throws<Exception>();

            // Act
            var result = async () => await _testee!.GetTrialByIdAsync(trialId);

            // Assert
            await result.Should().ThrowAsync<Exception>();
        }

        [TestMethod]
        public async Task GetClinicalTrialsByFilterOptionsAsync_FilterByStatus_ShouldReturnTrials_WhenFound()
        {
            // Arrange
            var filter = new ClinicalTrialFilter
            {
                Status = "Completed"
            };
            var expectedTrials = new List<ClinicalTrial>
            {
                new ClinicalTrial 
                { 
                    TrialId = "12434",
                    Title = "Test",
                    Participants = 12,
                    StartDate = DateTime.Now.Date,
                    Status = "Completed",
                }
            };
            A.CallTo(() => _clinicalTrialsRepository!.RetrieveByFilterOprionsAsync(filter)).Returns(expectedTrials.ToList());

            // Act
            var result = await _testee!.GetClinicalTrialsByFilterOptionsAsync(filter);

            // Assert
            result.Should().NotBeNull();
            result.Count().Should().Be(expectedTrials.Count());
            result.First().Title.Should().Be(expectedTrials.First().Title);
            result.First().TrialId.Should().Be(expectedTrials.First().TrialId);
            result.First().Participants.Should().Be(expectedTrials.First().Participants);
            result.First().StartDate.Should().Be(expectedTrials.First().StartDate);
            result.First().Status.Should().BeEquivalentTo(expectedTrials.First().Status);
        }

        [TestMethod]
        public async Task GetClinicalTrialsByFilterOptionsAsync_FilterByParticipants_ShouldReturnTrials_WhenFound()
        {
            // Arrange
            var filter = new ClinicalTrialFilter
            {
                Participants = 10
            };
            var expectedTrials = new List<ClinicalTrial>
            {
                new ClinicalTrial
                {
                    TrialId = "12434",
                    Title = "Test",
                    Participants = 12,
                    StartDate = DateTime.Now.Date,
                    Status = "Completed",
                }
            };
            A.CallTo(() => _clinicalTrialsRepository!.RetrieveByFilterOprionsAsync(filter)).Returns(expectedTrials.ToList());

            // Act
            var result = await _testee!.GetClinicalTrialsByFilterOptionsAsync(filter);

            // Assert
            result.Should().NotBeNull();
            result.Count().Should().Be(1);
            result.First().Title.Should().Be(expectedTrials.First().Title);
            result.First().TrialId.Should().Be(expectedTrials.First().TrialId);
            result.First().Participants.Should().Be(expectedTrials.First().Participants);
            result.First().StartDate.Should().Be(expectedTrials.First().StartDate);
            result.First().Status.Should().BeEquivalentTo(expectedTrials.First().Status);
        }

        [TestMethod]
        public async Task GetClinicalTrialsByFilterOptionsAsync_FilterByParticipantsAndParticipantOperator_ShouldReturnTrials_WhenFound()
        {
            // Arrange
            var filter = new ClinicalTrialFilter
            {
                Participants = 20,
                ParticipantOperator = "lt"
            };
            var expectedTrials = new List<ClinicalTrial>
            {
                new ClinicalTrial
                {
                    TrialId = "12434",
                    Title = "Test",
                    Participants = 12,
                    StartDate = DateTime.Now.Date,
                    Status = "Completed",
                },
                new ClinicalTrial
                {
                    TrialId = "4235",
                    Title = "Test 2",
                    Participants = 9,
                    StartDate = DateTime.Now.Date,
                    Status = "Completed",
                }
            };
            A.CallTo(() => _clinicalTrialsRepository!.RetrieveByFilterOprionsAsync(filter)).Returns(expectedTrials.ToList());

            // Act
            var result = await _testee!.GetClinicalTrialsByFilterOptionsAsync(filter);

            // Assert
            result.Should().NotBeNull();
            result.Count().Should().Be(2);
            result.First().Title.Should().Be(expectedTrials.First().Title);
            result.First().TrialId.Should().Be(expectedTrials.First().TrialId);
            result.First().Participants.Should().Be(expectedTrials.First().Participants);
            result.First().StartDate.Should().Be(expectedTrials.First().StartDate);
            result.First().Status.Should().BeEquivalentTo(expectedTrials.First().Status);
        }

        [TestMethod]
        public async Task HandleClinicalTrialAsync_ShouldReturnError_WhenFileSizeIsInvalid()
        {
            // Arrange
            var fileName = "test.json";
            var fileContent = new MemoryStream(new byte[10000000]); // Larger than the allowed size
            A.CallTo(() => _fileValidation!.IsValidFileSize(fileContent.Length)).Returns(false);

            // Act
            var result = await _testee!.HandleClinicalTrialAsync(fileName, fileContent);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("Invalid file size");
        }

        [TestMethod]
        public async Task HandleClinicalTrialAsync_ShouldReturnError_WhenFileExtensionIsInvalid()
        {
            // Arrange
            var fileName = "test.exe"; // Invalid extension
            var fileContent = new MemoryStream();
            A.CallTo(() => _fileValidation!.IsValidFileSize(fileContent.Length)).Returns(true);
            A.CallTo(() => _fileValidation!.IsAllowedExtension(fileName)).Returns(false);

            // Act
            var result = await _testee!.HandleClinicalTrialAsync(fileName, fileContent);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("File extension is not allowed");
        }

        [TestMethod]
        public async Task HandleClinicalTrialAsync_ShouldReturnError_WhenFileIsEmpty()
        {
            // Arrange
            var fileName = "test.json";
            var fileContent = new MemoryStream();
            var invalidJson = "";
            A.CallTo(() => _fileValidation!.IsValidFileSize(fileContent.Length)).Returns(true);
            A.CallTo(() => _fileValidation!.IsAllowedExtension(fileName)).Returns(true);

            // Act
            var result = await _testee!.HandleClinicalTrialAsync(fileName, fileContent);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("Corrupted file");
        }

        [TestMethod]
        public async Task HandleClinicalTrialAsync_ShouldReturnError_WhenSchemaIsInvalid()
        {
            // Arrange
            var fileName = "test.json";
            var fileContent = new MemoryStream();
            var invalidJson = "{\"invalid\": \"json\"}";
            var jsonBytes = Encoding.UTF8.GetBytes(invalidJson);
            fileContent.Write(jsonBytes, 0, jsonBytes.Length);
            fileContent.Seek(0, SeekOrigin.Begin);
            A.CallTo(() => _fileValidation!.IsValidFileSize(fileContent.Length)).Returns(true);
            A.CallTo(() => _fileValidation!.IsAllowedExtension(fileName)).Returns(true);
            A.CallTo(() => _jsonSchemValidation!.IsSchemaValid(A<string>._, A<string>._)).Returns(false);

            // Act
            var result = await _testee!.HandleClinicalTrialAsync(fileName, fileContent);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("Invalid JSON schema");
        }

        [TestMethod]
        public async Task HandleClinicalTrialAsync_ShouldReturnError_WhenMaterializationFailed()
        {
            // Arrange
            var fileName = "test.json";
            var fileContent = new MemoryStream();
            var invalidJson = "{\"invalid\": \"json\"}";
            var jsonBytes = Encoding.UTF8.GetBytes(invalidJson);
            fileContent.Write(jsonBytes, 0, jsonBytes.Length);
            fileContent.Seek(0, SeekOrigin.Begin);
    
                A.CallTo(() => _fileValidation!.IsValidFileSize(fileContent.Length)).Returns(true);
            A.CallTo(() => _fileValidation!.IsAllowedExtension(fileName)).Returns(true);
            A.CallTo(() => _jsonSchemValidation!.IsSchemaValid(A<string>._, A<string>._)).Returns(true);
            A.CallTo(() => _materializeHandler!.Materialized(invalidJson)).Returns(Option<ClinicalTrial>.None());

            // Act
            var result = await _testee!.HandleClinicalTrialAsync(fileName, fileContent);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("Invalid JSON structure.");
        }

        [TestMethod]
        public async Task HandleClinicalTrialAsync_ShouldReturnSuccess_WhenValidDataIsProcessed()
        {
            // Arrange
            var fileName = "test.json";
            var fileContent = new MemoryStream();
            var trial = new ClinicalTrial
            {
                TrialId = "12345",
                Title = "Test",
                StartDate = Convert.ToDateTime("2024-01-01"),
                Participants = 12,
                Status = "Ongoing"
            };
            var validJson = """
                {
                    "trialId": "12345",
                    "title": "Trial A",
                    "startDate": "2024-01-01",
                    "participants": 12,
                    "status": "Completed"
                }
                """;
            var jsonBytes = Encoding.UTF8.GetBytes(validJson);
            fileContent.Write(jsonBytes, 0, jsonBytes.Length);
            fileContent.Seek(0, SeekOrigin.Begin);

            A.CallTo(() => _fileValidation!.IsValidFileSize(fileContent.Length)).Returns(true);
            A.CallTo(() => _fileValidation!.IsAllowedExtension(fileName)).Returns(true);
            A.CallTo(() => _jsonSchemValidation!.IsSchemaValid(A<string>._, A<string>._)).Returns(true);
            A.CallTo(() => _materializeHandler!.Materialized(validJson)).Returns(Option<ClinicalTrial>.Some(trial));
            A.CallTo(() => _processingHandler!.Process(A<ClinicalTrial>._)).Returns(trial);
            A.CallTo(() => _clinicalTrialsRepository!.InsertAsync(A<ClinicalTrial>._)).Returns(Task.CompletedTask);

            // Act
            var result = await _testee!.HandleClinicalTrialAsync(fileName, fileContent);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }
    }
}
