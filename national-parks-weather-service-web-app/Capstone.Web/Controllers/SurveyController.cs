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
    /// Set up to interact with Model and Views for the Survey
    /// </summary>
    public class SurveyController : Controller
    {
        /// <summary>
        /// introduces member variable in order to call methods
        /// </summary>
        private IParkServiceDAL _dal;

        /// <summary>
        /// Constructor that sets dal for park service object when Survey Controller is created
        /// </summary>
        public SurveyController(IParkServiceDAL dal)
        {
            _dal = dal;
        }
        /// <summary>
        /// sends information to survey view
        /// </summary>
        /// <returns></returns>
        public ActionResult Survey()
        {
            return View();
        }
        
        /// <summary>
        /// passes results with all surveys to survey result view
        /// </summary>
        /// <returns></returns>
        public ActionResult SurveyResult()
        {
            List<SurveyResultModel> results = new List<SurveyResultModel>();
            results = _dal.GetAllSurveys();
            return View("SurveyResult", results);
        }

        /// <summary>
        /// takes information from survey and sends it to dal to go to database
        /// </summary>
        /// <param name="survey"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Post(Survey survey)
        {
            if (!ModelState.IsValid)
            {
                return View("Survey");
            }
            else
            {
                _dal.SaveNewSurvey(survey);
            }
            
            return RedirectToAction("SurveyResult");
        }
    }
}