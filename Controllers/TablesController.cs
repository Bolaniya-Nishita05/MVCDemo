using Microsoft.AspNetCore.Mvc;

namespace MVCDemo.Controllers
{
    public class TablesController : Controller
    {
        [Route("Tables/DataTable")]
        public IActionResult DataTable()
        {
            return View();
        }
    }
}
