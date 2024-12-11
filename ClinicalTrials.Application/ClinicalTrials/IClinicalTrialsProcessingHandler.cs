

using ClinicalTrials.Domain;

namespace ClinicalTrials.Application.ClinicalTrials
{
    public interface IClinicalTrialsProcessingHandler
    {
        ClinicalTrial Process(ClinicalTrial cLinicalTrial);
    }
}
