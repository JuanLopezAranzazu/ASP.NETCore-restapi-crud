using Microsoft.AspNetCore.Mvc;
using ProductApi.DTOs;
using ProductApi.Services.Interfaces;

namespace ProductApi.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _service;

    public CategoriesController(ICategoryService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCategoryDto dto)
    {
        await _service.CreateAsync(dto);
        return Ok();
    }
}
