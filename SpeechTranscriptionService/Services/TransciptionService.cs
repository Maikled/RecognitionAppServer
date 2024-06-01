using Grpc.Core;
using SpeechTranscriptionServer;
using Whisper;

namespace SpeechTranscriptionService.Services
{
    public class TransciptionService : SpeechTranscription.SpeechTranscriptionBase
    {
        private iModel _model;
        private iMediaFoundation _mediaFoundation;

        public TransciptionService()
        {
            _model = Library.loadModel(@"WhisperModels\ggml-medium.bin");
            _mediaFoundation = Library.initMediaFoundation();
        }

        public override Task<ResponseData> GetTranscription(RequestData request, ServerCallContext context)
        {
            using var whisperContext = _model.createContext();
            whisperContext.parameters.language = Enum.Parse<eLanguage>(request.Language);
            whisperContext.parameters.setFlag(eFullParamsFlags.NoContext, true);
            whisperContext.parameters.setFlag(eFullParamsFlags.PrintTimestamps, true);
            whisperContext.parameters.setFlag(eFullParamsFlags.TokenTimestamps, true);

            using var audioReader = _mediaFoundation.loadAudioFile(request.FilePath);
            whisperContext.runFull(audioReader);

            var whisperResults = whisperContext.results(eResultFlags.Timestamps | eResultFlags.Tokens);
            var result = new ResponseData();
            
            foreach(var segment in whisperResults.segments)
            {
                result.TranscriptionDatas.Add(new TranscriptionData()
                {
                    Start = segment.time.begin.TotalMilliseconds,
                    End = segment.time.end.TotalMilliseconds,
                    Text = segment.text
                });
            }

            return Task.FromResult(result);
        }
    }
}
