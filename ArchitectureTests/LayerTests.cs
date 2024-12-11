
using FluentAssertions;
using NetArchTest.Rules;

namespace ArchitectureTests
{
    [TestClass]
    public class LayerTests : BaseTest
    {
        [TestMethod]
        public void Domain_Should_NotHaveDependencyOn_Application()
        {
            // Act
            var result = Types.InAssembly(DomainAssembly)
                .Should()
                .NotHaveDependencyOn("Application")
                .GetResult();

            // Assert
            result.IsSuccessful.Should().BeTrue();
        }

        [TestMethod]
        public void DomainLayer_ShouldNotHaveDependencyOn_InfrastructureLayer()
        {
            // Act
            var result = Types.InAssembly(DomainAssembly)
                .Should()
                .NotHaveDependencyOn(InfrastructureAssembly.GetName().Name)
                .GetResult();

            // Assert
            result.IsSuccessful.Should().BeTrue();
        }

        [TestMethod]
        public void DomainLayer_ShouldNotHaveDependencyOn_PresentationLayer()
        {
            // Act
            var result = Types.InAssembly(DomainAssembly)
                .Should()
                .NotHaveDependencyOn(PresentationAssembly.GetName().Name)
                .GetResult();

            // Assert
            result.IsSuccessful.Should().BeTrue();
        }

        [TestMethod]
        public void ApplicationLayer_ShouldNotHaveDependencyOn_InfrastructureLayer()
        {
            // Act
            var result = Types.InAssembly(ApplicationAssembly)
                .Should()
                .NotHaveDependencyOn(InfrastructureAssembly.GetName().Name)
                .GetResult();

            // Assert
            result.IsSuccessful.Should().BeTrue();
        }

        [TestMethod]
        public void ApplicationLayer_ShouldNotHaveDependencyOn_PresentationLayer()
        {
            // Act
            var result = Types.InAssembly(ApplicationAssembly)
                .Should()
                .NotHaveDependencyOn(PresentationAssembly.GetName().Name)
                .GetResult();

            // Assert
            result.IsSuccessful.Should().BeTrue();
        }

        [TestMethod]
        public void InfrastructureLayer_ShouldNotHaveDependencyOn_PresentationLayer()
        {
            // Act
            var result = Types.InAssembly(InfrastructureAssembly)
                .Should()
                .NotHaveDependencyOn(PresentationAssembly.GetName().Name)
                .GetResult();

            // Assert
            result.IsSuccessful.Should().BeTrue();
        }
    }
}
