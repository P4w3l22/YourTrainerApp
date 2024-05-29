
namespace DbDataAccess.DbAccess;

public interface ISqlDataAccess
{
	Task<IEnumerable<T>> GetData<T, U>(string storedProcedure, U parameters, string connectionId = "DefaultSQLConnection");
	Task SaveData<T>(string storedProcedure, T parameters, string connectionId = "DefaultSQLConnection");
}