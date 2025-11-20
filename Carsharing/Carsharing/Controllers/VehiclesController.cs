using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using Carsharing.Data.Data;
using Carsharing.Data.Models;

namespace Carsharing.Controllers
{
    [Authorize] 
    public class VehiclesController : Controller
    {
        private readonly CarsharingDbContext _context;

        public VehiclesController(CarsharingDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var items = _context.Vehicles
            .OrderBy(v => v.Make).ThenBy(v => v.Model)
            .ToList();
            return View(items);
        }

        public IActionResult Details(long id)
        {
            var vehicle = _context.Vehicles
            .Include(v => v.MaintenanceRecords)
            .FirstOrDefault(v => v.VehicleID == id);
            if (vehicle == null) return NotFound();
            return View(vehicle);
        }

        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Create()
        {
            return View(new Vehicle());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Create(Vehicle vehicle)
        {
            if (!ModelState.IsValid)
            {
                return View(vehicle);
            }
            vehicle.CreatedAt = DateTime.UtcNow;
            _context.Vehicles.Add(vehicle);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Edit(long id)
        {
            var vehicle = _context.Vehicles.Find(id);
            if (vehicle == null) return NotFound();
            return View(vehicle);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Edit(long id, Vehicle vehicle)
        {
            if (id != vehicle.VehicleID) return BadRequest();
            if (!ModelState.IsValid)
            {
                return View(vehicle);
            }
            var entity = _context.Vehicles.FirstOrDefault(v => v.VehicleID == id);
            if (entity == null) return NotFound();

            entity.Make = vehicle.Make;
            entity.Model = vehicle.Model;
            entity.Year = vehicle.Year;
            entity.VIN = vehicle.VIN;
            entity.LicensePlate = vehicle.LicensePlate;
            entity.Category = vehicle.Category;
            entity.DailyRate = vehicle.DailyRate;
            entity.Status = vehicle.Status;
            entity.UpdatedAt = DateTime.UtcNow;

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(long id)
        {
            var vehicle = _context.Vehicles.FirstOrDefault(v => v.VehicleID == id);
            if (vehicle == null) return NotFound();
            return View(vehicle);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteConfirmed(long id)
        {
            var entity = _context.Vehicles.FirstOrDefault(v => v.VehicleID == id);
            if (entity == null) return NotFound();
            _context.Vehicles.Remove(entity);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
