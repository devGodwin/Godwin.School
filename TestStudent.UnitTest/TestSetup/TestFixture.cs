using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Student.Management.Api.Services;
using TestSchool.UnitTest.TestSetup;

namespace TestStudent.UnitTest.TestSetup;

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
        services.AddScoped<IStudentServices, StudentServices>();
       
        ServiceProvider = services.BuildServiceProvider();
    }
}