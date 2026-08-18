using PWMS.Core.Entities;

namespace PWMS.Core.Interfaces
{
	public interface IPersonRepository
	{
		Task<int> InsertAsync(Person person);
		Task<Person?> SelectByDocument(string document);
	}
}
