using System.ComponentModel.DataAnnotations.Schema;

namespace web_api_with_ef_core_odata.Models;

public class Product
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    [ForeignKey("CategoryId")]
    public Category? Category { get; set; }
}