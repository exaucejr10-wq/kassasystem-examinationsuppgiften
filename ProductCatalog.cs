using Kassasystem.Data;
using Kassasystem.Models;

namespace Kassasystem.Logic;

// Håller alla produkter i minnet så att de går snabbt att söka på ID.
public class ProductCatalog
{
    // Dictionary gör att vi hittar en produkt direkt via ID,
    // istället för att gå igenom hela listan varje gång.
    private readonly Dictionary<int, Product> _products = new();

    public ProductCatalog(IProductRepository repository)
    {
        foreach (Product product in repository.LoadProducts())
            _products[product.Id] = product;
    }

    public int Count => _products.Count;

    public Product? FindById(int id)
    {
        _products.TryGetValue(id, out Product? product);
        return product;
    }
}
