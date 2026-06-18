using System;

namespace BankingApp;

public static class AddressGenerator
{
    private static readonly Random _random = new Random();

    public static string Generate()
    {
        return $"FI{Two()} {Four()} {Two()}";
    }

    private static string Two()
    {
        return _random.Next(0, 100).ToString("D2");
    }

    private static string Four()
    {
        return _random.Next(0, 10000).ToString("D4");
    }
}