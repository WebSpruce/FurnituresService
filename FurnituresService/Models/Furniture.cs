using System.ComponentModel.DataAnnotations.Schema;

namespace FurnituresService.Models
{
    public class Furniture
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        [Column(TypeName = "decimal(6,2)")]
        public decimal Price { get; set; }
        public byte[]? ImageData { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public ICollection<OrderFurniture>? OrderFurnitures { get; set; }
        public ICollection<CartFurniture>? CartFurnitures { get; set; }
    }
}
