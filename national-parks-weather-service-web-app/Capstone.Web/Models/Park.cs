using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capstone.Web.Models
{
    /// <summary>
    /// Holds all properties and data of a park
    /// </summary>
    public class Park
    {
        public string ParkCode { get; set; }

        public string ParkName { get; set; }

        public string State { get; set; }

        public int Acreage { get; set; }

        public int ElevationInFeet { get; set; }

        public float MilesofTrail { get; set; }

        public int NumberOfCampsites { get; set; }

        public string ClimateType { get; set; }

        public int YearFounded { get; set; }

        public int AnnualVisitorCount { get; set; }

        public string InspirationalQuote { get; set; }

        public string QuoteSource { get; set; }

        public string ParkDescription { get; set; }

        public int EntryFee { get; set; }

        public int NumberOfSpecies { get; set; }
    }
}