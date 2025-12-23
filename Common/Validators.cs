namespace Radiotech.Common;

public static class Validators
{
    public static (bool isValid, string error) LettersOnly(string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) 
            return (false, "Поле не может быть пустым.");
        if (!input.All(char.IsLetter))
            return (false, "Разрешены только буквы");
        return (true, string.Empty);
    }
    
    public static (bool isValid, string error) RequiredMidName(string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) 
            return (true, "");
        if (!input.All(char.IsLetter))
            return (false, "Разрешены только буквы.");
        return (true, string.Empty);
    }
    
    public static (bool isValid, string error) RequiredDiscountCard(string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) 
            return (true, "");
        if (!input.All(char.IsDigit))
            return (false, "Разрешены только цифры.");
        if (input.Length != 7 && input.Length > 0)
            return (false, $"Необходимая длина - 7 цифр.");
        return (true, string.Empty);
    }
    
    public static (bool isValid, string error) DigitsOnlyFixedLength(string? input, int length)
    {
        if (string.IsNullOrWhiteSpace(input)) 
            return (false, "Поле не может быть пустым.");
        if (!input.All(char.IsDigit))
            return (false, "Разрешены только цифры.");
        if (input.Length != length)
            return (false, $"Необходимая длина - {length} цифр.");
        return (true, string.Empty);
    }

    public static (bool isValid, string error) RequiredDigitsOnly(string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) 
            return (false, "Поле не может быть пустым.");
        if (!input.All(char.IsDigit))
            return (false, "Разрешены только цифры.");
        return (true, string.Empty);
    }
    
    public static (bool isValid, string error) LettersSpacesCommasOnly(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return (false, "Поле не может быть пустым.");
        foreach (char c in input)
        {
            if (!char.IsLetter(c) && c != ' ' && c != ',' && c != '"' && c != '.' && c != '-')
                return (false, "Разрешены только буквы, пробелы, дефисы, точки и запятые.");
        }

        return (true, "");
    }

    public static (bool isValid, string error) RequiredNotNull(string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) 
            return (false, "Поле не может быть пустым.");
        return (true, string.Empty);
    }
    public static (bool, string) RequiredBeforeToday(string? input)
    {
        if (!DateTime.TryParse(input, out var date) || date >= DateTime.Today)
            return (false, "Дата не может быть позже сегодняшней.");
        return (true, "");
    }
    // public static (bool, string) RequiredAfterToday(string? input)
    // {
    //     if (!DateTime.TryParse(input, out var date) || date >= DateTime.Today)
    //         return (false, "Дата превышает доступные пределы");
    //     return (true, "");
    // }

    public static (bool, string) RequiredPositiveDouble(string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) 
            return (false, "Укажите стоимость");
        if (!double.TryParse(input, out var d) || d <= 0)
            return (false, "Стоимость должна быть положительным числом");
        return (true, "");
    }
}