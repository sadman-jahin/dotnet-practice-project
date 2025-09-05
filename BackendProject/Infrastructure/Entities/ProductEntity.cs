using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class ProductEntity
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Range(0, long.MaxValue)]
        public long Quantity { get; set; }
    }
}
