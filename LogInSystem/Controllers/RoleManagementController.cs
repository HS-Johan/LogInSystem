using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LogInSystem.Controllers
{
    public class RoleManagementController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public RoleManagementController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [Authorize(Roles ="Super Admin")]
        public IActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                ModelState.AddModelError("", "Role Name Not Given");
                return View();
            }

            var roleExitsts = await _roleManager.RoleExistsAsync(roleName);
            if (roleExitsts)
            {
                ModelState.AddModelError("", "Role Already Exitsts");
                return View();
            }

            var result = await _roleManager.CreateAsync(new IdentityRole(roleName));

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Role Created Successfully";
                return RedirectToAction(nameof(CreateRole));
            }

            return View();
        }

        [Authorize(Roles = "Super Admin")]
        public IActionResult AssignRole()
        {
            ViewBag.User = _userManager.Users.ToList();
            ViewBag.role = _roleManager.Roles.ToList();

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AssignRole(string userId, string roleName)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(roleName))
            {
                return RedirectToAction(nameof(AssignRole));
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return RedirectToAction(nameof(AssignRole));
            }

            var roleExitsts = await _roleManager.RoleExistsAsync(roleName);
            if( !roleExitsts )
            {
                return RedirectToAction(nameof(AssignRole));
            }

            var result = await _userManager.AddToRoleAsync(user,roleName);
            if(result.Succeeded)
            {
                TempData["SuccessMessage"] = "Role Assigned to User Successfully";
                return RedirectToAction(nameof(AssignRole));
            }

            return RedirectToAction(nameof(AssignRole));
        }
    }
}
