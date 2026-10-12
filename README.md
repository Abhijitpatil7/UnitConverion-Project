# Unit Conversion API

A small ASP.NET Core Web API that converts values between units of length, mass and temperature.

## Solution structure

```
src/
  UnitConversion.Api          Presentation layer: controller, Swagger, startup (DI wiring)
  UnitConversion.Business     Business layer: conversion logic (ConversionService), result model
  UnitConversion.Repository   Data layer: unit data (UnitRepository), Unit model
```

Dependencies: Api -> Business -> Repository (Api also references Repository only to register it in DI).
Flow: Controller -> Service -> Repository.

## Run locally

Requires the [.NET SDK](https://dotnet.microsoft.com/download) (10.0 or later).

```bash
dotnet restore
dotnet run --project src/UnitConversion.Api
```

Swagger UI: http://localhost:5080/swagger

Example request:

```
GET http://localhost:5080/api/convert?category=length&from=meter&to=foot&value=10
```

```json
{ "category": "Length", "from": "meter", "to": "foot", "value": 10, "result": 32.808399 }
```

Units can be given by name or symbol, case-insensitive (`meter`/`m`, `celsius`/`C`, `pound`/`lb`).
Invalid input returns `400` with an error message.

## Design decisions

- **Three layers with one job each.** API handles HTTP, Business holds the conversion rules, Repository owns the data.
- **Convert via a base unit.** Each unit stores a factor (and offset) relative to a base unit per category
  (`base = value * Factor + Offset`). Adding a unit is one line in `UnitRepository.cs`, and temperature
  fits the same formula.
- **Repository interface.** `IUnitRepository` hides where units come from. For hundreds of units, replace the
  hard-coded list with a database-backed implementation; Business and Api stay unchanged.
- **`decimal`** avoids floating-point noise; results are rounded to 6 decimals.
- **Kept small on purpose:** one endpoint, no auth, no database.

## Supported Units

The Unit Conversion API supports three categories: **Length, Mass, and Temperature**. Each unit is defined by its name, symbol, category, conversion factor, and optional offset relative to the category's base unit.

### Length

The base unit is **meter (m)**.

| Unit | Symbol | Factor | Offset |
|---|---|---:|---:|
| Meter | `m` | 1 | 0 |
| Kilometer | `km` | 1000 | 0 |
| Centimeter | `cm` | 0.01 | 0 |
| Mile | `mi` | 1609.344 | 0 |
| Foot | `ft` | 0.3048 | 0 |
| Inch | `in` | 0.0254 | 0 |

### Mass

The base unit is **kilogram (kg)**.

| Unit | Symbol | Factor | Offset |
|---|---|---:|---:|
| Kilogram | `kg` | 1 | 0 |
| Gram | `g` | 0.001 | 0 |
| Pound | `lb` | 0.45359237 | 0 |
| Ounce | `oz` | 0.028349523125 | 0 |

### Temperature

The base unit is **Kelvin (K)**.

| Unit | Symbol | Factor | Offset |
|---|---|---:|---:|
| Celsius | `C` | 1 | 273.15 |
| Fahrenheit | `F` | `FahrenheitFactor` (5/9) | 255.372222... |
| Kelvin | `K` | 1 | 0 |

*Note: The Fahrenheit offset is calculated as `273.15 - 32 × (5/9)`. Use the full decimal precision supported by `decimal` for accurate calculations.*

## Conversion Formula

The API converts values through the base unit of each category.

**Step 1: Convert the input to the base unit**

```text
BaseValue = InputValue × Factor + Offset
```

**Step 2: Convert the base value to the target unit**

```text
Result = (BaseValue - TargetOffset) / TargetFactor
```

For length and mass, the offset is zero. Temperature conversions use offsets to account for differences between temperature scales.

## Examples

| Conversion | Input | Expected Result |
|---|---:|---:|
| Kilometer → Meter | 1 km | 1000 m |
| Mile → Kilometer | 1 mi | 1.609344 km |
| Kilogram → Gram | 1 kg | 1000 g |
| Pound → Kilogram | 1 lb | 0.453592 kg |
| Celsius → Fahrenheit | 25°C | 77°F |
| Celsius → Kelvin | 25°C | 298.15 K |
| Fahrenheit → Celsius | 32°F | 0°C |

Results are rounded to six decimal places.

## Adding a New Unit

To add a unit, define its name, symbol, category, and conversion factor relative to the category's base unit in `UnitRepository.cs`. For temperature units, provide the appropriate offset as well.

Units can only be converted when they belong to the same category. For example, kilometers can be converted to miles, but kilograms cannot be converted to meters.

