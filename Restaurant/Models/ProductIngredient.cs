namespace Restaurant.Models
{
    public class ProductIngredient
    {
        public int ProductId { get; set; }
        public Product Product { get; set; } // Ensure Product is not null
        public int IngredientId { get; set; }
        public Ingredient Ingredient { get; set; } // Ensure Ingredient is not null
    }
}   