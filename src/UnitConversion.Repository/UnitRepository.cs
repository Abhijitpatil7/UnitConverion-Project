using UnitConversion.Repository.Models;
using static UnitConversion.Repository.Models.UnitCategory;

namespace UnitConversion.Repository;

/// <summary>Hard-coded units. Base units: meter, kilogram, kelvin. To add a unit, add one line.</summary>
public class UnitRepository : IUnitRepository
{
    private const decimal FahrenheitFactor = 5m / 9m;

    private static readonly List<Unit> Units =
    [
        new("meter", "m", Length, 1m),
        new("kilometer", "km", Length, 1000m),
        new("centimeter", "cm", Length, 0.01m),
        new("mile", "mi", Length, 1609.344m),
        new("foot", "ft", Length, 0.3048m),
        new("inch", "in", Length, 0.0254m),

        new("kilogram", "kg", Mass, 1m),
        new("gram", "g", Mass, 0.001m),
        new("pound", "lb", Mass, 0.45359237m),
        new("ounce", "oz", Mass, 0.028349523125m),

        new("celsius", "C", Temperature, 1m, 273.15m),
        new("fahrenheit", "F", Temperature, FahrenheitFactor, 273.15m - 32m * FahrenheitFactor),
        new("kelvin", "K", Temperature, 1m),
    ];

    public IReadOnlyList<Unit> GetByCategory(UnitCategory category) =>
        Units.Where(u => u.Category == category).ToList();
}
