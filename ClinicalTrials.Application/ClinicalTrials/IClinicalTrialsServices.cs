
using ClinicalTrials.Application.Helpers;
using ClinicalTrials.Domain;

namespace ClinicalTrials.Application.ClinicalTrials
{
    public interface IClinicalTrialsServices
    {
        Task<Result> HandleClinicalTrialAsync(string fileName, Stream fileContent);
        Task<ClinicalTrial> GetTrialByIdAsync(string id);
        Task<IEnumerable<ClinicalTrial>> GetClinicalTrialsByFilterOptionsAsync(ClinicalTrialFilter status);
    }
}
