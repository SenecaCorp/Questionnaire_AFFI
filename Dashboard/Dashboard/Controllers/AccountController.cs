using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dashboard.Models;
using System.Web.Security;
using Dashboard.Infrastructure;
using Dashboard.Infrastructure.Avectra;
using serviceutils;

namespace Dashboard.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(SignInModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                string errorMessage = "";
                if (ValidateUser(model.Email, model.Password, ref errorMessage))
                {
                    FormsAuthentication.SetAuthCookie(model.Email, false);
                    
                    if (UrlSafe(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", errorMessage);
            }
            return View(model);
        }

        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }

        private bool ValidateUser(string Email, string Password, ref string errorMessage)
        {
            HttpContext.Session["purchasedItems"] = null;

            IPurchasedItemsProvider purchasedItemsProvider = new AvectraDataProvider("affixwebuser2", "4ff!xw3bu53r2");
            try
            {
                HttpContext.Session["purchasedItems"] = purchasedItemsProvider.getPurchasedItems(Email, Password);
            }
            catch (AvectraFetchingDataException e)
            {
                errorMessage = e.ErrorDescription;
                return false;
            }

            return true;
        }

        private bool UrlSafe(string url)
        {
            return Url.IsLocalUrl(url) && url.Length > 1 && url.StartsWith("/")
                   && !url.StartsWith("//") && !url.StartsWith("/\\");
        }
    }
}
