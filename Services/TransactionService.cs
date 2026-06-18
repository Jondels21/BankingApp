
namespace BankingApp;

public class TransactionService
{
    private readonly AccountService _account = new();
    private readonly JsonStorageService _storage = new();
    public OperationResult Deposit(int accountId, decimal amount)
    {
        var session = _storage.LoadSession();

        if (session == null)
            return OperationResult.Fail("You must login first!");

        if (amount <= 0)
            return OperationResult.Fail("Amount must be greater than 0.");

        var account = _account.GetAccountById(session.UserId, accountId);

        if (account == null)
            return OperationResult.Fail("Account not found.");

        account.Balance += amount;
        _account.UpdateAccount(account.UserId, account.Id, account.Balance);

        return OperationResult.Ok("Deposit successful.");

    }

    public OperationResult Withdraw(int accountId, decimal amount)
    {
        var session = _storage.LoadSession();

        if (session == null)
            return OperationResult.Fail("You must login first!");

        if (amount <= 0)
            return OperationResult.Fail("Amount must be greater than 0.");

        var account = _account.GetAccountById(session.UserId, accountId);

        if (account == null)
            return OperationResult.Fail("Account not found.");

        if (amount > account.Balance)
            return OperationResult.Fail("Insufficient funds.");

        account.Balance -= amount;
        _account.UpdateAccount(account.UserId, account.Id, account.Balance);

        return OperationResult.Ok("Withdraw successful.");

    }

    public OperationResult Transfer(int accountId, decimal amount, string destination)
    {
        var session = _storage.LoadSession();

        if (session == null)
            return OperationResult.Fail("You must login first!");

        if (amount <= 0)
            return OperationResult.Fail("Amount must be greater than 0.");

        var senderAccount = _account.GetAccountById(session.UserId, accountId);
        if (senderAccount == null)
            return OperationResult.Fail("Sender account not found.");

        var destinationAccount = _account.GetAccountByAddress(destination);
        if (destinationAccount == null)
            return OperationResult.Fail("Destination account not found.");


        if (destinationAccount.Address == senderAccount.Address)
            return OperationResult.Fail("Cannot transfer to same account.");

        if (senderAccount.Balance < amount)
            return OperationResult.Fail("Insufficient funds.");

        senderAccount.Balance -= amount;
        destinationAccount.Balance += amount;

        _account.UpdateAccount(senderAccount.UserId, senderAccount.Id, senderAccount.Balance);
        _account.UpdateAccount(destinationAccount.UserId, destinationAccount.Id, destinationAccount.Balance);

        return OperationResult.Ok("Transfer successful.");

    }
}