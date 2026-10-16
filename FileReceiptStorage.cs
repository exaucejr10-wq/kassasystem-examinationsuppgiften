using Kassasystem.Models;

namespace Kassasystem.Data;

// Sparar kvitton i filer som RECEIPT_20261016.txt.
// Alla kvitton samma dag hamnar i samma fil, längst ner (append),
// så att gamla kvitton aldrig skrivs över.
public class FileReceiptStorage
{
    private readonly string _folder;

    public FileReceiptStorage(string folder)
    {
        _folder = folder;
    }

    public static string GetFileName(DateTime date)
    {
        return $"RECEIPT_{date:yyyyMMdd}.txt";
    }

    public string Save(Receipt receipt)
    {
        Directory.CreateDirectory(_folder);
        string path = Path.Combine(_folder, GetFileName(receipt.Date));

        // AppendAllText lägger till i slutet av filen och skapar den om den inte finns.
        File.AppendAllText(path, receipt.ToText() + Environment.NewLine);
        return path;
    }
}
