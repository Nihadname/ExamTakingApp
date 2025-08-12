using System.ComponentModel.DataAnnotations;

namespace ExamTakingApp.ViewModels.Auth;

public sealed record RegisterVM
{
    [Required, StringLength(200)]
    public required string FirstName { get; init; }
    [Required, StringLength(100)]
    public required string LastName { get; init; }
    [Required, DataType(DataType.Password)]
    public required string Password { get; init; }
    [Required, DataType(DataType.Password), Compare(nameof(Password))]
    public required string RepeatPassword { get; init; }
    [Required,Phone, DataType(DataType.PhoneNumber)]
    public required string PhoneNumber { get; init; }
}