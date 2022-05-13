using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OrderProcessor;
using SimpleOrderDomain.Models;

Console.WriteLine("OrderProcesorApp");

IConfiguration configuration = new ConfigurationBuilder()
      .AddJsonFile("appsettings.json", true, true)
      .Build();
//Console.WriteLine($"b {configuration.GetConnectionString("MyDatabase")}");
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
           OrderDataKafka orderDataKafka = JsonConvert.DeserializeObject<OrderDataKafka>(cr.Message.Value);
            // EF
            using (var context = new SimpleOrderAppContext())
            {
                var user = context.Users.Where(o => o.UserName == orderDataKafka.UserName).SingleOrDefault();
                //List<Order> orders = new ();
                //foreach(var order in orderDataKafka)
                //{
                    Order currOrder = new();
                    currOrder.Code = orderDataKafka.Code;
                    currOrder.UserId = user.Id;
                    currOrder.ProductId = orderDataKafka.ProductId;
                    currOrder.Quantity = orderDataKafka.Quantity;
                //}

                context.Orders.Add(currOrder);
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

