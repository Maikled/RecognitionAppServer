using Grpc.Net.Client;
using SpeechTranscriptionClient;

namespace Server.Services
{
    public class SpeechTranscriptionService
    {
        private GrpcChannel _channel;

        public SpeechTranscriptionService(string serviceAddress)
        {
            _channel = GrpcChannel.ForAddress(serviceAddress);
        }

        public async Task<ResponseData> TranscriptionAsync(string filePath, string language)
        {
            var client = new SpeechTranscription.SpeechTranscriptionClient(_channel);
            var requestData = new RequestData() { FilePath = filePath, Language = language };
            return await client.GetTranscriptionAsync(requestData);
        }
    }
}
