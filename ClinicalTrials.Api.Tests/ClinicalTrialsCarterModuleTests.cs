
using ClinicalTrials.Api.Features;
using ClinicalTrials.Application.ClinicalTrials;
using ClinicalTrials.Domain;
using FakeItEasy;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using FluentAssertions;
using ClinicalTrials.Application.Helpers;

namespace ClinicalTrials.Api.Tests
{

    [TestClass]
    public class ClinicalTrialsCarterModuleTests
    {
        private readonly ILogger<ClinicalTrialsCarterModule> _logger = A.Fake<ILogger<ClinicalTrialsCarterModule>>();
        private readonly IClinicalTrialsServices _clinicalTrialsServices = A.Fake<IClinicalTrialsServices>();
        private readonly ClinicalTrialsCarterModule _module = new ClinicalTrialsCarterModule();

        [TestMethod]
        public async Task GetClinicalTrialsById_ReturnsOk_WhenTrialExists()
        {
            // Arrange
            var trialId = "123";
            var trial = new ClinicalTrial
            {
                TrialId = trialId,
                Title = "test",
                Participants = 10,
                StartDate = DateTime.Now.Date,
                Status = "Ongoing"
            };
            A.CallTo(() => _clinicalTrialsServices.GetTrialByIdAsync(trialId)).Returns(Task.FromResult(trial));

            // Act
            var result = await _module.GetClinicalTrialsById(trialId, _clinicalTrialsServices, _logger, CancellationToken.None);

            // Assert
            result.Should().BeOfType<Results<BadRequest<string>, Ok<ClinicalTrial>>>();
            var okResult = result.Result.As<Ok<ClinicalTrial>>();
            okResult.Value.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);
            okResult.Value.TrialId.Should().Be(trial.TrialId);
            okResult.Value.Title.Should().Be(trial.Title);
            okResult.Value.Participants.Should().Be(trial.Participants);
            okResult.Value.StartDate.Should().Be(trial.StartDate);
            okResult.Value.Status.Should().Be(trial.Status);
        }

        [TestMethod]
        public async Task GetClinicalTrialsById_ReturnsBadRequest_WhenExceptionOccurs()
        {
            // Arrange
            var trialId = "123";
            A.CallTo(() => _clinicalTrialsServices.GetTrialByIdAsync(trialId)).Throws(new Exception("Error"));

            // Act
            var result = await _module.GetClinicalTrialsById(trialId, _clinicalTrialsServices, _logger, CancellationToken.None);

            // Assert
            result.Should().BeOfType<Results<BadRequest<string>, Ok<ClinicalTrial>>>();

            var badRequest = result.Result.As<BadRequest<string>>();
            badRequest.Value.Should().NotBeNull();
            badRequest.StatusCode.Should().Be(400);
        }

        [TestMethod]
        public async Task GetClinicalTrialsByFilterOptions_ReturnsOk_WhenFilteredTrialsExist()
        {
            // Arrange
            var filter = new ClinicalTrialFilter { Status = "Ongoing" };
            var trials = new List<ClinicalTrial>
            {
                new ClinicalTrial
                {
                    TrialId = "123",
                    Title = "test",
                    Participants = 10,
                    StartDate = DateTime.Now.Date,
                    Status = "Ongoing"
                }
            };
            A.CallTo(() => _clinicalTrialsServices.GetClinicalTrialsByFilterOptionsAsync(A<ClinicalTrialFilter>._))
                .Returns(Task.FromResult<IEnumerable<ClinicalTrial>>(trials));

            // Act
            var result = await _module.GetClinicalTrialsByFilterOptions("Ongoing", null, null, null, null, _clinicalTrialsServices, _logger, CancellationToken.None);

            // Assert
            result.Should().BeOfType<Results<BadRequest<string>, Ok<IEnumerable<ClinicalTrial>>>>();
            var okResult = result.Result.As<Ok<IEnumerable<ClinicalTrial>>>();
            okResult.Value.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().HaveCount(1);
            okResult.Value.ToList().First().TrialId.Should().Be(trials.First().TrialId);
            okResult.Value.ToList().First().Title.Should().Be(trials.First().Title);
            okResult.Value.ToList().First().Participants.Should().Be(trials.First().Participants);
            okResult.Value.ToList().First().StartDate.Should().Be(trials.First().StartDate);
            okResult.Value.ToList().First().Status.Should().Be(trials.First().Status);
        }

        [TestMethod]
        public async Task GetClinicalTrialsByFilterOptions_ReturnsBadRequest_WhenExceptionOccurs()
        {
            // Arrange
            A.CallTo(() => _clinicalTrialsServices.GetClinicalTrialsByFilterOptionsAsync(A<ClinicalTrialFilter>._))
                .Throws(new Exception("Error"));

            // Act
            var result = await _module.GetClinicalTrialsByFilterOptions(null, null, null, null, null, _clinicalTrialsServices, _logger, CancellationToken.None);

            // Assert
            var badRequest = result.Result.As<BadRequest<string>>();
            badRequest.Value.Should().NotBeNull();
            badRequest.StatusCode.Should().Be(400);
        }

        [TestMethod]
        public async Task UploadClinicalTrials_ReturnsNoContent_WhenSuccessful()
        {
            // Arrange
            var file = A.Fake<IFormFile>();
            var stream = new MemoryStream();
            A.CallTo(() => file.OpenReadStream()).Returns(stream);
            A.CallTo(() => file.FileName).Returns("clinical_trial_data.json");

            var request = A.Fake<HttpRequest>();
            A.CallTo(() => request.HasFormContentType).Returns(true);
            A.CallTo(() => request.Form.Files).Returns(new FormFileCollection { file });

            A.CallTo(() => _clinicalTrialsServices.HandleClinicalTrialAsync(A<string>._, A<Stream>._)).Returns(Result.FromSuccess());

            // Act
            var result = await _module.UploadClinicalTrials(request, _clinicalTrialsServices, _logger, CancellationToken.None);

            // Assert
            result.Should().BeOfType<Results<BadRequest<string>, NoContent>>();
            var noContent = result.Result.As<NoContent>();
            noContent.StatusCode.Should().Be(204);
        }

        [TestMethod]
        public async Task UploadClinicalTrials_ReturnsBadRequest_WhenFileNotUploaded()
        {
            // Arrange
            var request = A.Fake<HttpRequest>();
            A.CallTo(() => request.HasFormContentType).Returns(true);
            A.CallTo(() => request.Form.Files.Count).Returns(0);

            // Act
            var result = await _module.UploadClinicalTrials(request, _clinicalTrialsServices, _logger, CancellationToken.None);

            // Assert
            var badRequest = result.Result.As<BadRequest<string>>();
            badRequest.Value.Should().NotBeNull();
            badRequest.StatusCode.Should().Be(400);
        }

        [TestMethod]
        public async Task UploadClinicalTrials_ReturnsBadRequest_WhenErrorOccurs()
        {
            // Arrange
            var file = A.Fake<IFormFile>();
            var stream = new MemoryStream();
            A.CallTo(() => file.OpenReadStream()).Returns(stream);
            A.CallTo(() => file.FileName).Returns("clinical_trial_data.json");

            var request = A.Fake<HttpRequest>();
            A.CallTo(() => request.Form.Files).Returns(new FormFileCollection { file });

            A.CallTo(() => _clinicalTrialsServices.HandleClinicalTrialAsync(A<string>._, A<Stream>._)).Throws(new Exception("Error"));

            // Act
            var result = await _module.UploadClinicalTrials(request, _clinicalTrialsServices, _logger, CancellationToken.None);

            // Assert
            var badRequest = result.Result.As<BadRequest<string>>();
            badRequest.Value.Should().NotBeNull();
            badRequest.StatusCode.Should().Be(400);
        }
    }
}
