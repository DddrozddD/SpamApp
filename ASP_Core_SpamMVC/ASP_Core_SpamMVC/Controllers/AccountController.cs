using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Domain.Models;
using ASP_Core_SpamMVC.Models;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace ASP_Core_SpamMVC.Controllers
{
   
        public class AccountController : Controller
        {
            private readonly UserManager<Client> userManager;

            private readonly SignInManager<Client> signInManager;
            private readonly IEmailSender _emailSender;
            private readonly RoleManager<IdentityRole> _roleManager;

            public AccountController(UserManager<Client> userManager, SignInManager<Client> signInManager, IEmailSender emailSender, RoleManager<IdentityRole> roleManager)
            {
                this.userManager = userManager;
                this.signInManager = signInManager;
                this._emailSender = emailSender;
                this._roleManager = roleManager;
            }

            [HttpGet]
            public IActionResult Login()
            {
                return View();
            }

            [HttpPost]
            public async Task<IActionResult> Login(LoginViewModel loginViewModel)
            {
            var tmpClient = await userManager.FindByEmailAsync(loginViewModel.Email);
                if(tmpClient != null)
            {
                var res = await signInManager.PasswordSignInAsync(tmpClient.UserName, loginViewModel.Password, true, false);
                if (res.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                
                return RedirectToAction("Verification", "Home");

            }
                return View(loginViewModel);
            }

            [HttpGet]
            public IActionResult Register()
            {
                return View();
            }

            [HttpPost]
            public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
            {
            var user = new Client()
            {
                Email = registerViewModel.Email,
                UserName = registerViewModel.Login
            };

            var res = await userManager.CreateAsync(user, registerViewModel.Password);
            if (res.Succeeded)
            {
                if (await _roleManager.FindByNameAsync("user") == null)
                {
                    var role = await _roleManager.CreateAsync(new IdentityRole("user"));
                    if (role.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, "user");
                    }
                }
                else
                {
                    await userManager.AddToRoleAsync(user, "user");
                }
                var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = Url.Action("", "confirmation", new { guid = token, userEmail = user.Email }, Request.Scheme, Request.Host.Value);
                await _emailSender.SendEmailAsync(user.Email, "Confirmation Link", $"Link=> {confirmationLink}");

                return RedirectToAction("Verification", "Home");
            }

            return RedirectToAction("Error", "Home"); ;
        }

            [HttpGet]
            public IActionResult Logout()
            {
                signInManager.SignOutAsync();
                return RedirectToAction("Index", "Home");
            }
        }
}
