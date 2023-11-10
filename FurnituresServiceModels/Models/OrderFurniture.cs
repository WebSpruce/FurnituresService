namespace FurnituresServiceModels.Models
{
    public class OrderFurniture
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int FurnitureId { get; set; }
        public Order Order { get; set; } = null!;
        public Furniture Furniture { get; set; } = null!;
    }
}