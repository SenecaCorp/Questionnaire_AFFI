using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuestionnairePrototype.Models.Membership;
using System.Web.Security;

namespace QuestionnairePrototype.Controllers
{
    public class SignInController : Controller
    {
        private UserRepository userRepository = new UserRepository();
        //
        // GET: /SignIn/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(SignInModel model, string returnUrl)
        {
            HttpContext.Session["facilityDateHash"] = null;
            if (ModelState.IsValid)
            {
                string errorMessage = "";
                string hash = "";
                if (userRepository.ValidateUser(model.Email, model.Password, ref errorMessage, ref hash))
                {
                    FormsAuthentication.SetAuthCookie(model.Email, model.RememberMe);
                    HttpContext.Session["facilityDateHash"] = hash;

                    HttpContext.Session["email"] = model.Email;

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

            HttpContext.Session["facilityDateHash"] = null;

            return RedirectToAction("Index");
        }

        private bool UrlSafe(string url)
        {
            return Url.IsLocalUrl(url) && url.Length > 1 && url.StartsWith("/")
                   && !url.StartsWith("//") && !url.StartsWith("/\\");
        }

    }
}
