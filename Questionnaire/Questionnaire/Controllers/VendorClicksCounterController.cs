using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Questionnaire.Entities;
using QuestionnairePrototype.Models;
using QuestionnairePrototype.Models.Vendors;
using QuestionnairePrototype.Models.Membership;
using Questionnaire.Mailing.Infrastructure;
using QuestionnairePrototype.Infrastructure;

namespace QuestionnairePrototype.Controllers
{
    [Authorize]
    public class VendorClicksCounterController : Controller
    {
        //private IMailService mailService = new MailRuMailService("senecatest", "test1234");
        private IMailService mailService = new SmptMailService(System.Configuration.ConfigurationManager.AppSettings["smtpServer"]);
        
        private QuestionnaireContext _context = new QuestionnaireContext();
        private UserRepository _userRepository = new UserRepository();
        //
        // GET: /VendorClicksCounter/

        public ActionResult Index(int vendorId, int categoryId)
        {
            VendorStatisticsEntry entry = new VendorStatisticsEntry();
            entry.CategoryId = categoryId;
            entry.VendorId = vendorId;
            entry.Type = VendorStatisticsEntry.CLICK_TYPE;
            entry.OccurredAt = DateTime.Now;
            entry.UserId = _userRepository.GetUserByEmail(User.Identity.Name,
                                                          (string)HttpContext.Session["facilityDateHash"]).Id;

            _context.VendorStatisticsEntries.Add(entry);
            _context.SaveChanges();

            //TODO: generate link using something like URLConstructor(LinkToWebsite);
            return Redirect("http://" + _context.Vendors.Find(vendorId).LinkToWebsite);
        }

        public ActionResult ContactMe(int vendorId, int categoryId)
        {
            VendorStatisticsEntry entry = new VendorStatisticsEntry();
            entry.CategoryId = categoryId;
            entry.VendorId = vendorId;
            entry.Type = VendorStatisticsEntry.CONTACT_ME_TYPE;
            entry.OccurredAt = DateTime.Now;

            UserPurchase user = _userRepository.GetUserByEmail(User.Identity.Name,
                                                               (string)HttpContext.Session["facilityDateHash"]);
            entry.UserId = user.Id;

            _context.VendorStatisticsEntries.Add(entry);
            _context.SaveChanges();

            Vendor vendor = _context.Vendors.Find(vendorId);

            sendEmails(vendor, user, categoryId);

            return Content("success");
        }

        private void sendEmails(Vendor vendor, UserPurchase user, int categoryId)
        {
            MailContent vendorMailContent = MailConfigurator.createVendorReport(vendor, user, categoryId);
            MailContent vendorAdminMailContent = MailConfigurator.createVendorAdminReport(vendor, user, categoryId);
            
            mailService.sendMail(vendorMailContent, true);
            mailService.sendMail(vendorAdminMailContent, true);
        }

    }
}
