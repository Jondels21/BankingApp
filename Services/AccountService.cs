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
}