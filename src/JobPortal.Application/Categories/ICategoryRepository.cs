using JobPortal.Domain.Entities;

namespace JobPortal.Application.Categories;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllAsync();

    Task<bool> ExistsAsync(Guid id);
}
