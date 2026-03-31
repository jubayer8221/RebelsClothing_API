namespace RebelsClothing.Models.Entities
{
    public class Product
    {
       public int Id { get; set; }

        public int ProductId { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required decimal Price { get; set; }

        public decimal Discount { get; set; }

        public int Stock { get; set; }

        public int CategoryId { get; set; }               // Foreign Key
        //public Category? Category { get; set; }          // Navigation property

        public string? Fabric { get; set; }

        public string? Color { get; set; }

        public string? Size { get; set; }

        public string? Type { get; set; }

        public string? Brand { get; set; }

        public string? Material { get; set; }

        public string? CareInstructions { get; set; }

        public string? GenderID { get; set; }


        public  string? ImageUrl { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
