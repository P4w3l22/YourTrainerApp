using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using YourTrainer_App.Services.APIServices.IServices;
using YourTrainer_Utility;
using YourTrainerApp.Areas.Admin.Models;
using YourTrainerApp.Areas.Auth.Models;
using YourTrainerApp.Models;
using LoginRequest = YourTrainerApp.Areas.Auth.Models.LoginRequest;

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
		APIResponse apiResponse = await _authService.LoginAsync<APIResponse>(loginRequest);
        if (apiResponse is not null)
        {
            if (apiResponse.IsSuccess)
            {
				await SignInSession(apiResponse.Result.ToString());
				TempData["success"] = "Zalogowano!";

				return RedirectToAction("Index", "Home", new { area = "Visitor" });
			}
			else if (apiResponse.Errors is not null && apiResponse.Errors.Count > 0)
			{
				ModelState.AddModelError("CustomError", apiResponse.Errors.FirstOrDefault());
			}
		}
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
        // BŁĄD - REJESTRACJA NIE DZIAŁA
        if (ModelState.IsValid)
        {
            if (registerRequest.RegisterationRequest.Password != registerRequest.ConfirmPassword)
            {
                ModelState.AddModelError("CustomError", "Hasła nie są identyczne");
                return View(registerRequest);
            }

            var apiResponse = await _authService.RegisterAsync<APIResponse>(registerRequest.RegisterationRequest);
            if (apiResponse is not null)
            {
                if (apiResponse.IsSuccess)
                {
                    TempData["success"] = "Zarejestrowano!";
                    await SignInSession(apiResponse.Result.ToString());

                    return RedirectToAction("Index", "Home", new { area = "Visitor" });
                }
                else if (apiResponse.Errors is not null && apiResponse.Errors.Count > 0)
                {
                    ModelState.AddModelError("CustomError", apiResponse.Errors.FirstOrDefault());
                }
            }
        }
        
		return View(registerRequest);
    }

    private async Task SignInSession(string? apiResponseResult)
    {
        if (apiResponseResult is not null)
        {
            LoginResponse? loginResponse = JsonConvert.DeserializeObject<LoginResponse>(Convert.ToString(apiResponseResult));

            ClaimsIdentity identity = new(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Name, loginResponse.User.UserName));
            identity.AddClaim(new Claim(ClaimTypes.Role, loginResponse.User.Role));
            ClaimsPrincipal principal = new(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            SetSessionTokenAndUsername(loginResponse.Token, loginResponse.User.UserName);
            HttpContext.Session.SetString("UserId", loginResponse.User.Id.ToString());
        }
	}

    private void SetSessionTokenAndUsername(string sessionToken, string userName)
    {
        HttpContext.Session.SetString(StaticDetails.SessionToken, sessionToken);
        HttpContext.Session.SetString("Username", userName);
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
		SetSessionTokenAndUsername(string.Empty, string.Empty);

        TempData["success"] = "Wylogowano!";
        return RedirectToAction("Index", "Home", new { area = "Visitor" });
    }

    public IActionResult AccessDenied()
    {
        return View();
    }
}