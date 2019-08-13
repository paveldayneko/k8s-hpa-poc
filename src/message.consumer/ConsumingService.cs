namespace message.consumer
{
    using System;
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

                c.ReceiveEndpoint(host,"NPS.PodAutoscaling_All", e =>
                {
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
                for (int i = 0; i != 10; ++i)
                {
                    new Thread(o => { Thread.Sleep(500); }).Start();
                }
                Console.WriteLine("Message consumed");
                return Task.CompletedTask;
            }
        }

    }
}
