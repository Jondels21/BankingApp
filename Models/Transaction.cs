using System.Diagnostics.Contracts;

namespace BankingApp;

public class Transaction
{
    public int Id { get; set; }

    public string Type { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public string FromAddress { get; set; } = string.Empty;

    public string ToAddress { get; set; } = string.Empty;

    public DateTime Timestamp { get; set; }
    
}