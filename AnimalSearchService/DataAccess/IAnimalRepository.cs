using Contract.ElasticSearch.Models;
using Contract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalSearchService.DataAccess
{
    public interface IAnimalRepository
    {
        Task Add(AnimalES animal);
        Task<List<AnimalES>> Find(string queryText, string country, string city, int animalType, string creatorId);
    }
}
