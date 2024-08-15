namespace YourTrainer_DBDataAccess.Models;

public class MemberDataModel
{
	public int MemberId { get; set; }
	public string? MemberName { get; set; }
	public string? Description { get; set; }
	public string? Email { get; set; }
	public string? PhoneNumber { get; set; }
	public string? TrainersId { get; set; }
	public string? TrainersPlan { get; set; }
}
