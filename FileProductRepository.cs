using System.Globalization;
using Kassasystem.Models;

namespace Kassasystem.Data;

// Läser produkter från en textfil där varje rad ser ut så här:
// 300;Bananer;29.90;Kg
public class FileProductRepository : IProductRepository
{
    private readonly string _filePath;

    public FileProductRepository(string filePath)
    {
        _filePath = filePath;
    }

    public List<Product> LoadProducts()
    {
        if (!File.Exists(_filePath))
            throw new FileNotFoundException($"Hittar inte produktfilen: {_filePath}");

        var products = new List<Product>();
        string[] lines = File.ReadAllLines(_filePath);

        foreach (string rawLine in lines)
        {
            string line = rawLine.Trim();
            if (line == "" || line.StartsWith("#"))
                continue; // hoppa över tomma rader och kommentarer

            Product? product = ParseLine(line);
            if (product != null)
                products.Add(product); // felaktiga rader hoppas över
        }

        return products;
    }

    // Returnerar null om raden är felaktig, istället för att krascha.
    public static Product? ParseLine(string line)
    {
        string[] parts = line.Split(';');
        if (parts.Length != 4)
            return null;

        if (!int.TryParse(parts[0].Trim(), out int id) || id <= 0)
            return null;

        string name = parts[1].Trim();
        if (name == "")
            return null;

        string priceText = parts[2].Trim().Replace(',', '.');
        if (!decimal.TryParse(priceText, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal price)
            || price < 0)
            return null;

        if (!Enum.TryParse(parts[3].Trim(), ignoreCase: true, out PriceType type)
            || !Enum.IsDefined(type))
            return null;

        return new Product(id, name, price, type);
    }
}
