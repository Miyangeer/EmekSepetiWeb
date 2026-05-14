using Microsoft.AspNetCore.Mvc;

namespace EmekSepetiWeb.Controllers
{
    public class UserController : Controller
    {
        // Register yerine KayitOl yaptık
        public IActionResult KayitOl()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }
    }
}