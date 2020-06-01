using Contract.ElasticSearch.Models;
using Contract.Models;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalSearchService.DataAccess
{
    public class AnimalRepository : IAnimalRepository
    {
        private readonly ElasticClient elasticClient;

        public AnimalRepository(ElasticClient elasticClient)
        {
            this.elasticClient = elasticClient;
        }

        public async Task Add(AnimalES animal)
        {
            await elasticClient.IndexDocumentAsync(animal);
        }

        public async Task<List<AnimalES>> Find(string alias, string country, string city, int animalType, string creatorId)
        {
            var result = await elasticClient
                .SearchAsync<AnimalES>(
                    s =>
                        s.From(0)
                        .Size(30)
                        .Query(q => q
                            .Bool(b => b
                                .Must(sh => sh
                                    .Match(c => c
                                        .Field(p => p.Alias)
                                        .Query(alias)
                                    ),
                                    sh => sh
                                    .Match(c => c
                                        .Field(p => p.Localization.Country)
                                        .Query(country)
                                    ),
                                    sh => sh
                                    .Match(c => c
                                        .Field(p => p.Localization.City)
                                        .Query(city)
                                    ),
                                    sh => sh
                                    .Match(c => c
                                        .Field(p => p.AnimalTypeRef)
                                        .Query(animalType == 0 ? "" : animalType.ToString())
                                    ),
                                    sh => sh
                                    .Match(c => c
                                        .Field(p => p.CreatorId)
                                        .Query(creatorId)
                                    )
                                )
                            )
                        )
                    );
            return result.Documents.ToList();
        }
    }
}
