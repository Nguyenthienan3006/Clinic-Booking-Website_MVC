using Microsoft.AspNetCore.Mvc;

namespace Website_Mvc.Controllers.DoctorControllers
{
	public class DoctorHomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
