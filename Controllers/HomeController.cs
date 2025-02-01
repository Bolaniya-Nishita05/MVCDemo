using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MVCDemo.Models;
using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace MVCDemo.Controllers
{
    public class CheckAccess : ActionFilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            if (filterContext.HttpContext.Session.GetString("UserID") == null)
            {
                filterContext.Result = new RedirectResult("~/User/Login");
            }
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            context.HttpContext.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            context.HttpContext.Response.Headers["Expires"] = "-1";
            context.HttpContext.Response.Headers["Pragma"] = "no-cache";
            base.OnResultExecuting(context);
        }
    }

    [CheckAccess]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public static List<object> countries = new List<object>
            {
                new { name = "United States", code = "US" },
                new { name = "Canada", code = "CA" },
                new { name = "India", code = "IND" }
            };

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Route("")]
        [Route("Home")]
        [Route("Home/Index")]

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult DemoTable()
        {
            return View();
        }

        public IActionResult AjaxDemo()
        {
            var countries = GetCountries();
            return View(countries);
        }

        [HttpGet]
        public JsonResult GetCountries()
        {
            return Json(countries);
        }

        [HttpPost]
        public JsonResult AddCountry(string name, string code)
        {
            countries.Add(new { name = name, code = code });
            return Json(new { success = true });
        }


        [HttpDelete]
        public JsonResult DeleteCountry(string name)
        {
            // Deletion logic here
            return Json(new { success = true });
        }


        public IActionResult CreateCookie()
        {
            string key = "Nishita";
            string value = "Hello Cookie";
            CookieOptions co = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(5)
            };
            Response.Cookies.Append(key,value,co);

            return View("Index");
        }

        public IActionResult ReadCookie()
        {
            string key = "Nishita";
            var ans = Request.Cookies[key];
            return View("Index");
        }

        public IActionResult RemoveCookie()
        {
            string key = "Nishita";
            string value = String.Empty;
            CookieOptions co = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(-1)
            };
            Response.Cookies.Append(key, value, co);

            return View("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
