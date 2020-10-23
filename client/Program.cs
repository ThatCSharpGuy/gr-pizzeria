using Grpc.Net.Client;
using System;
using System.Collections.Generic;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            // This switch must be set before creating the GrpcChannel/HttpClient.
            AppContext.SetSwitch(
                "System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            var channel = GrpcChannel.ForAddress("http://localhost:50051" );
            var client = new Pizzeria.PizzeriaClient(channel);

            var order = new Order
            {
                Address = "My house",
                CustomerName = "Antonio Feregrino",
            };
            for(int inchex = 10; inchex < 15; inchex++)
            {
                order.Pizzas.Add(new Pizza
                {
                    Cheese = true,
                    Inches = inchex
                });
            }
            var confirmation = client.RegisterOrder(order);
            Console.WriteLine(confirmation.EstimatedDelivery);
        }
    }
}
