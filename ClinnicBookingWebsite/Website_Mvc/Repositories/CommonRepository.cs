using System.Linq;
using Microsoft.EntityFrameworkCore;
using Website_Mvc.Models;


namespace Website_Mvc.Repositories
{
	public class CommonRepository
	{
		private readonly ClinicBookingProjectContext _context;

		public CommonRepository(ClinicBookingProjectContext context)
		{
			_context = context;
		}


		public Account Decentralization(string username, string password, HttpContext httpContext)
		{
			if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
			{
				// Xử lý lỗi nếu tên đăng nhập hoặc mật khẩu trống
				return null;
			}

			// Kiểm tra xem có người dùng có tên đăng nhập và mật khẩu tương ứng không
			var user = _context.Accounts
				.FirstOrDefault(u => u.Username == username && u.Password == password);
			httpContext.Session.SetString("UserRole", user.Roll.ToString());
			httpContext.Session.SetString("UserName", user.Username);

			return user;
		}
		public IEnumerable<(int IdUser, string fullName, string address, string departmentName, string diseaseName, string userImage, string departmentImage)> GetDoctorsInfoWithDisease()
		{
			var doctorInfo = _context.Accounts
				.Where(u => u.RollNavigation.RollName == "Doctor")
				.Join(_context.AccountInfos, acc => acc.IdUser, accInfo => accInfo.IdUser,
					(acc, accInfo) => new { Account = acc, AccountInfo = accInfo })
				.Join(_context.DoctorDepartments, acc => acc.Account.IdUser, dd => dd.IdUser,
					(acc, dd) => new { Account = acc.Account, AccountInfo = acc.AccountInfo, DoctorDepartment = dd })
				.Join(_context.Departments, dd => dd.DoctorDepartment.DepartmentId, d => d.DepartmentId,
					(dd, d) => new { Account = dd.Account, AccountInfo = dd.AccountInfo, Department = d })
				.Select(dd => new
				{
					dd.AccountInfo.IdUser,
					dd.AccountInfo.FullName,
					dd.AccountInfo.Address,
					dd.AccountInfo.User_Image,
					dd.Department.DepartmentName,
					dd.Department.Image, // Thêm trường DepartmentImage vào Select
					DepartmentId = dd.Department.DepartmentId
				})
				.ToList();

			var doctorInfoWithDisease = doctorInfo.Select(dd => new
			{
				dd.IdUser,
				dd.FullName,
				dd.Address,
				dd.User_Image,
				dd.DepartmentName,
				dd.Image,
				DiseaseName = _context.Diseases.FirstOrDefault(d => d.IdDepartment == dd.DepartmentId)?.DiseaseName
			})
			.ToList();

			return doctorInfoWithDisease.Select(u => (u.IdUser, u.FullName, u.Address, u.DepartmentName, u.DiseaseName, u.User_Image, u.Image));
		}



	}
}