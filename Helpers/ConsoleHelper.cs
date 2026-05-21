namespace AssetTrackingSystem.Helpers;

public static class ConsoleHelper
{
    public static string ReadRequiredString(string label)
    {
        while (true)
        {
            Console.Write(label);
            string? value = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(value))
            {
                return value.Trim();
            }

            Console.WriteLine("Value cannot be empty.");
        }
    }

    public static string? ReadOptionalString(string label)
    {
        Console.Write(label);
        string? value = Console.ReadLine();
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }

    public static decimal ReadPositiveDecimal(string label)
    {
        while (true)
        {
            Console.Write(label);
            string? input = Console.ReadLine();

            if (decimal.TryParse(input, out decimal value) && value > 0)
            {
                return value;
            }

            Console.WriteLine("Please enter a valid positive number.");
        }
    }

    public static int ReadInt(string label, int min, int max)
    {
        while (true)
        {
            Console.Write(label);
            string? input = Console.ReadLine();

            if (int.TryParse(input, out int value) && value >= min && value <= max)
            {
                return value;
            }

            Console.WriteLine($"Please enter a number from {min} to {max}.");
        }
    }

    public static int? ReadOptionalInt(string label)
    {
        while (true)
        {
            Console.Write(label);
            string? input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                return null;
            }

            if (int.TryParse(input, out int value) && value > 0)
            {
                return value;
            }

            Console.WriteLine("Please enter a valid positive number, or leave it empty.");
        }
    }

    public static DateTime ReadDate(string label)
    {
        while (true)
        {
            Console.Write(label);
            string? input = Console.ReadLine();

            if (DateTime.TryParse(input, out DateTime date))
            {
                return date.Date;
            }

            Console.WriteLine("Please enter a valid date, for example 2025-05-21.");
        }
    }

    public static void Pause()
    {
        Console.WriteLine();
        Console.Write("Press Enter to continue...");
        Console.ReadLine();
    }
}
