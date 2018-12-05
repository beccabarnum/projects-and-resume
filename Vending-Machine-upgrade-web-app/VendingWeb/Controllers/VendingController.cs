using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VendingService.Helpers;
using VendingService.Interfaces;

namespace VendingWeb.Controllers
{
    public class VendingController : VendingBaseController
    {
        private IVendingService _db;
        private ILogService _log;
                
        public VendingController(IVendingService db, ILogService log) : base(db,log)
        {
            _db = db;
            _log = log;
        }
        
        [HttpGet]
        public ActionResult Index()
        {
            return GetAuthenticatedView("Index");
        }

        [HttpGet]
        public ActionResult Log()
        {
            return GetAuthenticatedView("Log");
        }

        [HttpGet]
        public ActionResult Report()
        {
            return GetAuthenticatedView("Report");
        }

        [HttpGet]
        public ActionResult About()
        {
            return GetAuthenticatedView("About");
        }
    }
}