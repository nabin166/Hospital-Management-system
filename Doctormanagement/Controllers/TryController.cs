using Microsoft.AspNetCore.Mvc;

namespace Doctormanagement.Controllers
{
    public class TryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
