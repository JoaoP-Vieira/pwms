namespace PWMS.Core.Entities
{
	public sealed class User
	{
		public int Id { get; private set; }
		public string Email { get; private set; }
		public string FirstName { get; private set; }
		public string LastName { get; private set; }
		public string PasswordHash { get; private set; }

		private User() { }

		public User(string email, string firstName, string lastName, string passwordHash)
		{
			if (string.IsNullOrWhiteSpace(email))
				throw new ArgumentException("User email should be informed", nameof(email));

			if (string.IsNullOrWhiteSpace(firstName))
				throw new ArgumentException("User first name should be informed", nameof(firstName));

			if (string.IsNullOrWhiteSpace(lastName))
				throw new ArgumentException("User last name should be informed", nameof(lastName));

			if (string.IsNullOrWhiteSpace(passwordHash))
				throw new ArgumentException("User password hash should be informed", nameof(passwordHash));

			Email = email;
			FirstName = firstName;
			LastName = lastName;
			PasswordHash = passwordHash;
		}
	}
}
