using Kassasystem.Logic;

namespace Kassasystem.Tests;

public class InputParserTests
{
    [Fact]
    public void GiltigRad_GerIdOchAntal()
    {
        bool ok = InputParser.TryParseProductLine("300 2", out int id, out decimal quantity, out _);

        Assert.True(ok);
        Assert.Equal(300, id);
        Assert.Equal(2m, quantity);
    }

    [Theory]
    [InlineData("300 1.5")]
    [InlineData("300 1,5")]
    [InlineData("  300    1.5  ")]
    public void Decimaler_MedPunktEllerKomma_Fungerar(string input)
    {
        bool ok = InputParser.TryParseProductLine(input, out _, out decimal quantity, out _);

        Assert.True(ok);
        Assert.Equal(1.5m, quantity);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("300")]
    [InlineData("abc 2")]
    [InlineData("300 två")]
    [InlineData("300 2 5")]
    public void OgiltigInput_ReturnerarFalseOchFelmeddelande(string? input)
    {
        bool ok = InputParser.TryParseProductLine(input, out _, out _, out string error);

        Assert.False(ok);
        Assert.NotEqual("", error);
    }
}
