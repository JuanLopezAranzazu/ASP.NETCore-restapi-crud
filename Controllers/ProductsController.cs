using Microsoft.AspNetCore.Mvc;
using ProductApi.DTOs;
using ProductApi.Services.Interfaces;

namespace ProductApi.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _service;

    public ProductsController(IProductService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProductDto dto)
    {
        await _service.CreateAsync(dto);
        return Ok();
    }
}
