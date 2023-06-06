using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Teacher.Management.Api.Services;
using TestSchool.UnitTest.TestSetup;

namespace TestTeacher.UnitTest.TestSetup;

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
        services.AddScoped<ITeacherServices, TeacherServices>();

        ServiceProvider = services.BuildServiceProvider();
    }
}