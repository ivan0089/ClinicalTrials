

using ClinicalTrials.Domain;

namespace ClinicalTrials.Application.ClinicalTrials
{
    internal class ClinicalTrialsProcessingHandler : IClinicalTrialsProcessingHandler
    {
        public ClinicalTrial Process(ClinicalTrial cLinicalTrial)
        {
            cLinicalTrial.SetDefaultEndDate();
            cLinicalTrial.CalculateDuration();

            return cLinicalTrial;
        }
    }
}
