using ProductApi.DTOs;

namespace ProductApi.Services.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDto>> GetAllAsync();
    Task CreateAsync(CreateCategoryDto dto);
}
