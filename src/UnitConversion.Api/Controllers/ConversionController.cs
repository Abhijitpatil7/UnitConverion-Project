using Microsoft.AspNetCore.Mvc;
using UnitConversion.Business.Models;
using UnitConversion.Business.Services;

namespace UnitConversion.Api.Controllers;

[ApiController]
[Route("api/convert")]
public class ConversionController(IConversionService service) : ControllerBase
{
    /// <summary>Convert a value from one unit to another.</summary>
    /// <param name="category">length, mass or temperature</param>
    /// <param name="from">Source unit name or symbol, e.g. meter or m</param>
    /// <param name="to">Target unit name or symbol, e.g. foot or ft</param>
    /// <param name="value">Number to convert</param>
    [HttpGet]
    [ProducesResponseType<ConversionResult>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<ConversionResult> Convert(string category, string from, string to, decimal value)
    {
        try
        {
            return Ok(service.Convert(category, from, to, value));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
