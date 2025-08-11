using System.ComponentModel.DataAnnotations;
using WebApplication1.Models.Common;

namespace WebApplication1.Models;

public sealed class AnswerOption:BaseEntity
{
    public Guid QuestionId { get; set; }
    public Question Question { get; set; } = null!;
    [Required, StringLength(200)]
    public string Text { get; set; } = null!;
    public bool IsCorrect { get; set; }
}