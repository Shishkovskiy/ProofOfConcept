using Microsoft.Azure.CognitiveServices.FormRecognizer;
using System;
using System.Threading.Tasks;

namespace ProofOfConcept.Abstractions
{
    interface IAnalyzeForm
    {
        Task<string> AnalyzePdfFormAsync(IFormRecognizerClient formClient, Guid modelId, string file);
    }
}
