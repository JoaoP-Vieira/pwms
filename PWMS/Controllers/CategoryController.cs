using Microsoft.AspNetCore.Mvc;
using ModernMediator;
using PWMS.Service.Queries;

namespace PWMS.API.Controllers
{
	[ApiController]
	[Route("category")]
	public class CategoryController : Controller
	{
		private readonly ISender _sender;

		public CategoryController(ISender sender)
		{
			_sender = sender;
		}

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var query = new GetAllCategoriesQuery();

			var result = await _sender.SendAsync(query);

			return Ok(result);
		}
	}
}
