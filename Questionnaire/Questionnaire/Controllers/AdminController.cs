using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuestionnairePrototype.Models.Membership;
using QuestionnairePrototype.Models.Admin;
using QuestionnairePrototype.Models.Questionnaire;
using QuestionnairePrototype.Models;
using QuestionnairePrototype.Models.Vendors;
using Questionnaire.Entities;
using System.IO;
using QuestionnairePrototype.Infrastructure;
using QuestionnairePrototype.Models.Report;
using System.Web.Security;

namespace QuestionnairePrototype.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private QuestionnaireAnswersRepository _questionnaireAnswersRepository = new QuestionnaireAnswersRepository();
        private UserRepository _userRepository = new UserRepository();

        private QuestionnaireContext _context = new QuestionnaireContext();

        //
        // GET: /Admin/

        public ActionResult Index()
        {
            var hash = (string)HttpContext.Session["facilityDateHash"];
            var userId = _userRepository.GetUserByEmail(User.Identity.Name, hash).Id;

            //Get User's Email address
            var userEmail = getUsersEmail();

           //Create ViewBag Attribute to hold Current User Details
            ViewBag.CurrentUser = _userRepository.GetUserById(userId);


            //Get User's information
            UserPurchase thisUser = _context.UserPurchases.First(x => x.Email == userEmail);

            //if user is not an admin, then logout user
            if (!thisUser.IsAdmin)
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Index");
            }
            
            return View(new UsersList());
        }

        private string getUsersEmail()
        {
            return HttpContext.Session["email"].ToString();
        }

        public ActionResult AddUser()
        {
            UserPurchase userPurchase = new UserPurchase();
            userPurchase.UserExpirationDate = DateTime.Now;
            //userPurchase.ActivatorsEmail = User.Identity.Name;
            return View(userPurchase);
        }

        [HttpPost]
        public ActionResult AddUser(UserPurchase userPurchase)
        {
            UserRepository userRepository = new UserRepository();

            if (ModelState.IsValid)
            {
                var salt = "ururu";

                userPurchase.Password = UserRepository.HashPassword(userPurchase.Password.Trim(), salt);
                userPurchase.PasswordSalt = salt;
                userPurchase.Email = userPurchase.Email.ToLower().Trim();
                if (userPurchase.ActivatorsEmail != null)
                {
                    userPurchase.ActivatorsEmail = userPurchase.ActivatorsEmail.ToLower().Trim();
                }

                userPurchase.DateOfPurchase = DateTime.Now;
                userPurchase.UserRegistrationDate = DateTime.Now;

                //Check to make sure the combination of Email, Password, and Facilty is unique
                int existingUserCheck = _context.UserPurchases.Where(x => x.Email == userPurchase.Email && x.Password == userPurchase.Password && x.FacilityName == userPurchase.FacilityName).Count();

                if (existingUserCheck == 0)
                {
                    _context.UserPurchases.Add(userPurchase);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("Email", "The combination of Email, Password, and Facility Name must be unique. ");
                }
            }

            return View(userPurchase);
        }

        public ActionResult EditUser(int? id)
        {
            if (id == null)
                return Redirect("../");

            try
            {
                UserPurchase userPurchase = _context.UserPurchases.First(x => x.Id == id);

                return View(userPurchase);
            }
            catch (Exception e)
            {
                return Redirect("../");
            }
        }

        [HttpPost]
        public ActionResult EditUser(UserPurchase userPurchase)
        {
            UserPurchase temp = _context.UserPurchases.First(x => x.Id == userPurchase.Id);
            UserRepository userRepository = new UserRepository();

            if (ModelState.IsValid)
            {
                String oldPassword = temp.Password;

                var salt = "ururu";
                var pw = UserRepository.HashPassword(userPurchase.Password.Trim(), salt);

                //If password changed
                if (userPurchase.Password != oldPassword)
                {
                    temp.Password = pw;
                    temp.PasswordSalt = salt;
                }

                temp.UserRegistrationDate = DateTime.Now;
                temp.UserExpirationDate = userPurchase.UserExpirationDate;
                temp.ActivatorsEmail = User.Identity.Name;
                temp.Name = userPurchase.Name;
                temp.Email = userPurchase.Email.ToLower().Trim();
                if (userPurchase.ActivatorsEmail != null)
                {
                    temp.ActivatorsEmail = userPurchase.ActivatorsEmail.ToLower().Trim();
                }
                else
                {
                    temp.ActivatorsEmail = null;
                }
                temp.FacilityName = userPurchase.FacilityName;
                temp.IsAdmin = userPurchase.IsAdmin;

                //Check to make sure the combination of Email, Password, and Facilty is unique
                int existingUserCheck = _context.UserPurchases.Where(x => x.Email == userPurchase.Email && x.Password == pw && x.FacilityName == userPurchase.FacilityName && x.Id != userPurchase.Id).Count();

                if (existingUserCheck == 0)
                {
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("Email", "The combination of Email, Password, and Facility Name must be unique. ");
                }
            }

            return View(userPurchase);
        }

        //
        // GET: /Admin/Details/5
        public ViewResult Details(int userId)
        {
            UserDetails userDetails = new UserDetails(
                _userRepository.GetUserById(userId),
                _questionnaireAnswersRepository.getQuestionnaireReport(userId)
            );

            return View(userDetails);
        }


        /// <summary>
        /// Disable / Enable User
        /// </summary>
        /// <param name="revertUserId"></param>
        /// <returns></returns>
        public ActionResult MarkAsDelete(int revertUserId)
        {
            _context.UserPurchases.First(x => x.Id == revertUserId).UserIsDeleted = !(_context.UserPurchases.First(x => x.Id == revertUserId).UserIsDeleted);

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public HttpResponseBase ExportStatistics()
        {
            ExcelWriter writer = new ExcelWriter();
            writer.startWorksheet("Users");
            writer.startRow();
            writer.addCell("Name");
            writer.addCell("Email");
            writer.addCell("Facility");
            writer.addCell("Size of Facility");
            writer.addCell("Industrial Classification");
            writer.addCell("2nd Classification");
            writer.addCell("3rd Classification");
            writer.addCell("Registration Date");
            writer.addCell("Expiration Date");
            writer.addCell("Overall H,M,Low");
            writer.addCell("Overall Score #");

            //Header
            int questionId = 1;
            IList<Category> categories = _context.Categories.ToList();

            foreach (Category category in categories)
            {
                writer.addCell(category.Index + ". " + category.Title + "  H,M,L");
                writer.addCell(category.Index + ". " + category.Title + "  Score");
                writer.addCell(category.Index + ". " + category.Title + "  Max");
                writer.addCell(category.Index + ". " + category.Title + "  Score #");
                for (int i = 0; i < category.Questions.Count; i++)
                {
                    writer.addCell("Q." + questionId + " answer");
                    writer.addCell("Answer Score");
                    writer.addCell("Max Score");
                    questionId++;
                }
            }
            writer.endRow();

            //Cycle through user entries and create lists
            IList<UserEntry> ul = (new UsersList()).Users;
            foreach (UserEntry userEntry in ul)
            {
                if (!userEntry.WasCompleteQuestionnaire)
                    continue;

                writer.startRow();
                
                //Common info
                writer.addCell(userEntry.User.Name);
                writer.addCell(userEntry.User.Email);
                writer.addCell(userEntry.User.FacilityName);
                writer.addCell(userEntry.User.SizeOfFacilityValue);
                writer.addCell(userEntry.User.IndustrialClassificationValue);
                writer.addCell(userEntry.User.AdditionalProductClassificationValue);
                writer.addCell(userEntry.User.AnotherProductClassificationValue);
                writer.addCell(userEntry.User.UserRegistrationDate);
                writer.addCell(userEntry.User.UserExpirationDate);

                //Get report for this user
                QuestionnairePrototype.Models.Report.QuestionnaireReport report = _questionnaireAnswersRepository.getQuestionnaireReport(userEntry.User.Id);
                
                //Overall
                writer.addCell(report.Overall.RiskTypeName);
                writer.addCell(report.Overall.OverallScore);

                //Categories and answers
                foreach (CategoryRecomendation category in report.Categories)
                {
                    writer.addCell(category.RiskTypeName);
                    writer.addCell(category.QuestionsValueSum);
                    writer.addCell(category.QuestionsMaxValueSum);
                    writer.addCell(category.CategoryScore);
                    foreach (QuestionRecomendation question in category.QuestionRecomendations)
                    {
                        writer.addCell(question.answerId);
                        writer.addCell(question.checkedValue);
                        writer.addCell(question.maxValue);
                    }
                }
                writer.endRow();
            }
            
            writer.endWorksheet();
            writer.endWorkbook();

            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=Users submissions.xls");
            Response.Charset = "";
            Response.ContentType = "application/ms-excel";
            Response.Write(writer.getString());
            Response.End();

            return Response;
        }
    }
}
