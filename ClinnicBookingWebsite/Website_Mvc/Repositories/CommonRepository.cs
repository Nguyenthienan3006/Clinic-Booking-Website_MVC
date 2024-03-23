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

            if(user != null)
            {
                httpContext.Session.SetString("UserRole", user.Roll.ToString());
                httpContext.Session.SetString("UserName", user.Username);
                httpContext.Session.SetString("UserId", user.IdUser.ToString());

                return user;
            }
            return null;
		}
		
        public bool ChangePass(string username, string newpassword, HttpContext httpContext)
        {
            var userId = httpContext.Session.GetString("UserId");
            var userRole = httpContext.Session.GetString("UserRole");

            var accountToDelete = _context.Accounts.FirstOrDefault(a => a.IdUser == int.Parse(userId));

            if (accountToDelete != null)
            {
                //Xóa account với mk cũ
                _context.Accounts.Remove(accountToDelete);


                //thêm lại account với mk mới
                Account account = new Account();
                account.IdUser = int.Parse(userId);
                account.Username = username;
                account.Password = newpassword;
                account.Roll = int.Parse(userRole);

                _context.Accounts.Add(account);
                _context.SaveChanges();

            }

            return true;
        }

        public bool SignUp(string userName, string createPassword, string firstName, string lastName, string bloodGroup, DateTime birthdate, string mobileNumber,
      string address, string email, string roleSelect)
        {
            Account account = new Account();
            AccountInfo accountInfo = new AccountInfo();

            account.Username = userName;
            account.Password = createPassword;
            account.Roll = int.Parse(roleSelect);


            accountInfo.FirstName = firstName;
            accountInfo.LastName = lastName;
            accountInfo.DateOfBirth = birthdate;
            accountInfo.BloodGroup = bloodGroup;
            accountInfo.Phonenumber = mobileNumber;
            accountInfo.Address = address;
            accountInfo.Email = email;

            _context.Accounts.Add(account);
            _context.AccountInfos.Add(accountInfo);
            _context.SaveChanges();

            return true;
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
                    fullName = $"{dd.AccountInfo.FirstName} {dd.AccountInfo.LastName}",
                    dd.AccountInfo.Address,
                    dd.AccountInfo.User_Image,
                    dd.Department.DepartmentName,
                    departmentImage = dd.Department.Department_Image, // Thay đổi tên trường thành departmentImage
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
                dd.fullName,
                dd.Address,
                dd.User_Image,
                dd.DepartmentName,
                dd.departmentImage, // Thay đổi tên trường thành departmentImage
                DiseaseName = _context.Diseases.FirstOrDefault(d => d.IdDepartment == dd.DepartmentId)?.DiseaseName,
                ReviewCount = doctorReviewsCount.FirstOrDefault(dr => dr.IdDoctor == dd.IdUser)?.ReviewCount ?? 0
            })
            .ToList();

            return doctorInfoWithDisease.Select(u => (u.IdUser, u.fullName, u.Address, u.DepartmentName, u.DiseaseName, u.User_Image, u.departmentImage, u.ReviewCount)); // Thay đổi tên trường thành departmentImage
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
                    FullName = $"{dd.AccountInfo.FirstName} {dd.AccountInfo.LastName}",
                    dd.AccountInfo.Address,
                    dd.AccountInfo.User_Image,
                    dd.Department.DepartmentName,
                    departmentImage = dd.Department.Department_Image,
                    DepartmentId = dd.Department.DepartmentId,
                    AccountInfo = dd.AccountInfo // Thêm trường AccountInfo vào projection
                });

            if (!string.IsNullOrWhiteSpace(doctorName))
            {
                query = query.Where(dd => (dd.AccountInfo.FirstName + " " + dd.AccountInfo.LastName).Contains(doctorName));
            }

            if (!string.IsNullOrWhiteSpace(departmentName))
            {
                query = query.Where(dd => dd.DepartmentName == departmentName);
            }

            var doctorInfoWithReviewCount = query
                .Select(dd => new
                {
                    dd.IdUser,
                    FullName = $"{dd.AccountInfo.FirstName} {dd.AccountInfo.LastName}",
                    dd.Address,
                    dd.User_Image,
                    dd.DepartmentName,
                    dd.departmentImage,
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
                    u.departmentImage,
                    u.ReviewCount));
        }


        public (int IdUser, string fullName, string address, string departmentName, string diseaseName, string userImage, string departmentImage, int reviewCount) GetDoctorProfileById(int userId)
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
                    FullName = $"{dd.AccountInfo.FirstName} {dd.AccountInfo.LastName}",
                    dd.AccountInfo.Address,
                    dd.AccountInfo.User_Image,
                    dd.Department.DepartmentName,
                    departmentImage = dd.Department.Department_Image,
                    DepartmentId = dd.Department.DepartmentId
                })
                .FirstOrDefault();

            if (doctorInfo != null)
            {
                var reviewCount = _context.PatientReviewsDoctors.Count(pr => pr.IdDoctor == doctorInfo.IdUser);
                var diseaseName = _context.Diseases.FirstOrDefault(d => d.IdDepartment == doctorInfo.DepartmentId)?.DiseaseName;

                return (doctorInfo.IdUser, doctorInfo.FullName, doctorInfo.Address, doctorInfo.DepartmentName, diseaseName, doctorInfo.User_Image, doctorInfo.departmentImage, reviewCount);
            }

            return (0, "", "", "", "", "", "", 0); // Trả về giá trị mặc định nếu không tìm thấy thông tin bác sĩ
        }


		public IEnumerable<string> GetDoctorDiseaseNames(int userId)
		{
			var diseaseNames = _context.Departments
				.Where(d => d.DoctorDepartment != null && d.DoctorDepartment.IdUser == userId)
				.SelectMany(d => d.Diseases.Select(di => di.DiseaseName))
				.Distinct();

			return diseaseNames;
		}

        public IEnumerable<(PatientReviewsDoctor review, string reviewerName)> GetDoctorReviews(int doctorId)
        {
            var doctorReviews = _context.PatientReviewsDoctors
                .Where(review => review.IdDoctor == doctorId)
                .ToList();

            var reviewsWithReviewerNames = doctorReviews.Select(review =>
            {
                var reviewerName = "";

                if (review.IdPatient != null)
                {
                    var reviewer = _context.AccountInfos
                        .FirstOrDefault(accountInfo => accountInfo.IdUser == review.IdPatient);

                    if (reviewer != null)
                    {
                        reviewerName = $"{reviewer.FirstName} {reviewer.LastName}";
                    }
                }

                return (review, reviewerName);
            });

            return reviewsWithReviewerNames;
        }
    }
}