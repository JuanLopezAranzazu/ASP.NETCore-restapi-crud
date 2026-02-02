using Microsoft.EntityFrameworkCore;
using ProductApi.Data;
using ProductApi.DTOs;
using ProductApi.Models;
using ProductApi.Services.Interfaces;

namespace ProductApi.Services.Implementations;

public class ProductService : IProductService
{
    private readonly AppDbContext _context;

    public ProductService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProductDto>> GetAllAsync()
    {
        return await _context.Products
            .Include(p => p.Category)
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                CategoryName = p.Category.Name
            })
            .ToListAsync();
    }

    public async Task<ProductDto> CreateAsync(CreateProductDto dto)
    {
        // Validar que la categoría exista
        var categoryExists = await _context.Categories
            .AnyAsync(c => c.Id == dto.CategoryId);

        if (!categoryExists)
            throw new BadRequestException("La categoría no existe.");

        // Validar que no exista un producto con el mismo nombre en la misma categoría
        var exists = await _context.Products
            .AnyAsync(p =>
                p.Name.ToLower() == dto.Name.ToLower() &&
                p.CategoryId == dto.CategoryId);

        if (exists)
            throw new ConflictException("El producto ya existe en esa categoría.");

        var product = new Product
        {
            Name = dto.Name,
            Price = dto.Price,
            CategoryId = dto.CategoryId
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return await GetByIdAsync(product.Id);
    }

    public async Task<ProductDto> GetByIdAsync(int id)
    {
        // Buscar el producto por ID e incluir la categoría
        var product = await _context.Products
            .Include(p => p.Category)
            .Where(p => p.Id == id)
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                CategoryName = p.Category.Name
            })
            .FirstOrDefaultAsync();

        if (product == null)
            throw new NotFoundException("Producto no encontrado.");

        return product;
    }

    public async Task<ProductDto> UpdateAsync(int id, UpdateProductDto dto)
    {   
        // Buscar el producto por ID
        var product = await _context.Products.FindAsync(id);

        if (product == null)
            throw new NotFoundException("Producto no encontrado.");

        // Validar que la categoría exista
        var categoryExists = await _context.Categories
            .AnyAsync(c => c.Id == dto.CategoryId);

        if (!categoryExists)
            throw new BadRequestException("La categoría no existe.");

        // Validar que no exista otro producto con el mismo nombre en la misma categoría
        var exists = await _context.Products
            .AnyAsync(p =>
                p.Id != id &&
                p.Name.ToLower() == dto.Name.ToLower() &&
                p.CategoryId == dto.CategoryId);

        if (exists)
            throw new ConflictException("Ya existe otro producto con ese nombre en la categoría.");

        product.Name = dto.Name;
        product.Price = dto.Price;
        product.CategoryId = dto.CategoryId;

        await _context.SaveChangesAsync();

        return await GetByIdAsync(id);
    }

    public async Task DeleteAsync(int id)
    {
        // Buscar el producto por ID
        var product = await _context.Products.FindAsync(id);

        if (product == null)
            throw new NotFoundException("Producto no encontrado.");

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }

}
