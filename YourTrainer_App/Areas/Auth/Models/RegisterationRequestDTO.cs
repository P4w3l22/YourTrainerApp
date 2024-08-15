using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using YourTrainer_App.Areas.Auth.Models;

namespace YourTrainer_App.Areas.Admin.Models;

public class RegisterationRequestDTO
{
    public RegisterationRequest RegisterationRequest { get; set; }

	[Required(ErrorMessage = "Pole potwierdzenia hasła jest wymagane")]
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