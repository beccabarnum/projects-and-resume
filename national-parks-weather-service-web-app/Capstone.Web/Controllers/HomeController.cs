using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Capstone.Web.DAL;
using Capstone.Web.Models;

namespace Capstone.Web.Controllers
{
    /// <summary>
    /// Set up to interact with Model and Views for the Home
    /// </summary>
    public class HomeController : Controller
    {
        
        /// <summary>
        /// introduces member variable in order to call methods
        /// </summary>
         IParkServiceDAL _dal = null;

        /// <summary>
        /// Constructor that sets dal for park service object when Home Controller is created
        /// </summary>
        /// <param name="dal"></param>
        public HomeController(IParkServiceDAL dal)
        {
            _dal = dal;
        }

        /// <summary>
        /// action to take to Index view and passes our HomeViewModel(list of all parks)
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            HomeViewModel ParkList = new HomeViewModel();
            ParkList.Parks = _dal.GetAllParks();

            return View("Index", ParkList);
        }
    }
}