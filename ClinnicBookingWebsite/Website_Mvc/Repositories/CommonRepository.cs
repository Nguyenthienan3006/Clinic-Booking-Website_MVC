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
    }
}
