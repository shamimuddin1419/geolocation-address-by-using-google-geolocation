using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoogleGeolocationApp.Models
{
    public class vmTask
    {
        public Int64 UserID { get; set; }
        public string BuyerName { get; set; }
        public string Achivement { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Address { get; set; }        
        public string Description { get; set; }
    }
}