using ProofOfConcept.Conts;
using System.Threading.Tasks;

namespace ProofOfConcept
{
    public class Program
    {

        public static void Main(string[] args)
        {
            var formRecognizerClient = RunFormRecognizerClient();
            Task.WaitAll(formRecognizerClient);
        }

        private static async Task RunFormRecognizerClient()
        {
            var recognizerForm = new FormRecognizer(new ConsolePrinter());

            var formClient = recognizerForm.CreateRecognizerClient();

            var trainedModel = await recognizerForm.TrainModelAsync(formClient, Urls.TrainingDataUrl);




            var json = await recognizerForm.AnalyzePdfFormAsync(formClient, trainedModel.ModelId, Urls.ParticularFile);
            //recognizerForm.Mapping(json);
            await recognizerForm.CretateDirection(json);

            await recognizerForm.DeleteModelAsync(formClient, trainedModel.ModelId);






        }

    }
}

