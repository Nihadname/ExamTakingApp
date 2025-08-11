using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.ViewModels.Exam;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class ExamController : Controller
    {
        private readonly AppDbContext _context;

        public ExamController(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // GET: AdminArea/Exam
        public async Task<IActionResult> Index()
        {
            try
            {
                var exams = await _context.Exams
                    .AsNoTracking()
                    .Select(e => new ExamListDto
                    {
                        Id = e.Id,
                        Title = e.Title,
                        Description = e.Description,
                        DurationMinutes = e.DurationMinutes,
                        CreatedAt = e.CreatedTime,
                        QuestionCount = e.Questions.Count
                    })
                    .OrderBy(e => e.CreatedAt)
                    .ToListAsync();

                return View(exams);
            }
            catch (Exception ex)
            {
                // Log exception (implement proper logging in production)
                TempData["ErrorMessage"] = "An error occurred while fetching exams.";
                return View(new List<ExamListDto>());
            }
        }

        // GET: AdminArea/Exam/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            var exam = await _context.Exams
                .Include(e => e.Questions)
                .ThenInclude(q => q.AnswerOptions)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (exam == null)
            {
                return NotFound();
            }

            var examDetailsDto = new ExamDetailsDto
            {
                Id = exam.Id,
                Title = exam.Title,
                Description = exam.Description,
                DurationMinutes = exam.DurationMinutes,
                CreatedAt = exam.CreatedTime,
                Questions = exam.Questions.Select(q => new QuestionListDto
                {
                    Id = q.Id,
                    Text = q.ImageUrl,
                    QuestionType = q.QuestionType,
                    AnswerOptions = q.AnswerOptions.Select(a => new AnswerOptionDto
                    {
                        Id = a.Id,
                        Text = a.Text,
                        IsCorrect = a.IsCorrect
                    }).ToList()
                }).ToList()
            };

            return View(examDetailsDto);
        }

        // GET: AdminArea/Exam/Create
        public IActionResult Create()
        {
            return View(new ExamCreateDto());
        }

        // POST: AdminArea/Exam/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ExamCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            try
            {
                var exam = new Exam
                {
                    Id = Guid.NewGuid(),
                    Title = dto.Title?.Trim(),
                    Description = dto.Description?.Trim(),
                    DurationMinutes = dto.DurationMinutes,
                    CreatedTime = DateTime.UtcNow
                };

                await _context.Exams.AddAsync(exam);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Exam created successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log exception (implement proper logging in production)
                ModelState.AddModelError("", "An error occurred while creating the exam.");
                return View(dto);
            }
        }

        // GET: AdminArea/Exam/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            var exam = await _context.Exams.FindAsync(id);
            if (exam == null)
            {
                return NotFound();
            }

            var examEditDto = new ExamEditDto
            {
                Id = exam.Id,
                Title = exam.Title,
                Description = exam.Description,
                DurationMinutes = exam.DurationMinutes
            };

            return View(examEditDto);
        }

        // POST: AdminArea/Exam/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ExamEditDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            try
            {
                var exam = await _context.Exams.FindAsync(id);
                if (exam == null)
                {
                    return NotFound();
                }

                exam.Title = dto.Title?.Trim();
                exam.Description = dto.Description?.Trim();
                exam.DurationMinutes = dto.DurationMinutes;

                _context.Update(exam);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Exam updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Exams.AnyAsync(e => e.Id == id))
                {
                    return NotFound();
                }
                throw;
            }
            catch (Exception ex)
            {
                // Log exception (implement proper logging in production)
                ModelState.AddModelError("", "An error occurred while updating the exam.");
                return View(dto);
            }
        }

        // POST: AdminArea/Exam/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var exam = await _context.Exams
                    .Include(e => e.Questions)
                    .ThenInclude(q => q.AnswerOptions)
                    .FirstOrDefaultAsync(e => e.Id == id);

                if (exam == null)
                {
                    TempData["ErrorMessage"] = "Exam not found.";
                    return RedirectToAction(nameof(Index));
                }

                _context.Exams.Remove(exam);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Exam deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log exception (implement proper logging in production)
                TempData["ErrorMessage"] = "An error occurred while deleting the exam.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: AdminArea/Exam/CreateQuestion/5
        public async Task<IActionResult> CreateQuestion(Guid examId)
        {
            var exam = await _context.Exams.FindAsync(examId);
            if (exam == null)
            {
                return NotFound();
            }

            ViewBag.ExamId = examId;
            ViewBag.ExamTitle = exam.Title;
            return View(new QuestionCreateDto { ExamId = examId });
        }

        // POST: AdminArea/Exam/CreateQuestion
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateQuestion(QuestionCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ExamId = dto.ExamId;
                ViewBag.ExamTitle = (await _context.Exams.FindAsync(dto.ExamId))?.Title;
                return View(dto);
            }

            var exam = await _context.Exams.FindAsync(dto.ExamId);
            if (exam == null)
            {
                ModelState.AddModelError("", "Selected exam does not exist.");
                ViewBag.ExamId = dto.ExamId;
                return View(dto);
            }

            try
            {
                var question = new Question
                {
                    Id = Guid.NewGuid(),
                    ImageUrl = dto.Text?.Trim(),
                    ExamId = dto.ExamId,
                    QuestionType = dto.QuestionType,
                    AnswerOptions = dto.AnswerOptions?.Select(o => new AnswerOption
                    {
                        Id = Guid.NewGuid(),
                        Text = o.Text?.Trim(),
                        IsCorrect = o.IsCorrect
                    }).ToList() ?? new List<AnswerOption>(),
                    CreatedTime = DateTime.UtcNow
                };

                // Validate that at least one correct answer exists for multiple choice
                if (!question.AnswerOptions.Any(o => o.IsCorrect))
                {
                    ModelState.AddModelError("", "All questions must have at least one correct answer.");
                    ViewBag.ExamId = dto.ExamId;
                    ViewBag.ExamTitle = exam.Title;
                    return View(dto);
                }

                await _context.Questions.AddAsync(question);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Question created successfully.";
                return RedirectToAction(nameof(Details), new { id = dto.ExamId });
            }
            catch (Exception ex)
            {
                // Log exception (implement proper logging in production)
                ModelState.AddModelError("", "An error occurred while creating the question.");
                ViewBag.ExamId = dto.ExamId;
                ViewBag.ExamTitle = exam.Title;
                return View(dto);
            }
        }
    }
}