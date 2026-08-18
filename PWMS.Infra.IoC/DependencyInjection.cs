using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModernMediator;
using PWMS.Core.Interfaces;
using PWMS.Core.Interfaces.Address;
using PWMS.Core.Interfaces.Fiscal;
using PWMS.Infra.Data;
using PWMS.Infra.Data.Logging;
using PWMS.Infra.Data.Repositories;
using PWMS.Service.Queries;
using Serilog;

namespace PWMS.Infra.IoC
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddInfrastructure(
				this IServiceCollection services,
				IConfiguration configuration)
		{
			string connectionString = configuration.GetConnectionString("DefaultConnection")
				?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

			services.AddScoped<IPgDbContext>((c) => new PgDbContext(connectionString));
			services.AddScoped<IUnitOfWork, UnitOfWork>();

			services.AddScoped<ICategoryRepository, CategoryRepository>();
			services.AddScoped<IPersonRepository, PersonRepository>();
			services.AddScoped<IInvoiceRepository, InvoiceRepository>();
			services.AddScoped<IInvoiceItemRepository, InvoiceItemRepository>();
			services.AddScoped<IMaterialRepository, MaterialRepository>();
			services.AddScoped<ILocationRepository, LocationRepository>();

			return services;
		}

		public static IServiceCollection AddServices(
			this IServiceCollection services,
			IConfiguration configuration)
		{
			services.AddModernMediator(cfg => cfg.RegisterServicesFromAssemblies(typeof(GetAllCategoriesQuery).Assembly));

			return services;
		}

		public static IServiceCollection AddLogging(
			this IServiceCollection services,
			IConfiguration configuration)
		{
			var logger = new LoggerConfiguration()
				.ReadFrom.Configuration(configuration)
				.CreateLogger();

			services.AddSingleton<ILogger>(logger);
			services.AddScoped<IApplicationLogger, SerilogApplicationLogger>();

			return services;
		}
	}
}
