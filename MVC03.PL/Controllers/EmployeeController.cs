using Microsoft.AspNetCore.Mvc;

namespace MVC03.PL.Controllers
{
    public class EmployeeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
