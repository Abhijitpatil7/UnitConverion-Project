using UnitConversion.Business.Models;

namespace UnitConversion.Business.Services;

public interface IConversionService
{
    /// <exception cref="ArgumentException">Unknown category or unit.</exception>
    ConversionResult Convert(string category, string from, string to, decimal value);
}
