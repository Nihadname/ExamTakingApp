using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Models;

public sealed class AppUser:IdentityUser
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public DateTimeOffset CreatedTime { get; set; }
}