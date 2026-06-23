
namespace BankingApp;

public class TransactionService
{
    private readonly AccountService _account = new();
    private readonly JsonStorageService _storage = new();

    public void Create(string type, decimal amount, string from, string to)
    {
        var transactions = _storage.LoadTransactions();

        var transaction = new Transaction
        {
            Id = transactions.Any()
                ? transactions.Max(t => t.Id) + 1
                : 1,
            Type = type,
            Amount = amount,
            FromAddress = from,
            ToAddress = to,
            Timestamp = DateTime.UtcNow
  
        };

        transactions.Add(transaction);

        _storage.SaveTransaction(transactions);
    }


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

        Create("Deposit,", amount, "SYSTEM", account.Address);

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

        Create("Withdraw", amount, account.Address, "CASH");

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

        Create("Transfer", amount, senderAccount.Address, destinationAccount.Address);

        return OperationResult.Ok("Transfer successful.");

    }
}