using Microsoft.AspNetCore.Mvc;
using ModernMediator;
using PWMS.Service.Commands;

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

		[HttpPost]
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

		[HttpPut("assing/vehicle")]
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

		[HttpPost("confer")]
		public async Task<IActionResult> ConferItem([FromBody] ConferInvoiceItemCommand body)
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
