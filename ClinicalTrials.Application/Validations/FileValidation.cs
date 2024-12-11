

namespace ClinicalTrials.Application.Validations
{
    internal class FileValidation(FileConfigurationSettings settings) : IFileValidation
    {
        public bool IsAllowedExtension(string fileName)
        {
            var extension = Path.GetExtension(fileName);
            return settings.AllowedFileExtensions!.Contains(extension, StringComparer.OrdinalIgnoreCase);
        }

        public bool IsValidFileSize(double fileLength)
        {
            return fileLength < settings.MaxFileSizeInBytes ? true : false;
        }
    }
}
