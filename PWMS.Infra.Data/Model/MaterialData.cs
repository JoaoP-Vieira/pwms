namespace PWMS.Infra.Data.Model
{
	public class MaterialData
	{
		public Guid id { get; set; }
		public string sku { get; set; } = string.Empty;
		public string barcode { get; set; } = string.Empty;
		public string name { get; set; } = string.Empty;
		public string description { get; set; } = string.Empty;
		public Guid? category_id { get; set; }
		public decimal weight { get; set; }
		public decimal height { get; set; }
		public decimal width { get; set; }
		public decimal length { get; set; }
		public int status { get; set; }
		public DateTime created_at { get; set; }
		public DateTime? updated_at { get; set; }
	}
}
