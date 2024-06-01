using Grpc.Core;
using Server.Models;
using Server.Models.Exceptions;
using SpeechProcessingServer;
using System.Text;
using System.Text.Json;

namespace Server.Services
{
    public class ProcessingService : SpeechProcessing.SpeechProcessingBase
    {
        public override async Task<Response> ProcessingAudio(IAsyncStreamReader<Request> requestStream, ServerCallContext context)
        {
            var response = new Response();

            try
            {
                var clientFileIdHeader = context.RequestHeaders.FirstOrDefault(p => p.Key == "file_id");
                if (clientFileIdHeader == null)
                {
                    throw new SpeechException("Отсутствует ID клиенского аудиофайла", 1);
                }

                var clientFileNameHeader = context.RequestHeaders.FirstOrDefault(p => p.Key == "file_name");
                if (clientFileNameHeader == null)
                {
                    throw new SpeechException("Отсутствует название клиентского аудиофайла", 2);
                }

                var clientFileSpeechLanguage = context.RequestHeaders.FirstOrDefault(p => p.Key == "file_language");
                if (clientFileSpeechLanguage == null)
                {
                    throw new SpeechException("Отсутствует параметр языка распознавания аудио", 3);
                }

                var userFilePath = await UploadUserFile(requestStream, clientFileIdHeader.Value, clientFileNameHeader.Value);

                var speechParameters = new SpeechParameters();
                speechParameters.FilePath = userFilePath;
                speechParameters.SpeechLanguage = clientFileSpeechLanguage.Value;

                var result = GetSpeechResult(speechParameters);
                var convertedResults = ConvertResults(result);
                await SaveResults(convertedResults, clientFileIdHeader.Value);

                response.Id = clientFileIdHeader.Value;
                response.ResultsData.AddRange(convertedResults);
                response.StatusCode = 200;
            }
            catch (Exception ex)
            {
                response.Error = ex.Message;

                if(ex is SpeechException speechException)
                {
                    response.StatusCode = speechException.ErrorCode;
                }
            }

            return response;
        }

        private async Task<string> UploadUserFile(IAsyncStreamReader<Request> requestStream, string fileId, string fileName)
        {
            var userFolderPath = Path.Combine(Environment.CurrentDirectory, "UserFiles", fileId);
            if (!Directory.Exists(userFolderPath))
            {
                Directory.CreateDirectory(userFolderPath);
            }

            var userFilePath = Path.Combine(userFolderPath, fileName);
            using (var fileStream = new FileStream(userFilePath, FileMode.Create))
            {
                await foreach (var request in requestStream.ReadAllAsync())
                {
                    await fileStream.WriteAsync(request.Data.ToArray());
                }
            }

            return userFilePath;
        }

        private IEnumerable<SpeechResult> GetSpeechResult(SpeechParameters speechParameters)
        {
            var transcriptionTask = Task.Run(async () =>
            {
                var speechTranscriptionService = new SpeechTranscriptionService("https://localhost:7001");
                var transcriptionResult = await speechTranscriptionService.TranscriptionAsync(speechParameters.FilePath, speechParameters.SpeechLanguage);
                return transcriptionResult;
            });

            var separationTask = Task.Run(async () =>
            {
                var speechSeparationService = new SpeechSeparationService("http://localhost:7002");
                var separationResult = await speechSeparationService.RecognitionAsync(speechParameters.FilePath);
                return separationResult;
            });

            Task.WaitAll(transcriptionTask, separationTask);

            var transcriptionResult = transcriptionTask.Result;
            var separationResult = separationTask.Result;

            return UnionServicesResults(separationResult, transcriptionResult);
        }

        private IEnumerable<SpeechResult> UnionServicesResults(SpeechSeparationClient.ResponseData separationData, SpeechTranscriptionClient.ResponseData transcriptionData)
        {
            var result = new List<SpeechResult>();

            var speakersData = separationData.SeparationDatas.Select(p => new SpeakerData(p.SpeakerName, TimeSpan.FromMilliseconds(p.Start), TimeSpan.FromMilliseconds(p.End))).ToList();

            int speakersPassedCount = 0;
            var speakersTotal = new List<SpeakerData>() { speakersData[speakersPassedCount] };
            while(speakersPassedCount < speakersData.Count - 1)
            {
                speakersPassedCount++;
                if (speakersData[speakersPassedCount].SpeakerName == speakersTotal[speakersTotal.Count - 1].SpeakerName)
                {
                    speakersTotal[speakersTotal.Count - 1].End += speakersData[speakersPassedCount].Duration;
                }
                else
                {
                    speakersTotal.Add(speakersData[speakersPassedCount]);
                }
            }

            var segmentsData = transcriptionData.TranscriptionDatas.Select(p => new SpeechSegment(p.Text, TimeSpan.FromMilliseconds(p.Start), TimeSpan.FromMilliseconds(p.End)));

            int currentSpeaker = 0;
            var speechResult = new SpeechResult(speakersTotal[currentSpeaker].SpeakerName);
            result.Add(speechResult);

            foreach (var segment in segmentsData)
            {
                if(currentSpeaker + 1 < speakersTotal.Count)
                {
                    if (segment.Start.Seconds >= speakersTotal[currentSpeaker + 1].Start.Seconds)
                    {
                        speechResult = new SpeechResult(speakersTotal[currentSpeaker + 1].SpeakerName);
                        result.Add(speechResult);
                        currentSpeaker++;
                    }
                }

                speechResult.Segments.Add(segment);
            }

            return result.Where(p =>p.Segments.Count > 0);
        }

        private IEnumerable<SpeechData> ConvertResults(IEnumerable<SpeechResult> speechResults)
        {
            var convertedResult = new List<SpeechData>();
            foreach (var result in speechResults)
            {
                var speechData = new SpeechData();
                speechData.CurrentSpeaker = result.SpeakerName;
                speechData.SegmentsData.AddRange(result.Segments.Select(p => new SegmentData() { Text = p.Text, Start = p.Start.TotalMilliseconds, End = p.End.TotalMilliseconds }));
                convertedResult.Add(speechData);
            }

            return convertedResult;
        }

        private async Task SaveResults(IEnumerable<SpeechData> results, string fileId)
        {
            var userResultsFilePath = Path.Combine(Environment.CurrentDirectory, "UserFiles", fileId, "results.json");
            
            using(var fileStream = new FileStream(userResultsFilePath, FileMode.Create))
            {
                await JsonSerializer.SerializeAsync(fileStream, results);
            }
        }
    }
}
