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
                if (user.Roll == 1)
                {
                    HttpContext.Session.SetString("UserRoleName", "Admin");
                    return RedirectToAction("Index", "AdminHome");
                }
                else if (user.Roll == 2)
                {
                    HttpContext.Session.SetString("UserRoleName", "Doctor");
                    return RedirectToAction("Index", "DoctorHome");
                }
                else if (user.Roll == 3)
                {
                    HttpContext.Session.SetString("UserRoleName", "Patient");
                    return RedirectToAction("Index", "PatientHome");
                }

            }
            else
            {
                // Người dùng không tồn tại hoặc tên đăng nhập/mật khẩu không đúng
                // Xử lý lỗi tại đây, có thể redirect về trang login hoặc hiển thị thông báo lỗi
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        
    }
}
