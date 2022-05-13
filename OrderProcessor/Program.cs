using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using SimpleOrderDomain.Models;

Console.WriteLine("OrderProcesorApp");

IConfiguration configuration = new ConfigurationBuilder()
      .AddJsonFile("appsettings.json", true, true)
      .Build();

var config = new ConsumerConfig
{
    //BootstrapServers = "localhost:9092",
    BootstrapServers = configuration.GetSection("KafkaSettings").GetSection("Server").Value,
    GroupId = "tester",
    AutoOffsetReset = AutoOffsetReset.Earliest
};

var topic = "simpleorderapp";
CancellationTokenSource cts = new CancellationTokenSource();
Console.CancelKeyPress += (_, e) =>
{
    e.Cancel = true; // prevent the process from terminating.
    cts.Cancel();
};

using (var consumer = new ConsumerBuilder<string, string>(config).Build())
{
    Console.WriteLine("Connected");
    consumer.Subscribe(topic);
    try
    {
        while (true)
        {
            var cr = consumer.Consume(cts.Token); // blocking
            Console.WriteLine($"Consumed record with key: {cr.Message.Key} and value: {cr.Message.Value}");

            // EF
            using (var context = new SimpleOrderAppContext())
            {
                Order order = new Order();
                order.Code = cr.Message.Key;
                //order.UserId = Convert.ToInt32(cr.Message.Value);
                order.ProductId = Convert.ToInt32(cr.Message.Value);
                order.Quantity = Convert.ToInt32(cr.Message.Value);

                context.Orders.Add(order);
                context.SaveChanges();
            }
        }
    }
    catch (OperationCanceledException)
    {
        // Ctrl-C was pressed.
    }
    finally
    {
        consumer.Close();
    }

}

