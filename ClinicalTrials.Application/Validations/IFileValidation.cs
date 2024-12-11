
namespace ClinicalTrials.Application.Validations
{
    public interface IFileValidation
    {
        bool IsValidFileSize(double fileLength);

        bool IsAllowedExtension(string fileName);
    }
}
