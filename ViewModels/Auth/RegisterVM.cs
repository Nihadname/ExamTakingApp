using System.ComponentModel.DataAnnotations;

namespace ExamTakingApp.ViewModels.Auth;

public sealed record RegisterVM
{
    [Required, StringLength(200)]
    public required string FirstName { get; init; }
    [Required, StringLength(100)]
    public required string LastName { get; init; }
    [Required, StringLength(100),Phone]
    public required string PhoneNumber { get; init; }
}
public sealed record RegisterSatExamVM
{
    [Required, StringLength(200)]
    public required string FirstName { get; init; }
    [Required, StringLength(100)]
    public required string LastName { get; init; }
    [Required, StringLength(100),Phone]
    public required string PhoneNumber { get; init; }
    [Required, StringLength(100)]
    public required string Password { get; init; }
    
    public required string RepeatPassword { get; init; }
    
}