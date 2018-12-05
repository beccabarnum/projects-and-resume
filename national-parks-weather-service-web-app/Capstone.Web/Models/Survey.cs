using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Capstone.Web.Models
{
    /// <summary>
    /// Holds properties of a survey that is checked through validation
    /// </summary>
    public class Survey
    {
        [Required]
        public string ParkCode { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string State { get; set; }

        [Required(ErrorMessage = "Please select an activity level")]
        public string ActivityLevel { get; set; }
    }
}