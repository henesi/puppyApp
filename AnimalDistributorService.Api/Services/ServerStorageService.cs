using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Contract.Models;
using Minio;
using Minio.Exceptions;
using Minio.DataModel;

namespace AnimalDistributorService.Api.Services
{
    public class ServerStorageService : IStorageService
    {
        private readonly string bucketName = "animals";
        private string storageServer;
        private string apigatewayServer;
        private MinioClient _minioClient;

        public ServerStorageService(string fileStorageServer, string apiGateway, string accessKey, string secretKey)
        {
            this.storageServer = fileStorageServer;
            this.apigatewayServer = apiGateway;
            this._minioClient = new MinioClient(fileStorageServer, accessKey, secretKey);
        }
        public Task DeleteMediaAsync(Guid animalIdentifier, string fileName)
        {
            return _minioClient.RemoveObjectAsync(bucketName, fileName);
        }

        public string GetMediaUrl(Guid animalIdentifier, MediaType mediaType, string fileDirectory, string fileName, int animalType = 0)
        {
            string url = "";

            //workaround to make a dummy images
            var catUrl = "https://static9.depositphotos.com/1577591/1076/v/450/depositphotos_10768928-stock-illustration-blackcat.jpg";
            var dogUrl = "https://st2.depositphotos.com/1341440/5399/v/450/depositphotos_53994269-stock-illustration-dachshund-dog-vector-black-silhouette.jpg";

            if (fileName == null)
            {
                if(animalType == 1)
                {
                    url = dogUrl;
                }
                else
                {
                    url = catUrl;
                }
            }
            else
            {
                url = _minioClient.PresignedGetObjectAsync(bucketName, $"{animalIdentifier.ToString()}/{(int)mediaType}/{fileDirectory}/{fileName}", 604800).Result;
            }
            return url.Replace(storageServer, apigatewayServer);
        }

        public Task SaveMediaAsync(Stream mediaBinaryStream, string dirs, string fileName)
        {
            bool found = this._minioClient.BucketExistsAsync(bucketName).Result;
            if (!found)
            {
               var task = this._minioClient.MakeBucketAsync(bucketName, "us-east-1");
               task.Wait(TimeSpan.FromSeconds(15));
            }

            try
            {
                return _minioClient.PutObjectAsync(bucketName, $"{dirs}/{fileName}", mediaBinaryStream, mediaBinaryStream.Length);
            }
            catch(Exception e)
            {
                throw new Exception(e.ToString());
            }
        }
    }
}
