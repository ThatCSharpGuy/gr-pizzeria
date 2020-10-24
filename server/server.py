from concurrent import futures
from datetime import datetime, timedelta

import grpc

from service_pb2 import OrderConfirmation
from service_pb2_grpc import PizzeriaServicer, add_PizzeriaServicer_to_server


class PizzeriaService(PizzeriaServicer):

    def IsReady(self, request, context):
        return request

    def RegisterOrder(self, request, context):
        print("New order received:")
        print(f"\tFrom: {request.customer_name}")
        print(f"\tTo: {request.address}")
        print("\tPizzas:")
        for idx, pizza in enumerate(request.pizzas, 1):
            toppings = ", ".join(pizza.toppings)
            print(f"\t\tPizza {idx}: {pizza.inches}\" ({toppings})")

        estimated_delivery = (datetime.now() + timedelta(minutes=30)) - datetime(1970, 1, 1)
        return OrderConfirmation(estimated_delivery=int(estimated_delivery.total_seconds()))


def serve():
    server = grpc.server(futures.ThreadPoolExecutor(max_workers=10))
    add_PizzeriaServicer_to_server(PizzeriaService(), server)
    server.add_insecure_port('[::]:50051')
    print("The server is running!")
    server.start()
    server.wait_for_termination()


if __name__ == "__main__":
    serve()
