namespace vanchik21Car.Models.ViewModels
{
    public class PagingInfo
    {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages => ItemsPerPage ==0 ?0 : (int)System.Math.Ceiling((decimal)TotalItems / ItemsPerPage);
    }
}
