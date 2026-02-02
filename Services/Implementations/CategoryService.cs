using Microsoft.EntityFrameworkCore;
using ProductApi.Data;
using ProductApi.DTOs;
using ProductApi.Models;
using ProductApi.Services.Interfaces;

namespace ProductApi.Services.Implementations;

public class CategoryService : ICategoryService
{
    private readonly AppDbContext _context;

    public CategoryService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CategoryDto>> GetAllAsync()
    {
        return await _context.Categories
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name
            })
            .ToListAsync();
    }

    public async Task<CategoryDto> CreateAsync(CreateCategoryDto dto)
    {
        // Verificar si la categoría ya existe
        var exists = await _context.Categories
            .AnyAsync(c => c.Name.ToLower() == dto.Name.ToLower());

        if (exists)
        {
            throw new ConflictException("La categoría ya existe.");
        }

        var category = new Category
        {
            Name = dto.Name
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name
        };
    }

    public async Task<CategoryDto> GetByIdAsync(int id)
    {
        // Buscar la categoría por ID
        var category = await _context.Categories
            .Where(c => c.Id == id)
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name
            })
            .FirstOrDefaultAsync();

        if (category == null)
            throw new NotFoundException("Categoría no encontrada.");

        return category;
    }


    public async Task<CategoryDto> UpdateAsync(int id, UpdateCategoryDto dto)
    {
        // Buscar la categoría por ID
        var category = await _context.Categories.FindAsync(id);

        if (category == null)
            throw new NotFoundException("Categoría no encontrada.");

        // Verificar si otra categoría con el mismo nombre ya existe
        var exists = await _context.Categories
            .AnyAsync(c => c.Name.ToLower() == dto.Name.ToLower() && c.Id != id);

        if (exists)
            throw new ConflictException("Ya existe otra categoría con ese nombre.");

        category.Name = dto.Name;
        await _context.SaveChangesAsync();

        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name
        };
    }

    public async Task DeleteAsync(int id)
    {   
        // Buscar la categoría por ID
        var category = await _context.Categories.FindAsync(id);

        if (category == null)
            throw new NotFoundException("Categoría no encontrada.");

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
    }

}
