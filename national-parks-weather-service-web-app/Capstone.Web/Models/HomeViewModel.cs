using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capstone.Web.Models
{
    /// <summary>
    /// Holds list of parks for use in the home view
    /// </summary>
    public class HomeViewModel
    {
        public List<Park> Parks { get; set; }
    }
}