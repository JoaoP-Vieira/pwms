using Dapper;
using PWMS.Core.Entities;
using PWMS.Core.Interfaces;
using PWMS.Infra.Data.Model;

namespace PWMS.Infra.Data.Repositories
{
	public class PersonRepository : BaseRepository, IPersonRepository
	{
		private const string INSERT_PERSON = "INSERT INTO person " +
		"(name, document, address) VALUES (@Name, @Document, @Address)";

		private const string SELECT_BY_DOCUMENT = @"SELECT id, name, document, address 
		FROM person WHERE document = @Document";

		private const string GET_LAST_ID = "SELECT currval(pg_get_serial_sequence('person','id'))";

		public PersonRepository(IPgDbContext dbContext) : base(dbContext) { }
		
		public async Task<int> InsertAsync(Person person)
		{
			var conn = _dbContext.GetConnection();
			var transaction = _dbContext.Transaction;

			await conn.ExecuteAsync(INSERT_PERSON,
			new {
				Name = person.Name,
				Document = person.GetDocument(),
				Address = person.Address
			}, transaction);

			var result = await conn.QueryAsync<int>(GET_LAST_ID);

			return result.FirstOrDefault();
		}

		public async Task<Person?> SelectByDocument(string document)
		{
			var conn = _dbContext.GetConnection();

			var result = await conn.QueryFirstOrDefaultAsync<Person>(SELECT_BY_DOCUMENT, new { Document = document });

			return result;
		}
	}
}
