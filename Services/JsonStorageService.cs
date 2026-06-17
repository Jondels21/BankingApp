using System.Text.Json;

namespace BankingApp;

public class JsonStorageService
{
    private readonly string _accountPath = "Data/accounts.json";
    private readonly string _userPath = "Data/users.json";

    private readonly string _sessionPath ="Data/session.json";

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

    public Session? LoadSession()
    {
        if (!File.Exists(_sessionPath))
            return null;

        var json = File.ReadAllText(_sessionPath);

        return JsonSerializer.Deserialize<Session>(json);
    }


    public void SaveSession(Session session)
    {
        var json = JsonSerializer.Serialize(
            session,
            new JsonSerializerOptions
            {
                WriteIndented = true
            });

        File.WriteAllText(_sessionPath, json);
    }

    public void ClearSession()
    {
        if (File.Exists(_sessionPath))
        {
            File.Delete(_sessionPath);
        }
    }
}