using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Areas.AdminArea.Controllers;
[Area("AdminArea")]
public class DashboardController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}