import torch
import SpeechSeparation_pb2, SpeechSeparation_pb2_grpc
from nemo.collections.asr.models.msdd_models import NeuralDiarizer


class SpeechSeparation(SpeechSeparation_pb2_grpc.SpeechSeparationServicer):
    def __init__(self) -> None:
        self.model = NeuralDiarizer.from_pretrained("quartznet-15x5-en").to(torch.device("cuda"))
        self.model.eval()
        
    def GetSeparation(self, request : SpeechSeparation_pb2.RequestData, context):
        diarization_result = self.model(request.file_path)
        rttm = diarization_result.to_rttm()
        
        responseData = SpeechSeparation_pb2.ResponseData()
        responseData.file_name = request.file_path

        for line_result in rttm.splitlines():
            split_line = line_result.split()
            start_time, duration, current_speaker = float(split_line[3]), float(split_line[4]), split_line[7]
            end_time = float(start_time) + float(duration)
            
            data = SpeechSeparation_pb2.SeparationData()
            data.start = start_time * 1000
            data.end = end_time * 1000
            data.speaker_name = str(current_speaker)
            responseData.separation_datas.append(data)

        return responseData

