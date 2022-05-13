using HotChocolate.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SimpleOrderDomain.Models;
using System.Security.Claims;

namespace OrderService.GraphQL
{
    public class Mutation
    {
        [Authorize]
        public async Task<OrderOutput> SubmitOrderAsync(
          OrderInput input,
           ClaimsPrincipal claimsPrincipal,
           [Service] IOptions<KafkaSettings> settings)
        {
            var userName = claimsPrincipal.Identity.Name;

                OrderDataKafka order = new();
                order.Code = input.Code;
                order.ProductId = input.ProductId;
                order.UserName = userName;
                order.Quantity = input.Quantity;

            var dts = DateTime.Now.ToString();
            var key = "order-" + dts;

            var val = JsonConvert.SerializeObject(order);

            var result = await KafkaHelper.SendMessage(settings.Value, "simpleorderapp", key, val);
            Console.WriteLine(val);
            OrderOutput resp = new OrderOutput
            {
                TransactionDate = dts,
                Message = "Order was submitted successfully"
            };

            if (!result)
                resp.Message = "Failed to submit data";

            return await Task.FromResult(resp);
        }


    }
}
