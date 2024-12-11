
namespace ClinicalTrials.Domain
{
    public class ClinicalTrialFilter
    {
        public string? Status { get; set; }

        public int? Participants { get; set; }

        public string? ParticipantOperator { get; set; }

        public DateTime? StartDate { get; set; }

        public string? StartDateOperator { get; set; }
    }
}
