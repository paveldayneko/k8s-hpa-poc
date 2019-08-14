namespace message.consumer
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using MassTransit;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using NPS.Contracts.LoadMessaging;

    public class ConsumingService : IHostedService
    {
        private readonly IBusControl _bus;

        public ConsumingService(IConfiguration cfg)
        {
            _bus = Bus.Factory.CreateUsingRabbitMq(c =>
            {
                var host = c.Host(cfg.GetValue<Uri>("r_host"), h =>
                {
                    h.Username(cfg.GetValue<string>("r_user"));
                    h.Password(cfg.GetValue<string>("r_pwd"));
                });

                c.ReceiveEndpoint(host, "NPS.PodAutoscaling_All", e =>
                {
                    e.PrefetchCount = 100;
                    e.Consumer<LoadConsumer>();
                });
            });
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return _bus.StartAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return _bus.StopAsync(cancellationToken);
        }

        private class LoadConsumer : IConsumer<IMessage>
        {
            public Task Consume(ConsumeContext<IMessage> context)
            {
                Random rand = new Random();
                Stopwatch watch = new Stopwatch();
                watch.Start();
                long num = 0;
                while (true)
                {
                    if (watch.ElapsedMilliseconds > 1000)
                    {
                        break;
                       
                    }

                    new Thread(() =>
                    {
                        num += rand.Next(100, 1000);
                        if (num > 1000000)
                        {
                            num = 0;
                        }
                    });

                }

                Console.WriteLine("Message consumed");
                watch.Stop();
                return Task.CompletedTask;
            }


        }
    }
}
