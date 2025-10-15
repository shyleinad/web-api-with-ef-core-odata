namespace web_api_with_ef_core_odata.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<Product>? Products { get; set; }
}