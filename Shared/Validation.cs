namespace BankingApp;

public static class AccountValidator
{
    public static OperationResult ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return OperationResult.Fail("Account name cannot be empty.");

        name = name.Trim();

        if (name.Length < 3 || name.Length > 20)
            return OperationResult.Fail("Account name must be between 3 and 20 characters.");

        if(!name.All(c => char.IsLetterOrDigit(c) || c == ' ' || c == '_'))
            return OperationResult.Fail("Account name contains invalid characters.");

        return OperationResult.Ok("Valid");
    }
    
}

public static class UserValidator
{
    public static OperationResult ValidateUsername(string name)
    {  
        if (string.IsNullOrWhiteSpace(name))
            return OperationResult.Fail("Username cannot be empty.");

        name = name.Trim();

        if (name.Length == 1 || name.Length > 20)
            return OperationResult.Fail("Username must be between 2 and 20 characters.");

        if(!name.All(c => char.IsLetter(c) || c == ' ' || c == '_'))
            return OperationResult.Fail("Username contains invalid characters.");

        return OperationResult.Ok("Valid");
        
    }

    public static OperationResult ValidatePassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            return OperationResult.Fail("Password cannot be empty.");

        if (password.Length != 4)
            return OperationResult.Fail("Password must contain 4 digits.");

        if (!password.All(c => char.IsDigit(c)))
            return OperationResult.Fail("Password can only contain numbers");

        return OperationResult.Ok("Valid");
    }
}