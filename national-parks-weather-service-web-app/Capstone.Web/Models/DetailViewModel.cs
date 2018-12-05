using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capstone.Web.Models
{
    /// <summary>
    /// Holds list of parks for use in the detail view
    /// </summary>
    public class DetailViewModel
    {
        public List<Park> ParkList { get; set; }


    }
}