using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModernMediator;
using PWMS.Service.Commands;
using PWMS.Service.Queries;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PWMS.API.Controllers
{
	[ApiController]
	[Route("invoice")]
	public class InvoiceController : Controller
	{
		private readonly ISender _sender;

		public InvoiceController(ISender sender)
		{
			_sender = sender;
		}

		[HttpGet("list-created")]
		[Authorize]
		public async Task<IActionResult> GetInvoicesReadyToAssignVehicle()
		{
			try
			{
				var query = new GetInvoicesReadyToAssignVehicleQuery();

				var result = await _sender.SendAsync(query);

				return Ok(result);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> SingInvoice([FromBody] CreateInvoiceCommand body)
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

		[Authorize]
		[HttpPut("assign/vehicle")]
		public async Task<IActionResult> AssingVehicleToInvoice([FromBody] AssingVehicleToInvoiceCommand body)
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

		[Authorize]
		[HttpPost("confer")]
		public async Task<IActionResult> ConferItem([FromBody] ConferInvoiceItemBody body)
		{
			try
			{
				var userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub);

				if (string.IsNullOrWhiteSpace(userId)) return Unauthorized();

				var result = await _sender.SendAsync(new ConferInvoiceItemCommand(body.barCode, body.quantity, int.Parse(userId)));

				return Ok(result);
			}
			catch (ArgumentException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (InvalidOperationException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}
	}
}
