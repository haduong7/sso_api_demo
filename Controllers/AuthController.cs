using SSO_Api.Data;
using SSO_Api.Models;
using SSO_Api.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.HttpResults;

[ApiController]
[Route("auth")]
public class AuthController(SignInManager<AppUser> signInManager, AppDbContext db, IConfiguration configuration) : ControllerBase
{
    //[HttpPost("token")]
    //public async Task<IActionResult> Token([FromBody] LoginVM loginmodel)
    //{

    //    //var user = await userManager.FindByEmailAsync(loginmodel.Username!);

    //    if(string.IsNullOrWhiteSpace(loginmodel.Username) || string.IsNullOrWhiteSpace(loginmodel.Password))
    //        return Unauthorized("Enter username or password");

    //    loginmodel.Username = loginmodel.Username.Trim().ToLower();

    //    var user = db.Users.AsNoTracking().FirstOrDefault(q=>q.UserName == loginmodel.Username);
    //    if (user == null)
    //        return Unauthorized("User not found");

    //    var result = await signInManager.CheckPasswordSignInAsync(user, loginmodel.Password!, lockoutOnFailure: false);
    //    if (!result.Succeeded)
    //        return Unauthorized("Invalid password");

    //    //var claims = new[]
    //    //{
    //    //    new Claim(ClaimTypes.NameIdentifier, user.Id),
    //    //    new Claim(ClaimTypes.Name, user.UserName!)
    //    //};

    //    var token = await GenTokenAsync(user);

    //    return Ok(new
    //    {
    //        token = new JwtSecurityTokenHandler().WriteToken(token)
    //    });
    //}

    //private async Task<JwtSecurityToken> GenTokenAsync(AppUser user)
    //{
    //    var principal = await signInManager.CreateUserPrincipalAsync(user);

    //    var secretKey = configuration["Jwt:Key"] ?? "KSAJEF83URIWUHAE834JFKIQSEJFBAKN";
    //    var issuer = configuration["Jwt:Issuer"] ?? "your-sso-server";
    //    var audience = configuration["Jwt:Audience"] ?? "your-client-apps";

    //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
    //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


    //    var token = new JwtSecurityToken(
    //        issuer: issuer,
    //        audience: audience,
    //        claims: principal.Claims,
    //        expires: DateTime.UtcNow.AddHours(8),
    //        signingCredentials: creds);
    //    return token;
    //}

    //[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //[HttpGet("profile")]
    //public IActionResult GetProfile()
    //{
    //    var username = User.Identity?.Name;

    //    var user = db.Users.AsNoTracking().FirstOrDefault(q => q.UserName == username);
    //    if (user == null)
    //        return NotFound("User not found");

    //    return Ok(new { user });
    //}

    //[HttpPost("login")]
    //public async Task<IActionResult> Login(LoginVM loginmodel, string? returnUrl = null)
    //{
    //    if (string.IsNullOrWhiteSpace(loginmodel.Username) || string.IsNullOrWhiteSpace(loginmodel.Password))
    //        return Unauthorized("Enter username or password");

    //    loginmodel.Username = loginmodel.Username.Trim().ToLower();

    //    var user = db.Users.AsNoTracking().FirstOrDefault(q => q.UserName == loginmodel.Username);
    //    if (user == null)
    //        return Unauthorized("User not found");

    //    var result = await signInManager.CheckPasswordSignInAsync(user, loginmodel.Password!, lockoutOnFailure: false);
    //    if (!result.Succeeded)
    //        return Unauthorized("Invalid password");

    //    //var identity = new ClaimsIdentity("Cookies");
    //    //identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName!));
    //    //var principal = new ClaimsPrincipal(identity);

    //    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);


    //    var principal = await signInManager.CreateUserPrincipalAsync(user!);

    //    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

    //    return Ok(user);
    //}

    //[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    //[HttpGet("getToken")]
    //public async Task<IActionResult> GetTokenAsync()
    //{
    //    var username = User.Identity?.Name;

    //    var user = db.Users.AsNoTracking().FirstOrDefault(q => q.UserName == username);
    //    if (user == null)
    //        return NotFound("User not found");

    //    var token = await GenTokenAsync(user);

    //    return Ok(new
    //    {
    //        token = new JwtSecurityTokenHandler().WriteToken(token)
    //    });
    //}
}
