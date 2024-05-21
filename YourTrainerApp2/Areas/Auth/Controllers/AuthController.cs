using System.Net;
using DbDataAccess.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YourTrainer_Utility;
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
            return RedirectToAction("Index", "Home");
        }
        ModelState.AddModelError("CustomError", apiResponse.Errors.FirstOrDefault());
        return View(loginRequest);
    }

    [HttpGet]
    public IActionResult Register() =>
        View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterationRequest registerRequest)
    {
        var apiResponse = await _authService.RegisterAsync<APIResponse>(registerRequest);
        if (apiResponse is not null && apiResponse.IsSuccess)
        {
            return RedirectToAction("Login");
        }

        return View(registerRequest);
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        HttpContext.Session.SetString(StaticDetails.SessionToken, string.Empty);
        return RedirectToAction("Index", "Home");
    }

    public IActionResult AccessDenied() =>
        View();
}