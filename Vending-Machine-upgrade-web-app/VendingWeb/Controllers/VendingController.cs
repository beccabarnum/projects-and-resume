using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VendingService.Helpers;
using VendingService.Interfaces;
using VendingService.Models;
using VendingWeb;

namespace VendingWeb.Controllers
{
    public class VendingController : Controller
    {
        private IVendingService _db;
        private ILogService _log;

        private const string InvMgrKey = "InventoryManagerKey";
        private const string RepMgrKey = "ReportManagerKey";
        private const string TransMgrKey = "TransactionManagerKey";

        public VendingController(IVendingService db, ILogService log)
        {
            _db = db;
            _log = log;
        }
        
        [HttpGet]
        public ActionResult Index()
        {
            
            return View();
        }

        [HttpGet]
        public ActionResult Log()
        {
            

            return View();
        }

        [HttpGet]
        public ActionResult Report()
        {
            

            return View();
        }
        [HttpGet]
        public ActionResult About()
        {
            return View();
        }

        [HttpGet]
        [Route("api/inventory")]
        public ActionResult GetVendingItems()
        {
            var inv = GetInvMgr();
            InventoryViewModel model = new InventoryViewModel(inv.RowCount, inv.ColCount);
            for (int c = 1; c <= inv.ColCount; c++)
            {
                for (int r = 1; r <= inv.RowCount; r++)
                {
                    model.AddItem(inv.GetVendingItem(r, c));
                }
            }

            return Json(model.Inventory, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("api/balance")]
        public ActionResult GetVendingBalance()
        {
            var transMgr = GetTransMgr();
            
            return Json(transMgr.RunningTotal, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Route("api/feedmoney")]
        public ActionResult FeedMoney(double amount)
        {
            StatusViewModel result = null;
            var transMgr = GetTransMgr();
            try
            {
                transMgr.AddFeedMoneyOperation(amount);
                result = new StatusViewModel(eStatus.Success);
            }
            catch(Exception ex)
            {
                result = new StatusViewModel(eStatus.Error, ex.Message);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Route("api/purchase")]
        public ActionResult Purchase(int row, int col)
        {
            StatusViewModel result = null;
            var transMgr = GetTransMgr();
            var invMgr = GetInvMgr();
            try
            {
                if(invMgr.IsSlotEmpty(row,col))
                {
                    throw new Exception("Product is sold out.");
                }
                var productId = invMgr.GetVendingItem(row, col).Product.Id;
                transMgr.AddPurchaseTransaction(productId);
                result = new StatusViewModel(eStatus.Success);
            }
            catch (Exception ex)
            {
                result = new StatusViewModel(eStatus.Error, ex.Message);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Route("api/change")]
        public ActionResult MakeChange()
        {
            Change change = null;
            StatusViewModel status = null;

            var transMgr = GetTransMgr();
            try
            {
                change = transMgr.AddGiveChangeOperation();
                transMgr.CommitTransaction();
                status = new StatusViewModel(eStatus.Success);
            }
            catch (Exception ex)
            {
                status = new StatusViewModel(eStatus.Error, ex.Message);
            }

            return Json(new ChangeViewModel(change, status), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("api/report")]
        public ActionResult GetReport(int? year)
        {
            if(year == null)
            {
                year = DateTime.Now.Year;
            }
            var reportMgr = GetReportMgr();
            var report = reportMgr.GetReport((int)year, _db.GetProductItems());

            return Json(report, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("api/log")]
        public ActionResult GetLog(DateTime? startDate, DateTime? endDate)
        {
            IList<VendingOperation> result = null;
            if(startDate != null && endDate != null)
            {
                result = _log.GetLogData((DateTime) startDate, (DateTime) endDate);
            }
            else
            {
                result = _log.GetLogData();
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the inventory manager object from the session
        /// </summary>
        /// <returns>InventoryManager</returns>
        private InventoryManager GetInvMgr()
        {
            InventoryManager inv = Session[InvMgrKey] as InventoryManager;

            if(inv == null)
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
        private ReportManager GetReportMgr()
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
        private TransactionManager GetTransMgr()
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