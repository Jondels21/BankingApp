using System.Text.Json;

namespace BankingApp;

public class JsonStorageService
{
    private readonly string _accountPath = "Data/accounts.json";
    private readonly string _userPath = "Data/users.json";

    public List<Account> LoadAccounts()
    {
        if (!File.Exists(_accountPath))
            return new List<Account>();

        var json = File.ReadAllText(_accountPath);

        return JsonSerializer.Deserialize<List<Account>>(json)
            ?? new List<Account>();
    }

    public List<User> LoadUsers()
    {
        if (!File.Exists(_userPath))
            return new List<User>();

        var json = File.ReadAllText(_userPath);

        return JsonSerializer.Deserialize<List<User>>(json)
            ?? new List<User>();
    }
}