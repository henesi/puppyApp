﻿using AnimalDistributorService.Api.Services;
using ComputerVisionService.MessageBroker.Producers;
using ComputerVisionService.Utils;
using Contract.Minio;
using Contract.Models;
using Contract.Models.ComputerVision;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace ComputerVisionService.MessageBroker.Consumers
{
    public class ProfileConsumer : IConsumer<Profile>
    {
        private readonly ILogger<ProfileConsumer> _logger;
        private readonly IStorageService _storageService;
        private readonly AnimalProfileComputedProducer _animalProfileComputedProducer;
        private readonly MediaComputedStatisticsProducer _mediaComputedStatisticsProducer;

        public ProfileConsumer(ILogger<ProfileConsumer> logger, IStorageService storageService, MediaComputedStatisticsProducer mediaComputedStatisticsProducer, AnimalProfileComputedProducer animalMediaProducer)
        {
            this._logger = logger;
            this._storageService = storageService;
            this._animalProfileComputedProducer = animalMediaProducer;
            this._mediaComputedStatisticsProducer = mediaComputedStatisticsProducer;
        }
        public async Task Consume(ConsumeContext<Profile> context)
        {
            // prepare local paths
            var localImagePath = Path.Combine(Directory.CreateDirectory(Path.Combine("results", context.Message.AnimalRef.ToString(), context.Message.FileName)).FullName, context.Message.FileName);
            var localMappingPath = Path.Combine(Path.Combine("results", context.Message.AnimalRef.ToString(), context.Message.FileName), "mapping");
            var localTransformedImagePath = Path.Combine(Path.Combine("results", context.Message.AnimalRef.ToString(), context.Message.FileName), "image_new.jpg");

            // prepare minio paths
            var minioPath = $"{context.Message.AnimalRef.ToString()}/{((int)context.Message.MediaType).ToString()}/{context.Message.FileName}";

            // download image to local folder

            var imageUrl = _storageService.GetMediaUrl(context.Message.AnimalRef, context.Message.MediaType, context.Message.FileName, context.Message.FileName);

            using (WebClient webClient = new WebClient())
            {
                byte[] data = webClient.DownloadData(imageUrl);
                using (MemoryStream mem = new MemoryStream(data))
                {
                    using (FileStream file = new FileStream(localImagePath, FileMode.Create, FileAccess.Write))
                    {
                        byte[] bytes = new byte[mem.Length];
                        mem.Read(bytes, 0, (int)mem.Length);
                        file.Write(bytes, 0, bytes.Length);
                        mem.Close();
                    }
                }
            }

            // run script
            var result = ScriptRunner.RunFromCmd("python", "object_detection.py " + context.Message.AnimalRef + " " + context.Message.FileName, out string elapsedTime);

            _logger.LogDebug($"Media result for animal/filename: {context.Message.AnimalRef}/{context.Message.FileName}\n" + result);

            await Task.WhenAll(WhenFileCreated(localMappingPath), WhenFileCreated(localTransformedImagePath));

            // send to minio mapping file & transformed file
            using (FileStream file = new FileStream(localMappingPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                await this._storageService.SaveMediaAsync(file, minioPath, FileNames.MAPPING_FILE);
            }

            using (FileStream file = new FileStream(localTransformedImagePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                await this._storageService.SaveMediaAsync(file, minioPath, FileNames.COMPUTED_IMAGE);
            }

            // send message to rabbitMQ to inform about results
            await this._animalProfileComputedProducer.Send(context.Message);

            // send statistics data
            await this._mediaComputedStatisticsProducer.Send(new Statistics()
            {
                AnimalId = context.Message.AnimalRef,
                ElapsedTime = elapsedTime,
                FileName = context.Message.FileName,
                TypeOfMedia = (int)MediaType.Image
            });

            // clean  up
            Directory.Delete(Directory.CreateDirectory(Path.Combine("results", context.Message.AnimalRef.ToString(), context.Message.FileName)).FullName, true);
        }

        public static Task WhenFileCreated(string path)
        {
            if (File.Exists(path))
                return Task.FromResult(true);

            var tcs = new TaskCompletionSource<bool>();
            FileSystemWatcher watcher = new FileSystemWatcher(Path.GetDirectoryName(path));

            FileSystemEventHandler createdHandler = null;
            RenamedEventHandler renamedHandler = null;
            createdHandler = (s, e) =>
            {
                if (e.Name == Path.GetFileName(path))
                {
                    tcs.TrySetResult(true);
                    watcher.Created -= createdHandler;
                    watcher.Dispose();
                }
            };

            renamedHandler = (s, e) =>
            {
                if (e.Name == Path.GetFileName(path))
                {
                    tcs.TrySetResult(true);
                    watcher.Renamed -= renamedHandler;
                    watcher.Dispose();
                }
            };

            watcher.Created += createdHandler;
            watcher.Renamed += renamedHandler;

            watcher.EnableRaisingEvents = true;

            return tcs.Task;
        }
    }
}
