using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using static System.Console;
class ReceiveLogs
{
    public static void Main()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using(var connection = factory.CreateConnection())
            using(var channel = connection.CreateModel())
        {
            channel.ExchangeDeclare(exchange:"logs", type: ExchangeType.Fanout);
            var queueName = channel.QueueDeclare().QueueName;
            channel.QueueBind(queue: queueName, exchange: "logs", routingKey: "");
            WriteLine("[*] Waiting for logs.");

            var cosumer = new EventingBasicConsumer(channel);
            cosumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                WriteLine("[x] {0}", message);
            };

            channel.BasicConsume(queue: queueName,autoAck:true,consumer: cosumer);
            WriteLine("Press [enter] to exit.");
            ReadLine();
        }
    }
}