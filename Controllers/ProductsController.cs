using Microsoft.AspNetCore.Mvc;
using InvoicesApp.Models;
using InvoicesApp.Repositories.Interfaces;

namespace InvoicesApp.Controllers;

public class ProductsController : Controller
{
    private readonly ILogger<ProductsController> _logger;
    private readonly IProductRepository _productRepository;

    public ProductsController(
        ILogger<ProductsController> logger,
        IProductRepository productRepository
    )
    {
        _logger = logger;
        _productRepository = productRepository;
    }

    [HttpGet("Products/GetProductById/{productId}")]
    public async Task<IActionResult> GetProductById(int productId)
    {
        ProductModel result = await _productRepository.GetByIdAsync(productId);
        return StatusCode(StatusCodes.Status200OK, result);
    }
}
