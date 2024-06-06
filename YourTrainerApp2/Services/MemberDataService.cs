﻿using YourTrainer_Utility;
using YourTrainerApp.Models;
using YourTrainerApp.Services.IServices;

namespace YourTrainerApp.Services;

public class MemberDataService : BaseService, IMemberDataService
{
	private string APIUrl;
	public MemberDataService(IHttpClientFactory client, IConfiguration configuration) : base(client)
	{
		APIUrl = configuration.GetValue<string>("ServiceUrls:ExerciseAPI");
	}

	public Task<T> GetAllAsync<T>() =>
		SendAsync<T>(new APIRequest()
		{
			ApiType = StaticDetails.ApiType.GET,
			Url = APIUrl + "/api/MemberData/"
		});

	public Task<T> GetAsync<T>(int id) =>
		SendAsync<T>(new APIRequest()
		{
			ApiType = StaticDetails.ApiType.GET,
			Url = APIUrl + "/api/MemberData/" + id
		});

	public Task<T> CreateAsync<T>(MemberDataModel memberData) =>
		SendAsync<T>(new APIRequest()
		{
			ApiType = StaticDetails.ApiType.POST,
			Url = APIUrl + "/api/MemberData/",
			Data = memberData
		});

	public Task<T> UpdateAsync<T>(MemberDataModel memberData) =>
		SendAsync<T>(new APIRequest()
		{
			ApiType = StaticDetails.ApiType.PUT,
			Url = APIUrl + "/api/MemberData/",
			Data = memberData
		});

	public Task<T> DeleteAsync<T>(int id) =>
		SendAsync<T>(new APIRequest()
		{
			ApiType = StaticDetails.ApiType.DELETE,
			Url = APIUrl + "/api/MemberData/" + id
		});
}
