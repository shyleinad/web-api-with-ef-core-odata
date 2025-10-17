using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace web_api_with_ef_core_odata.Models;

public class Product
{
    public int Id { get; set; }

    [Required]
    public int CategoryId { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Range(0, 999_999_999)]
    public decimal Price { get; set; }
    [ForeignKey("CategoryId")]
    public Category? Category { get; set; }
}