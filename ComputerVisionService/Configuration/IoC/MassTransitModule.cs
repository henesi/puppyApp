using ComputerVisionService.MessageBroker.Consumers;
using Autofac;
using GreenPipes;
using MassTransit;
using RabbitMQ.Client;
using System;
using Contract.MessageBroker;

namespace ComputerVisionService.Configuration.IoC
{
    public class MassTransitModule : Module
    {
        public ConfigurationOptions ConfigurationOptions { get; set; }
        protected override void Load(ContainerBuilder builder)
        {
            builder.AddMassTransit(x =>
            {
                x.AddConsumers(System.Reflection.Assembly.GetExecutingAssembly());
                x.AddBus(context => Bus.Factory.CreateUsingRabbitMq(busCfg =>
                {
                    var brokerHost = $"{ConfigurationOptions.MESSAGEBROKER_SERVER}";
                    var host = busCfg.Host(new Uri(brokerHost), hostCfg =>
                    {
                        hostCfg.Username(ConfigurationOptions.MESSAGEBROKER_USERNAME);
                        hostCfg.Password(ConfigurationOptions.MESSAGEBROKER_PASSWORD);
                    });

                    busCfg.UseMessageRetry(r => r.Interval(10, TimeSpan.FromSeconds(6)));

                    busCfg.ReceiveEndpoint(Queues.ANIMAL_MEDIA_ADDED_QUEUE, e =>
                    {
                        e.PrefetchCount = 1;
                        e.ConfigureConsumer<MediaConsumer>(context,
                            p => p.UseConcurrencyLimit(1));
                    });

                    busCfg.ReceiveEndpoint(Queues.ANIMAL_PROFILE_ADDED_QUEUE, e =>
                    {
                        e.PrefetchCount = 1;
                        e.ConfigureConsumer<ProfileConsumer>(context,
                            p => p.UseConcurrencyLimit(1));
                    });
                }));
            });
        }
    }
}
