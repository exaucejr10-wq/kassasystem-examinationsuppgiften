using Kassasystem.Models;

namespace Kassasystem.Tests;

public class SaleTests
{
    private readonly Product _bananer = new(300, "Bananer", 29.90m, PriceType.Kg);
    private readonly Product _mjolk = new(301, "Mjölk", 15.90m, PriceType.Styck);

    [Fact]
    public void LineTotal_StyckProdukt_PrisGangerAntal()
    {
        var sale = new Sale();
        sale.AddItem(_mjolk, 2);

        Assert.Equal(31.80m, sale.Items[0].LineTotal);
    }

    [Fact]
    public void LineTotal_KiloProdukt_FungerarMedDecimaler()
    {
        var sale = new Sale();
        sale.AddItem(_bananer, 1.5m);

        Assert.Equal(44.85m, sale.Items[0].LineTotal);
    }

    [Fact]
    public void LineTotal_AvrundasTillHelaOren()
    {
        var sale = new Sale();
        sale.AddItem(_bananer, 0.333m); // 29,90 * 0,333 = 9,9567

        Assert.Equal(9.96m, sale.Items[0].LineTotal);
    }

    [Fact]
    public void Total_SummerarAllaRader()
    {
        var sale = new Sale();
        sale.AddItem(_mjolk, 2);      // 31,80
        sale.AddItem(_bananer, 1.5m); // 44,85

        Assert.Equal(76.65m, sale.Total);
    }

    [Fact]
    public void Total_TomtKop_ArNoll()
    {
        var sale = new Sale();

        Assert.Equal(0m, sale.Total);
        Assert.True(sale.IsEmpty);
    }

    [Fact]
    public void AddItem_SammaProduktTvaGanger_SlasIhopTillEnRad()
    {
        var sale = new Sale();
        sale.AddItem(_mjolk, 2);
        sale.AddItem(_mjolk, 1);

        Assert.Single(sale.Items);
        Assert.Equal(3m, sale.Items[0].Quantity);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void AddItem_NollEllerNegativtAntal_KastarFel(double quantity)
    {
        var sale = new Sale();

        Assert.Throws<ArgumentException>(() => sale.AddItem(_mjolk, (decimal)quantity));
        Assert.True(sale.IsEmpty);
    }

    [Fact]
    public void AddItem_DecimalPaStyckProdukt_KastarFel()
    {
        var sale = new Sale();

        Assert.Throws<ArgumentException>(() => sale.AddItem(_mjolk, 1.5m));
    }

    [Fact]
    public void AddItem_ForStortAntal_KastarFel()
    {
        var sale = new Sale();

        Assert.Throws<ArgumentException>(() => sale.AddItem(_mjolk, 1001));
    }
}
