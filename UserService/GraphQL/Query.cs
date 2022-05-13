using HotChocolate.AspNetCore.Authorization;
using SimpleOrderDomain.Models;

namespace UserService.GraphQL
{
    public class Query
    {
        [Authorize(Roles = new[] { "ADMIN" })] // dapat diakses kalau sudah login
        public IQueryable<UserData> GetUsers([Service] SimpleOrderAppContext context) =>
          context.Users.Select(p => new UserData()
          {
              Id = p.Id,
              FullName = p.FullName,
              Email = p.Email,
              Username = p.UserName
          });
    }
}
