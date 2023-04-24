using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using SimpleAd_April18.Data;
using System.Security.Claims;

namespace SimpleAds_April18.Controllers
{
    public class AccountController : Controller
    {
        private string _connectionString = @"Data Source=.\sqlexpress;Initial Catalog=SimpleAds;Integrated Security=true;";

        public IActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Signup(User user, String password)
        {
            var repo = new SimpleAdRepository(_connectionString);
            repo.AddUser(user, password);
            return RedirectToAction("index", "home");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var repo = new SimpleAdRepository(_connectionString);
            var user = repo.Login(email, password);

            if (user == null)
            {
                TempData["message"] = "Invalid Login";
                return Redirect("/account/login");
            }
            var claims = new List<Claim>
            {
                new Claim("user", email) 
            };

            HttpContext.SignInAsync(new ClaimsPrincipal(
                new ClaimsIdentity(claims, "Cookies", "user", "role")))
                .Wait();

            return Redirect("/home/index");
        }
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync().Wait();
            return Redirect("/home/index");
        }
    }
}
