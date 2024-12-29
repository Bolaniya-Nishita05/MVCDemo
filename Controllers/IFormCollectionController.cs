using Microsoft.AspNetCore.Mvc;

namespace MVCDemo.Controllers
{
    public class IFormCollectionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Submit(IFormCollection fc)
        {
            ViewBag.StudentName = fc["StudentName"];
            ViewBag.Age = fc["Age"];
            return View("Index");
        }
    }
}
