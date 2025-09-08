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
        public DateTime OrderDate { get; set; }

        [Required]
        [MaxLength(200)]
        public string CustomerName { get; set; }

        [Range(0, double.MaxValue)]
        public long TotalAmount { get; set; }
        public List<OrderItemEntity> Items { get; set; } = new();

    }

    public class OrderItemEntity
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public long OrderId { get; set; }

        [ForeignKey("OrderId")]
        public OrderEntity Order { get; set; }
    }
}
