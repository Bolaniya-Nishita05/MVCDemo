using Microsoft.AspNetCore.Mvc;

namespace MVCDemo.Controllers
{
    public class StudentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
