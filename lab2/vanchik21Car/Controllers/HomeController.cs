using Microsoft.AspNetCore.Mvc;
using vanchik21Car.Models;
using vanchik21Car.Models.ViewModels;

namespace vanchik21Car.Controllers
{
    public class HomeController : Controller
    {
        private readonly IVehicleRepository repository;
        private const int PageSize = 4;

        public HomeController(IVehicleRepository repo)
        {
            repository = repo;
        }

        public IActionResult Index(int page = 1)
        {
            var query = repository.Vehicles
                .OrderBy(v => v.VehicleID);

            var items = query
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            var model = new VehicleListViewModel
            {
                Vehicles = items,
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
