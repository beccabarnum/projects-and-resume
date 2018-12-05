using Capstone.Web.DAL;
using Capstone.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Capstone.Web.Controllers
{
    /// <summary>
    /// Set up to interact with Model and Views for the Details
    /// </summary>
    public class DetailController : Controller
    {
        /// <summary>
        /// introduces member variable in order to call methods
        /// </summary>
        private IParkServiceDAL _dal;

        /// <summary>
        /// Constructor that sets dal for park service object when Detail Controller is created
        /// </summary>
        public DetailController(IParkServiceDAL dal)
        {
            _dal = dal;
        }
        /// <summary>
        /// sets model to a single park
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Detail(string id)
        {
            Park model = _dal.GetSinglePark(id);
            Session["parkid"] = id;
            Session["ParkName"] = model.ParkName;
            //get parkCode from model and save in session data to be accessed by Weather action

            return View("Detail", model);
        }
        /// <summary>
        /// sets forecastModel to forecasts based on park ID and changes IsFahrenheit bool property on each weather object
        /// </summary>
        /// <returns></returns>
        public ActionResult Weather()
        {
            List<Weather> forecastModel = new List< Weather>();

            string parkID = Session["parkid"].ToString();
            //get parkCode of selected park from TempData on detail view
            if (Session["Weather"] == null)
            {
                Session["Weather"] = true;
            }
            forecastModel = _dal.GetForecastByParkCode(parkID);
            foreach(Weather item in forecastModel)
            {
                item.IsFahrenheit = Convert.ToBoolean(Session["Weather"]);
            }
            return View("Weather", forecastModel);
        }
        /// <summary>
        /// called to change weather from celsius to fahrenheit or fahrenheit to celsius
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ChangeWeather(string id)
        {
            Session["Weather"] = Convert.ToBoolean(id);
            return RedirectToAction("Weather");
        }
    }
}
