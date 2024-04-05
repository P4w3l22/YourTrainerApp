using static YourTrainer_Utility.StaticDetails;

namespace YourTrainerApp2.Models;

public class APIRequest
{
	public ApiType ApiType { get; set; } = ApiType.GET;
	public string Url { get; set; }
	public object Data { get; set; }
}
