using Contract;
using Contract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalDistributorService.MessageBroker.Producers
{
    public interface IPublisher<T>
    {
        public Task Send(T message);
    }
}
