using System;
using System.Collections.Generic;

namespace SimpleOrderDomain.Models
{
    public partial class Product
    {
        public Product()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Stock { get; set; }
        public int Price { get; set; }
        public DateTime Created { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
