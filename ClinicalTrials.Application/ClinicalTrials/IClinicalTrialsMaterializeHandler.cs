using ClinicalTrials.Application.Helpers;
using ClinicalTrials.Domain;

namespace ClinicalTrials.Application.ClinicalTrials
{
    public interface IClinicalTrialsMaterializeHandler
    {
        Option<ClinicalTrial> Materialized(string jsonFile);
    }
}
