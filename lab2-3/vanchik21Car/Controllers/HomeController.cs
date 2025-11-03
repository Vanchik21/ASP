using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Http;
using vanchik21Car.Models;
using vanchik21Car.Models.ViewModels;

namespace vanchik21Car.Controllers
{
    public class HomeController : Controller
    {
        private readonly IVehicleRepository repository;
        private const int PageSize = 6;
        private const string SessionCategoryKey = "CurrentCategory";

        public HomeController(IVehicleRepository repo)
        {
            repository = repo;
        }

        public IActionResult Index(string? category, int page = 1)
        {
            // Determine effective category using incoming value or session fallback
            string? effectiveCategory = category;
            if (effectiveCategory is null)
            {
                effectiveCategory = HttpContext.Session.GetString(SessionCategoryKey);
            }

            // Handle explicit 'All' click (empty string clears filter)
            if (category != null && string.IsNullOrWhiteSpace(category))
            {
                HttpContext.Session.Remove(SessionCategoryKey);
                effectiveCategory = null;
            }
            else if (!string.IsNullOrWhiteSpace(category))
            {
                HttpContext.Session.SetString(SessionCategoryKey, category);
            }

            var query = repository.Vehicles
                .Where(v => string.IsNullOrEmpty(effectiveCategory) || v.Category == effectiveCategory)
                .OrderBy(v => v.VehicleID);

            var items = query
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            var model = new VehicleListViewModel
            {
                Vehicles = items,
                CurrentCategory = effectiveCategory,
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = query.Count()
                }
            };

            return View(model);
        }
    }
}
