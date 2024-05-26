using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourTrainerApp.Areas.Auth.Models;

public class LoginResponse
{
	public LocalUser User { get; set; }
	public string Token { get; set; }
	public List<string>? Errors { get; set; }

}
