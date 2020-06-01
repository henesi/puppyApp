using AnimalDistributorService.Configuration;
using Contract.MessageBroker;
using Contract.Models;
using MassTransit;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalDistributorService.MessageBroker.Producers
{
    public class AnimalCreationProducer : IPublisher<Animal>
    {
        private readonly ConfigurationOptions _configurationOptions;
        private readonly ISendEndpointProvider _sendEndpointProvider;
        public ConfigurationOptions ConfigurationOptions { get; set; }
        public AnimalCreationProducer(IOptions<ConfigurationOptions> options, ISendEndpointProvider sendEndpointProvider)
        {
            _configurationOptions = options.Value;
            _sendEndpointProvider = sendEndpointProvider;
        }

        public async Task Send(Animal message)
        {
            var url = new UriBuilder(_configurationOptions.MESSAGEBROKER_SERVER)
            {
                Path = Queues.ANIMAL_CREATION_QUEUE
            }.Uri;
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(url);
            await endpoint.Send(message);
        }
    }
}
