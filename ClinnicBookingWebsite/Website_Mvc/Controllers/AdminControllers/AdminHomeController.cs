using Microsoft.AspNetCore.Mvc;

namespace Website_Mvc.Controllers
{
	public class AdminHomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
