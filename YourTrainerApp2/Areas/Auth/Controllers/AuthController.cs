using System.Net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Protocol.Plugins;
using YourTrainer_Utility;
using YourTrainerApp.Areas.Admin.Models;
using YourTrainerApp.Areas.Auth.Models;
using YourTrainerApp.Models;
using YourTrainerApp.Services.IServices;
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
            SetSessionStrings(loginResponse.Token, loginResponse.User.UserName);

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
        if (registerRequest.RegisterationRequest.Password != registerRequest.ConfirmPassword)
        {
            ModelState.AddModelError("CustomError", "Hasła nie są identyczne");
            return View(registerRequest);
        }
        
        var apiResponse = await _authService.RegisterAsync<APIResponse>(registerRequest.RegisterationRequest);
        if (apiResponse is not null && apiResponse.IsSuccess)
        {
            TempData["success"] = "Zarejestrowano!";

            LoginResponse loginResponse = JsonConvert.DeserializeObject<LoginResponse>(Convert.ToString(apiResponse.Result));
            SetSessionStrings(loginResponse.Token, loginResponse.User.UserName);

			return RedirectToAction("Index", "Home", new { area = "Visitor" });
        }
        
        ModelState.AddModelError("CustomError", apiResponse.Errors.FirstOrDefault());

        return View(registerRequest);
    }

    private void SetSessionStrings(string sessionToken, string userName)
    {
        HttpContext.Session.SetString(StaticDetails.SessionToken, sessionToken);
        HttpContext.Session.SetString("Username", userName);
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        SetSessionStrings(string.Empty, string.Empty);

        TempData["success"] = "Wylogowano!";
        return RedirectToAction("Index", "Home", new { area = "Visitor" });
    }

    public IActionResult AccessDenied() =>
        View();
}