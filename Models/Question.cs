using ExamTakingApp.Models.Common;

namespace ExamTakingApp.Models;

public sealed class Question:BaseEntity
{
    public Guid ExamId { get; set; }
    public Exam Exam { get; set; } = null!;
    public required string ImageUrl { get; set; } 
    public QuestionType QuestionType { get; set; } 
    public ICollection<AnswerOption> AnswerOptions { get; set; } = new List<AnswerOption>();

}
public enum QuestionType
{
    MultipleChoice,
    TrueFalse,
}
