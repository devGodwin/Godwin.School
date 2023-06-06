using Microsoft.Extensions.Configuration;

namespace TestCourse.UnitTest.TestSetup;

public static class ConfigurationManger
{
    public static IConfiguration Configuration { get; private set; }

    public static void SetupConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
            .AddEnvironmentVariables();

        Configuration = builder.Build();
    }
}