using Orders.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Infrastructure.Entities
{
    public class OrderEntity
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [MaxLength(256)]
        public string CustomerEmail { get; set; }
        public OrderStatus Status { get; set; }
        public ICollection<OrderItemEntity> Items { get; set; } = new List<OrderItemEntity>();
    }

    public class OrderItemEntity
    {
        [Key]
        public long Id { get; set; }
        public long ProductId { get; set; }

        [Required]
        public long Quantity { get; set; }

        public long OrderId { get; set; }

        [ForeignKey("OrderId")]
        public OrderEntity Order { get; set; }
    }
}
