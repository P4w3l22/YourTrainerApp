namespace YourTrainer_Utility;

public class StaticDetails
{
	public enum ApiType
	{
		GET,
		POST, 
		PUT, 
		DELETE
	}

	public static string SessionToken = "JWTToken";

	public enum MessageType
	{
		Text,
		TrainingPlan,
		Report,
		ConfirmClient,
		AcceptClient,
		RejectClient,
		Resignation
	}
}
