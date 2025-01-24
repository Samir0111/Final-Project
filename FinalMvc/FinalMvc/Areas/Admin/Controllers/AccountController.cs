using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinalMvc.Models;
using FinalMvc.ViewModels.Account;

namespace FinalMvc.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class AccountController : Controller
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }







        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home", new { Area = string.Empty });
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }




        [HttpGet]
        public async Task<IActionResult> AdjustRoles()
        {
            var users = await _userManager.Users.ToListAsync();
            var roles = await _roleManager.Roles.ToListAsync();

            var model = new AdjustRolesVM
            {
                Users = users,
                Roles = roles
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdjustRoles(AdjustRolesVM model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null) return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, userRoles);

            if (!result.Succeeded)
            {
                return View(model);
            }

            if (!string.IsNullOrEmpty(model.SelectedRole))
            {
                result = await _userManager.AddToRoleAsync(user, model.SelectedRole);
                if (!result.Succeeded)
                {
                    return View(model);
                }
            }

            return RedirectToAction(nameof(Index));
        }







    }
}
