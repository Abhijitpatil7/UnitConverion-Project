using UnitConversion.Repository.Models;

namespace UnitConversion.Repository;

public interface IUnitRepository
{
    IReadOnlyList<Unit> GetByCategory(UnitCategory category);
}
