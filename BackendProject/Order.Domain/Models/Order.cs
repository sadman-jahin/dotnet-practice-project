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
        public DateTime OrderDate { get; set; }
        public List<OrderItem> Items { get; set; } = new();
        public string CustomerName { get; set; }
        public long TotalAmount { get; set; }
    }

    public class OrderItem
    {
        public long Id { get; set; }
        public long Name { get; set; }
    }
}
