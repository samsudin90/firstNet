using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

public class AuthService
{
    private readonly UserRepository _repo;
    private readonly IConfiguration _config;

    public AuthService(UserRepository repo, IConfiguration config)
    {
        _repo = repo;
        _config = config;
    }

    public string? Login(string email, string password)
    {
        var user = _repo.GetByEmail(email);

        if (user == null)
            return null;

        if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
            return null;

        return GenerateToken(user);
    }

    private string GenerateToken(User user)
    {
        var jwt = _config.GetSection("Jwt");

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwt["Key"])
        );

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("name", user.Name)
        };

        var token = new JwtSecurityToken(
            issuer: jwt["Issuer"],
            audience: jwt["Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(int.Parse(jwt["ExpireMinutes"])),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}