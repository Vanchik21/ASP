using Carsharing.Data.Data;
using Carsharing.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Carsharing.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MaintenanceApiController : ControllerBase
{
    private readonly CarsharingDbContext _context;
    public MaintenanceApiController(CarsharingDbContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MaintenanceRecord>>> GetAll([FromQuery] long? vehicleId)
    {
        var query = _context.MaintenanceRecords.Include(m=>m.Vehicle).AsQueryable();
        if (vehicleId.HasValue) query = query.Where(m=>m.VehicleID==vehicleId);
        return Ok(await query.OrderByDescending(m=>m.PerformedAt).ToListAsync());
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<MaintenanceRecord>> Get(long id)
    {
        var entity = await _context.MaintenanceRecords.Include(m=>m.Vehicle).FirstOrDefaultAsync(m=>m.MaintenanceRecordID==id);
        if (entity==null) return NotFound();
        return entity;
    }

    [HttpPost]
    [Authorize(Roles="Admin,Manager")]
    public async Task<ActionResult<MaintenanceRecord>> Create(MaintenanceRecord record)
    { 
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        _context.MaintenanceRecords.Add(record);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = record.MaintenanceRecordID }, record);
    }

    [HttpPut("{id:long}")]
    [Authorize(Roles="Admin,Manager")]
    public async Task<IActionResult> Update(long id, MaintenanceRecord model)
    {
        if (id != model.MaintenanceRecordID) return BadRequest();
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        var entity = await _context.MaintenanceRecords.FirstOrDefaultAsync(m=>m.MaintenanceRecordID==id);
        if (entity==null) return NotFound();
        entity.VehicleID = model.VehicleID;
        entity.Description = model.Description;
        entity.PerformedAt = model.PerformedAt;
        entity.Cost = model.Cost;
        entity.PerformedBy = model.PerformedBy;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:long}")]
    [Authorize(Roles="Admin")]
    public async Task<IActionResult> Delete(long id)
    {
        var entity = await _context.MaintenanceRecords.FirstOrDefaultAsync(m=>m.MaintenanceRecordID==id);
        if (entity==null) return NotFound();
        _context.MaintenanceRecords.Remove(entity);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}