using Grpc.Net.Client;
using System;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new Pizzeria.PizzeriaClient(channel);
        }
    }
}
