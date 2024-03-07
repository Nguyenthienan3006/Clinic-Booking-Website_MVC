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
		public IEnumerable<(int IdUser, string fullName, string address, string departmentName, string diseaseName, string userImage, string departmentImage, int reviewCount)> GetDoctorsInfoWithDisease()
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
					departmentImage = dd.Department.Image, // Thay đổi tên trường thành departmentImage
					DepartmentId = dd.Department.DepartmentId
				})
				.ToList();

			var doctorReviewsCount = _context.PatientReviewsDoctors
				.GroupBy(pr => pr.IdDoctor)
				.Select(g => new
				{
					IdDoctor = g.Key,
					ReviewCount = g.Count()
				})
				.ToList();

			var doctorInfoWithDisease = doctorInfo.Select(dd => new
			{
				dd.IdUser,
				dd.FullName,
				dd.Address,
				dd.User_Image,
				dd.DepartmentName,
				dd.departmentImage, // Thay đổi tên trường thành departmentImage
				DiseaseName = _context.Diseases.FirstOrDefault(d => d.IdDepartment == dd.DepartmentId)?.DiseaseName,
				ReviewCount = doctorReviewsCount.FirstOrDefault(dr => dr.IdDoctor == dd.IdUser)?.ReviewCount ?? 0
			})
			.ToList();

			return doctorInfoWithDisease.Select(u => (u.IdUser, u.FullName, u.Address, u.DepartmentName, u.DiseaseName, u.User_Image, u.departmentImage, u.ReviewCount)); // Thay đổi tên trường thành departmentImage
		}
		public IEnumerable<(int IdUser, string fullName, string address, string departmentName, string diseaseName, string userImage, string departmentImage, int reviewCount)> SearchDoctors(string doctorName, string departmentName)
		{
			var query = _context.Accounts
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
					dd.Department.Image,
					DepartmentId = dd.Department.DepartmentId
				});

			if (!string.IsNullOrWhiteSpace(doctorName))
			{
				query = query.Where(dd => dd.FullName.Contains(doctorName));
			}

			if (!string.IsNullOrWhiteSpace(departmentName))
			{
				query = query.Where(dd => dd.DepartmentName == departmentName);
			}

			var doctorInfoWithReviewCount = query
				.Select(dd => new
				{
					dd.IdUser,
					dd.FullName,
					dd.Address,
					dd.User_Image,
					dd.DepartmentName,
					dd.Image,
					DepartmentId = dd.DepartmentId,
					DiseaseName = _context.Diseases.FirstOrDefault(d => d.IdDepartment == dd.DepartmentId) != null ? _context.Diseases.FirstOrDefault(d => d.IdDepartment == dd.DepartmentId).DiseaseName : null,
					ReviewCount = _context.PatientReviewsDoctors.Count(pr => pr.IdDoctor == dd.IdUser)
				})
				.ToList();

			return doctorInfoWithReviewCount
				.Select(u => (
					u.IdUser,
					u.FullName,
					u.Address,
					u.DepartmentName,
					u.DiseaseName,
					u.User_Image,
					u.Image,
					u.ReviewCount));
		}

		public IEnumerable<(int IdUser, string fullName, string address, string departmentName, string diseaseName, string userImage, string departmentImage, int reviewCount)> GetDoctorProfileById(int userId)
		{
			var doctorInfo = _context.Accounts
				.Where(u => u.RollNavigation.RollName == "Doctor" && u.IdUser == userId)
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
					dd.Department.Image, // Adding DepartmentImage field to Select
					DepartmentId = dd.Department.DepartmentId
				})
				.ToList();

			var doctorProfileList = doctorInfo.Select(dd => new
			{
				dd.IdUser,
				dd.FullName,
				dd.Address,
				dd.User_Image,
				dd.DepartmentName,
				dd.Image,
				DiseaseName = _context.Diseases.FirstOrDefault(d => d.IdDepartment == dd.DepartmentId)?.DiseaseName,
				ReviewCount = _context.PatientReviewsDoctors.Count(pr => pr.IdDoctor == dd.IdUser)
			})
			.ToList();

			return doctorProfileList.Select(u => (u.IdUser, u.FullName, u.Address, u.DepartmentName, u.DiseaseName, u.User_Image, u.Image, u.ReviewCount));
		}




	}
}