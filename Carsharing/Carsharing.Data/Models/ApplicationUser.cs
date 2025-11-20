using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Carsharing.Data.Models;

public class ApplicationUser : IdentityUser
{
     [MaxLength(100)]
     public string? FirstName { get; set; }

     [MaxLength(100)]
     public string? LastName { get; set; }

     public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;

     public string FullName => ($"{FirstName} {LastName}").Trim();
}