using Npgsql;

public class DatabaseInitializer
{
    private readonly string _connectionString;

    public DatabaseInitializer(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task InitializeDatabaseAsync()
    {
        try
        {
            // Read the SQL file from your project
            var sqlScript = await File.ReadAllTextAsync("schema.sql");
            
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            
            // Split the script by semicolons and execute each statement
            var statements = sqlScript.Split(';', StringSplitOptions.RemoveEmptyEntries);
            
            foreach (var statement in statements)
            {
                if (!string.IsNullOrWhiteSpace(statement))
                {
                    using var command = new NpgsqlCommand(statement.Trim(), connection);
                    await command.ExecuteNonQueryAsync();
                }
            }
            
            Console.WriteLine("Database initialized successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Database initialization failed: {ex.Message}");
            throw;
        }
    }
}