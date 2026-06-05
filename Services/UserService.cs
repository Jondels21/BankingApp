namespace BankingApp;

public class UserService
{
    private readonly JsonStorageService _storage = new();

    public List<User> GetUsers()
    {
        return _storage.LoadUsers();
    }
}