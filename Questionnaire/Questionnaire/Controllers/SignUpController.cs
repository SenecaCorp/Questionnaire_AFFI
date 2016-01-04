using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuestionnairePrototype.Models.Membership;

namespace QuestionnairePrototype.Controllers
{
    public class SignUpController : Controller
    {
        private UserRepository userRepository = new UserRepository();

        //
        // GET: /SignUp/
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(SignUpModel model)
        {
            try
            {
                var password = userRepository.CreateNewUser(model);

                if (password != null)
                {
                    return Content(string.Format("<html><head></head><body><h2>Succes</h2><p>Password is: {0}</p></body></html>", password));
                }
                else
                {
                    throw new Exception("Cannot create user");
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View(model);
            }
        }
    }
}
