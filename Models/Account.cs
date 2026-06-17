namespace BankingApp;

public class Account
{
    public int UserId {get; set; }
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Balance { get; set; }
}