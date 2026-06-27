
namespace BankingApp;



public class App
{

    public int Execute(string[] args)
    {
        if (args.Length == 0)
        {
            PrintHelp();
            return 0;
        }

        var command = args[0];


        switch (command)
        {
            case "init":
                return Init();

            case "register":
                return Register(args);

            case "login":
                return Login(args);

            case "logout":
                return Logout();

            case "create":
                return CreateAccount(args);

            case "deposit":
                return Deposit(args);

            case "withdraw":
                return Withdraw(args);

            case "transfer":
                return Transfer(args);

            case "list":
                return HandleList(args);

            case "show":
                return HandleShow(args);

            case "--help":
            case "-h":
                PrintHelp();
                return 0;

            case "--version":
            case "-V":
                Console.WriteLine("Banking CLI v1.0");
                return 0;

            default:
                Console.WriteLine("Unknown command");
                return 1;
        }
    }


    private int Init()
    {
        var dataDir = "Data";

        if(!Directory.Exists(dataDir))
        {
            Directory.CreateDirectory(dataDir);
        }

        var usersPath = Path.Combine(dataDir, "users.json");
        var accountsPath = Path.Combine(dataDir, "accounts.json");

        if (!File.Exists(usersPath))
            File.WriteAllText(usersPath, "[]");

        if (!File.Exists(accountsPath))
            File.WriteAllText(accountsPath, "[]");

        
        Console.WriteLine("BankingApp initialized");
        return 0;
    }

    private int Register(string[] args)
    {
        if (args.Length < 3)
        {
            Console.WriteLine("Usage: register <name> <password>");
            return 1;
        }

        var storage = new JsonStorageService();
        var session = storage.LoadSession();

        if (session != null)
        {
            Console.WriteLine("You must logout first!");
            return 1;
        }

        var name = args[1];
        var password = args[2];

        var service = new UserService();
        var user = service.Register(name, password);

        Console.WriteLine("User created!");
        Console.WriteLine($"Name: {user.Name}");
        Console.WriteLine($"Account number: {user.AccountNumber}");

        return 0;
    }

    private int Login(string[] args)
    {
        if (args.Length < 3)
        {
            Console.WriteLine("Usage: login <accountNumber> <password>");
            return 1;
        }

        var accountNumber = args[1];
        var password = args[2];

        var service = new UserService();
        var users = service.GetUsers();

        var user = users.FirstOrDefault(u =>
            u.AccountNumber == accountNumber &&
            u.Password == password);

        if (user == null)
        {
            Console.WriteLine("Invalid credentials");
            return 1;
        }

        var storage = new JsonStorageService();

        storage.SaveSession(new Session
        {
            UserId = user.Id
        });

        Console.WriteLine("Login successful");
        Console.WriteLine($"Welcome {user.Name}");

        return 0;
    }

    private static int Logout()
    {
        var storage = new JsonStorageService();
        var session = storage.LoadSession();

        if (session != null)
        {
            storage.ClearSession();
            Console.WriteLine("Logged out");
            return 0;
        }
        else
        {
            Console.WriteLine("You are not logged in.");
            return 0;
        }

    }

    private int CreateAccount(string[] args)
    {
        if (args.Length != 2)
        {
            Console.WriteLine("Usage: Create <Account name>");
            return 1;
        }

        var name = args[1];

        var service = new AccountService();

        var result = service.CreateAccount(name);

        if(!result.Success)
        {
            Console.WriteLine(result.Message);
            return 1;
        }
        Console.WriteLine(result.Message);
        Console.WriteLine($"Account name: {result.Data.Name}");
        Console.WriteLine($"Account address: {result.Data.Address}");
        Console.WriteLine($"Account balance: {result.Data.Balance}");

        return 0;
    }

    private int Deposit(string[] args)
    {
        if (args.Length != 3)
        {
            Console.WriteLine("Usage: Deposit <Account> <Amount>");
            return 1;
        }

        if (!int.TryParse(args[1], out var accountId) ||
            !decimal.TryParse(args[2], out var amount))
        {
            Console.WriteLine("Invalid input.");
            return 1;
        }


        var service = new TransactionService();

        var result = service.Deposit(accountId, amount);


        Console.WriteLine(result.Message);

        return result.Success ? 0 : 1;
    }

    private int Withdraw(string[] args)
    {
        if (args.Length != 3)
        {
            Console.WriteLine("Usage: Withdraw <Account> <Amount>");
            return 1;   
        }

        if (!int.TryParse(args[1], out var accountId) ||
            !decimal.TryParse(args[2], out var amount))
        {
            Console.WriteLine("Invalid input.");
            return 1;
        }

        var service = new TransactionService();

        var result = service.Withdraw(accountId,amount);

        Console.WriteLine(result.Message);

        return result.Success ? 0 : 1;
    }

    private int Transfer(string[] args)
    {
        if (args.Length != 4)
        {
            Console.WriteLine("Usage: Transfer <Account> <Amount> <Destination Address");
            return 1;
        }

        if (!int.TryParse(args[1], out var accountId) ||
            !decimal.TryParse(args[2], out var amount))
        {
            Console.WriteLine("Invalid input.");
            return 1;
        }

        var destination = args[3];

        var service = new TransactionService();

        var result = service.Transfer(accountId, amount, destination);

        Console.WriteLine(result.Message);

        return result.Success ? 0 : 1;
    }

    private int HandleList(string[] args)
    {
        if (args.Length < 1)
        {
            Console.WriteLine("Error: missing list-subcommand");
            return 1;
        }

        var sub = args[1];

        switch (sub)
        {
            case "accounts":
                var storage = new JsonStorageService();
                var session = storage.LoadSession();

                if (session == null)
                {
                    Console.WriteLine("You must login first!");
                    return 1;
                }


                var service = new AccountService();
                var accounts = service.GetAccounts(session.UserId);

                if(accounts.Count != 0)
                {
                    foreach (var acc in accounts)
                    {
                        Console.WriteLine($"{acc.Id} | {acc.Name} | {acc.Address} | {acc.Balance}");
                    }
                }
                else
                {
                    Console.WriteLine("No accounts found.");
                }

                break;


            case "users":
                var userservice = new UserService();
                var users = userservice.GetUsers();

                foreach (var user in users)
                {
                    Console.WriteLine($"{user.Id} | {user.Name}");
                }
                break;

            case "transactions":

                if (args.Length < 3)
                {
                    Console.WriteLine("Usage: list transactions <account>");
                    return 1;
                }

                if (!int.TryParse(args[2], out var account))
                {
                    Console.WriteLine("Invalid input.");
                    return 1;
                }

                var transactionService = new TransactionService();
                var result = transactionService.GetTransactions(account);

                if(!result.Success)
                {
                    Console.WriteLine(result.Message);
                    return 1;
                }

                Console.WriteLine(result.Message);

                foreach (var transaction in result.Data)
                {
                    Console.WriteLine($"{transaction.Id} | Type: {transaction.Type} | From: {transaction.FromAddress} | To: {transaction.ToAddress} | Amount: {transaction.Amount} | At: {transaction.Timestamp}");
                }


                break;

            case "counterparties":
                Console.WriteLine("Listing counterparties...");
                break;

            case "balances":
                Console.WriteLine("Listing balances...");
                break;

            case "outgoings":
                Console.WriteLine("Listing outgoings...");
                break;

            case "incomings":
                Console.WriteLine("Listing incomings...");
                break;

            default:
                Console.WriteLine("Unknown list-command");
                return 1;
        }
        return 0;
    }

    private int HandleShow(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Error: missing show-subcommand");
            return 1;
        }

        var sub = args[1];

        switch (sub)
        {
            case "balance":
                Console.WriteLine("Showing balance...");
                break;

            case "outgoing":
                Console.WriteLine("Showing outgoing...");
                break;

            case "incoming":
                Console.WriteLine("Showing incoming...");
                break;

            default:
                Console.WriteLine("Unknown show-command");
                return 1;
        }

    return 0;
    }

    private static void PrintHelp()
    {
        Console.WriteLine("Usage:");
        Console.WriteLine("     BankingApp init");
        Console.WriteLine("     BankingApp [list] accounts");
        Console.WriteLine("     BankingApp [list] transactions [<account> --timeframe=<tf> --show-description]");
        Console.WriteLine("     BankingApp [list] counterparties [<account> --timeframe=<tf> --count=<n>]");
        Console.WriteLine("     BankingApp [list] (balances|outgoings|incomings) [<account> --interval=<itv> --timeframe=<tf> --output=<of>]");
        Console.WriteLine("     BankingApp [show] balance [<account> --hide-currency]");
        Console.WriteLine("     BankingApp [show] outgoing [<account> --hide-currency]");
        Console.WriteLine("     BankingApp [show] incoming [<account> --hide-currency]");
        Console.WriteLine("     BankingApp [--help | --version]");
        Console.WriteLine();
        Console.WriteLine("Commands:");
        Console.WriteLine("     init        Configure");
        Console.WriteLine("     list accounts        List accounts.");
        Console.WriteLine("     list transactions        List transactions.");
        Console.WriteLine("     list counterparties        List outgoing amounts grouped by counterparties.");
        Console.WriteLine("     list balances        List balances during a timeframe.");
        Console.WriteLine("     list outgoings        List outgoings during a timeframe.");
        Console.WriteLine("     list incomings        List incomings during a timeframe.");
        Console.WriteLine("     show balance        Show the current balance.");
        Console.WriteLine("     show outgoing        Show the current outgoing.");
        Console.WriteLine("     show incoming        Show the current incoming.");
        Console.WriteLine();
        Console.WriteLine("NOTE: By default commands are applied to the 'current' <account>.");
        Console.WriteLine();
        Console.WriteLine("Options:");
        Console.WriteLine("     -h --help       Show this screen.");
        Console.WriteLine("     -V --version       Show version.");
        Console.WriteLine("     -i --interval=<itv>       Group by an interval of time [default: monthly].");
        Console.WriteLine("     -t --timeframe=<tf>       Operate upon a named period of time [default: 6-months].");
        Console.WriteLine("     -c --count=<n>       Only the top N elements [default: 10].");
        Console.WriteLine("     -d --show-description       Show description against transactions.");
        Console.WriteLine("     -m --hide-currency       Show money without currency codes.");
        Console.WriteLine("     -o --output=<of>       output in a particular format (e.g spark).");
    }

}