using System.ComponentModel.DataAnnotations;
using WebApplication1.Models;

namespace WebApplication1.ViewModels.Exam;

public sealed record QuestionCreateDto()
{
    [Required, StringLength(500)]
    public string Text { get; set; } = null!;

    [Required]
    public Guid ExamId { get; set; }

    [Required]
    public QuestionType QuestionType { get; set; }
    public List<AnswerOptionCreateDto> AnswerOptions { get; set; } = new List<AnswerOptionCreateDto>();


};
public sealed class AnswerOptionCreateDto
{
    [Required, StringLength(200)]
    public string Text { get; set; } = null!;

    public bool IsCorrect { get; set; }
}