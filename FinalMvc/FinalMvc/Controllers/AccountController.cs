using FinalMvc.Helpers.Enums;
using FinalMvc.Models;
using FinalMvc.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;
using FinalMvc.Services.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;


namespace FinalMvc.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;
        private readonly IEmailSenderService _emailSender;

        public AccountController(IEmailSenderService emailSender, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailService = emailService;
            _emailSender = emailSender;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }



        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM request)
        {
            if (!ModelState.IsValid)
            {

                return View();
            }
            AppUser user = new()
            {
                FullName = request.FullName,
                Email = request.Email,
                UserName = request.Username,
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);

                }

                return View();
            }

            await _userManager.AddToRoleAsync(user, Roles.SuperAdmin.ToString());

            string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            string url = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, token }, Request.Scheme, Request.Host.ToString());

            string subject = "Rergister confirm email";

            string html = string.Empty;

            using (StreamReader reader = new("wwwroot/templates/verification.html"))
            {
                html = reader.ReadToEnd();
            }

            html = html.Replace("{{confirm-link}}", url);


            _emailService.Send(user.Email, subject, html);

            return RedirectToAction(nameof(VerifyEmail));

        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);

            await _userManager.ConfirmEmailAsync(user, token);

            await _signInManager.SignInAsync(user, false);
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public IActionResult VerifyEmail()
        {
            return View();
        }




        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM request)
        {
            if (!ModelState.IsValid) return View();

            var existUser = await _userManager.FindByEmailAsync(request.UsernameOrEmail);

            if (existUser is null)
            {
                existUser = await _userManager.FindByNameAsync(request.UsernameOrEmail);
            }

            if (existUser is null)
            {
                ModelState.AddModelError(string.Empty, "User is not registered");
                return View();
            }


            var result = await _signInManager.PasswordSignInAsync(existUser, request.Password, false, false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Wrong Email/Username or Password");
                return View();
            }


            return RedirectToAction("Index", "Home");

        }



        //[HttpGet]
        //public async Task<IActionResult> CreateRoles()
        //{
        //    foreach (var role in Enum.GetValues(typeof(Roles)))
        //    {
        //        if (!await _roleManager.RoleExistsAsync(nameof(role)))
        //        {
        //            await _roleManager.CreateAsync(new IdentityRole { Name = role.ToString() });
        //        }
        //    }
        //    return Ok();
        //}


        //[Route("Account/CreateRoles")]
        //[HttpGet]
        //public async Task<IActionResult> CreateRoles()
        //{
        //    foreach (Roles role in Enum.GetValues(typeof(Roles)))
        //    {
        //        if (!await _roleManager.RoleExistsAsync(role.ToString()))
        //        {
        //            await _roleManager.CreateAsync(new IdentityRole { Name = role.ToString() });
        //        }
        //    }
        //    return Ok("Roles created successfully.");
        //}



        //ChangePassword



        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login");
            }

            // Store email and username in TempData
            TempData["Email"] = user.Email;
            TempData["Username"] = user.UserName;

            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM model)
        {
            // Validate if CurrentPassword is provided
            if (string.IsNullOrEmpty(model.CurrentPassword))
            {
                ModelState.AddModelError("CurrentPassword", "Current password is required.");
            }

            // Validate if NewPassword is provided
            if (string.IsNullOrEmpty(model.NewPassword))
            {
                ModelState.AddModelError("NewPassword", "New password is required.");
            }

            // Validate if ConfirmPassword is provided
            if (string.IsNullOrEmpty(model.ConfirmPassword))
            {
                ModelState.AddModelError("ConfirmPassword", "Please confirm your new password.");
            }

            // Check if NewPassword and ConfirmPassword match
            if (!string.IsNullOrEmpty(model.NewPassword) && !string.IsNullOrEmpty(model.ConfirmPassword)
                && model.NewPassword != model.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "New password and confirmation password do not match.");
            }

            // If ModelState has errors, redisplay the form
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Get the current user
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            // Validate if CurrentPassword is correct
            var isCurrentPasswordValid = await _userManager.CheckPasswordAsync(user, model.CurrentPassword);
            if (!isCurrentPasswordValid)
            {
                ModelState.AddModelError("CurrentPassword", "The current password is incorrect.");
                return View(model);
            }

            // Attempt to change the password
            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            // Re-sign the user to refresh security context
            await _signInManager.SignInAsync(user, isPersistent: false);

            // Set success message and redirect
            TempData["SuccessMessage"] = "Your password has been updated successfully!";
            return RedirectToAction("ChangePassword");
        }



        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ChangePassword(ChangePasswordVM model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(); // If model state is invalid, redisplay the form
        //    }

        //    var user = await _userManager.GetUserAsync(User);

        //    if (user == null)
        //    {
        //        return RedirectToAction("Login");
        //    }

        //    var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

        //    if (!result.Succeeded)
        //    {
        //        foreach (var error in result.Errors)
        //        {
        //            ModelState.AddModelError(string.Empty, error.Description);
        //        }

        //        return View();
        //    }
        //    await _signInManager.SignInAsync(user, isPersistent: false);

        //    TempData["SuccessMessage"] = "Your password has been updated successfully!";
        //    return RedirectToAction("ChangePassword");
        //}



        //ForgotPassword

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        // POST: ForgotPassword
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Find user by email
            var user = await _userManager.FindByEmailAsync(model.Email);

            // Redirect to confirmation regardless of whether the user exists or is confirmed
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                return RedirectToAction("ForgotPasswordConfirmation");
            }

            // Generate reset token
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            // Create reset link
            var resetLink = Url.Action(
                "ResetPassword",
                "Account",
                new { token, email = model.Email },
                Request.Scheme);

            // Send email
            try
            {
                // Load and customize the HTML template
                string html = await System.IO.File.ReadAllTextAsync(
                    Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/templates/ForgotPassword.html"));
                html = html.Replace("{{reset-link}}", resetLink);

                // Send the email using the email sender
                await _emailSender.SendEmailAsync(model.Email, "Reset Your Password", html);
            }
            catch (Exception ex)
            {
                // Optionally log the exception for debugging purposes
                Console.WriteLine($"Error sending email: {ex.Message}");
            }

            // Redirect to confirmation page
            return RedirectToAction("ForgotPasswordConfirmation");
        }


        // GET: ForgotPasswordConfirmation
        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }








        //reset
        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
            {
                return BadRequest("Invalid password reset token or email.");
            }

            var model = new ResetPasswordVM
            {
                Token = token,
                Email = email
            };

            return View(); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM model)
        {
            // Check if all required fields are provided
            if (string.IsNullOrEmpty(model.Password))
            {
                TempData["ErrorMessage"] = "Password is required.";
                return View(model);
            }

            if (model.Password.Length < 6)
            {
                TempData["ErrorMessage"] = "Password must be at least 6 characters long.";
                return View(model);
            }

            if (!model.Password.Any(char.IsDigit))
            {
                TempData["ErrorMessage"] = "Password must contain at least one digit.";
                return View(model);
            }

            if (!model.Password.Any(char.IsUpper))
            {
                TempData["ErrorMessage"] = "Password must contain at least one uppercase letter.";
                return View(model);
            }

            if (!model.Password.Any(char.IsSymbol) && !model.Password.Any(char.IsPunctuation))
            {
                TempData["ErrorMessage"] = "Password must contain at least one special character.";
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Invalid email or reset token.";
                return View(model);
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
          


            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }


            return RedirectToAction("ResetPasswordConfirmation");
        }


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ResetPassword(ResetPasswordVM model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }

        //    var user = await _userManager.FindByEmailAsync(model.Email);
        //    if (user == null)
        //    {
        //        // Don't reveal that the user does not exist
        //        return RedirectToAction("ResetPasswordConfirmation");
        //    }

        //    var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
        //    if (result.Succeeded)
        //    {
        //        return RedirectToAction("ResetPasswordConfirmation");
        //    }

        //    foreach (var error in result.Errors)
        //    {
        //        ModelState.AddModelError(string.Empty, error.Description);
        //    }

        //    //return View(model);

        //    return RedirectToAction("ResetPasswordConfirmation");

        //}



        // GET: ResetPasswordConfirmation
        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }


    }
}







