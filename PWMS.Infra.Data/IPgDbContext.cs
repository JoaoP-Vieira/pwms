using Npgsql;

namespace PWMS.Infra.Data
{
	public interface IPgDbContext
	{
		NpgsqlConnection GetConnection();
		NpgsqlTransaction BeginTransaction();
		NpgsqlTransaction? Transaction { get; }
		Task<NpgsqlTransaction> BeginTransactionAsync();
	}
}
