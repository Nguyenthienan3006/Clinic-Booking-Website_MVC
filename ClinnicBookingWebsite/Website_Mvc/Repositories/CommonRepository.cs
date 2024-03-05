using Microsoft.AspNetCore.Http;
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

            if (user != null) 
            {
                httpContext.Session.SetString("UserRole", user.Roll.ToString());
                httpContext.Session.SetString("UserName", user.Username);
                httpContext.Session.SetString("UserId", user.IdUser.ToString());


                return user;
            }


            return null;
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

        public bool ChangePass(string username, string newpassword, HttpContext httpContext)
        {
            var userId = httpContext.Session.GetString("UserId");
            var userRole = httpContext.Session.GetString("UserRole");

            var accountToDelete = _context.Accounts.FirstOrDefault(a => a.IdUser == int.Parse(userId));

            if(accountToDelete != null)
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
    }
}
