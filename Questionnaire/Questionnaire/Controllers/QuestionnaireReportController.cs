using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.IO;
using Questionnaire.Entities;
using Questionnaire.Entities.Infrastructure;
using QuestionnairePrototype.Models.Membership;
using QuestionnairePrototype.Models.Report;
using QuestionnairePrototype.Models.Questionnaire;
using QuestionnairePrototype.Infrastructure;
using Questionnaire.Mailing.Infrastructure;

namespace QuestionnairePrototype.Controllers
{
    [Authorize]
    public class QuestionnaireReportController : Controller
    {
        private UserRepository _userRepository = new UserRepository();
        private QuestionnaireRepository _questionnaireRepository = new QuestionnaireRepository();
        
        //
        // GET: /QuestionnaireHandler/
        [HttpPost]
        public ActionResult Index(FormCollection formCollection)
        {
            //TODO: Add generation of report from existing answers in database

            //try
            //{
            QuestionnaireFormCollectionParser parser = new QuestionnaireFormCollectionParser(formCollection);
            var userId = _userRepository.GetUserByEmail(User.Identity.Name, (string)HttpContext.Session["facilityDateHash"]).Id;

            _questionnaireRepository.rewriteUserAnswersWithNew(
                userId,
                parser.QuestionAnswerMapping
            );

            UserPurchase user = _userRepository.GetUserById(userId);
            DemographicData demographicData = parser.DemographicData;

            user.SizeOfFacility = demographicData.SizeOfFacility;
            user.IndustrialClassification = demographicData.IndustrialClassification;
            user.AnotherProductClassification = demographicData.AnotherProductClassification;
            user.AdditionalProductClassification = demographicData.AdditionalProductClassification;

            _userRepository.SaveChanges();

            if (formCollection["completeLaterOnSubmit"] == "false")
            {
                _questionnaireRepository.rememberThatUserHasCompleteQuestionnaire(userId);

                QuestionnaireReport newReport = new QuestionnaireReport(parser.QuestionAnswerMapping);
                newReport.calculateScore();

                QuestionnaireReportForUser questionnaireReportForUser = new QuestionnaireReportForUser(newReport, user);

                //IMailService mailService = new MailRuMailService("senecatest", "test1234");
                IMailService mailService = new SmptMailService(System.Configuration.ConfigurationManager.AppSettings["smtpServer"]);

                String htmlText = ToHtml(
                    "MailTemplate",
                    new ViewDataDictionary(questionnaireReportForUser),
                    this.ControllerContext
                );

                MailContent userMailContent = MailConfigurator.createUserReportEmail(user, newReport);
                userMailContent.HtmlText = htmlText;
                mailService.sendMail(userMailContent);

                if (user.Email != user.ActivatorsEmail)
                {
                    //Also send an email to a person, who activated our purchase
                    MailContent activatorMailContent = MailConfigurator.createActivatorReportEmail(user, newReport);
                    activatorMailContent.HtmlText = htmlText;
                    mailService.sendMail(activatorMailContent);
                }

                MailContent managerMailContent = MailConfigurator.createManagerReportEmail(user, newReport);
                managerMailContent.HtmlText = htmlText;
                mailService.sendMail(managerMailContent);

                return View(questionnaireReportForUser);
            }
            // TODO: make special view for this message
            else
            {
                AFFIHeaderModel savedPageModel = new AFFIHeaderModel(user.FacilityName, user.Name, user.Email, true, DateTime.Now.Date);
                return View("ReportSaved", savedPageModel);
            }
            //}
            //catch
            //{
            //    return Redirect("Error");
            //}
        }

        private string ToHtml(string viewToRender, ViewDataDictionary viewData, ControllerContext controllerContext)
        {
            var result = ViewEngines.Engines.FindView(controllerContext, viewToRender, null);

            var output = new StringWriter();
            var viewContext = new ViewContext(controllerContext, result.View, viewData, controllerContext.Controller.TempData, output);
            result.View.Render(viewContext, output);
            result.ViewEngine.ReleaseView(controllerContext, result.View);

            return output.ToString();
        }
    }
}