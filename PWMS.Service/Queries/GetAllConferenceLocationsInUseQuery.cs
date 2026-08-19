using ModernMediator;
using PWMS.Core.Interfaces.Address;
using PWMS.Service.DTO;

namespace PWMS.Service.Queries
{
	public record GetAllConferenceLocationsInUseQuery : IRequest<IEnumerable<LocationDTO>>;

	public class GetAllConferenceLocationsInUseQueryHandler : IValueTaskRequestHandler<GetAllConferenceLocationsInUseQuery, IEnumerable<LocationDTO>>
	{
		private readonly ILocationRepository _locationRepository;

		public GetAllConferenceLocationsInUseQueryHandler(ILocationRepository locationRepository)
		{
			_locationRepository = locationRepository;
		}

		public async ValueTask<IEnumerable<LocationDTO>> Handle(GetAllConferenceLocationsInUseQuery request, CancellationToken cancellationToken = default)
		{
			var result = await _locationRepository.GetAllConferenceLocationInUse();

			return result.Select(x => new LocationDTO()
			{
				Id = x.Id,
				Identification = x.Identification,
				IsLocked = x.IsLocked
			});
		}
	}
}
