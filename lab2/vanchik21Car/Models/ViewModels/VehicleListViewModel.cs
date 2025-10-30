using System.Collections.Generic;

namespace vanchik21Car.Models.ViewModels
{
 public class VehicleListViewModel
 {
 public IEnumerable<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
 public PagingInfo PagingInfo { get; set; } = new PagingInfo();
 }
}
