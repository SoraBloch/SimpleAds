using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleAd_April18.Data
{
    public class Ad
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Title { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DatePosted { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
    }
}
