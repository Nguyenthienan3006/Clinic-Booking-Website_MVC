using Microsoft.AspNetCore.Http;
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
        public IActionResult Index(int id)
        {
            var session = HttpContext.Session;
            List<String> listDiseases = _repository.getDiseases(id);
            var DoctorInfo = _repository.GetDoctorById(id);

            session.SetString("DoctorId", DoctorInfo.IdUser.ToString());
            session.SetString("DoctorName", DoctorInfo.FullName);
            session.SetString("DoctorAddress", DoctorInfo.Address);

            if (ViewBag.ListDiseases == null)
            {
                ViewBag.ListDiseases = new SelectList(listDiseases);
            }

            if (DoctorInfo != null)
            {
                ViewBag.DoctorInfo = DoctorInfo;
            }

            return View();
        }
        [HttpGet]
        public IActionResult CheckOut(string date, string time, string selectedDisease, string description)
        {
            var session = HttpContext.Session;

            //lấy giá của bệnh
            var disease = _repository.getDiseasesPrice(selectedDisease);

            if(disease != null)
            {
                session.SetString("BookingDate", date);
                session.SetString("BookingTime", time);
                session.SetString("BookingDisease", selectedDisease);
                session.SetString("DiseasePrice", disease.Price.ToString());
                session.SetString("BookingDescription", description);

                return View();
            }
            return RedirectToAction("Index", "Booking");
        }

        [HttpPost]
        public IActionResult CheckOut()
        {
            _repository.AddAppointment(HttpContext);

            return RedirectToAction("BookingSuccess", "Booking");

        }


        public IActionResult BookingSuccess()
        {
            return View();
        }
    }
}
