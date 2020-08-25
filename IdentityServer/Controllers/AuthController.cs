using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AuthController(UserManager<IdentityUser> userManager
            ,SignInManager<IdentityUser> signInManager,RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.Error = "";
            return View(new LoginViewModel {ReturnUrl= returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.UserName, model.Password, isPersistent: false, false);
                if (result.Succeeded)
                {
                    return Redirect(model.ReturnUrl);
                }
            }
            ViewBag.Error = "Some thing going wrong!";
            return View();
        }



        [HttpGet]
        public IActionResult Register()
        {
            ViewBag.Error = "";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.UserName };
                var result = await userManager.CreateAsync(user, password: model.Password);
                if (result.Succeeded)
                {
                    var addRoleResult = await roleManager.CreateAsync(new IdentityRole { Name = model.RoleName });
                    if (addRoleResult.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, model.RoleName);

                        return Redirect("/Auth/Login");
                    }


                }
            }
            ViewBag.Error = "Some thing going wrong!";
            return View();
        }

    }
}
