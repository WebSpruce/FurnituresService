using FurnituresServiceModels.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace FurnituresServiceApi.Dto
{
    public class CartDto
    {
        public int Id { get; set; }
        [ForeignKey("IdentityUser")]
        public string UserId { get; set; }
        public int? CouponId { get; set; }
    }
}
