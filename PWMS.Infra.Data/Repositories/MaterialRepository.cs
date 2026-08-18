using Dapper;
using PWMS.Core.Entities;
using PWMS.Core.Interfaces;

namespace PWMS.Infra.Data.Repositories
{
	public class MaterialRepository : BaseRepository, IMaterialRepository
	{
		private const string SELECT_BY_SKU = @"SELECT m.id AS ""Id"",
			m.sku AS ""SKU"",
			m.barcode AS ""Barcode"",
			m.name AS ""Name"",
			m.description AS ""Description"",
			m.weight AS ""Weight"",
			m.height AS ""Height"",
			m.width AS ""Width"",
			m.length AS ""Length"",
			m.status AS ""Status"",
			m.created_at AS ""CreatedAt"",
			m.updated_at AS ""UpdatedAt"",
			c.id AS ""Id"",
			c.name AS ""Name"",
			c.description AS ""Description""
		FROM public.material m
			INNER JOIN public.category c ON c.id = m.category_id
		WHERE m.sku = @Sku;";

		public MaterialRepository(IPgDbContext dbContext) : base(dbContext)
		{ }

		public async Task<Material?> GetBySkuAsync(string sku)
		{
			var conn = _dbContext.GetConnection();

			var result = await conn.QueryAsync<Material, Category, Material>(SELECT_BY_SKU, (material, category) =>
			{
				material.SetCategory(category);
				return material;
			}, new { Sku = sku });

			return result.FirstOrDefault();
		}
	}
}
