using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VendingService.Interfaces;
using VendingService.Helpers;
using VendingService.Models;

namespace VendingWeb.Controllers
{
    public class UserController : VendingBaseController
    {
        private IVendingService _db;
        private ILogService _log;

        public UserController(IVendingService db, ILogService log) : base(db, log)
        {
            _db = db;
            _log = log;
        }

        [HttpGet]
        public ActionResult Login()
        {
            if (base.IsAuthenticated)
            {
                LogUserOut();
            }

            return View("Login");
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            ActionResult result = null;

            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception();
                }

                UserItem user = null;
                try
                {
                    user = _db.GetUserItem(model.Username);
                }
                catch(Exception)
                {
                    ModelState.AddModelError("invalid-user", "Either the username or the password is invalid.");
                    throw;
                }

                PasswordHelper passHelper = new PasswordHelper(model.Password, user.Salt);
                if (!passHelper.Verify(user.Hash))
                {
                    ModelState.AddModelError("invalid-user", "Either the username or the password is invalid.");
                    throw new Exception();
                }

                // Happy Path
                base.LogUserIn(user);

                result = RedirectToAction("Index", "Vending");
            }
            catch (Exception)
            {
                result = View("Login");
            }

            return result;
        }

        [HttpGet]
        public ActionResult Register()
        {
            if (base.IsAuthenticated)
            {
                LogUserOut();
            }

            return View("Register");
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            ActionResult result = null;

            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception();
                }

                UserItem user = null;
                try
                {
                    user = _db.GetUserItem(model.Username);
                }
                catch(Exception)
                {
                }

                if (user != null)
                {
                    ModelState.AddModelError("invalid-user", "The username is already taken.");
                    throw new Exception();
                }

                PasswordHelper passHelper = new PasswordHelper(model.Password);
                UserItem newUser = new UserItem()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Username = model.Username,
                    Salt = passHelper.Salt,
                    Hash = passHelper.Hash
                };

                _db.AddUserItem(newUser);
                base.LogUserIn(newUser);

                result = RedirectToAction("Index", "Vending");
            }
            catch (Exception)
            {
                result = View("Register");
            }

            return result;
        }
    }
}