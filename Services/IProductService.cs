using web_api_with_ef_core_odata.Models;

namespace web_api_with_ef_core_odata.Services;

public interface IProductService
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(int id);
    Task<Product> CreateAsync(Product product);
    Task<Product?> UpdateAsync(int id, Product updateData);
    Task<bool> DeleteAsync(int id);
}
