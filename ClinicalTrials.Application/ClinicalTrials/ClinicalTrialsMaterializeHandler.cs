
using System.Text.Json;
using ClinicalTrials.Application.Helpers;
using ClinicalTrials.Domain;

namespace ClinicalTrials.Application.ClinicalTrials
{
    internal class ClinicalTrialsMaterializeHandler : IClinicalTrialsMaterializeHandler
    {
        public Option<ClinicalTrial> Materialized(string jsonFile)
        {
            var clinicalTrial = JsonSerializer.Deserialize<ClinicalTrial>(jsonFile, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            });

            if (clinicalTrial is null || clinicalTrial.IsDefaultObject())
            {
                return Option<ClinicalTrial>.None();
            }

            return Option<ClinicalTrial>.Some(clinicalTrial);
        }
    }
}
