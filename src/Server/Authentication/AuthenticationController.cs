using Microsoft.AspNetCore.Mvc;
using TradingJournal.Application.Common.Interfaces;
using TradingJournal.Application.Common.Models;

namespace TradingJournal.Server.Authentication;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationRepository _authRepository;

    public AuthenticationController(IAuthenticationRepository authRepository)
    {
        _authRepository = authRepository;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserRegistration registration)
    {
        var response = await _authRepository.Register(registration);

        if (response.Success)
            return Ok(response);

        return BadRequest(response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLogin login)
    {
        var response = await _authRepository.Login(login);

        if (response.Success)
            return Ok(response);

        return BadRequest(response);
    }
}