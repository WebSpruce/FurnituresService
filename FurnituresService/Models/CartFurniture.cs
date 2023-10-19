namespace asp_mvc_1.Models
{
    public class CartFurniture
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public int FurnitureId { get; set; }
        public Cart Cart { get; set; } = null!;
        public Furniture Furniture { get; set; } = null!;
    }
}