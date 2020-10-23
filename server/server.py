from concurrent import futures

from service_pb2 import OrderConfirmation
from service_pb2_grpc import PizzeriaServicer, add_PizzeriaServicer_to_server

import grpc


class PizzeriaService(PizzeriaServicer):

    def RegisterOrder(self, request, context):
        print("New order received:\n")
        print(f"\tFrom: {request.customer_name}\n")
        print(f"\tTo: {request.address}\n")

        return OrderConfirmation(estimated_delivery=10)

def serve():
  server = grpc.server(futures.ThreadPoolExecutor(max_workers=10))
  add_PizzeriaServicer_to_server(PizzeriaService(), server)
  server.add_insecure_port('[::]:50051')
  server.start()
  server.wait_for_termination()

if __name__ == "__main__":
    serve()