
using System.Reflection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using ClinicalTrials.Domain;
using ClinicalTrials.Application;
using ClinicalTrials.Infrastructure;

namespace ArchitectureTests
{
    public abstract class BaseTest
    {
        protected static readonly Assembly DomainAssembly = typeof(ClinicalTrial).Assembly;
        protected static readonly Assembly ApplicationAssembly = typeof(ApplicationModule).Assembly;
        protected static readonly Assembly InfrastructureAssembly = typeof(InfrastructureModule).Assembly;
        protected static readonly Assembly PresentationAssembly = typeof(Program).Assembly;
    }
}
