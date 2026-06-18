namespace BankingApp;

public class Account
{
    public int UserId {get; set; }
    public int Id { get; set; }
    public string Address { get; set;} = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal Balance { get; set; }
}