using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Website_Mvc.Repositories;

namespace Website_Mvc.Controllers.PatientControllers
{
    public class BookingController : Controller
    {
        private readonly PatientRepository _repository;

        public BookingController(PatientRepository repository)
        {
            _repository = repository;
        }
        public IActionResult Index()
        {
            int doctorId = 5;
            List<String> listDiseases = _repository.getDiseases(doctorId);
            ViewBag.ListDiseases = new SelectList(listDiseases);
            return View();
        }

        public IActionResult CheckOut(string datetime)
        {
            var patientId = HttpContext.Session.GetString("UserId");
            
            //var patientInfo = _repository.getPatientInfo(int.Parse(patientId));


            return View();
        }
    }
}
