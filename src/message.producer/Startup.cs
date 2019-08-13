namespace message.producer
{
    using System;
    using MassTransit;
    using Microsoft.Extensions.Configuration;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var config = new ConfigurationBuilder().AddEnvironmentVariables().Build();

            services.AddMvc();

            var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host(config.GetValue<Uri>("r_host"), h =>
                {
                    h.Username(config.GetValue<string>("r_user"));
                    h.Password(config.GetValue<string>("r_pwd"));
                });
            });

            services.AddSingleton<IBus>(bus);

            bus.Start();
        }
       
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
           
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
