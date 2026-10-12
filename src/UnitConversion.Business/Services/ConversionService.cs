using UnitConversion.Business.Models;
using UnitConversion.Repository;
using UnitConversion.Repository.Models;

namespace UnitConversion.Business.Services;

public class ConversionService(IUnitRepository repository) : IConversionService
{
    public ConversionResult Convert(string category, string from, string to, decimal value)
    {
        if (!Enum.TryParse<UnitCategory>(category, ignoreCase: true, out var cat) || !Enum.IsDefined(cat))
            throw new ArgumentException(
                $"Unknown category '{category}'. Supported: {string.Join(", ", Enum.GetNames<UnitCategory>())}.");

        var units = repository.GetByCategory(cat);
        var source = Find(units, from, cat);
        var target = Find(units, to, cat);

        // Convert to the base unit first, then to the target unit.
        var baseValue = value * source.Factor + source.Offset;
        var result = (baseValue - target.Offset) / target.Factor;

        return new ConversionResult(cat.ToString(), source.Name, target.Name, value,
            Math.Round(result, 6, MidpointRounding.AwayFromZero));
    }

    private static Unit Find(IReadOnlyList<Unit> units, string text, UnitCategory category) =>
        units.FirstOrDefault(u =>
            string.Equals(u.Name, text, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(u.Symbol, text, StringComparison.OrdinalIgnoreCase))
        ?? throw new ArgumentException($"Unknown {category} unit '{text}'.");
}
