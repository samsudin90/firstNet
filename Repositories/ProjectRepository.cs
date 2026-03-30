using MySql.Data.MySqlClient;

public class ProjectRepository
{
    private readonly string _connectionString;

    public ProjectRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public List<Project> GetAll()
    {
        var projects = new List<Project>();

        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        var query = "SELECT id, name, created_at FROM projects";

        using var command = new MySqlCommand(query, connection);
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            projects.Add(new Project
            {
                Id = reader.GetInt32("id"),
                Name = reader.GetString("name"),
                CreatedAt = reader.GetDateTime("created_at").ToString("yyyy-MM-dd HH:mm:ss")
            });
        }

        return projects;
    }

    public Project GetById(int id)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        var query = "SELECT id, name, created_at FROM projects WHERE id=@id";

        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@id", id);

        using var reader = command.ExecuteReader();

        if (reader.Read())
        {
            return new Project
            {
                Id = reader.GetInt32("id"),
                Name = reader.GetString("name"),
                CreatedAt = reader.GetDateTime("created_at").ToString("yyyy-MM-dd HH:mm:ss")
            };
        }

        return null;
    }

    public void Add(Project project)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        var query = "INSERT INTO projects (name, created_at) VALUES (@name, @created_at)";

        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@name", project.Name);
        command.Parameters.AddWithValue("@created_at", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

        command.ExecuteNonQuery();
    }

    public void Update(int id, Project project)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        var query = "UPDATE projects SET name=@name, created_at=@created_at WHERE id=@id";

        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@name", project.Name);
        command.Parameters.AddWithValue("@created_at", project.CreatedAt);
        command.Parameters.AddWithValue("@id", id);

        command.ExecuteNonQuery();
    }

    public void Delete(int id)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        var query = "DELETE FROM projects WHERE id=@id";

        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@id", id);

        command.ExecuteNonQuery();
    }
}
