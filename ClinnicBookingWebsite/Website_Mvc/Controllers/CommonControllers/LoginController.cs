using Microsoft.AspNetCore.Mvc;
using Website_Mvc.Models;

namespace Website_Mvc.Controllers
{
	public class LoginController : Controller
	{
		private readonly ClinicBookingProjectContext _context;

		public LoginController(ClinicBookingProjectContext context) 
		{
			_context = context;
		}

		[HttpGet]
		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Index(string username, string password)
		{
			if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
			{
				// Xử lý lỗi nếu tên đăng nhập hoặc mật khẩu trống
				return RedirectToAction("Index");
			}

			// Kiểm tra xem có người dùng có tên đăng nhập và mật khẩu tương ứng không
			var user = _context.Accounts
				.FirstOrDefault(u => u.Username == username && u.Password == password);

			HttpContext.Session.SetString("UserRole", user.Roll.ToString());
			HttpContext.Session.SetString("UserName", user.Username);
			if(user.Roll == 1)
			{
                HttpContext.Session.SetString("UserRoleName", "Admin");
            }else if(user.Roll == 2)
			{
                HttpContext.Session.SetString("UserRoleName", "Doctor");
            }else if (user.Roll == 3)
			{
                HttpContext.Session.SetString("UserRoleName", "Patient");
            }

			if (user != null)
			{
				if(user.Roll == 1)
				{
					return RedirectToAction("Index", "AdminHome");
				}
				else if(user.Roll == 2)
				{
					return RedirectToAction("Index", "DoctorHome");
				}
				else if(user.Roll == 3) 
				{
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
