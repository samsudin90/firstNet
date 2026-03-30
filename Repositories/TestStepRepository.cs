using MySql.Data.MySqlClient;

public class TestStepRepository
{
    private readonly string _connectionString;

    public TestStepRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public List<TestStep> GetAll()
    {
        var testSteps = new List<TestStep>();

        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        var query = "SELECT id, test_case_id, step_order, action, selector, value, created_at FROM test_steps ORDER BY test_case_id, step_order";

        using var command = new MySqlCommand(query, connection);
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            testSteps.Add(new TestStep
            {
                Id = reader.GetInt32("id"),
                TestCaseId = reader.GetInt32("test_case_id"),
                StepOrder = reader.GetInt32("step_order"),
                Action = reader.GetString("action"),
                Selector = reader.IsDBNull(reader.GetOrdinal("selector")) ? null : reader.GetString("selector"),
                Value = reader.IsDBNull(reader.GetOrdinal("value")) ? null : reader.GetString("value"),
                CreatedAt = reader.GetDateTime("created_at").ToString("yyyy-MM-dd HH:mm:ss")
            });
        }

        return testSteps;
    }

    public List<TestStep> GetByTestCaseId(int testCaseId)
    {
        var testSteps = new List<TestStep>();

        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        var query = "SELECT id, test_case_id, step_order, action, selector, value, created_at FROM test_steps WHERE test_case_id=@test_case_id ORDER BY step_order";

        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@test_case_id", testCaseId);

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            testSteps.Add(new TestStep
            {
                Id = reader.GetInt32("id"),
                TestCaseId = reader.GetInt32("test_case_id"),
                StepOrder = reader.GetInt32("step_order"),
                Action = reader.GetString("action"),
                Selector = reader.IsDBNull(reader.GetOrdinal("selector")) ? null : reader.GetString("selector"),
                Value = reader.IsDBNull(reader.GetOrdinal("value")) ? null : reader.GetString("value"),
                CreatedAt = reader.GetDateTime("created_at").ToString("yyyy-MM-dd HH:mm:ss")
            });
        }

        return testSteps;
    }

    public TestStep GetById(int id)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        var query = "SELECT id, test_case_id, step_order, action, selector, value, created_at FROM test_steps WHERE id=@id";

        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@id", id);

        using var reader = command.ExecuteReader();

        if (reader.Read())
        {
            return new TestStep
            {
                Id = reader.GetInt32("id"),
                TestCaseId = reader.GetInt32("test_case_id"),
                StepOrder = reader.GetInt32("step_order"),
                Action = reader.GetString("action"),
                Selector = reader.IsDBNull(reader.GetOrdinal("selector")) ? null : reader.GetString("selector"),
                Value = reader.IsDBNull(reader.GetOrdinal("value")) ? null : reader.GetString("value"),
                CreatedAt = reader.GetDateTime("created_at").ToString("yyyy-MM-dd HH:mm:ss")
            };
        }

        return null;
    }

    public void Add(TestStep testStep)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        var query = "INSERT INTO test_steps (test_case_id, step_order, action, selector, value, created_at) VALUES (@test_case_id, @step_order, @action, @selector, @value, @created_at)";

        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@test_case_id", testStep.TestCaseId);
        command.Parameters.AddWithValue("@step_order", testStep.StepOrder);
        command.Parameters.AddWithValue("@action", testStep.Action);
        command.Parameters.AddWithValue("@selector", (object)testStep.Selector ?? DBNull.Value);
        command.Parameters.AddWithValue("@value", (object)testStep.Value ?? DBNull.Value);
        command.Parameters.AddWithValue("@created_at", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

        command.ExecuteNonQuery();

        testStep.Id = (int)command.LastInsertedId;
    }

    public void Update(int id, TestStep testStep)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        var query = "UPDATE test_steps SET test_case_id=@test_case_id, step_order=@step_order, action=@action, selector=@selector, value=@value WHERE id=@id";

        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@test_case_id", testStep.TestCaseId);
        command.Parameters.AddWithValue("@step_order", testStep.StepOrder);
        command.Parameters.AddWithValue("@action", testStep.Action);
        command.Parameters.AddWithValue("@selector", (object)testStep.Selector ?? DBNull.Value);
        command.Parameters.AddWithValue("@value", (object)testStep.Value ?? DBNull.Value);
        command.Parameters.AddWithValue("@id", id);

        command.ExecuteNonQuery();
    }

    public void Delete(int id)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        var query = "DELETE FROM test_steps WHERE id=@id";

        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@id", id);

        command.ExecuteNonQuery();
    }
}
