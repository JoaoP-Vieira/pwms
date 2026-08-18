using Microsoft.AspNetCore.Mvc;
using ModernMediator;
using PWMS.Service.Commands;
using PWMS.Service.Queries;

namespace PWMS.API.Controllers
{
	[ApiController]
	[Route("person")]
	public class PersonController : Controller
	{
		private readonly ISender _sender;

		public PersonController(ISender sender)
		{
			_sender = sender;
		}

		[HttpGet("{document}")]
		public async Task<IActionResult> GetByDocument(string document)
		{
			try
			{
				var result = await _sender.SendAsync(new GetPersonByDocumentQuery(document));

				return Ok(result);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpPost]
		public async Task<IActionResult> Post([FromBody] CreatePersonCommand body)
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
	}
}
