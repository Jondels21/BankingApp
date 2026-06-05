using System;
using System.Collections.Generic;

namespace BankingApp;

public class UserService
{
    private readonly JsonStorageService _storage = new();
    private const string _userFile = "Data/users.json";

    public List<User> GetUsers()
    {
        return _storage.LoadUsers();
    }

    public User Register(string name, string password)
    {
        var users = GetUsers();

        var rnd = new Random();
        string accountNumber;

        do
        {
            accountNumber = rnd.Next(100000,999999).ToString();
        }
        while (users.Exists(u => u.AccountNumber == accountNumber));

        var newUser = new User
        {
            Id = users.Count > 0 ? users [^1].Id + 1 : 1,
            Name = name,
            AccountNumber = accountNumber,
            Password = password
        };

        users.Add(newUser);

        File.WriteAllText(
            _userFile,
            System.Text.Json.JsonSerializer.Serialize(users, new System.Text.Json.JsonSerializerOptions {WriteIndented = true})
        );
        
        return newUser;
    }
}