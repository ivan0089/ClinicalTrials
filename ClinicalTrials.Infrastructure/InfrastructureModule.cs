
using Autofac;
using ClinicalTrials.Application.ClinicalTrials;
using ClinicalTrials.Infrastructure.Repository;

namespace ClinicalTrials.Infrastructure
{
    public class InfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ClinicalTrialsRepository>().As<IClinicalTrialsRepository>();
        }
    }
}
