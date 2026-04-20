namespace MGM.GoodHamburger.Domain.Entities;

public class Order
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    public int? SandwichId { get; set; }
    public MenuItem? Sandwich { get; set; }
    
    public int? SideDishId { get; set; }
    public MenuItem? SideDish { get; set; }
    
    public int? DrinkId { get; set; }
    public MenuItem? Drink { get; set; }
    
    public decimal Subtotal { get; set; }
    public decimal DiscountPercentage { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal Total { get; set; }
}