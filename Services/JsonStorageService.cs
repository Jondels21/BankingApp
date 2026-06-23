using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks.Dataflow;

namespace BankingApp;

public class JsonStorageService
{
    private readonly string _accountPath = "Data/accounts.json";
    private readonly string _userPath = "Data/users.json";

    private readonly string _sessionPath ="Data/session.json";

    private readonly string _transactionPath ="Data/transactions.json";

    private static readonly JsonSerializerOptions _options =
        new JsonSerializerOptions {WriteIndented = true};

    public List<Account> LoadAccounts()
    {
        if (!File.Exists(_accountPath))
            return new List<Account>();

        var json = File.ReadAllText(_accountPath);

        return JsonSerializer.Deserialize<List<Account>>(json)
            ?? new List<Account>();
    }


    public void SaveAccounts(List<Account> accounts)
    {
        File.WriteAllText(_accountPath, JsonSerializer.Serialize(accounts, _options));
    }

    public List<User> LoadUsers()
    {
        if (!File.Exists(_userPath))
            return new List<User>();

        var json = File.ReadAllText(_userPath);

        return JsonSerializer.Deserialize<List<User>>(json)
            ?? new List<User>();
    }

    public void SaveUsers(List<User> users)
    {
        File.WriteAllText(_userPath, JsonSerializer.Serialize(users, _options));
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

    public List<Transaction> LoadTransactions()
    {
        if (!File.Exists(_transactionPath))
            return new List<Transaction>();

        var json = File.ReadAllText(_transactionPath);

        return JsonSerializer.Deserialize<List<Transaction>>(json)
            ?? new List<Transaction>();
    }

    public void SaveTransaction(List<Transaction> transactions)
    {
        File.WriteAllText(_transactionPath, JsonSerializer.Serialize(transactions, _options));
    }
}