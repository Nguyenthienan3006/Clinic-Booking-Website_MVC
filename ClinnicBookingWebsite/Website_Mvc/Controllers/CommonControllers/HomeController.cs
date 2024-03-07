using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Website_Mvc.Models;
using Website_Mvc.Repositories;

namespace Website_Mvc.Controllers
{
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly CommonRepository _commonRepository;

        public HomeController(ILogger<HomeController> logger, CommonRepository commonRepository)
        {
            _commonRepository = commonRepository;
            _logger = logger;
        }

        public IActionResult Index()
        {
			HttpContext.Session.Remove("UserRole");
			HttpContext.Session.Remove("UserRoleName");

			return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult SearchDoctor(string doctorName, string department)
        {
            // Gọi hàm SearchDoctors từ repository để tìm kiếm bác sĩ theo tên
            var doctors = _commonRepository.SearchDoctors(doctorName, department);
            if (!string.IsNullOrEmpty(doctorName))
            {
                ModelState["doctorName"].AttemptedValue = doctorName;
            }
            // Chuyển kết quả tìm kiếm sang view để hiển thị
            return View(doctors);
        }
        public IActionResult GetDoctorList()
        {
            var doctorsInfo = _commonRepository.GetDoctorsInfoWithDisease();
            return View("SearchDoctor", doctorsInfo);
        }
        public IActionResult ViewProfile(int id)
        {
            var doctor = _commonRepository.GetDoctorProfileById(id);

            return View("DoctorProfile", doctor);
        }
    }
}
