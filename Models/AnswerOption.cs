using System.ComponentModel.DataAnnotations;
using ExamTakingApp.Models.Common;

namespace ExamTakingApp.Models;

public sealed class AnswerOption:BaseEntity
{
    public Guid QuestionId { get; set; }
    public Question Question { get; set; } = null!;
    [Required, StringLength(200)]
    public string Text { get; set; } = null!;
    public bool IsCorrect { get; set; }
}