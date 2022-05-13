using HotChocolate.AspNetCore.Authorization;
using SimpleOrderDomain.Models;
using System.Security.Claims;

namespace OrderService.GraphQL
{
    public class Query
    {
        [Authorize]
        public IQueryable<Order> GetOrders([Service] SimpleOrderAppContext context, ClaimsPrincipal claimsPrincipal)
        {
            var userName = claimsPrincipal.Identity.Name;
            var user = context.Users.Where(o => o.UserName == userName).FirstOrDefault();
            if (user != null)
            {
                var orders = context.Orders.Where(o => o.UserId == user.Id);
                return orders.AsQueryable();
            }
            return new List<Order>().AsQueryable();
        }

        [Authorize(Roles = new[] { "MANAGER" })]
        public IQueryable<Order> GetManagerOrders([Service] SimpleOrderAppContext context) =>
          context.Orders;
    }
}
