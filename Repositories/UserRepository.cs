using MySql.Data.MySqlClient;
using BCrypt.Net;

public class UserRepository
{
    private readonly string _connectionString;

    public UserRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public List<User> GetAll()
    {
        var users = new List<User>();

        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        var query = "SELECT id, name, email FROM users";

        using var command = new MySqlCommand(query, connection);
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            users.Add(new User
            {
                Id = reader.GetInt32("id"),
                Name = reader.GetString("name"),
                Email = reader.GetString("email")
            });
        }

        return users;
    }

    public User GetById(int id)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        var query = "SELECT id, name, email FROM users WHERE id=@id";

        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@id", id);

        using var reader = command.ExecuteReader();

        if (reader.Read())
        {
            return new User
            {
                Id = reader.GetInt32("id"),
                Name = reader.GetString("name"),
                Email = reader.GetString("email")
            };
        }

        return null;
    }

    public void Add(User user)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);

        var query = "INSERT INTO users (name, email, password) VALUES (@name, @email, @password)";

        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@name", user.Name);
        command.Parameters.AddWithValue("@email", user.Email);
        command.Parameters.AddWithValue("@password", hashedPassword);

        command.ExecuteNonQuery();
    }

    public void Update(int id, User user)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        var query = "UPDATE users SET name=@name, email=@email WHERE id=@id";

        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@name", user.Name);
        command.Parameters.AddWithValue("@email", user.Email);
        command.Parameters.AddWithValue("@id", id);

        command.ExecuteNonQuery();
    }

    public void Delete(int id)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        var query = "DELETE FROM users WHERE id=@id";

        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@id", id);

        command.ExecuteNonQuery();
    }

    public User? GetByEmail(string email)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        var query = "SELECT id, name, email, password FROM users WHERE email=@email LIMIT 1";

        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@email", email);

        using var reader = command.ExecuteReader();

        if (reader.Read())
        {
            return new User
            {
                Id = reader.GetInt32("id"),
                Name = reader.GetString("name"),
                Email = reader.GetString("email"),
                Password = reader.GetString("password")
            };
        }

        return null;
    }

    public bool EmailExists(string email)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        var query = "SELECT COUNT(*) FROM users WHERE email=@email";

        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@email", email);

        var count = Convert.ToInt32(command.ExecuteScalar());

        return count > 0;
    }

}