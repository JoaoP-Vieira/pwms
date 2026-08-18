using Dapper;
using PWMS.Core.Entities;
using PWMS.Core.Entities.Address;
using PWMS.Core.Entities.Fiscal;
using PWMS.Core.Interfaces.Fiscal;

namespace PWMS.Infra.Data.Repositories
{
	public class InvoiceRepository : BaseRepository, IInvoiceRepository
	{
		private const string SELECT_INVOICE_BY_INVOICE_NUMBER = @"SELECT i.id AS ""Id"",
			i.invoice_number AS ""InvoiceNumber"",
			i.series AS ""Series"",
			i.verification_code AS ""VerificationCode"",
			i.invoice_type AS ""InvoiceType"",
			i.status AS ""Status"",
			i.issue_date AS ""IssueDate"",
			i.expected_delivery_date AS ""ExpectedDeliveryDate"",
			i.total_amount AS ""TotalAmount"",
			i.total_volumes AS ""TotalVolumes"",
			i.created_at AS ""CreatedAt"",
			i.modified_at AS ""ModifiedAt"",
			i.plate_num_veh AS ""Plate_num_veh"",
			isp.id AS ""Id"",
			isp.name AS ""Name"",
			isp.document AS ""Document"",
			isp.address AS ""Address"",
			rcp.id AS ""Id"",
			rcp.name AS ""Name"",
			rcp.document AS ""Document"",
			rcp.address AS ""Address"",
			crp.id AS ""Id"",
			crp.name AS ""Name"",
			crp.document AS ""Document"",
			crp.address AS ""Address"",
			l.id AS ""Id"",
			l.""zone"" AS ""Zone"",
			l.is_locked AS ""IsLocked"",
			l.identification AS ""Identification"",
			l.aisle AS ""Aisle"",
			l.""column"" AS ""Column"",
			l.""level"" AS ""Level"",
			l.slot AS ""Slot"",
			l.""depth"" AS ""Depth""
		FROM public.invoice i
			LEFT JOIN person isp ON isp.id = i.issuer_id
			LEFT JOIN person rcp ON rcp.id = i.carrier_id
			LEFT JOIN person crp ON crp.id = i.carrier_id
			LEFT JOIN location l ON l.id = i.conference_location_id
		WHERE i.invoice_number = @InvoiceNumber;
		";

		private const string INSERT_INVOICE = @"INSERT INTO invoice 
		(
			id,
			invoice_number,
			series,
			verification_code,
			invoice_type,
			status,
			issuer_id,
			recipient_id,
			carrier_id,
			issue_date,
			expected_delivery_date,
			total_amount,
			total_volumes,
			created_at,
			modified_at
		)
		VALUES
		(
			@Id,
			@InvoiceNumber,
			@Series,
			@VerificationCode,
			@InvoiceType,
			@Status,
			@IssuerId,
			@RecipientId,
			@CarrierId,
			@IssueDate,
			@ExpectedDeliveryDate,
			@TotalAmount,
			@TotalVolumes,
			@CreatedAt,
			NULL
		);";

		private const string INSERT_INVOICE_ITEM = @"INSERT INTO invoice_item
		(
			invoice_id,
			line_number,
			declared_quantity,
			processed_quantity,
			unity_price,
			material_id
		)
		VALUES
		(
			@InvoiceId,
			@LineNumber,
			@DeclaredQuantity,
			NULL,
			@UnitPrice,
			@MaterialId
		);";

		private const string UPDATE_PLATE_NUM = @"UPDATE invoice SET
			status = 1,
			plate_num_veh = @PlateNum,
			conference_location_id = @LocationId,
			modified_at = @ModifiedAt
		WHERE
		invoice_number = @InvoiceNumber";

		private const string UPDATE_STATUS = @"UPDATE invoice SET
			status = @Status,
			modified_at = @ModifiedAt
		WHERE
		invoice_number = @InvoiceNumber";

		public InvoiceRepository(IPgDbContext dbContext) : base(dbContext) { }

		public async Task<Invoice?> SelectAsync(string invoiceNumber)
		{
			var conn = _dbContext.GetConnection();

			var result = await conn.QueryAsync<Invoice, Person, Person, Person, Location, Invoice>(
			SELECT_INVOICE_BY_INVOICE_NUMBER, (invoice, issuer, recipient, carrier, location) =>
			{
				invoice.SetIssuer(issuer);
				invoice.SetRecipient(recipient);
				invoice.SetCarrier(carrier);
				invoice.SetConferenceLocation(location);
				return invoice;
			}, new { InvoiceNumber = invoiceNumber });

			return result.FirstOrDefault();
		}

		public async Task InsertAsync(Invoice invoice)
		{
			var conn = _dbContext.GetConnection();
			var transaction = _dbContext.Transaction;

			await conn.ExecuteAsync(INSERT_INVOICE,
				new {
					Id = invoice.Id,
					InvoiceNumber = invoice.InvoiceNumber,
					Series = invoice.Series,
					VerificationCode = invoice.VerificationCode,
					InvoiceType = invoice.Type,
					Status = invoice.Status,
					IssuerId = invoice.Issuer.Id,
					RecipientId = invoice.Recipient.Id,
					CarrierId = invoice.Carrier?.Id,
					IssueDate = invoice.IssueDate,
					ExpectedDeliveryDate = invoice.ExpectedDeliveryDate,
					TotalAmount = invoice.TotalAmount,
					TotalVolumes = invoice.TotalVolumes,
					CreatedAt = DateTime.UtcNow,
				}, transaction);

			foreach (var item in invoice.Items)
			{
				await conn.ExecuteAsync(INSERT_INVOICE_ITEM, new
				{
					InvoiceId = invoice.Id,
					LineNumber = item.LineNumber,
					DeclaredQuantity = item.DeclaredQuantity,
					UnitPrice = item.UnitPrice,
					MaterialId = item.ItemMaterial.Id
				}, transaction);
			}
		}

		public async Task AssingPlateNumAsync(string invoiceNumber, string plateNum, int? locationId)
		{
			var conn = _dbContext.GetConnection();
			var transaction = _dbContext.Transaction;

			await conn.ExecuteAsync(UPDATE_PLATE_NUM, new
			{
				PlateNum = plateNum,
				LocationId = locationId,
				InvoiceNumber = invoiceNumber,
				ModifiedAt = DateTime.UtcNow,
			}, transaction);
		}

		public async Task UpdateStatusAsync(string invoiceNumber, InvoiceStatus status)
		{
			var conn = _dbContext.GetConnection();
			var transaction = _dbContext.Transaction;

			await conn.ExecuteAsync(UPDATE_STATUS, new
			{
				Status = status,
				ModifiedAt = DateTime.UtcNow,
				InvoiceNumber = invoiceNumber,
			}, transaction);
		}
	}
}
