
using System.Runtime.CompilerServices;
using Autofac;
using ClinicalTrials.Application.ClinicalTrials;
using ClinicalTrials.Application.Validations;

[assembly: InternalsVisibleTo("ClinicalTrials.Application.Tests")]
namespace ClinicalTrials.Application
{
    public class ApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ClinicalTrialsServices>().As<IClinicalTrialsServices>();
            builder.RegisterType<FileValidation>().As<IFileValidation>();
            builder.RegisterType<JsonSchemValidation>().As<IJsonSchemValidation>();
            builder.RegisterType<ClinicalTrialsProcessingHandler>().As<IClinicalTrialsProcessingHandler>();
            builder.RegisterType<ClinicalTrialsMaterializeHandler>().As<IClinicalTrialsMaterializeHandler>();
        }
    }
}
