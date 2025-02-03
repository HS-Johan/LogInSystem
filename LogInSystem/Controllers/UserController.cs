using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LogInSystem.Controllers
{
    [Authorize(Roles = "User")]
    public class UserController : Controller
    {
        public IActionResult User()
        {
            return View();
        }
    }
}
