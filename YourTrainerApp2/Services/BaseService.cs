﻿using Azure.Core;
using ExerciseAPI.Models;
using Newtonsoft.Json;
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

		public BaseService(IHttpClientFactory httpClient)
		{
			this.response = new();
			this.httpClient = httpClient;
		}

		public async Task<T> SendAsync<T>(APIRequest request)
		{
			try
			{
				return await ResponseMessage<T>(request);
			}

			catch (Exception ex)
			{
				return ErrorResponseMessage<T>(ex.Message);
			}
		}

		private async Task<T> ResponseMessage<T>(APIRequest request)
		{
			var client = httpClient.CreateClient("ExerciseAPI");
			HttpResponseMessage message = null;

			message = await GetResponse(client, RequestMessage(request));

			var apiContent = await message.Content.ReadAsStringAsync();
			var apiResponse = JsonConvert.DeserializeObject<T>(apiContent);
			return apiResponse;
		}

		private async Task<HttpResponseMessage> GetResponse(HttpClient client, HttpRequestMessage message) =>
			await client.SendAsync(message);

		private HttpRequestMessage RequestMessage(APIRequest request)
		{
			HttpRequestMessage message = new();
			message.Headers.Add("Accept", "application/json");
			message.RequestUri = new Uri(request.Url);
			if (request.Data is not null)
			{
				message.Content = new StringContent(JsonConvert.SerializeObject(request.Data),
													Encoding.UTF8, "application/json");
			}

			switch (request.ApiType)
			{
				case StaticDetails.ApiType.POST:
					message.Method = HttpMethod.Post;
					break;
				case StaticDetails.ApiType.PUT:
					message.Method = HttpMethod.Put;
					break;
				case StaticDetails.ApiType.DELETE:
					message.Method = HttpMethod.Delete;
					break;
				default:
					message.Method = HttpMethod.Get;
					break;
			}

			return message;
		}

		private T ErrorResponseMessage<T>(string exMessage)
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
	
	}


}
