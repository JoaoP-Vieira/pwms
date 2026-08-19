using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ModernMediator;
using PWMS.Core.Interfaces;
using PWMS.Core.Interfaces.Address;
using PWMS.Core.Interfaces.Fiscal;
using PWMS.Infra.Data;
using PWMS.Infra.Data.Logging;
using PWMS.Infra.Data.Repositories;
using PWMS.Infra.Data.Security;
using PWMS.Service.Queries;
using PWMS.Service.RabbitMQ;
using PWMS.Service.RabbitMQ.Interfaces;
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
			services.AddScoped<IUserRepository, UserRepository>();

			services.AddSingleton<IPasswordHasher, PasswordHasher>();
			services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

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
			// O Serilog já está configurado no Program.cs via UseSerilog()
			// Aqui apenas registramos o wrapper IApplicationLogger
			services.AddScoped<IApplicationLogger>(sp =>
			{
				var logger = Log.Logger;
				return new SerilogApplicationLogger(logger);
			});

			return services;
		}

		public static IServiceCollection AddJwtAuthentication(
			this IServiceCollection services,
			IConfiguration configuration)
		{
			var jwtSection = configuration.GetSection("Jwt");

			services.Configure<JwtSettings>(jwtSection);

			var settings = jwtSection.Get<JwtSettings>()
				?? throw new InvalidOperationException("Jwt configuration section not found.");

			if (string.IsNullOrWhiteSpace(settings.Key))
				throw new InvalidOperationException("Jwt:Key configuration is required.");

			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options =>
			{
				options.MapInboundClaims = false;
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidIssuer = settings.Issuer,
					ValidateAudience = true,
					ValidAudience = settings.Audience,
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Key)),
					ValidateLifetime = true,
					ClockSkew = TimeSpan.FromMinutes(1)
				};
			});

			services.AddAuthorization();

			return services;
		}

		public static IServiceCollection AddRabbitMQ(
			this IServiceCollection services,
			IConfiguration configuration)
		{
			var rabbitMQConfig = configuration.GetSection("RabbitMQ");
			
			var hostName = rabbitMQConfig["HostName"] ?? "localhost";
			var port = int.Parse(rabbitMQConfig["Port"] ?? "5672");
			var userName = rabbitMQConfig["UserName"] ?? "guest";
			var password = rabbitMQConfig["Password"] ?? "guest";

			services.AddSingleton<IRabbitMQConnection>(sp =>
			{
				var logger = sp.GetRequiredService<ILogger>();
				return new RabbitMQConnection(hostName, port, userName, password, logger);
			});

			services.AddSingleton(sp => sp.GetRequiredService<IRabbitMQConnection>().GetConnection());

			services.AddScoped<IRabbitMQPublisher, RabbitMQPublisher>();

			return services;
		}
	}
}
