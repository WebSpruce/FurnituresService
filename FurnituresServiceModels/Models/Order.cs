using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace FurnituresServiceModels.Models
{
    public class Order
    {
        public int? Id { get; set; }
        public string? Status { get; set; } = "Purchased";
        public decimal? Price { get; set; }
        public DateTime? OrderDate { get; set; } = DateTime.Now;
        [ForeignKey("IdentityUser")]
        public string? UserId { get; set; }
        public IdentityUser? User { get; set; }
        public ICollection<OrderFurniture> OrderFurnitures { get; set; }
    }
}
