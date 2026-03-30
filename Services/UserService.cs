public class UserService
{
    private readonly UserRepository _userRepository;

    public UserService(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public List<UserResponse> GetAll()
    {
        return _userRepository.GetAll().Select(u => new UserResponse
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.Email
        }).ToList();
    }
    public UserResponse GetById(int id)
    {
        var user = _userRepository.GetById(id);
        if (user == null)
        {
            return null;
        }
        return new UserResponse
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email
        };
    }
    public void Add(User user) 
    {
        if (_userRepository.EmailExists(user.Email))
        {
            throw new Exception("Email sudah terdaftar");
        }
        _userRepository.Add(user);
    }
    public void Update(int id, User user) => _userRepository.Update(id, user);
    public void Delete(int id) => _userRepository.Delete(id);
}