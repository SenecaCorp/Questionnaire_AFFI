using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Questionnaire.Entities;
using QuestionnairePrototype.Models.Report;
using Questionnaire.Mailing.Infrastructure;
using QuestionnairePrototype.Models;

namespace QuestionnairePrototype.Infrastructure
{
    public class MailConfigurator
    {
        private static string adminEmail = "riskmanagement@seneca.com";
        //private static string adminEmail = "senecatest@mail.ru";
        private static string vendorsContactEmail = "FSMA-Referrals@Seneca.com";

        private const string subjectTemplate = "Risk Assessment Report for {0} from {1}";

        public static MailContent createUserReportEmail(UserPurchase user, QuestionnaireReport report)
        {
            MailContent mailContent = new MailContent();
            mailContent.MailFrom = adminEmail;
            mailContent.MailingAdress = user.Email;
            mailContent.Subject = "FSMA Self Assessment Report for " + user.FacilityName;
            return mailContent;
        }

        public static MailContent createActivatorReportEmail(UserPurchase user, QuestionnaireReport report)
        {
            MailContent mailContent = new MailContent();
            mailContent.MailFrom = adminEmail;
            mailContent.MailingAdress = user.ActivatorsEmail;
            mailContent.Subject = "FSMA Self Assessment Report for " + user.FacilityName;
            return mailContent;
        }


        public static MailContent createManagerReportEmail(UserPurchase user, QuestionnaireReport report)
        {
            MailContent mailContent = new MailContent();
            mailContent.MailFrom = adminEmail;
            mailContent.MailingAdress = adminEmail;
            mailContent.Subject = "FSMA Self Assessment Report for " + user.FacilityName;
            return mailContent;
        }

        public static MailContent createVendorReport(Vendor vendor, UserPurchase user, int categoryId)
        {
            return createVendorReportEmail(vendor, vendor.Email, user, categoryId);
        }

        public static MailContent createVendorAdminReport(Vendor vendor, UserPurchase user, int categoryId)
        {
            return createVendorReportEmail(vendor, adminEmail, user, categoryId);
        }

        private static MailContent createVendorReportEmail(Vendor vendor, String recipient, UserPurchase user, int categoryId)
        {
            MailContent mailContent = new MailContent();
            mailContent.MailFrom = vendorsContactEmail;
            mailContent.MailingAdress = recipient;
            QuestionnaireContext _context = new QuestionnaireContext();
            String sectionName = (categoryId != 0 ) ? _context.Categories.Where(x => x.Id == categoryId).First().Title
                : "General List of ‘Suggested Service Providers’";

            mailContent.Subject = "Referral from AFFI-FSMA Assessments – Request for Information";
            mailContent.HtmlText = "<table border=\"0\" cellpadding=\"1\" cellspacing=\"1\" style=\"width: 600px;\">"
                + "<tbody><tr><td>"
                + "<img alt=\"\" src=\"http://affi-fsma.seneca.com/Content/img/affi_logo.jpg\" style=\"width: 100px; height: 73px;\" /></td>"
                + "<td><div><span style=\"font-size:18px;\"><strong>Responses from the AFFI &ndash; FSMA Self Assessment</strong></span></div>"
                + "<div><span style=\"font-size:18px;\"><strong>Request for Information to &ldquo;Suggested Service Provider&rdquo;</strong></span></div>"
                + "</td></tr></tbody></table><br/>"
                + "<span style=\"font-size:16px;\">TO: " + vendor.Name + "<br/><br/>"
                + "The following person has clicked on the “Contact Me” button while taking the AFFI – FSMA Self-Assessment:<br/><br/>"
                + "Name of person: " + user.Name + "<br/>"
                + "Facility name: " + user.FacilityName + "<br/>"
                + "Email address: " + user.Email + "<br/>"
                + "Date and time: " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToShortTimeString() + "<br/><br/>"
                + "Section of the AFFI – FSMA Self-Assessment Report: " + sectionName + "<br/><br/>"
                + "By clicking on the Contact Me button the person anticipates that they will receive information about your company’s services related to the section of the FSMA Self-Assessment Report as referenced above.<br/><br/>"
                + "If you have any questions about this email, please “Reply” to this email with your questions.<br/><br/>"
                + "Thank you – <br/>"
                + "Seneca Corporation</span>";
            return mailContent;
        }
    }
}