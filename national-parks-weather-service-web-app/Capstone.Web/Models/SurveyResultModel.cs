using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capstone.Web.Models
{
    /// <summary>
    /// Holds properties for use in the survey result view
    /// </summary>
    public class SurveyResultModel
    {
        public string ParkName { get; set; }
        public string ParkCode { get; set; }
        public int SurveyCount{ get; set; }
        public string ParkDesc { get; set; }
    }
}