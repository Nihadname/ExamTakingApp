using System.ComponentModel.DataAnnotations;

namespace ExamTakingApp.ViewModels.Exam;

public sealed class ExamEditDto
{
    public Guid Id { get; set; }
    [Required, StringLength(200)]
    public required string Title { get; set; }
    [Required, StringLength(200)]
    public required string Description { get; set; }
    public int DurationMinutes { get; set; }
}