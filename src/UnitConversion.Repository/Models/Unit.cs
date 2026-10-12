namespace UnitConversion.Repository.Models;

public enum UnitCategory { Length, Mass, Temperature }

/// <summary>A unit relative to its category's base unit: base = value * Factor + Offset.</summary>
public record Unit(string Name, string Symbol, UnitCategory Category, decimal Factor, decimal Offset = 0m);
