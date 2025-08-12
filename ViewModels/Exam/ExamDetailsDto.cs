using ExamTakingApp.Models;

namespace ExamTakingApp.ViewModels.Exam;

public record ExamDetailsDto
{
    public Guid Id { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
    public int DurationMinutes { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
    public List<QuestionListDto> Questions { get; init; } = new List<QuestionListDto>();
}

public class QuestionListDto
{
    public Guid Id { get; init; }
    public string Text { get; init; }
    public QuestionType QuestionType { get; init; }
    public List<AnswerOptionDto> AnswerOptions { get; init; } = new List<AnswerOptionDto>();
}

public class AnswerOptionDto
{
    public Guid Id { get; init; }
    public string Text { get; init; }
    public bool IsCorrect { get; init; }
}