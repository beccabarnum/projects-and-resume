using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VendingService.Interfaces;
using VendingService.Models;

namespace VendingWeb.Controllers
{
    public class VendingApiController : VendingBaseController
    {
        private IVendingService _db;
        private ILogService _log;

        public VendingApiController(IVendingService db, ILogService log) : base(db, log)
        {
            _db = db;
            _log = log;
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

            var jsonResult = Json(model.Inventory, JsonRequestBehavior.AllowGet);
            return GetAuthenticatedJson(jsonResult);
        }

        [HttpGet]
        [Route("api/balance")]
        public ActionResult GetVendingBalance()
        {
            var transMgr = GetTransMgr();

            var jsonResult = Json(transMgr.RunningTotal, JsonRequestBehavior.AllowGet);
            return GetAuthenticatedJson(jsonResult);
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
            catch (Exception ex)
            {
                result = new StatusViewModel(eStatus.Error, ex.Message);
            }

            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return GetAuthenticatedJson(jsonResult);
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
                invMgr.PurchaseItem(row, col);
                var item = invMgr.GetVendingItem(row, col);
                transMgr.AddPurchaseTransaction(item.Product.Id);
                result = new StatusViewModel(eStatus.Success, item.Category.Noise);
            }
            catch (Exception ex)
            {
                result = new StatusViewModel(eStatus.Error, ex.Message);
            }

            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return GetAuthenticatedJson(jsonResult);
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

            var jsonResult = Json(new ChangeViewModel(change, status), JsonRequestBehavior.AllowGet);
            return GetAuthenticatedJson(jsonResult);
        }

        [HttpGet]
        [Route("api/report")]
        public ActionResult GetReport(int? year)
        {
            if (year == null)
            {
                year = DateTime.Now.Year;
            }
            var reportMgr = GetReportMgr();
            var report = reportMgr.GetReport((int)year, _db.GetProductItems());

            var jsonResult = Json(report, JsonRequestBehavior.AllowGet);
            return GetAuthenticatedJson(jsonResult);
        }

        [HttpGet]
        [Route("api/log")]
        public ActionResult GetLog(DateTime? startDate, DateTime? endDate)
        {
            IList<VendingOperation> result = null;
            if (startDate != null && endDate != null)
            {
                result = _log.GetLogData((DateTime)startDate, (DateTime)endDate);
            }
            else
            {
                result = _log.GetLogData();
            }

            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return GetAuthenticatedJson(jsonResult);
        }
    }
}