using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Website_Mvc.Repositories;

namespace Website_Mvc.Controllers.CommonControllers
{
    public class ChangePassController : Controller
    {
        private readonly CommonRepository _repository;

        public ChangePassController(CommonRepository repository)
        {
            _repository = repository;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string username, string newpassword)
        {
            bool checkChangePass = _repository.ChangePass(username, newpassword, HttpContext);
            return View();
        }

    }
}
