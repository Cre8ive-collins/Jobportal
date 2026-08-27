using JobPortal.Application.Auth;
using Microsoft.AspNetCore.Mvc;

namespace JobPortal.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    [EndpointSummary("Register an account")]
    [EndpointDescription(
        "Creates a job seeker or employer account using the supplied profile " +
        "and credentials. Email addresses must be unique."
    )]
    [ProducesResponseType(typeof(RegisterResponse), StatusCodes.Status201Created)]
    public async Task<ActionResult<RegisterResponse>> Register(
        RegisterRequest request
    )
    {
        var user = await _authService.RegisterAsync(request);

        return StatusCode(
            StatusCodes.Status201Created,
            new RegisterResponse
            {
                User = user
            }
        );
    }

    [HttpPost("login")]
    [EndpointSummary("Log in")]
    [EndpointDescription(
        "Authenticates an active account and returns the user profile and a " +
        "JWT access token for authorized API requests."
    )]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
    {
        var response = await _authService.LoginAsync(request);

        return Ok(response);
    }
}
