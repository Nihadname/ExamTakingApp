using System.ComponentModel.DataAnnotations;
using WebApplication1.Models.Common;

namespace WebApplication1.Models;

public sealed class Exam:BaseEntity
{
    [Required, StringLength(200)]
    public string Title { get; set; } = null!;
    [Required, StringLength(400)]
    public string Description { get; set; } = null!;
    
    public int DurationMinutes { get; set; } = 30; 
    public ICollection<Question> Questions { get; set; } = new List<Question>();

}