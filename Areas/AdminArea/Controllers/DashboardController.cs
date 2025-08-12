using Microsoft.AspNetCore.Mvc;

namespace ExamTakingApp.Areas.AdminArea.Controllers;
[Area("AdminArea")]
public class DashboardController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}