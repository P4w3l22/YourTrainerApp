namespace YourTrainerApp.Areas.Auth.Models;

public class RegisterationRequest
{
    public string Username { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }

}