# kassasystem-examinationsuppgiften

Konsolapplikation i C# (.NET 8) som simulerar en kassa i en mindre butik.

## Köra projektet

```
git clone <repo-länk>
cd Kassasystem
dotnet restore
dotnet build
dotnet test
dotnet run --project src/Kassasystem
```

## Använda kassan

- Välj `1` för Ny kund.
- Skriv `<produktid> <antal>`, t.ex. `301 2` eller `300 1,5` (kilovaror får ha decimaler).
- `PAY` avslutar köpet, visar kvittot och sparar det.
- `CANCEL` avbryter köpet.

Produkterna finns i `src/Kassasystem/products.txt` (format `id;namn;pris;Styck/Kg`).
Kvitton sparas i mappen `Kvitton/` som `RECEIPT_yyyyMMdd.txt`. Flera kvitton samma dag hamnar i samma fil.

## Struktur

| Mapp | Innehåll |
|------|----------|
| `Models` | Product, PriceType, Sale, SaleItem, Receipt |
| `Data` | IProductRepository, FileProductRepository, FileReceiptStorage |
| `Logic` | ProductCatalog, InputParser |
| `UI` | CashRegisterApp (menyer och konsol) |
| `tests` | Enhetstester med xUnit |
