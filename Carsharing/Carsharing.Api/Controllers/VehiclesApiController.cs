using Carsharing.Data.Data;
using Carsharing.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Carsharing.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VehiclesApiController : ControllerBase
{
    private readonly CarsharingDbContext _context;
    public VehiclesApiController(CarsharingDbContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Vehicle>>> GetAll()
    {
        var vehicles = await _context.Vehicles.AsNoTracking().ToListAsync();
        return Ok(vehicles);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<Vehicle>> Get(long id)
    {
        var vehicle = await _context.Vehicles.Include(v=>v.MaintenanceRecords).FirstOrDefaultAsync(v=>v.VehicleID==id);
        if (vehicle==null) return NotFound();
        return vehicle;
    }

    [HttpPost]
    [Authorize(Roles="Admin,Manager")]
    public async Task<ActionResult<Vehicle>> Create(Vehicle vehicle)
    { 
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        vehicle.CreatedAt = DateTime.UtcNow;
        _context.Vehicles.Add(vehicle);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = vehicle.VehicleID }, vehicle);
    }

    [HttpPut("{id:long}")]
    [Authorize(Roles="Admin,Manager")]
    public async Task<IActionResult> Update(long id, Vehicle update)
    {
        if (id != update.VehicleID) return BadRequest();
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        var entity = await _context.Vehicles.FirstOrDefaultAsync(v=>v.VehicleID==id);
        if (entity==null) return NotFound();
        entity.Make = update.Make;
        entity.Model = update.Model;
        entity.Year = update.Year;
        entity.VIN = update.VIN;
        entity.LicensePlate = update.LicensePlate;
        entity.Category = update.Category;
        entity.DailyRate = update.DailyRate;
        entity.Status = update.Status;
        entity.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:long}")]
    [Authorize(Roles="Admin")]
    public async Task<IActionResult> Delete(long id)
    {
        var entity = await _context.Vehicles.FirstOrDefaultAsync(v=>v.VehicleID==id);
        if (entity==null) return NotFound();
        _context.Vehicles.Remove(entity);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}