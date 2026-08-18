using Dapper;
using PWMS.Core.Entities;
using PWMS.Core.Interfaces;

namespace PWMS.Infra.Data.Repositories
{
	public class CategoryRepository : BaseRepository, ICategoryRepository
	{
		public CategoryRepository(IPgDbContext dbContext) : base(dbContext) { }

		public async Task<IEnumerable<Category>> GetAllAsync()
		{
			const string sql = "SELECT id, name, description FROM category";
			using var connection = _dbContext.GetConnection();
			return await connection.QueryAsync<Category>(sql);
		}

		public async Task<Category?> GetByIdAsync(int id)
		{
			const string sql = "SELECT id, name, description FROM category WHERE id = @Id";
			using var connection = _dbContext.GetConnection();
			return await connection.QueryFirstOrDefaultAsync<Category>(sql, new { Id = id });
		}
	}
}
