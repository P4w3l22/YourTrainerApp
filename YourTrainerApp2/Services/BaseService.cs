using Azure.Core;
using ExerciseAPI.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using YourTrainer_Utility;
using YourTrainerApp2.Models;
using YourTrainerApp2.Services.IServices;

namespace YourTrainerApp2.Services
{
	public class BaseService : IBaseService
	{
		public APIResponse response { get; set; }
		public IHttpClientFactory httpClient { get; set; }
		private APICommunication _APICommunication;

		public BaseService(IHttpClientFactory httpClient)
		{
			this.response = new();
			this.httpClient = httpClient;
			this._APICommunication = new(this.httpClient);
		}

		public async Task<T> SendAsync<T>(APIRequest request)
		{
			try
			{
				return await _APICommunication.GetResponse<T>(request);
			}

			catch (Exception ex)
			{
				return _APICommunication.GetErrorResponse<T>(ex.Message);
			}
		}
	}

	internal class APICommunication
	{
		private IHttpClientFactory httpClient {  get; set; }

        internal APICommunication(IHttpClientFactory httpClient)
        {
            this.httpClient = httpClient;
        }

		internal T GetErrorResponse<T>(string exMessage)
		{
			var message = new APIResponse
			{
				Errors = new List<string> { Convert.ToString(exMessage) },
				IsSuccess = false
			};

			var serializedMessage = JsonConvert.SerializeObject(message);
			var apiResponse = JsonConvert.DeserializeObject<T>(serializedMessage);
			return apiResponse;
		}

		internal async Task<T> GetResponse<T>(APIRequest request)
		{
			var client = httpClient.CreateClient("ExerciseAPI");
			HttpResponseMessage message = null;

			message = await GetAPIResponse(client, GetRequest(request));

			var apiContent = await message.Content.ReadAsStringAsync();
			var apiResponse = JsonConvert.DeserializeObject<T>(apiContent);
			return apiResponse;
		}

		private async Task<HttpResponseMessage> GetAPIResponse(HttpClient client, HttpRequestMessage message) =>
			await client.SendAsync(message);

		private HttpRequestMessage GetRequest(APIRequest request)
		{
			HttpRequestMessage message = new();
			message.Headers.Add("Accept", "application/json");
			message.RequestUri = new Uri(request.Url);
			if (request.Data is not null)
			{
				message.Content = new StringContent(JsonConvert.SerializeObject(request.Data),
													Encoding.UTF8, "application/json");
			}

			message.Method = request.ApiType switch
			{
				StaticDetails.ApiType.POST
					=> HttpMethod.Post,
				StaticDetails.ApiType.PUT
					=> HttpMethod.Put,
				StaticDetails.ApiType.DELETE
					=> HttpMethod.Delete,
				_
					=> HttpMethod.Get
			};

			return message;
		}
	} 
}
