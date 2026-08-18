using Dapper;
using PWMS.Core.Entities;
using PWMS.Core.Interfaces;

namespace PWMS.Infra.Data.Repositories
{
	public class UserRepository : BaseRepository, IUserRepository
	{
		private const string INSERT_USER = @"INSERT INTO ""user"" 
		(email, first_name, last_name, password_hash) VALUES (@Email, @FirstName, @LastName, @PasswordHash)";

		private const string SELECT_BY_EMAIL = @"SELECT id AS ""Id"", email AS ""Email"", first_name AS ""FirstName"",
			last_name AS ""LastName"", password_hash AS ""PasswordHash""
		FROM ""user"" WHERE email = @Email";

		private const string GET_LAST_ID = "SELECT currval(pg_get_serial_sequence('user','id'))";

		public UserRepository(IPgDbContext dbContext) : base(dbContext) { }

		public async Task<int> InsertAsync(User user)
		{
			var conn = _dbContext.GetConnection();
			var transaction = _dbContext.Transaction;

			await conn.ExecuteAsync(INSERT_USER,
			new
			{
				Email = user.Email,
				FirstName = user.FirstName,
				LastName = user.LastName,
				PasswordHash = user.PasswordHash
			}, transaction);

			var result = await conn.QueryAsync<int>(GET_LAST_ID);

			return result.FirstOrDefault();
		}

		public async Task<User?> SelectByEmailAsync(string email)
		{
			var conn = _dbContext.GetConnection();

			var result = await conn.QueryFirstOrDefaultAsync<User>(SELECT_BY_EMAIL, new { Email = email });

			return result;
		}
	}
}
