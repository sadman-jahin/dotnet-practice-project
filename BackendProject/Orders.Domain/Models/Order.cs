using Orders.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Domain.Models
{
    public class Order
    {
        public long Id { get; set; }
        public List<OrderItem> Items { get; set; } = new();
        public string CustomerEmail { get; set; }
        public OrderStatus Status { get; set; }
    }

    public class OrderItem
    {
        public long Id { get; set; }
        public long ProductId { get; set; } 
        public long Quantity { get; set; }
    }
}
