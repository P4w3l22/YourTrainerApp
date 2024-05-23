using Microsoft.AspNetCore.Mvc.Rendering;
using YourTrainerApp.Areas.Auth.Models;

namespace YourTrainerApp.Areas.Admin.Models;

public class RegisterationRequestDTO
{
    public RegisterationRequest RegisterationRequest { get; set; }

    public string ConfirmPassword { get; set; }
    
    public IEnumerable<SelectListItem> RoleOptionsList = new List<SelectListItem>()
    {
        new SelectListItem
        {
            Text = "Trener",
            Value = "trainer"
        },
        new SelectListItem
        {
            Text = "Trenujący",
            Value = "gym member"
        }
    };
}