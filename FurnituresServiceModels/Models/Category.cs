namespace FurnituresServiceModels.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Furniture> Furnitures { get; set;}
    }
}
