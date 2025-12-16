namespace Radiotech.Common;

public static class Validators
{
    public static (bool isValid, string error) RequiredLettersOnly(string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) 
            return (false, "The field cannot be empty");
        if (!input.All(char.IsLetter))
            return (false, "Only letters (Latin or Cyrillic) are allowed");
        return (true, string.Empty);
    }
    
    public static (bool isValid, string error) RequiredMidName(string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) 
            return (true, "");
        if (!input.All(char.IsLetter))
            return (false, "Only letters (Latin or Cyrillic) are allowed");
        return (true, string.Empty);
    }
    
    public static (bool isValid, string error) RequiredDigitsOnlyFixedLength(string? input, int length)
    {
        if (string.IsNullOrWhiteSpace(input)) 
            return (false, "The field cannot be empty");
        if (!input.All(char.IsDigit))
            return (false, "Only digits are allowed");
        if (input.Length != length)
            return (false, $"Required length is - {length} symbols");
        return (true, string.Empty);
    }

    public static (bool isValid, string error) RequiredPositiveDigitsOnly(string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) 
            return (false, "The field cannot be empty");
        if (!input.All(char.IsDigit))
            return (false, "Only digits are allowed");
        // if (int.Parse(input) > 0)
        //     return (false, "The value of the number must be positive");
        return (true, string.Empty);
    }

    public static (bool isValid, string error) RequiredLength(string? input, int length)
    {
        if (string.IsNullOrWhiteSpace(input)) 
            return (false, "The field cannot be empty");
        if (input.Length == length)
            return (false, $"Required length is - {length} characters");
        return (true, string.Empty);
    }

    public static (bool isValid, string error) RequiredNotNull(string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) 
            return (false, "The field cannot be empty");
        return (true, string.Empty);
    }
}