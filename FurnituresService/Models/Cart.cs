using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace FurnituresService.Models
{
    public class Cart
    {
        public int Id { get; set; }
        [ForeignKey("IdentityUser")]
        public int UserId { get; set; }
        public IdentityUser User { get; set; }
        public ICollection<CartFurniture>? CartFurnitures { get; set; }
    }
}
