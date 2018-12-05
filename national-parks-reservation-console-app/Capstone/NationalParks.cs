using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone
{
    public class NationalParks
    {
        private DAL BLDAL { get; }
        public Park Park { get; private set; }
        public Dictionary<int, Park> Parks { get;}
        public Dictionary<int, Campground> CGs { get; set; }
        public int LongestCampGroundName { get;  private set; }
        public DateTime ReservationFrom { get; private set; }
        public DateTime ReservationTo { get; private set; }
        public Campground Campground { get; set; }
        public decimal ReservationCost
        {
            get
            {
                TimeSpan span = ReservationTo.Subtract(ReservationFrom);
                decimal numDays = (decimal)span.TotalDays;
                decimal totalCost = Campground.DailyFee * numDays;
                return Campground.DailyFee * numDays;
            }
        }

        public NationalParks(string connString)
        {
            BLDAL = new DAL(connString);
            Parks = DictOfParks();
        }

        #region Methods
        public void SelectPark(int input)
        {
            Park = Parks[input];
        }

        //public List<Site> GetSites(Campground)
        //{

        //}

        public void SetCampGround(int input)
        {
            Campground = CGs[input];
        }

        public void SetReservation(string fromDate, string toDate)
        {
            DateTime fromDateDT = Convert.ToDateTime(fromDate);
            DateTime toDateDT = Convert.ToDateTime(toDate);
            TimeSpan span = toDateDT.Subtract(fromDateDT);
            if (span.TotalDays > 0)
            {
                ReservationFrom = Convert.ToDateTime(fromDate);
                ReservationTo = Convert.ToDateTime(toDate);
            }
            else
            {
                throw new NegativeTimeSpanException();
            }
           

        }
        #endregion




        #region DAL Methods

        public Dictionary<int, Park> DictOfParks()
        {
            List<Park> parksList = BLDAL.GetAllParks();
            Dictionary<int, Park> parksDict = new Dictionary<int, Park>();

            for (int i = 0; i < parksList.Count; i++)
            {
                parksDict[i+1] = parksList[i];
            }

            return parksDict;
        }

        public Dictionary<int, Campground> DictOfCampGrounds(int parkId)
        {
            List<Campground> cgList = BLDAL.GetCampGroundsByParkID(parkId);
            Dictionary<int, Campground> cgDict = new Dictionary<int, Campground>();
            int longest = 0;

            for (int i = 0; i < cgList.Count; i++)
            {
                if(cgList[i].Name.Length > longest)
                {
                    longest = cgList[i].Name.Length;
                }
                cgDict[i + 1] = cgList[i];
            }
            LongestCampGroundName = longest;
            CGs = cgDict;
            return cgDict;
        }

        public Dictionary<int, Site> GetSiteByDate(int cgID, DateTime fromDate, DateTime toDate)
        {
            Dictionary<int, Site> sitesDict = new Dictionary<int, Site>();
            List<Site> siteList = BLDAL.GetSitesByDates(cgID, fromDate, toDate);
            foreach (var site in siteList)
            {
                sitesDict[site.SiteNum] = site;
            }
            return sitesDict;
        }
        
        
       public int MakeReservation(int siteId, string name, DateTime fromDate, DateTime toDate)
        {
            int reservationId = BLDAL.MakeReservation(siteId, name, fromDate, toDate);

            return reservationId;
        }
        #endregion
    }
}
