namespace PWMS.Service.DTO.Auth
{
	public class AuthResultDTO
	{
		public string Token { get; set; } = string.Empty;
		public DateTime ExpiresAtUtc { get; set; }
		public UserDTO User { get; set; } = new UserDTO();
	}
}
