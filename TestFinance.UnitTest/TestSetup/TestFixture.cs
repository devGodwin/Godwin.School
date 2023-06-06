using AutoMapper;
using Finance.Management.Api.Services;
using Finance.Management.Api.Services.AuthServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TestFinance.UnitTest.TestSetup;

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
        services.AddScoped<IStatementOfAccountServices, StatementOfAccountServices>();
        services.AddScoped<IAuthServices, AuthServices>();
       
        ServiceProvider = services.BuildServiceProvider();
    }
}