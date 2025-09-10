using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderClose.Application.Models
{
    internal class OrderDto
    {
        public long Id { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();
        public string CustomerEmail { get; set; }
        public OrderStatus Status { get; set; }
    }

    public class OrderItemDto
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        public long Quantity { get; set; }
    }

    public enum OrderStatus
    {
        Pending = 1,
        Closed = 2,
    }
}
