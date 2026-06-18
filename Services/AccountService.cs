
namespace BankingApp;

public class AccountService
{
    private readonly JsonStorageService _storage = new();

    public List<Account> GetAccounts(int userid)
    {
        var accounts = _storage.LoadAccounts();

        return accounts
            .Where(a => a.UserId == userid)
            .ToList();
    }

    public Account CreateAccount(int userid, string name)
    {
        var accounts = _storage.LoadAccounts();

        var userAccounts = accounts.Where(a => a.UserId == userid);

        var newAccount = new Account
        {
            UserId = userid,
            Id = userAccounts.Any()
                ? userAccounts.Max(a => a.Id) + 1
                : 1,
            Name = name,
        };

        accounts.Add(newAccount);

       _storage.SaveAccounts(accounts);

        return newAccount;
    }
}