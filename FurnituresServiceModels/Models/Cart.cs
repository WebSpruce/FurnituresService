using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace FurnituresServiceModels.Models
{
    public class Cart
    {
        public int Id { get; set; }
        [ForeignKey("IdentityUser")]
        public string UserId { get; set; }
        public IdentityUser User { get; set; }
        public ICollection<CartFurniture>? CartFurnitures { get; set; }
        public int? CouponId { get; set; }
        public Coupon? Coupon { get; set; }
    }
}
