using FurnituresServiceModels.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace FurnituresServiceApi.Dto
{
    public class OrderDto
    {
        public int? Id { get; set; }
        public string? Status { get; set; } = "Purchased";
        public decimal? Price { get; set; }
        public DateTime? OrderDate { get; set; } = DateTime.Now;
        [ForeignKey("IdentityUser")]
        public string? UserId { get; set; }
    }
}
