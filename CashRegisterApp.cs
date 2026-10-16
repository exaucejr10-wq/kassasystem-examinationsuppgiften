using Kassasystem.Data;
using Kassasystem.Logic;
using Kassasystem.Models;

namespace Kassasystem.UI;

// Allt som har med konsolen att göra (menyer, Console.ReadLine osv) ligger här.
// Själva logiken ligger i andra klasser.
public class CashRegisterApp
{
    private readonly ProductCatalog _catalog;
    private readonly FileReceiptStorage _receiptStorage;

    public CashRegisterApp(ProductCatalog catalog, FileReceiptStorage receiptStorage)
    {
        _catalog = catalog;
        _receiptStorage = receiptStorage;
    }

    public void Run()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("KASSASYSTEM");
            Console.WriteLine();
            Console.WriteLine("1. Ny kund");
            Console.WriteLine("2. Admin");
            Console.WriteLine("0. Avsluta");
            Console.WriteLine();
            Console.Write("Val: ");

            string? choice = Console.ReadLine();
            if (choice == null) // t.ex. om inputen tar slut
                return;

            switch (choice.Trim())
            {
                case "1":
                    NewCustomer();
                    break;
                case "2":
                    Console.WriteLine("Admin finns inte i den här versionen.");
                    break;
                case "0":
                    Console.WriteLine("Hej då!");
                    return;
                default:
                    Console.WriteLine("Ogiltigt val, välj 1, 2 eller 0.");
                    break;
            }
        }
    }

    private void NewCustomer()
    {
        var sale = new Sale();
        Console.WriteLine();
        Console.WriteLine("NY KUND - skriv <produktid> <antal>, PAY för att betala, CANCEL för att avbryta.");

        while (true)
        {
            Console.Write("> ");
            string? input = Console.ReadLine();
            if (input == null)
                return;

            string command = input.Trim().ToUpper();

            if (command == "PAY")
            {
                if (sale.IsEmpty)
                {
                    Console.WriteLine("Inga varor registrerade ännu.");
                    continue;
                }
                Pay(sale);
                return;
            }

            if (command == "CANCEL")
            {
                Console.WriteLine("Köpet avbröts.");
                return;
            }

            AddProduct(sale, input);
        }
    }

    private void AddProduct(Sale sale, string input)
    {
        if (!InputParser.TryParseProductLine(input, out int id, out decimal quantity, out string error))
        {
            Console.WriteLine(error);
            return;
        }

        Product? product = _catalog.FindById(id);
        if (product == null)
        {
            Console.WriteLine($"Produkt {id} finns inte.");
            return;
        }

        try
        {
            sale.AddItem(product, quantity);
            Console.WriteLine($"{product.Name} tillagd. Totalt hittills: {sale.Total:0.00} kr");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private void Pay(Sale sale)
    {
        var receipt = new Receipt(sale, DateTime.Now);
        Console.WriteLine();
        Console.WriteLine(receipt.ToText());

        try
        {
            string path = _receiptStorage.Save(receipt);
            Console.WriteLine($"Kvittot sparades i {path}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Kunde inte spara kvittot: {ex.Message}");
        }
    }
}
