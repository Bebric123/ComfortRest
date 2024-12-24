using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

public class DatabaseContext
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;

    public DatabaseContext(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = _configuration.GetConnectionString("DefaultConnection");
    }

    public SqlConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }
}