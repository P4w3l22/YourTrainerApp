using System.Net;
using DbDataAccess.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YourTrainer_Utility;
using YourTrainerApp.Areas.Admin.Models;
using YourTrainerApp.Services.IServices;
using YourTrainerApp2.Models;
using LoginRequest = YourTrainerApp.Areas.Auth.Models.LoginRequest;
using RegisterationRequest = YourTrainerApp.Areas.Auth.Models.RegisterationRequest;

namespace YourTrainerApp.Areas.Auth.Controllers;

[Area("Auth")]
public class AuthController : Controller
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpGet]
    public IActionResult Login()
    {
        LoginRequest loginRequest = new();
        return View(loginRequest);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginRequest loginRequest)
    {
        var apiResponse = await _authService.LoginAsync<APIResponse>(loginRequest);
        if (apiResponse is not null && apiResponse.IsSuccess)
        {
            LoginResponse loginResponse = JsonConvert.DeserializeObject<LoginResponse>(Convert.ToString(apiResponse.Result));
            HttpContext.Session.SetString(StaticDetails.SessionToken, loginResponse.Token);
            TempData["success"] = "Zalogowano!";
            return RedirectToAction("Index", "Home", new { area = "Visitor" });
        }
        ModelState.AddModelError("CustomError", apiResponse.Errors.FirstOrDefault());
        return View(loginRequest);
    }

    [HttpGet]
    public IActionResult Register()
    {
        RegisterationRequestDTO registerRequest = new();
		return View(registerRequest);
	}

	[HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterationRequestDTO registerRequest)
    {
        if (registerRequest.ConfirmPassword != registerRequest.RegisterationRequest.Password)
        {
            ModelState.AddModelError("CustomError", "Hasła nie są identyczne");
            return View(registerRequest);
        }
        
        var apiResponse = await _authService.RegisterAsync<APIResponse>(registerRequest.RegisterationRequest);
        if (apiResponse is not null && apiResponse.IsSuccess)
        {
            TempData["success"] = "Zarejestrowano!";

            LoginResponse loginResponse = JsonConvert.DeserializeObject<LoginResponse>(Convert.ToString(apiResponse.Result));
            HttpContext.Session.SetString(StaticDetails.SessionToken, loginResponse.Token);

            return RedirectToAction("Index", "Home", new { area = "Visitor" });
        }
        
        ModelState.AddModelError("CustomError", apiResponse.Errors.FirstOrDefault());

        return View(registerRequest);
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        HttpContext.Session.SetString(StaticDetails.SessionToken, string.Empty);
        TempData["success"] = "Wylogowano!";
        return RedirectToAction("Index", "Home", new { area = "Visitor" });
    }

    public IActionResult AccessDenied() =>
        View();
}