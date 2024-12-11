
using System.Linq.Expressions;
using ClinicalTrials.Application.ClinicalTrials;
using ClinicalTrials.Domain;
using ClinicalTrials.Infrastructure.ClinicalTrialContext;
using ClinicalTrials.Infrastructure.Util;
using Microsoft.EntityFrameworkCore;

namespace ClinicalTrials.Infrastructure.Repository
{
    internal class ClinicalTrialsRepository(IClinicalTrialsDbContext clinicalTrialsDbContext) : IClinicalTrialsRepository
    {
        private const string GreaterThan = "gt";
        private const string LessThan = "lt";

        public async Task InsertAsync(ClinicalTrial clinicalTrial)
        {
            await clinicalTrialsDbContext.ClinicalTrials.AddAsync(clinicalTrial);
            await clinicalTrialsDbContext.SaveChangesAsync();
        }

        public async Task<ClinicalTrial> RetrieveByIdAsync(string id)
        {
            return await clinicalTrialsDbContext.ClinicalTrials.SingleAsync(trial => trial.TrialId.ToLower() == id.ToLower());
        }

        public Task<IList<ClinicalTrial>> RetrieveByFilterOprionsAsync(ClinicalTrialFilter filter)
        {
            var trials = clinicalTrialsDbContext.ClinicalTrials.AsQueryable();

            trials = trials.WhereIf(!string.IsNullOrEmpty(filter.Status), trial => trial.Status.ToLower() == filter.Status!.ToLower());

            Expression<Func<ClinicalTrial, bool>> startDatePredicate = StartDateFilter(filter);
            trials = trials.WhereIf(filter.StartDate is not null, startDatePredicate);

            Expression<Func<ClinicalTrial, bool>> participantsPredicate = ParticipantFilter(filter);
            trials = trials.WhereIf(filter.Participants != 0, participantsPredicate);

            return Task.FromResult<IList<ClinicalTrial>>(trials.ToList());
        }

        private Expression<Func<ClinicalTrial, bool>> ParticipantFilter(ClinicalTrialFilter filter)
        {
            Expression<Func<ClinicalTrial, bool>> participantsPredicate = trial => true;
            if (filter.Participants is not null)
            {
                if (filter.ParticipantOperator is null || filter.ParticipantOperator!.Equals(GreaterThan, StringComparison.OrdinalIgnoreCase))
                {
                    return participantsPredicate = trial => trial.Participants >= filter.Participants;
                }
                return participantsPredicate = trial => trial.Participants < filter.Participants;
            }

            return participantsPredicate;
        }

        private Expression<Func<ClinicalTrial, bool>> StartDateFilter(ClinicalTrialFilter filter)
        {
            Expression<Func<ClinicalTrial, bool>> startDatePredicate = trial => true;
            if (filter.StartDate is not null)
            {
                if (filter.StartDateOperator is null || filter.StartDateOperator!.Equals(GreaterThan, StringComparison.OrdinalIgnoreCase))
                {
                    return startDatePredicate = trial => trial.StartDate > filter.StartDate;
                }
                return startDatePredicate = trial => trial.StartDate < filter.StartDate;
            }

            return startDatePredicate;
        }
    }
}
