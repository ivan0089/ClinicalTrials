
namespace ClinicalTrials.Application.Validations
{
    public class FileConfigurationSettings
    {
        public int MaxFileSizeInBytes { get; set; }
        public List<string>? AllowedFileExtensions { get; set; }
    }
}
