using AutoMapper;
using GradeAndAssessment.Api.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TestGradeAndAssessment.UnitTest.TestSetup;

public class TestFixture
{
    public ServiceProvider ServiceProvider { get; }

    public TestFixture()
    {
        var services = new ServiceCollection();
        ConfigurationManger.SetupConfiguration();

        services.AddSingleton(sp => ConfigurationManger.Configuration);

        services.AddLogging(x => x.AddConsole());
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddScoped<IStudentAssessmentServices, StudentAssessmentServices>();
       
        ServiceProvider = services.BuildServiceProvider();
    }
}