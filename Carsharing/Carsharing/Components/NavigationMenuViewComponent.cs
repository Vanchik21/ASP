using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Carsharing.Models;

namespace Carsharing.Components
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        private readonly VehicleDbContext _context;
        private const string SessionCategoryKey = "CurrentCategory";

        public NavigationMenuViewComponent(VehicleDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var categories = _context.Vehicles
                .Select(v => v.Category)
                .Distinct()
                .OrderBy(c => c)
                .ToList();

            string? selected = RouteData.Values.ContainsKey("category")
                ? RouteData.Values["category"]?.ToString()
                : HttpContext.Request.Query["category"].ToString();

            if (string.IsNullOrWhiteSpace(selected))
            {
                selected = HttpContext.Session.GetString(SessionCategoryKey);
            }

            ViewBag.SelectedCategory = string.IsNullOrWhiteSpace(selected) ? null : selected;

            return View(categories);
        }
    }
}