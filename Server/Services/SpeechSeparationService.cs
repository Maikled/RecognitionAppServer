using Grpc.Net.Client;
using SpeechSeparationClient;

namespace Server.Services
{
    public class SpeechSeparationService
    {
        private GrpcChannel _channel;

        public SpeechSeparationService(string serviceAddress)
        {
            _channel = GrpcChannel.ForAddress(serviceAddress);
        }

        public async Task<ResponseData> RecognitionAsync(string filePath)
        {
            var client = new SpeechSeparation.SpeechSeparationClient(_channel);
            var requestData = new RequestData() { FilePath = filePath };
            return await client.GetSeparationAsync(requestData);
        }
    }
}
