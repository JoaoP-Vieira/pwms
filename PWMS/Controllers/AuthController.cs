using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModernMediator;
using PWMS.Service.Commands;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PWMS.API.Controllers
{
	[ApiController]
	[Route("auth")]
	public class AuthController : Controller
	{
		private readonly ISender _sender;

		public AuthController(ISender sender)
		{
			_sender = sender;
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] CreateUserCommand body)
		{
			try
			{
				var result = await _sender.SendAsync(body);

				return Ok(result);
			}
			catch (ArgumentException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginCommand body)
		{
			try
			{
				var result = await _sender.SendAsync(body);

				return Ok(result);
			}
			catch (UnauthorizedAccessException ex)
			{
				return Unauthorized(ex.Message);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[Authorize]
		[HttpGet("me")]
		public IActionResult Me()
		{
			var id = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
			var email = User.FindFirstValue(JwtRegisteredClaimNames.Email);
			var firstName = User.FindFirstValue(JwtRegisteredClaimNames.GivenName);
			var lastName = User.FindFirstValue(JwtRegisteredClaimNames.FamilyName);

			return Ok(new { id, email, firstName, lastName });
		}
	}
}
