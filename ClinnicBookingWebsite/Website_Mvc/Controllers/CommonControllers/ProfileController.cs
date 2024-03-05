using Microsoft.AspNetCore.Mvc;

namespace Website_Mvc.Controllers.CommonControllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
