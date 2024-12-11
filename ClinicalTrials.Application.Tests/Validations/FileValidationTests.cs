
using ClinicalTrials.Application.Validations;
using FakeItEasy;
using FluentAssertions;

namespace ClinicalTrials.Application.Tests.Validations
{
    [TestClass]
    public class FileValidationTests
    {
        private FileValidation? _testee;
        private FileConfigurationSettings? _settings;

        [TestInitialize]
        public void TestInitialize()
        {
            _settings = A.Fake<FileConfigurationSettings>();

            _testee = new FileValidation(_settings);
        }

        [TestMethod]
        public void IsAllowedExtension_ShouldReturnTrue_ForAllowedExtension()
        {
            // Arrange
            _settings!.AllowedFileExtensions = new List<string> { ".txt", ".json" };

            // Act
            var result = _testee!.IsAllowedExtension("document.json");

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void IsAllowedExtension_ShouldReturnTrue_ForAllowedExtensionUppercase()
        {
            // Arrange
            _settings!.AllowedFileExtensions = new List<string> { ".txt", ".json" };

            // Act
            var result = _testee!.IsAllowedExtension("DOCUMENT.JSON");

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void IsAllowedExtension_ShouldReturnFalse_ForDisallowedExtension()
        {
            // Arrange
            _settings!.AllowedFileExtensions = new List<string> { ".txt", ".json" };

            // Act
            var result = _testee!.IsAllowedExtension("image.jpg");

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void IsValidFileSize_ShouldReturnTrue_WhenFileSizeIsWithinLimit()
        {
            // Arrange
            _settings!.MaxFileSizeInBytes = 1024; // 1 KB

            // Act
            var result = _testee!.IsValidFileSize(512); // 512 bytes

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void IsValidFileSize_ShouldReturnFalse_WhenFileSizeExceedsLimit()
        {
            // Arrange
            _settings!.MaxFileSizeInBytes = 1024; // 1 KB

            // Act
            var result = _testee!.IsValidFileSize(2048); // 2 KB

            // Assert
            result.Should().BeFalse();
        }
    }
}
