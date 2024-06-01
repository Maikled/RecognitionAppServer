import grpc
import logging
import asyncio
from concurrent.futures import ThreadPoolExecutor
from SpeechDiarization import SpeechSeparation
import SpeechSeparation_pb2_grpc


async def start_server() -> None:
    server = grpc.server(ThreadPoolExecutor())
    SpeechSeparation_pb2_grpc.add_SpeechSeparationServicer_to_server(SpeechSeparation(), server)
    listen_address = "localhost:7002"
    server.add_insecure_port(listen_address)
    server.start()
    logging.info(f"Starting server on {listen_address}")
    server.wait_for_termination()
    

logging.basicConfig(level=logging.INFO)
asyncio.run(start_server())
