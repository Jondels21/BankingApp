
using System.Runtime.CompilerServices;

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

    public Account CreateAccount(int userid, string name)
    {
        var accounts = _storage.LoadAccounts();

        var userAccounts = accounts.Where(a => a.UserId == userid);

        string accountAddress;

        accountAddress = AddressGenerator.Generate();

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

        return newAccount;
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