using System.ComponentModel.DataAnnotations.Schema;

namespace FurnituresServiceApi.Dto
{
    public class FurnitureDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        [Column(TypeName = "decimal(6,2)")]
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
    }
}
