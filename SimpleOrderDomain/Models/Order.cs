using System;
using System.Collections.Generic;

namespace SimpleOrderDomain.Models
{
    public partial class Order
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public virtual Product Product { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
