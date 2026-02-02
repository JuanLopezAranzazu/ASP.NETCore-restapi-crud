using ProductApi.DTOs;

namespace ProductApi.Services.Interfaces;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetAllAsync();
    Task CreateAsync(CreateProductDto dto);
}
