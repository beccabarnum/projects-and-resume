using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VendingService.Helpers;
using VendingService.Interfaces;
using VendingService.Models;

namespace VendingWeb.Controllers
{
    public class VendingBaseController : Controller
    {
        private const string InvMgrKey = "InventoryManagerKey";
        private const string RepMgrKey = "ReportManagerKey";
        private const string TransMgrKey = "TransactionManagerKey";
        public const string UserKey = "UserKey";
        
        private IVendingService _db;
        private ILogService _log;

        public VendingBaseController(IVendingService db, ILogService log)
        {
            _db = db;
            _log = log;
        }

        public ActionResult GetAuthenticatedView(string viewName, object model = null)
        {
            ActionResult result = null;
            if (IsAuthenticated)
            {
                result = View(viewName, model);
            }
            else
            {
                result = RedirectToAction("Login", "User");
            }
            return result;
        }

        public JsonResult GetAuthenticatedJson(JsonResult json)
        {
            JsonResult result = null;
            if (IsAuthenticated)
            {
                result = json;
            }
            else
            {
                result = Json(new { error = "User is not authenticated." }, JsonRequestBehavior.AllowGet);
            }
            return result;
        }

        /// <summary>
        /// Returns bool if user has authenticated in
        /// </summary>        
        public bool IsAuthenticated
        {
            get
            {
                return Session[UserKey] != null;
            }
        }

        /// <summary>
        /// "Logs" the current user in
        /// </summary>
        public void LogUserIn(UserItem user)
        {
            Session[UserKey] = user;
        }

        /// <summary>
        /// "Logs out" a user by removing the cookie.
        /// </summary>
        public void LogUserOut()
        {
            Session[UserKey] = null;
        }

        /// <summary>
        /// Gets the inventory manager object from the session
        /// </summary>
        /// <returns>InventoryManager</returns>
        public InventoryManager GetInvMgr()
        {
            InventoryManager inv = Session[InvMgrKey] as InventoryManager;

            if (inv == null)
            {
                var items = _db.GetVendingItems();
                inv = new InventoryManager(items);
                Session[InvMgrKey] = inv;
            }

            return inv;
        }

        /// <summary>
        /// Gets the report manager object from the session
        /// </summary>
        /// <returns>ReportManager</returns>
        public ReportManager GetReportMgr()
        {
            ReportManager report = Session[RepMgrKey] as ReportManager;

            if (report == null)
            {
                report = new ReportManager(_db);
                Session[RepMgrKey] = report;
            }

            return report;
        }

        /// <summary>
        /// Gets the transaction manager object from the session
        /// </summary>
        /// <returns>TransactionManager</returns>
        public TransactionManager GetTransMgr()
        {
            TransactionManager trans = Session[TransMgrKey] as TransactionManager;

            if (trans == null)
            {
                trans = new TransactionManager(_db, _log);
                Session[TransMgrKey] = trans;
            }

            return trans;
        }
    }
}