using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using Carsharing.Models;

namespace Carsharing.Controllers
{
    [Authorize]
    public class MaintenanceController : Controller
    {
        private readonly VehicleDbContext _context;

        public MaintenanceController(VehicleDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(long? vehicleId)
        {
            var query = _context.MaintenanceRecords
            .Include(m => m.Vehicle)
            .AsQueryable();

            if (vehicleId.HasValue)
            {
                query = query.Where(m => m.VehicleID == vehicleId);
            }

            var items = query
            .OrderByDescending(m => m.PerformedAt)
            .ToList();

            ViewBag.VehicleId = vehicleId;
            ViewBag.Vehicles = _context.Vehicles
            .OrderBy(v => v.Make)
            .ThenBy(v => v.Model)
            .Select(v => new { v.VehicleID, Title = v.Make + " " + v.Model })
            .ToList();

            return View(items);
        }

        public IActionResult Details(long id)
        {
            var entity = _context.MaintenanceRecords
            .Include(m => m.Vehicle)
            .FirstOrDefault(m => m.MaintenanceRecordID == id);
            if (entity == null) return NotFound();
            return View(entity);
        }

        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Create(long? vehicleId)
        {
            ViewData["VehicleID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
            _context.Vehicles
            .OrderBy(v => v.Make)
            .ThenBy(v => v.Model)
            .Select(v => new { v.VehicleID, Title = v.Make + " " + v.Model })
            .ToList(),
            "VehicleID", "Title", vehicleId);
            return View(new MaintenanceRecord { VehicleID = vehicleId ?? 0, PerformedAt = System.DateTime.UtcNow });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Create(MaintenanceRecord record)
        {
            if (!ModelState.IsValid)
            {
                ViewData["VehicleID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
                _context.Vehicles
                .OrderBy(v => v.Make)
                .ThenBy(v => v.Model)
                .Select(v => new { v.VehicleID, Title = v.Make + " " + v.Model })
                .ToList(),
                "VehicleID", "Title", record.VehicleID);
                return View(record);
            }

            _context.MaintenanceRecords.Add(record);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index), new { vehicleId = record.VehicleID });
        }

        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Edit(long id)
        {
            var entity = _context.MaintenanceRecords.Find(id);
            if (entity == null) return NotFound();

            ViewData["VehicleID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
            _context.Vehicles
            .OrderBy(v => v.Make)
            .ThenBy(v => v.Model)
            .Select(v => new { v.VehicleID, Title = v.Make + " " + v.Model })
            .ToList(),
            "VehicleID", "Title", entity.VehicleID);

            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Edit(long id, MaintenanceRecord model)
        {
            if (id != model.MaintenanceRecordID) return BadRequest();

            if (!ModelState.IsValid)
            {
                ViewData["VehicleID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
                _context.Vehicles
                .OrderBy(v => v.Make)
                .ThenBy(v => v.Model)
                .Select(v => new { v.VehicleID, Title = v.Make + " " + v.Model })
                .ToList(),
                "VehicleID", "Title", model.VehicleID);
                return View(model);
            }

            var entity = _context.MaintenanceRecords.FirstOrDefault(m => m.MaintenanceRecordID == id);
            if (entity == null) return NotFound();

            entity.VehicleID = model.VehicleID;
            entity.Description = model.Description;
            entity.PerformedAt = model.PerformedAt;
            entity.Cost = model.Cost;
            entity.PerformedBy = model.PerformedBy;

            _context.SaveChanges();
            return RedirectToAction(nameof(Index), new { vehicleId = entity.VehicleID });
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(long id)
        {
            var entity = _context.MaintenanceRecords
            .Include(m => m.Vehicle)
            .FirstOrDefault(m => m.MaintenanceRecordID == id);
            if (entity == null) return NotFound();
            return View(entity);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteConfirmed(long id)
        {
            var entity = _context.MaintenanceRecords.FirstOrDefault(m => m.MaintenanceRecordID == id);
            if (entity == null) return NotFound();
            var vid = entity.VehicleID;
            _context.MaintenanceRecords.Remove(entity);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index), new { vehicleId = vid });
        }
    }
}
