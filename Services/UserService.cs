
namespace BankingApp;

public class UserService
{
    private readonly JsonStorageService _storage = new();

    public List<User> GetUsers()
    {
        return _storage.LoadUsers();
    }

    public OperationResult<User> Register(string name, string password)
    {
        var session = _storage.LoadSession();
        if (session != null)
        {
            return OperationResult<User>.Fail("You must logout first!");
        }

        var NameValidation = UserValidator.ValidateUsername(name);

        if(!NameValidation.Success)
        {
            return OperationResult<User>.Fail(NameValidation.Message);
        }

        var PasswordValidation = UserValidator.ValidatePassword(password);
        
        if (!PasswordValidation.Success)
        {
            return OperationResult<User>.Fail(PasswordValidation.Message);
        }

        var users = GetUsers();

        var rnd = new Random();
        string accountNumber;

        do
        {
            accountNumber = rnd.Next(100000,999999).ToString();
        }
        while (users.Exists(u => u.AccountNumber == accountNumber));

        var newUser = new User
        {
            Id = users.Count > 0 ? users [^1].Id + 1 : 1,
            Name = name,
            AccountNumber = accountNumber,
            Password = password
        };

        users.Add(newUser);

        _storage.SaveUsers(users);
        
        return OperationResult<User>.Ok(newUser, "User created.");
    }
}