using Microsoft.AspNetCore.Mvc;

namespace MVCDemo.Controllers
{
    public class ProfileController : Controller
    {
        [Route("Profile/Index")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
