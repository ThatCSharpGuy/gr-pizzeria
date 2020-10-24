using Grpc.Core;
using Grpc.Net.Client;
using Polly;
using System;
using System.IO;
using System.Threading;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {

            var random = new Random();

            var client = Policy
              .Handle<Exception>()
              .WaitAndRetry(
                5,
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                (exception, timeSpan, context) =>
                {
                    Console.WriteLine($"Server not available yet!");
                }
              ).Execute(() => GetClient("http://127.0.0.1:50051"));

            while (true)
            {
                var customer = Customer.Customers[random.Next(Customer.Customers.Length)];
                var order = new Order
                {
                    Address = customer.Address,
                    CustomerName = customer.CustomerName,
                };


                var numberOfPizzas = random.Next(1, 5);
                for (int idx = 0; idx < numberOfPizzas; idx++)
                {
                    var pizza = new Pizza
                    {
                        Cheese = random.Next(10) % 2 == 0,
                        Inches = random.Next(10, 21)
                    };

                    foreach (string topping in Toppings.Select(10))
                    {
                        pizza.Toppings.Add(topping);
                    }
                    order.Pizzas.Add(pizza);
                }

                var confirmation = client.RegisterOrder(order);
                Console.WriteLine($"Your order will arrive at: {ToDateString(confirmation.EstimatedDelivery)}");
                Thread.Sleep(1000 * random.Next(1, 4));
            }
        }

        static Pizzeria.PizzeriaClient GetClient(string address)
        {
            // This switch must be set before creating the GrpcChannel/HttpClient.
            AppContext.SetSwitch(
                "System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            var channel = GrpcChannel.ForAddress(address);
            var client = new Pizzeria.PizzeriaClient(channel);
            client.IsReady(new Empty());
            return client;
        }

        static string ToDateString(long date)
        {
            var dt = DateTime.MinValue.AddSeconds(date);
            return dt.ToShortDateString() + " " + dt.ToShortTimeString();
        }
    }
}
