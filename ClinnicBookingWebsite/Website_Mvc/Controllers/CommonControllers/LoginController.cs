using Microsoft.AspNetCore.Mvc;
using Website_Mvc.Models;
using Website_Mvc.Repositories;

namespace Website_Mvc.Controllers
{
	public class LoginController : Controller
	{
		private readonly CommonRepository _repository;

		public LoginController(CommonRepository repository) 
		{
            _repository = repository;
		}

		[HttpGet]
		public IActionResult Index()
		{
			return View();
		}

        [HttpPost]
        public async Task<IActionResult> Index(string username, string password)
        {
            var user = _repository.Decentralization(username, password, HttpContext);
            
            if (user != null)
            {
                if(user.AccountStatus == 1)
                {
					if (user.Roll == 1)
					{
						HttpContext.Session.SetString("UserRoleName", "Admin");
						TempData["LoginSuccessMessage"] = "Login Successful!";
						return RedirectToAction("Index", "AdminHome");
					}
					else if (user.Roll == 2)
					{
						HttpContext.Session.SetString("UserRoleName", "Doctor");
						TempData["LoginSuccessMessage"] = "Login Successful!";
						return RedirectToAction("Index", "DoctorHome");
					}
					else if (user.Roll == 3)
					{
						HttpContext.Session.SetString("UserRoleName", "Patient");
						TempData["LoginSuccessMessage"] = "Login Successful!";
						return RedirectToAction("Index", "PatientHome");
					}
				}
				else
				{
					TempData["LoginFailMessage"] = "Account Was Banned!";
					return RedirectToAction("Index");
				}

            }
            else
            {
				TempData["LoginFailMessage"] = "Account Not Exists!";
				return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        
    }
}
