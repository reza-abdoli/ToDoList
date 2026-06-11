using System.Security.Claims;
using Application.Enum;
using Application.Interface;
using Data.Dto;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;


public class UserController : Controller
{
    private readonly IUserService _userService;


    public UserController(IUserService userService)
    {
        _userService = userService;
    }


    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(SignupLoginDto dto)
    {
        if (!ModelState.IsValid) return View(dto);

        var user = await _userService.Login(dto);
        if (user == null)
        {
            ModelState.AddModelError("", "Invalid credentials.");
            return View(dto);
        }

        // Claims میسازیم و توی Cookie ذخیره میکنیم
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        return RedirectToAction("Items", "ToDoList");
    }

    [HttpGet]
    public IActionResult Signup() => View();

    [HttpPost]
    public async Task<IActionResult> Signup(SignupLoginDto dto)
    {
        Console.WriteLine(dto.Name.Length);
        if (!ModelState.IsValid) return View(dto);

        var result = await _userService.Signup(dto);
        if (result == ServiceResult.AlreadyExists)
        {
            //general error
            ModelState.AddModelError("", "User already exists.");
            // ModelState.AddModelError("Name", "Username is already taken."); // specific error for Name field
            // model error/all error
            return View(dto);
        }

        return RedirectToAction("Login");
    }
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }

}
