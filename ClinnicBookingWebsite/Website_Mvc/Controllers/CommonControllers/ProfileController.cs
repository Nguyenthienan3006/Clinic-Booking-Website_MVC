using Microsoft.AspNetCore.Mvc;
using Website_Mvc.Models;

namespace Website_Mvc.Controllers.CommonControllers
{
    public class ProfileController : Controller
    {
		private readonly ClinicBookingProjectContext context;
		public ProfileController()
		{
			context = new ClinicBookingProjectContext();
		}

		public IActionResult Index()
		{
			var session = HttpContext.Session;
			string valueUser = session.GetString("UserId");
			var account = context.AccountInfos.Where(x => x.IdUser == int.Parse(valueUser)).FirstOrDefault();
			return View(account);
		}

		[HttpGet]

		public IActionResult Edit(string firstname, string lastname, string dob, string bloodGroup, string email, string phone, string address)
		{


			var session = HttpContext.Session;
			string valueUser = session.GetString("UserId");
			var acc = context.AccountInfos.Where(a => a.IdUser == int.Parse(valueUser)).FirstOrDefault();
			acc.FirstName = firstname;
			acc.LastName = lastname;
			acc.DateOfBirth = DateTime.Parse(dob);
			acc.BloodGroup = bloodGroup;
			acc.Email = email;
			acc.Phonenumber = phone;
			acc.Address = address;
			context.Update(acc);
			context.SaveChanges();
			return RedirectToAction("Index");

		}

	}
}
