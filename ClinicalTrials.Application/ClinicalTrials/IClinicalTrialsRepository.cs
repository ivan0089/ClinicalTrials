using ClinicalTrials.Domain;

namespace ClinicalTrials.Application.ClinicalTrials
{
    public interface IClinicalTrialsRepository
    {
        Task InsertAsync(ClinicalTrial clinicalTrial);

        Task<ClinicalTrial> RetrieveByIdAsync(string id);

        Task<IList<ClinicalTrial>> RetrieveByFilterOprionsAsync(ClinicalTrialFilter filter);
    }
}
