namespace Radiotech.Common;

public class Validators
{
    public static (bool isValid, string error) RequiredLettersOnly(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return (false, "The field cannot be empty");
        if (!input.All(c => char.IsLetter(c)))
            return (false, "Only letters (Latin or Cyrillic) are allowed");
        return (true, string.Empty);
    }

    public static (bool isValid, string error) RequiredMinLength(string? input, int minLength)
    {
        if (string.IsNullOrWhiteSpace(input))
            return (false, "The field cannot be empty");
        if (input.Length < minLength)
            return (false, $"Minimum length is - {minLength} characters");
        return (true, string.Empty);
    }
}