using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleAd_April18.Data;
using SimpleAds_April18.Models;
using System.Diagnostics;

namespace SimpleAds_April18.Controllers
{
    public class HomeController : Controller
    {
        private string _connectionString = @"Data Source=.\sqlexpress;Initial Catalog=SimpleAds;Integrated Security=true;";
        public IActionResult Index()
        {
            var repo = new SimpleAdRepository(_connectionString);
            var vm = new HomePageViewModel();
            vm.Ads = repo.GetAllAds();
            vm.IsLoggedIn = User.Identity.IsAuthenticated;
            var isLoggedIn = User.Identity.IsAuthenticated;
            if (isLoggedIn)
            {
                var currentUserEmail = User.Identity.Name;
                vm.User = repo.GetByEmail(currentUserEmail);
            }
            else
            {
                vm.User = null;
            }
            return View(vm);
        }
        [Authorize]
        public IActionResult MyAccount()
        {
            var repo = new SimpleAdRepository(_connectionString);
            var currentUserEmail = User.Identity.Name;
            User user = repo.GetByEmail(currentUserEmail);
            var vm = new MyAccountViewModel();
            vm.Ads = repo.GetAdsForUser(user.Id);
            return View(vm);
        }
        [HttpPost]
        public IActionResult DeleteAd(int id)
        {
            var repo = new SimpleAdRepository(_connectionString);
            repo.DeleteAd(id);
            return Redirect("/home/index");
        }
        public IActionResult NewAd()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("login", "account");
            }
            return View();
        }
        [HttpPost]
        public IActionResult NewAd(Ad ad)
        {
            var repo = new SimpleAdRepository(_connectionString);
            var currentUserEmail = User.Identity.Name;
            User currentUser = repo.GetByEmail(currentUserEmail);
            ad.UserId = currentUser.Id;
            ad.DatePosted = DateTime.Now;
            repo.AddAd(ad);
            return Redirect("/home/index");
        }
    }
}