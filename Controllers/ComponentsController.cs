using Microsoft.AspNetCore.Mvc;

namespace MVCDemo.Controllers
{
    public class ComponentsController : Controller
    {
        [Route("Components/Alerts")]
        public IActionResult Alerts()
        {
            return View();
        }

        [Route("Components/Accordion")]
        public IActionResult Accordion()
        {
            return View();
        }

        [Route("Components/Progress")]
        public IActionResult Progress()
        {
            return View();
        }
    }
}
