namespace ExamTakingApp.Models.Common;

public abstract class BaseEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedTime {  get; set; } 
    
}