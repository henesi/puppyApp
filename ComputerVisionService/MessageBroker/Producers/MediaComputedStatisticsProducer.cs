using ComputerVisionService.Configuration;
using Contract.MessageBroker;
using Contract.Models.ComputerVision;
using MassTransit;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace ComputerVisionService.MessageBroker.Producers
{
    public class MediaComputedStatisticsProducer : IPublisher<Statistics>
    {
        private readonly ConfigurationOptions _configurationOptions;
        private readonly ISendEndpointProvider _sendEndpointProvider;
        public ConfigurationOptions ConfigurationOptions { get; set; }
        public MediaComputedStatisticsProducer(IOptions<ConfigurationOptions> options, ISendEndpointProvider sendEndpointProvider)
        {
            _configurationOptions = options.Value;
            _sendEndpointProvider = sendEndpointProvider;
        }

        public async Task Send(Statistics message)
        {
            var url = new UriBuilder(_configurationOptions.MESSAGEBROKER_SERVER)
            {
                Path = Queues.ANIMAL_COMPUTED_STATISTICS
            }.Uri;
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(url);
            await endpoint.Send(message);
        }
    }
}
