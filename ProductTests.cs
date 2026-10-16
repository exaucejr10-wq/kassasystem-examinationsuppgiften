using Kassasystem.Data;
using Kassasystem.Logic;
using Kassasystem.Models;

namespace Kassasystem.Tests;

public class ProductTests
{
    [Fact]
    public void ParseLine_GiltigRad_SkaparProdukt()
    {
        Product? product = FileProductRepository.ParseLine("300;Bananer;29.90;Kg");

        Assert.NotNull(product);
        Assert.Equal(300, product!.Id);
        Assert.Equal("Bananer", product.Name);
        Assert.Equal(29.90m, product.Price);
        Assert.Equal(PriceType.Kg, product.PriceType);
    }

    [Theory]
    [InlineData("300;Bananer;29.90")]          // för få delar
    [InlineData("abc;Bananer;29.90;Kg")]       // ID är inte ett tal
    [InlineData("300;;29.90;Kg")]              // inget namn
    [InlineData("300;Bananer;gratis;Kg")]      // pris är inte ett tal
    [InlineData("300;Bananer;-5;Kg")]          // negativt pris
    [InlineData("300;Bananer;29.90;Liter")]    // okänd pristyp
    public void ParseLine_FelaktigRad_ReturnerarNull(string line)
    {
        Assert.Null(FileProductRepository.ParseLine(line));
    }

    [Fact]
    public void Catalog_FindById_HittarRattProdukt()
    {
        var catalog = new ProductCatalog(new FakeProductRepository());

        Assert.Equal("Mjölk", catalog.FindById(301)?.Name);
    }

    [Fact]
    public void Catalog_FindById_OkantId_GerNull()
    {
        var catalog = new ProductCatalog(new FakeProductRepository());

        Assert.Null(catalog.FindById(999));
    }

    [Fact]
    public void ReceiptFileName_FoljerFormatet()
    {
        string name = FileReceiptStorage.GetFileName(new DateTime(2026, 9, 7));

        Assert.Equal("RECEIPT_20260907.txt", name);
    }

    // En fejkad "databas" så att testerna inte behöver någon riktig fil.
    // Det här fungerar tack vare interfacet IProductRepository.
    private class FakeProductRepository : IProductRepository
    {
        public List<Product> LoadProducts() => new()
        {
            new Product(300, "Bananer", 29.90m, PriceType.Kg),
            new Product(301, "Mjölk", 15.90m, PriceType.Styck)
        };
    }
}
