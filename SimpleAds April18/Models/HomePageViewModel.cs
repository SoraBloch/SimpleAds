using SimpleAd_April18.Data;

namespace SimpleAds_April18.Models
{
    public class HomePageViewModel
    {
        public List<Ad> Ads { get; set; }
        public bool IsLoggedIn { get; set; }
        public User User { get; set; }
    }
}
