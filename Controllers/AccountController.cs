using SSO_Api.Models;
using SSO_Api.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using SSO_Api.Data;
using Microsoft.EntityFrameworkCore;

namespace SSO_Api.Controllers;

public class AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, AppDbContext db) : Controller
{
    //public IActionResult Login(string? returnUrl = null)
    //{
    //    ViewData["ReturnUrl"] = returnUrl;
    //    return View();
    //}

    //[HttpPost]
    //public async Task<IActionResult> Login(LoginVM model, string? returnUrl = null)
    //{
    //    ViewData["ReturnUrl"] = returnUrl;
    //    if (ModelState.IsValid)
    //    {
    //        //login
    //        var result = await signInManager.PasswordSignInAsync(model.Username!, model.Password!, model.RememberMe, false);

    //        if (result.Succeeded)
    //        {
    //            var user = db.Users.AsNoTracking().FirstOrDefault(q => q.UserName == model.Username);
    //            //var identity = new ClaimsIdentity("Cookies");
    //            //identity.AddClaim(new Claim(ClaimTypes.Name, user!.UserName!));
    //            //var principal = new ClaimsPrincipal(identity);

    //            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

    //            var principal = await signInManager.CreateUserPrincipalAsync(user!);

    //            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

    //            return RedirectToLocal(returnUrl);
    //        }

    //        ModelState.AddModelError("", "Invalid login attempt");
    //    }
    //    return View(model);
    //}

    //public IActionResult Register(string? returnUrl = null)
    //{
    //    ViewData["ReturnUrl"] = returnUrl;
    //    return View();
    //}

    //[HttpPost]
    //public async Task<IActionResult> Register(RegisterVM model, string? returnUrl = null)
    //{
    //    ViewData["ReturnUrl"] = returnUrl;
    //    if (ModelState.IsValid)
    //    {
    //        AppUser user = new()
    //        {
    //            Name = model.Name,
    //            UserName = model.Username,
    //            Email = model.Email,
    //            Address = model.Address
    //        };

    //        var result = await userManager.CreateAsync(user, model.Password!);

    //        if (result.Succeeded)
    //        {
    //            await signInManager.SignInAsync(user, false);

    //            return RedirectToLocal(returnUrl);
    //        }
    //        foreach (var error in result.Errors)
    //        {
    //            ModelState.AddModelError("", error.Description);
    //        }
    //    }
    //    return View(model);
    //}

    //public async Task<IActionResult> Logout()
    //{
    //    await signInManager.SignOutAsync();
    //    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    //    return RedirectToAction("Index","Home");
    //}

    //private IActionResult RedirectToLocal(string? returnUrl)
    //{
    //    return !string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)
    //        ? Redirect(returnUrl)
    //        : RedirectToAction(nameof(HomeController.Index), nameof(HomeController));
    //}
}
