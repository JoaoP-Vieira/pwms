using PWMS.Core.Entities.Address;

namespace PWMS.Core.Interfaces.Address
{
	public interface ILocationRepository
	{
		Task<Location?> GetAvaliableConferenceLocation();
		Task<IEnumerable<Location>> GetAllConferenceLocationInUse();
	}
}
