using AnimalDistributorService.DataAccess.EntityFramework;
using Contract.Models.ComputerVision;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalDistributorService.MessageBroker.Consumers
{
    public class StatisticsConsumer : IConsumer<Statistics>
    {
        private readonly IRepository<Statistics> _statisticsRepository;

        public StatisticsConsumer(IRepository<Statistics> statisticsRepository)
        {
            this._statisticsRepository = statisticsRepository;
        }

        public async Task Consume(ConsumeContext<Statistics> context)
        {
            await this._statisticsRepository.Add(new Statistics
            {
                AnimalId = context.Message.AnimalId,
                ElapsedTime = context.Message.ElapsedTime,
                FileName = context.Message.FileName,
                TypeOfMedia = context.Message.TypeOfMedia
            });
        }
    }
}
