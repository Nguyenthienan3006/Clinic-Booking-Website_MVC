using Microsoft.AspNetCore.Mvc;
using Website_Mvc.Repositories;

namespace Website_Mvc.Controllers.CommonControllers
{
    public class SignUpController : Controller
    {
        private readonly CommonRepository _repository;

        public SignUpController(CommonRepository repository)
        {
            _repository = repository;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string userName, string password, 
            string firstName, string lastName, string bloodGroup, DateTime birthdate, string mobileNumber, 
            string address, string email, string role)
        {
            bool checkSignUp = _repository.SignUp(userName, password, firstName, lastName, bloodGroup, birthdate, mobileNumber, address, email, role);
            return View();
        }
    }
}
