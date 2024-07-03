﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YourTrainer_App.Areas.Trainer.Services;
using YourTrainer_App.Services.APIServices.IServices;
using YourTrainerApp.Models;

namespace YourTrainerApp.Areas.Trainer.Controllers;

[Area("Trainer")]
public class DataSettingsController : Controller
{
	private readonly ITrainerDataService _trainerDataService;
    private readonly ITrainerDataSettingsService _trainerDataSettingsService;

    private int _trainerId
    {
        get => int.Parse(HttpContext.Session.GetString("UserId"));
    }


	public DataSettingsController(ITrainerDataService trainerDataService, ITrainerDataSettingsService trainerDataSettingsService)
    {
        _trainerDataService = trainerDataService;
        _trainerDataSettingsService = trainerDataSettingsService;
    }

    [HttpGet]
    public async Task<IActionResult> ShowData()
    {
        if (await _trainerDataSettingsService.TrainerDataIsPresent(_trainerId))
        {
            return View(await _trainerDataSettingsService.GetTrainerDataFromDb(_trainerId));
		}

        return View(_trainerDataSettingsService.GetTrainerDataDefault(_trainerId, HttpContext.Session.GetString("Username")));
    }

    [HttpPost]
    public async Task<IActionResult> ShowData(TrainerDataModel trainerData)
    {
        if (ModelState.IsValid) 
        {
		    if (await _trainerDataSettingsService.TrainerDataIsPresent(_trainerId))
            {
                await _trainerDataSettingsService.UpdateTrainerData(trainerData);
            }
            else
            {
			    await _trainerDataSettingsService.CreateTrainerData(trainerData);
		    }

			TempData["success"] = "Zapisano zmiany";
			return RedirectToAction("ShowData");
		}

        return View(trainerData);
    }

    public async Task<IActionResult> ClearData()
    {
        await _trainerDataSettingsService.ClearTrainerData(_trainerId);
        return RedirectToAction("ShowData");
    }
}
