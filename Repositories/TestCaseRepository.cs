using MySql.Data.MySqlClient;

public class TestCaseRepository
{
    private readonly string _connectionString;

    public TestCaseRepository(IConfiguration configuration)
    {
        var conn = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrEmpty(conn))
        {
            throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
        }
        _connectionString = conn;
    }

    public List<TestCase> GetAll()
    {
        var testCases = new List<TestCase>();

        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        // query all test cases, cek push
        var query = "SELECT id, project_id, name, created_at FROM test_cases";

        using var command = new MySqlCommand(query, connection);
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            testCases.Add(new TestCase
            {
                Id = reader.GetInt32("id"),
                ProjectId = reader.GetInt32("project_id"),
                Name = reader.GetString("name"),
                CreatedAt = reader.GetDateTime("created_at").ToString("yyyy-MM-dd HH:mm:ss")
            });
        }

        return testCases;
    }

    public List<TestCase> GetByProjectId(int projectId)
    {
        var testCases = new List<TestCase>();

        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        var query = "SELECT id, project_id, name, created_at FROM test_cases WHERE project_id=@project_id";

        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@project_id", projectId);

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            testCases.Add(new TestCase
            {
                Id = reader.GetInt32("id"),
                ProjectId = reader.GetInt32("project_id"),
                Name = reader.GetString("name"),
                CreatedAt = reader.GetDateTime("created_at").ToString("yyyy-MM-dd HH:mm:ss")
            });
        }

        return testCases;
    }

    public TestCase GetById(int id)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        var query = "SELECT id, project_id, name, created_at FROM test_cases WHERE id=@id";

        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@id", id);

        using var reader = command.ExecuteReader();

        if (reader.Read())
        {
            return new TestCase
            {
                Id = reader.GetInt32("id"),
                ProjectId = reader.GetInt32("project_id"),
                Name = reader.GetString("name"),
                CreatedAt = reader.GetDateTime("created_at").ToString("yyyy-MM-dd HH:mm:ss")
            };
        }

        throw new InvalidOperationException($"TestCase with id {id} was not found.");
    }

    public void Add(TestCase testCase)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        var query = "INSERT INTO test_cases (project_id, name, created_at) VALUES (@project_id, @name, @created_at)";

        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@project_id", testCase.ProjectId);
        command.Parameters.AddWithValue("@name", testCase.Name);
        command.Parameters.AddWithValue("@created_at", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

        command.ExecuteNonQuery();

        testCase.Id = (int)command.LastInsertedId;
    }

    public void Update(int id, TestCase testCase)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        var query = "UPDATE test_cases SET project_id=@project_id, name=@name WHERE id=@id";

        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@project_id", testCase.ProjectId);
        command.Parameters.AddWithValue("@name", testCase.Name);
        command.Parameters.AddWithValue("@id", id);

        command.ExecuteNonQuery();
    }

    public void Delete(int id)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        var query = "DELETE FROM test_cases WHERE id=@id";

        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@id", id);

        command.ExecuteNonQuery();
    }
}
