using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace YourTrainer_DBDataAccess.DbAccess;

public class SqlDataAccess : ISqlDataAccess
{
	private readonly IConfiguration _config;

	public SqlDataAccess(IConfiguration config)
	{
		_config = config;
	}

	public async Task<IEnumerable<T>> GetData<T, U>(string storedProcedure, U parameters, string connectionId = "DefaultSQLConnection")
	{
		using IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionId));

		return await connection.QueryAsync<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
	}

	public async Task SaveData<T>(string storedProcedure, T parameters, string connectionId = "DefaultSQLConnection")
	{
		using IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionId));

		await connection.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
	}
}
