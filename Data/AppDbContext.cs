using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data;

public sealed class AppDbContext:IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }
    public DbSet<Question> Questions { get; set; }
    public DbSet<AnswerOption> AnswerOptions { get; set; }
    public DbSet<Exam> Exams { get; set; }
    
}