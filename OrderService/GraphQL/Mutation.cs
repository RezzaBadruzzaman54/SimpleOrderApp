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
           [Service] IOptions<KafkaSettings> settings)
        {
            var dts = DateTime.Now.ToString();
            var key = "order-" + dts;
            var val = JsonConvert.SerializeObject(input);

            var result = await KafkaHelper.SendMessage(settings.Value, "simpleorderapp", key, val);

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
