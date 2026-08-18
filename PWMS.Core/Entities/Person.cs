using System.Text.RegularExpressions;

namespace PWMS.Core.Entities
{
	public sealed class Person
	{
		public int Id { get; private set; }
		public string Name { get; private set; }
		private string Document { get; set; }
		public string Address { get; private set; }

		private Person() { }

		public Person(string name, string document, string address)
		{
			if (string.IsNullOrWhiteSpace(name))
				throw new ArgumentException("Person name should be informed", "name");

			ValidateDocument(document);

			if (string.IsNullOrWhiteSpace(address))
				throw new ArgumentException("Person address should be informed", "address");

			Name = name;
			Document = document;
			Address = address;
		}

		public static void ValidateDocument(string document)
		{
			if (string.IsNullOrWhiteSpace(document))
				throw new ArgumentException("Person document should be informed", "document");

			var formatedDocument = document.Trim().ToUpper()
										.Replace(".", "")
										.Replace("-", "")
										.Replace("/", "")
										.Replace("\\", "");

			if (formatedDocument.Length != 11 && formatedDocument.Length != 14)
				throw new ArgumentException("Person document should be valid", "document");

			if (formatedDocument.Length == 11 && !ValidateCpf(formatedDocument))
				throw new ArgumentException("Person CPF should be valid", "document");
			else if (formatedDocument.Length == 14 && !validateCnpj(formatedDocument))
				throw new ArgumentException("Person CNPJ should be valid", "document");
		}

		public static bool validateCnpj(string cnpj)
		{
			if (Regex.Match(cnpj, @"[^A-Za-z0-9./-]").Success)
				return false;

			int[] multipDV1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
			int[] multipDV2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

			string calcDV1 = cnpj.Substring(0, 12);
			string calcDV2 = cnpj.Substring(0, 13);

			int sum = 0;
			int rest = 0;

			for (int i = 0; i < multipDV1.Length; i++)
			{
				sum += (Convert.ToInt32(calcDV1[i]) - 48) * multipDV1[i];
			}

			rest = (sum % 11);

			string digit1 = (rest <= 1 ? 0 : 11 - rest).ToString();

			sum = 0;

			for (int i = 0; i < multipDV2.Length; i++)
			{
				sum += (Convert.ToInt32(calcDV2[i]) - 48) * multipDV2[i];
			}

			rest = (sum % 11);

			string digit2 = (rest <= 1 ? 0 : 11 - rest).ToString();

			return cnpj.Equals($"{calcDV1}{digit1}{digit2}");
		}

		public static bool ValidateCpf(string cpf)
		{
			if (Regex.Match(cpf, @"[^A-Za-z0-9./-]").Success)
				return false;

			int[] mpp1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
			int[] mpp2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

			for (int j = 0; j < 10; j++)
				if (j.ToString().PadLeft(11, char.Parse(j.ToString())) == cpf)
					return false;

			string tempCpf = cpf.Substring(0, 9);
			int sum = 0;

			for (int i = 0; i < 9; i++)
				sum += int.Parse(tempCpf[i].ToString()) * mpp1[i];

			int rest = sum % 11;
			if (rest < 2)
				rest = 0;
			else
				rest = 11 - rest;

			string digit = rest.ToString();
			tempCpf = tempCpf + digit;
			sum = 0;
			for (int i = 0; i < 10; i++)
				sum += int.Parse(tempCpf[i].ToString()) * mpp2[i];

			rest = sum % 11;
			if (rest < 2)
				rest = 0;
			else
				rest = 11 - rest;

			digit = digit + rest.ToString();

			return cpf.EndsWith(digit);
		}

		public void SetId(int id)
		{
			Id = id;
		}

		public string GetDocument()
		{
			return Document;
		}
	}
}
