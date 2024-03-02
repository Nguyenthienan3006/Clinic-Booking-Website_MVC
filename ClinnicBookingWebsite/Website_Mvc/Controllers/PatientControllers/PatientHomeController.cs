using Microsoft.AspNetCore.Mvc;

namespace Website_Mvc.Controllers.PatientControllers
{
	public class PatientHomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
