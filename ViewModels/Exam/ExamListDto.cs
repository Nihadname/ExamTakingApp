namespace ExamTakingApp.ViewModels.Exam;

public sealed record ExamListDto()
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int DurationMinutes { get; set; }
    public DateTime? CreatedAt { get; set; }
    public int QuestionCount { get; set; }
};