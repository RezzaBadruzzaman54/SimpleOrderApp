using HotChocolate.AspNetCore.Authorization;
using SimpleOrderDomain.Models;

namespace ProductService.GraphQL
{
    public class Query
    {
        [Authorize]
        public IQueryable<ProductData> GetProducts([Service] SimpleOrderAppContext context) =>
           context.Products.Select(p=>new ProductData()
           {
               Id = p.Id,
               Name = p.Name,
               Stock = p.Stock,
               Price = p.Price,
               Created = p.Created
           });
    }
}
