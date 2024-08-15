namespace YourTrainer_DBDataAccess.Models;

public class LoginResponse
{
	public LocalUserModel? User { get; set; }
	public string? Token { get; set; }
	public List<string>? Errors { get; set; }
}
