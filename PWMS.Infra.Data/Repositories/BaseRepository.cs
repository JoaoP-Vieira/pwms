namespace PWMS.Infra.Data.Repositories
{
	public abstract class BaseRepository
	{
		protected readonly IPgDbContext _dbContext;

		protected BaseRepository(IPgDbContext dbContext)
		{
			_dbContext = dbContext;
		}
	}
}
