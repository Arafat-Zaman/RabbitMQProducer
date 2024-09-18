// RabbitMQProducer/Program.cs
using RabbitMQ.Client;
using System;
using System.Text;

class Producer
{
    static void Main(string[] args)
    {
        // Step 1: Establish connection to RabbitMQ server
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            // Step 2: Declare a durable queue
            channel.QueueDeclare(queue: "task_queue",
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            // Step 3: Construct a message (Task)
            string message = GetMessage(args);
            var body = Encoding.UTF8.GetBytes(message);

            // Step 4: Set message persistence
            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            // Step 5: Publish message to RabbitMQ queue
            channel.BasicPublish(exchange: "",
                                 routingKey: "task_queue",
                                 basicProperties: properties,
                                 body: body);

            Console.WriteLine($" [x] Sent {message}");
        }

        Console.WriteLine(" Press [enter] to exit.");
        Console.ReadLine();
    }

    // Helper method to create message
    private static string GetMessage(string[] args)
    {
        return ((args.Length > 0) ? string.Join(" ", args) : "Hello World!");
    }
}
