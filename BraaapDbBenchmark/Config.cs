using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace BraaapDbBenchmark
{
    public static class Config
    {
        public static IConfiguration Initialize(string[] args) => new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", true)
            .AddUserSecrets(Assembly.GetExecutingAssembly())
            .AddEnvironmentVariables()
            .AddCommandLine(args)
            .Build();
    }
}