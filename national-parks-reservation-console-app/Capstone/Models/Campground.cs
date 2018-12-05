using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone
{
    public class Campground
    {
        public static Dictionary<int, string> monthsDict = new Dictionary<int, string>()
        {
            {1, "January"},
            {2, "February"},
            {3, "March"},
            {4, "April"},
            {5, "May"},
            {6, "June" },
            {7, "July" },
            {8, "August" },
            {9, "September" },
            {10, "October" },
            {11, "November" },
            {12, "December" }
        };
        public int CampgroundID { get; set; }
        public int ParkID { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// Numerical month the campground is open for reservation
        /// </summary>
        public int OpenFromMonth { get; set; }
        /// <summary>
        /// Numerical month the campground is closed for reservation
        /// </summary>
        public int OpenToMonth { get; set; }
        public decimal DailyFee { get; set; }

        
    }
}
