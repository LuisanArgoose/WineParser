using Microsoft.AspNetCore.Mvc;

namespace WineParser.Controllers
{
    public class ParserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
