using Microsoft.AspNetCore.Mvc;
using ModernMediator;
using PWMS.Service.Queries;

namespace PWMS.API.Controllers
{
	[ApiController]
	[Route("location")]
	public class LocationController : Controller
	{
		private readonly ISender _sender;

		public LocationController(ISender sender)
		{
			_sender = sender;
		}

		[HttpGet("conference/in-use")]
		public async Task<IActionResult> GetAllConferenceLocationsInUse()
		{
			var query = new GetAllConferenceLocationsInUseQuery();

			var result = await _sender.SendAsync(query);

			return Ok(result);
		}
	}
}
