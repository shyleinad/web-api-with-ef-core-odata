using web_api_with_ef_core_odata.Models;

namespace web_api_with_ef_core_odata.Services;

public interface ICategoryService
{
    Task<IEnumerable<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(int id);
    Task<Category> CreateAsync(Category category);
    Task<Category?> UpdateAsync(int id, Category updateData);
    Task<bool> DeleteAsync(int id);
}
