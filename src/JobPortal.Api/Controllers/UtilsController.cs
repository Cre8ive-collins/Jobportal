using JobPortal.Application.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobPortal.Api.Controllers;

[ApiController]
[Authorize(Policy = "EmployerOnly")]
[Route("api/utils")]
public class UtilsController : ControllerBase
{
    private readonly CategoryService _categoryService;

    public UtilsController(CategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet("categories")]
    [ProducesResponseType(
        typeof(List<CategoryResponse>),
        StatusCodes.Status200OK
    )]
    public async Task<ActionResult<List<CategoryResponse>>> GetCategories()
    {
        return Ok(await _categoryService.GetAllAsync());
    }
}
