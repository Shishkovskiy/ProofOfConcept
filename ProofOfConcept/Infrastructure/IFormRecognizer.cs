using Microsoft.Azure.CognitiveServices.FormRecognizer;
using Microsoft.Azure.CognitiveServices.FormRecognizer.Models;
using System;
using System.Threading.Tasks;

namespace ProofOfConcept.Abstractions
{
    public interface IFormRecognizer
    {
        FormRecognizerClient CreateRecognizerClient();
        Task<TrainResult> TrainModelAsync(IFormRecognizerClient formRecognizerClient, string trainingDataUrl);
        Task CretateDirection(string json);
        Task DeleteModelAsync(IFormRecognizerClient formClient, Guid modelId);
    }
}