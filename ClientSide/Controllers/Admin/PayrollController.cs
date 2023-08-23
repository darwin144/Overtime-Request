using Microsoft.AspNetCore.Mvc;

namespace ClientSide.Controllers.Admin
{
    public class PayrollController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
