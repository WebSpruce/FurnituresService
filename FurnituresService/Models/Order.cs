using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace FurnituresService.Models
{
    public class Order
    {
        public int? Id { get; set; }
        public string? Status { get; set; } = "Purchased";
        public DateTime? OrderDate { get; set; } = DateTime.Now;
        [ForeignKey("IdentityUser")]
        public int? UserId { get; set; }
        public IdentityUser? User { get; set; }
        public ICollection<OrderFurniture> OrderFurnitures { get; set; }
    }
}
