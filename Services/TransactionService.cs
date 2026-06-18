
using System.Runtime;

namespace BankingApp;

public class TransactionService
{
    private readonly AccountService _account = new();
    private readonly JsonStorageService _storage = new();
    public void Deposit(int accountId, decimal amount)
    {
        var session = _storage.LoadSession();

        if (session == null)
            return;

        var account = _account.GetAccountById(session.UserId, accountId);

        if (account == null)
            return;

        var newBalance = account.Balance + amount;

        _account.UpdateAccount(account.UserId, account.Id, newBalance);

    }
}