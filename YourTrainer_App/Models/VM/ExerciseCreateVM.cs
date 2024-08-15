using Microsoft.AspNetCore.Mvc.Rendering;
using YourTrainer_App.Models;

namespace YourTrainer_App.Models.VM;

public class ExerciseCreateVM
{
    public Exercise Exercise { get; set; }
    public IEnumerable<SelectListItem> ForceOptionsList = new List<SelectListItem>()
    {
        new SelectListItem
        {
            Text = "Push",
            Value = "push"
        },
        new SelectListItem
        {
            Text = "Pull",
            Value = "pull",
        },
        new SelectListItem
        {
            Text = "Statyczne",
            Value = "static"
        }
    };
    public IEnumerable<SelectListItem> LevelOptionsList = new List<SelectListItem>()
    {
        new SelectListItem
        {
            Text = "Początkujący",
            Value = "beginner"
        },
        new SelectListItem
        {
            Text = "Średniozaawansowany",
            Value = "intermediate",
        },
        new SelectListItem
        {
            Text = "Zaawansowany",
            Value = "expert"
        }
    };
    public IEnumerable<SelectListItem> MechanicOptionsList = new List<SelectListItem>()
    {
        new SelectListItem
        {
            Text = "Brak",
            Value = "none"
        },
        new SelectListItem
        {
            Text = "Całe ciało",
            Value = "compound",
        },
        new SelectListItem
        {
            Text = "Izolacyjne",
            Value = "isolation"
        }
    };
    public IEnumerable<SelectListItem> PrimaryMuscleOptionsList = new List<SelectListItem>()
    {
        new SelectListItem
        {
            Text = "Klatka piersiowa",
            Value = "chest"
        },
        new SelectListItem
        {
            Text = "Plecy",
            Value = "back"
        },
        new SelectListItem
        {
            Text = "Barki",
            Value = "shoulders"
        },
        new SelectListItem
        {
            Text = "Triceps",
            Value = "triceps"
        },
        new SelectListItem
        {
            Text = "Biceps",
            Value = "biceps"
        },
        new SelectListItem
        {
            Text = "Przedramiona",
            Value = "forearms"
        },
        new SelectListItem
        {
            Text = "Nogi",
            Value = "legs"
        },
        new SelectListItem
        {
            Text = "Brzuch",
            Value = "abdominals"
        },
        new SelectListItem
        {
            Text = "Łydki",
            Value = "calves"
        }
    };
    public IEnumerable<SelectListItem> CategoryOptionsList = new List<SelectListItem>()
    {
        new SelectListItem
        {
            Text = "Wydolność",
            Value = "plyometrics"
        },
        new SelectListItem
        {
            Text = "Siła",
            Value = "strength"
        },
        new SelectListItem
        {
            Text = "Rozciąganie",
            Value = "stretching"
        },
        new SelectListItem
        {
            Text = "Strongman",
            Value = "strongman"
        }
    };
}
