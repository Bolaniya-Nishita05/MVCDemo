using Microsoft.AspNetCore.Mvc;

namespace MVCDemo.Controllers
{
    public class FormController : Controller
    {
        [Route("Form/FormElements")]
        public IActionResult FormElements()
        {
            return View();
        }

        [Route("Form/FormEditors")]
        public IActionResult FormEditors()
        {
            return View();
        }
    }
}
