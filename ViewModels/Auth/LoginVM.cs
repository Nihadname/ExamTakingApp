using System.ComponentModel.DataAnnotations;

namespace WebApplication1.ViewModels.Auth;

public sealed record LoginVM
{
    [Required,MaxLength(100)]
    public required string PhoneNumber { get; set; }
    [Required,MaxLength(100),DataType(DataType.Password)]
    public required string Password { get; set; }
    [Display(Name ="Remember Me ?")]
    public bool RememberMe { get; set; }
};