using System.Globalization;

namespace Kassasystem.Logic;

// Tolkar det kassören skriver, t.ex. "300 2" eller "300 1,5".
public static class InputParser
{
    public static bool TryParseProductLine(string? input, out int productId, out decimal quantity, out string error)
    {
        productId = 0;
        quantity = 0;
        error = "";

        if (string.IsNullOrWhiteSpace(input))
        {
            error = "Du skrev inget. Skriv t.ex. 300 2";
            return false;
        }

        string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 2)
        {
            error = "Fel format. Skriv <produktid> <antal>, t.ex. 300 2";
            return false;
        }

        if (!int.TryParse(parts[0], out productId))
        {
            error = $"\"{parts[0]}\" är inte ett giltigt produkt-ID.";
            return false;
        }

        // Både 1.5 och 1,5 ska fungera.
        string amountText = parts[1].Replace(',', '.');
        if (!decimal.TryParse(amountText, NumberStyles.Number, CultureInfo.InvariantCulture, out quantity))
        {
            error = $"\"{parts[1]}\" är inte ett giltigt antal.";
            return false;
        }

        return true;
    }
}
