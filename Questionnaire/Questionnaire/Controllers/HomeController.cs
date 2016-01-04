using System.Collections.Generic;
using System;
using System.Linq;
using System.Web.Mvc;
using QuestionnairePrototype.Models;
using System.Data.Entity.Infrastructure;
using QuestionnairePrototype.Models.Membership;
using QuestionnairePrototype.Models.Questionnaire;
using Questionnaire.Entities;

namespace QuestionnairePrototype.Controllers
{
    [Authorize]
    public class HomeController : Controller     
    {
        private UserRepository _userRepository = new UserRepository();
        private QuestionnaireRepository _questionnaireRepository = new QuestionnaireRepository();

        //
        // GET: /Home/
        public ActionResult Index()
        {
            //TODO: add try catch block - Mithgol, please unlogin
            QuestionnaireWithSelectedAnswers questionnaire;

            var hash = (string)HttpContext.Session["facilityDateHash"];
            var userId = _userRepository.GetUserByEmail(User.Identity.Name, hash).Id;
            questionnaire = _questionnaireRepository.getQuestionnaireProbablyWithSelectedAnswers(userId);

            UserPurchase user = _userRepository.GetUserById(userId);

            QuestionnaireWithSelectedAnswersForUser questionnaireForUser =
                new QuestionnaireWithSelectedAnswersForUser(questionnaire, user);

            if(questionnaire != null)
                return View(questionnaireForUser);
            else
                return Redirect("Error");
        }

        protected override void Dispose(bool disposing)
        {
            //TODO: dispose the repositories
            base.Dispose(disposing);
        }
    }
}
