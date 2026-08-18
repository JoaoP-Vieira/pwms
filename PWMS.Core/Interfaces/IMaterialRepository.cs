using PWMS.Core.Entities;

namespace PWMS.Core.Interfaces
{
	public interface IMaterialRepository
	{
		Task<Material?> GetBySkuAsync(string sku);
	}
}
