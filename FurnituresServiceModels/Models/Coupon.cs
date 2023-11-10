namespace FurnituresServiceModels.Models
{
	public class Coupon
	{
		public int Id { get; set; }
		public string Code { get; set; }
		public int CouponCategoryId { get; set; }
		public Category CouponCategory { get; set; }
		public decimal CouponValuePercentage { get; set; }
		public bool isActive { get; set; }
		public ICollection<Cart>? Carts { get; set; }
	}
}
