using System.Data;
using System.Data.SqlClient;

namespace InvoicesApp.Data;

public class DatabaseContext
{
    private readonly string _connectionString;

    public DatabaseContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<DataTable> ExecuteQueryAsync(string procedureName, SqlParameter[]? parameters = null)
    {
        using SqlConnection connection = new(_connectionString);
        using SqlCommand command = new(procedureName, connection);

        command.CommandType = CommandType.StoredProcedure;

        if (parameters != null)
        {
            command.Parameters.AddRange(parameters);
        }

        connection.Open();

        DataTable resultTable = new();
        using (SqlDataAdapter adapter = new(command))
        {
            await Task.Run(() => adapter.Fill(resultTable));
        }

        return resultTable;
    }

    public SqlConnection getConnection => new(_connectionString);
}
