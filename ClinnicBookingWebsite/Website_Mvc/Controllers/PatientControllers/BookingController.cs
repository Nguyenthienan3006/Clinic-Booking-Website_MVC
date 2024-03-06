using Microsoft.AspNetCore.Mvc;

namespace Website_Mvc.Controllers.PatientControllers
{
    public class BookingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CheckOut(string datetime)
        {
            return View();
        }
    }
}
