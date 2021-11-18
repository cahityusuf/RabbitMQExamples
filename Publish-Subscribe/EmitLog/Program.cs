using RabbitMQ.Client;
using System.Text;
using static System.Console;
class EmitLog
{
    public static void Main(string[] args)
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);

            var message = GetMessage(args);
            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange: "logs", routingKey: "", basicProperties: null, body: body);
            WriteLine("[x] Sent {0}", message);
        }
        WriteLine("Press [enter] to exit.");
        ReadLine();
    }
    private static string GetMessage(string[] args)
    {
        return ((args.Length > 0) ? string.Join(" ", args) : "info:Test Mesajı");
    }
}


