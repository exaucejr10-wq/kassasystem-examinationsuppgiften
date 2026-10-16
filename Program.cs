using System.Globalization;
using System.Text;
using Kassasystem.Data;
using Kassasystem.Logic;
using Kassasystem.UI;

// Program.cs gör bara en sak: kopplar ihop delarna och startar programmet.
Console.OutputEncoding = Encoding.UTF8;
CultureInfo.CurrentCulture = new CultureInfo("sv-SE"); // svenska priser, t.ex. 15,90

string productFile = Path.Combine(AppContext.BaseDirectory, "products.txt");
string receiptFolder = Path.Combine(Directory.GetCurrentDirectory(), "Kvitton");

ProductCatalog catalog;
try
{
    var repository = new FileProductRepository(productFile);
    catalog = new ProductCatalog(repository);
}
catch (Exception ex)
{
    Console.WriteLine($"Fel: {ex.Message}");
    Console.WriteLine("Programmet kan inte starta utan produkter.");
    return;
}

Console.WriteLine($"{catalog.Count} produkter inlästa.");

var app = new CashRegisterApp(catalog, new FileReceiptStorage(receiptFolder));
app.Run();
