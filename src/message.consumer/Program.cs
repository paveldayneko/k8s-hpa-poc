using System;

namespace message.consumer
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    class Program
    {
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder().AddEnvironmentVariables().Build();

            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<IConfiguration>(config);
                    services.AddHostedService<ConsumingService>();
                });

            builder
                .RunConsoleAsync().GetAwaiter().GetResult();
        }
    }
}
