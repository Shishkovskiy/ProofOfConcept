using Microsoft.Azure.CognitiveServices.FormRecognizer;
using Microsoft.Azure.CognitiveServices.FormRecognizer.Models;
using Newtonsoft.Json;
using ProofOfConcept.Abstractions;
using ProofOfConcept.Conts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ProofOfConcept
{
    internal class FormRecognizer : IFormRecognizer, IAnalyzeForm
    {

        private readonly IPrinter _printer;
        public FormRecognizer(IPrinter printer)
        {
            _printer = printer;
        }

        public FormRecognizerClient CreateRecognizerClient()
        {
            return new FormRecognizerClient(new ApiKeyServiceClientCredentials(Urls.SubscriptionKey))
            {
                Endpoint = Urls.FormRecognizerEndpoint
            };
        }

        public Task<TrainResult> TrainModelAsync(IFormRecognizerClient formRecognizerClient, string url)
        {
            return TrainModelAsync(formRecognizerClient, url);
        }

        public async Task<TrainResult> TrainModelAsync(FormRecognizerClient formRecognizerClient, string trainingDataUrl)
        {
            if (!Uri.IsWellFormedUriString(trainingDataUrl, UriKind.Absolute))
            {
                _printer.Print($"\nInvalid trainingDataUrl:\n{trainingDataUrl} \n");

            }

            _printer.Print($"Train Model with training data");
            var trainResult = await formRecognizerClient.TrainCustomModelAsync(new TrainRequest
            {
                Source = trainingDataUrl
            });

            await DisplayModelStatus(formRecognizerClient, trainResult);

            return trainResult;

        }

        public async Task<string> AnalyzePdfFormAsync(IFormRecognizerClient formClient, Guid modelId, string particularFile)
        {
            if (string.IsNullOrEmpty(particularFile))
            {
                _printer.Print($"\nInvalid pdf file");
            }

            _printer.Print($"\nAnalyze PDF form");

            using FileStream stream = new FileStream(particularFile, FileMode.Open);
            var analizedModel = await formClient.AnalyzeWithCustomModelAsync(modelId, stream, contentType: "application/pdf");

            _printer.Print($"PDF form has been analyzed\n");


            return JsonConvert.SerializeObject(analizedModel);
        }

        public async Task CretateDirection(string json)
        {
            if (!File.Exists(Urls.BaseUrl))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(@$"{Urls.BaseUrl}\{Urls.FolderName}\{Urls.FileName}"));
                _printer.Print($"Directory has been created");
            }

            using var writer = new StreamWriter(@$"{Urls.BaseUrl}\{Urls.FolderName}\{Urls.FileName}", append: false);
            await writer.WriteAsync(json);

            _printer.Print($"Data has been saved to: {Urls.BaseUrl}");
        }

        public async Task DeleteModelAsync(IFormRecognizerClient formClient, Guid modelId)
        {
            await formClient.DeleteCustomModelAsync(modelId);
            _printer.Print($"Model has been deleted: {DateTime.Now}\n");

        }

        private async Task DisplayModelStatus(IFormRecognizerClient formRecognizerClient, TrainResult trainResult)
        {
            var model = await formRecognizerClient.GetCustomModelAsync(trainResult.ModelId);
            if (model == null)
            {
                _printer.Print($"Invalid model result");
            }

            _printer.Print($"\nModel Id: {model.ModelId}");
            _printer.Print($"Status:  {model.Status}");
            _printer.Print($"Created Date: {model.CreatedDateTime}\n");
            _printer.Print($"Trained models: \n");

            foreach (var document in trainResult.TrainingDocuments)
            {
                _printer.Print($"{ document.DocumentName}");
            }
        }
    }
}
