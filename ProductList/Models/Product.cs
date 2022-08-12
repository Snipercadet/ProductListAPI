using System.ComponentModel.DataAnnotations;

public class Product
{
    [Key]
    public int ProductId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    
}