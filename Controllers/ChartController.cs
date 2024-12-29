using Microsoft.AspNetCore.Mvc;

namespace MVCDemo.Controllers
{
    public class ChartController : Controller
    {
        [Route("Chart/Index")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("Chart/ApexChart")]
        public IActionResult ApexChart()
        {
            return View();
        }
    }
}
