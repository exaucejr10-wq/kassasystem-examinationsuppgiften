using System.Globalization;
using System.Text;

namespace Kassasystem.Models;

// Ett kvitto skapas när kunden betalar (PAY).
public class Receipt
{
    private static readonly CultureInfo Swedish = new("sv-SE");

    public DateTime Date { get; }
    public IReadOnlyList<SaleItem> Items { get; }
    public decimal Total { get; }

    public Receipt(Sale sale, DateTime date)
    {
        Date = date;
        Items = sale.Items.ToList();
        Total = sale.Total;
    }

    // Gör om kvittot till text som kan visas i konsolen och sparas i fil.
    public string ToText()
    {
        var sb = new StringBuilder();
        sb.AppendLine(new string('=', 44));
        sb.AppendLine($"KVITTO  {Date.ToString("yyyy-MM-dd HH:mm:ss")}");
        sb.AppendLine(new string('-', 44));

        foreach (SaleItem item in Items)
        {
            string unit = item.Product.PriceType == PriceType.Kg ? "kg" : "st";
            string amount = item.Product.PriceType == PriceType.Kg
                ? item.Quantity.ToString("0.000", Swedish)
                : item.Quantity.ToString("0", Swedish);

            sb.AppendLine(item.Product.Name);
            sb.AppendLine(
                $"  {amount} {unit} x {item.Product.Price.ToString("0.00", Swedish)} kr/{unit}"
                    .PadRight(32) +
                item.LineTotal.ToString("0.00", Swedish).PadLeft(12));
        }

        sb.AppendLine(new string('-', 44));
        sb.AppendLine("TOTALT".PadRight(32) + (Total.ToString("0.00", Swedish) + " kr").PadLeft(12));
        sb.AppendLine(new string('=', 44));
        return sb.ToString();
    }
}
