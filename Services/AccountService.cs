
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

    public Account? GetAccountById(int userId, int accountId)
    {
        return GetAccounts(userId)
            .FirstOrDefault(a => a.Id == accountId);
    }

    public Account? GetAccountByAddress(string accountAddress)
    {
        var accounts = _storage.LoadAccounts();

        if (string.IsNullOrWhiteSpace(accountAddress))
            return null;

        var input = accountAddress.Replace(" ", "").ToUpper();

        return accounts.FirstOrDefault(a =>
            a.Address.Replace(" ", "").ToUpper() == input);

    }

    public OperationResult<Account> CreateAccount(string name)
    {
        var session = _storage.LoadSession();
        if(session == null)
        {
            return OperationResult<Account>.Fail("You must login first!");
        }

        var validation = AccountValidator.ValidateName(name);

        if (!validation.Success)
        {
            return OperationResult<Account>.Fail(validation.Message);
        }

        int userid = session.UserId;

        var accounts = _storage.LoadAccounts();

        var userAccounts = accounts.Where(a => a.UserId == userid);

        string accountAddress = AddressGenerator.Generate();

        var newAccount = new Account
        {
            UserId = userid,
            Id = userAccounts.Any()
                ? userAccounts.Max(a => a.Id) + 1
                : 1,
            Name = name,
            Address = accountAddress
        };

        accounts.Add(newAccount);

       _storage.SaveAccounts(accounts);

        return OperationResult<Account>.Ok(newAccount, "Account created.");
    }

    public Account? UpdateAccount(int userId, int accountId, decimal balance)
    {
        var accounts = _storage.LoadAccounts();

        var account = accounts.FirstOrDefault(a =>
            a.UserId == userId &&
            a.Id == accountId);

        if (account == null)
            return null;

        account.Balance = balance;

        _storage.SaveAccounts(accounts);

        return account;
        
    }
}