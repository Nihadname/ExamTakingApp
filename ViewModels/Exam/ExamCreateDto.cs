using System.ComponentModel.DataAnnotations;

namespace ExamTakingApp.ViewModels.Exam;

public sealed record ExamCreateDto
{
    [Required, StringLength(200)]
    public string Title { get; init; } = null!;

    [Required, StringLength(400)]
    public string Description { get; init; } = null!;

    [Range(1, 300)]
    public int DurationMinutes { get; init; }
}