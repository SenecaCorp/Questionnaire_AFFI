using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Dashboard.Models;
using Questionnaire.Entities;
using System.Web.Configuration;
using System.Data.Objects;
using Questionnaire.Mailing.Infrastructure;

namespace Dashboard.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        //
        // GET: /Dashboard/
        private DashboardContext context = new DashboardContext();

        public ActionResult Index()
        {
            List<PurchasedItem> purchasedItems = GetStoredPurchasedItems();
            if (purchasedItems == null)
            {
                return LogOutAndRedirectToLoginPage();
            }

            //TODO: refactor this algorithm.
            List<DashboardItem> dasboardItems = new List<DashboardItem>();
            List<UserPurchase> activatedItems = context.UserPurchases.Where(u => u.ActivatorsEmail == User.Identity.Name).ToList();

            foreach (UserPurchase userPurchase in activatedItems)
            {
                var dashboardItem = createDasboardItemByUserPurchase(userPurchase);
                dasboardItems.Add(dashboardItem);
            }

            foreach (PurchasedItem purchasedItem in purchasedItems) 
            {
                var activatedItem = activatedItems.Find(i => Math.Abs(i.DateOfPurchase.Subtract(purchasedItem.DateOfPurchase).TotalSeconds) < 1);

                if (activatedItem == null)
                {
                    var dashboardItem = new DashboardItem();

                    dashboardItem.Email = purchasedItem.Email;
                    dashboardItem.Name = purchasedItem.Name;
                    dashboardItem.Facility = purchasedItem.Facility;
                    dashboardItem.DateOfPurchase = purchasedItem.DateOfPurchase;
                    dashboardItem.status = PurchaseStatus.ReadyForActivate;
                    dasboardItems.Add(dashboardItem);
                }
            }

            DashboardModel dashboardModel = new DashboardModel();
            dashboardModel.Items = dasboardItems.OrderBy(d => d.UserExpirationDate);
            dashboardModel.LastPurchase = HttpContext.Session["lastPurchase"] as UserPurchase;
            dashboardModel.LastPurchasePassword = HttpContext.Session["lastPurchasePassword"] as string;

            HttpContext.Session["lastPurchase"] = null;
            HttpContext.Session["lastPurchasePassword"] = null;

            return View(dashboardModel);
        }

        [HttpPost]
        public ActionResult Activate(PurchasedItemModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
                return Redirect("Index");

            string facilityName = model.Facility;
            string userName = model.Name;
            string email = model.Email;
            string dateOfPurchase = model.DateOfPurchase;
            List<PurchasedItem> purchasedItems = GetStoredPurchasedItems();
            if (purchasedItems == null)
            {
                return LogOutAndRedirectToLoginPage();
            }
            DateTime date = DateTime.Parse(dateOfPurchase);

            var purchasedItem = purchasedItems.Find(i => Math.Abs(i.DateOfPurchase.Subtract(date).TotalSeconds) < 1);
            
            if (purchasedItem != null)
            {
                var count = context.UserPurchases.Where(u => EntityFunctions.DiffSeconds(u.DateOfPurchase, purchasedItem.DateOfPurchase) < 1).Count();
                
                //if(count != 0)
                //    return Content("<html><head></head><body><h1>Error! This item already activated </h1></body></html>");


                var salt = GenerateSequenceOfChars(20);
                // TODO: check facility name length
                var password = GeneratePassword(facilityName);
            
                //TODO: move following code to method
                var userPurchase = new UserPurchase();

                userPurchase.Email = email;
                userPurchase.ActivatorsEmail = User.Identity.Name;
                userPurchase.Name = userName;
                userPurchase.PasswordSalt = salt;
                userPurchase.Password = HashPassword(password, salt);
                userPurchase.FacilityName = facilityName;
                userPurchase.DateOfPurchase = purchasedItem.DateOfPurchase;
                userPurchase.UserRegistrationDate = DateTime.Now;
                userPurchase.UserExpirationDate = DateTime.Now.AddMonths(6);

                context.UserPurchases.Add(userPurchase);
                context.SaveChanges();

                HttpContext.Session["lastPurchase"] = userPurchase;
                HttpContext.Session["lastPurchasePassword"] = password;

                //Send an email to user
                IMailService mailService = new SmptMailService(System.Configuration.ConfigurationManager.AppSettings["smtpServer"]);
               
                MailContent mailContent = new MailContent();
                mailContent.MailFrom = "riskmanagement@seneca.com";
                mailContent.MailingAdress = userPurchase.Email;
                mailContent.Subject = "AFFI FSMA Self-Assessment Login Credentials and Instructions";

                mailContent.HtmlText = "<html><head></head><body>Dear " + userPurchase.Name + ":<br/>" +
                "<p>When you want to begin your FSMA Self-Assessment for your facility (<b>" + userPurchase.FacilityName + "</b>), you can just click on the Login link below.</p>" +
                "<p>Your login credentials for the FSMA Self-Assessment are: </p>" +
                "<p><b>Userid: </b>" + userPurchase.Email + "</p>" +
                "<p><b>Password: </b>" + password + "</p>" +
                "<p>If you have any questions regarding the FSMA Self-Assessment Tool, please contact AFFI Vice President of Regulatory and Technical Affairs Dr. Donna Garren at <a href=\"mailto:dgarren@affi.com\">dgarren@affi.com</a> or (703) 821-0770.</p>" +
                "<p>Thank you.</p>" +
                "<p><a href=\"http://affi-fsma.seneca.com/signIn\" target=\"_blank\">Click Here to Login</a></p></body></html>";

                mailContent.PlainText = mailContent.HtmlText;
                mailService.sendMail(mailContent);

                //And if needed - to a person who had activated the purchase (if not same person)
                if (userPurchase.Email != userPurchase.ActivatorsEmail)
                {
                    mailContent.MailingAdress = userPurchase.ActivatorsEmail;
                    mailService.sendMail(mailContent);
                }

                return Redirect("Index");
            }
            else
            {
                return Content("<html><head></head><body><h1>Error!Can't find purchasedItem</h1></body></html>");
            }       
        }

        private List<PurchasedItem> GetStoredPurchasedItems()
        {
            return HttpContext.Session["purchasedItems"] as List<PurchasedItem>;
        }

        private DashboardItem createDasboardItemByUserPurchase(UserPurchase userPurchase)
        {
            var dashboardItem = new DashboardItem();

            dashboardItem.Email = userPurchase.Email;
            dashboardItem.Name = userPurchase.Name;
            dashboardItem.Facility = userPurchase.FacilityName;
            dashboardItem.DateOfPurchase = userPurchase.DateOfPurchase;
            dashboardItem.UserRegistrationDate = userPurchase.UserRegistrationDate;
            dashboardItem.UserExpirationDate = userPurchase.UserExpirationDate;

            if (dashboardItem.UserExpirationDate > DateTime.Now)
                dashboardItem.status = PurchaseStatus.Active;
            else
                dashboardItem.status = PurchaseStatus.Expired;

            return dashboardItem;
        }

        private UserPurchase createUserPurchaseByPurchasedItem(PurchasedItem purchasedItem)
        {
            var salt = GenerateSequenceOfChars(20);
            // TODO: check facility name length
            var password = GeneratePassword(purchasedItem.Facility);

            var userPurchase = new UserPurchase();

            userPurchase.Email = purchasedItem.Email;
            userPurchase.Name = purchasedItem.Name;
            userPurchase.PasswordSalt = salt;
            userPurchase.Password = HashPassword(password, salt);
            userPurchase.FacilityName = purchasedItem.Facility;
            userPurchase.DateOfPurchase = purchasedItem.DateOfPurchase;
            userPurchase.UserRegistrationDate = DateTime.Now;
            userPurchase.UserExpirationDate = DateTime.Now.AddMonths(6);

            return userPurchase;
        }

        private string GeneratePassword(string facilityName)
        {
            return facilityName.Substring(0, 3) + GenerateSequenceOfChars(5);
        }

        private static string GenerateSequenceOfChars(int size)
        {
            var rng = new Random();
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

            var buffer = new char[size];

            for (var i = 0; i < size; i++)
            {
                buffer[i] = chars[rng.Next(chars.Length)];
            }

            return new string(buffer);
        }

        public static string HashPassword(string password, string salt)
        {
            var pw = salt + password;
            return FormsAuthentication.HashPasswordForStoringInConfigFile(pw, FormsAuthPasswordFormat.SHA1.ToString());
        }

        private ActionResult LogOutAndRedirectToLoginPage()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
    }
}
