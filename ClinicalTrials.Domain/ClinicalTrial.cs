
namespace ClinicalTrials.Domain
{
   
    public class ClinicalTrial
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string TrialId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int Participants { get; set; }
        public string Status { get; set; } = string.Empty;
        public int? DurationInDays { get; set; } = 0;


        public void CalculateDuration()
        {
            if (EndDate.HasValue)
            {
                DurationInDays = (EndDate.Value - StartDate).Days;
            }
        }

        public void SetDefaultEndDate()
        {
            if (!EndDate.HasValue && Status == "Ongoing")
            {
                EndDate = StartDate.AddMonths(1);
            }
        }

        public bool IsDefaultObject()
        {
            return string.IsNullOrEmpty(TrialId) &&
                   string.IsNullOrEmpty(Title) &&
                   StartDate == default &&
                   EndDate == null &&
                   Participants == 0 &&
                   string.IsNullOrEmpty(Status) &&
                   DurationInDays == 0;
        }
    }
}
