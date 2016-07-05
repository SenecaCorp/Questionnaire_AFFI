using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Questionnaire.Mailing.Infrastructure;

namespace Questionnaire.Webservices
{
    public class Mail
    {

        internal static void sendEmailToUser(string email)
        {
            var emailServer = getSMTPServer();
            IMailService mailService = new SmptMailService(emailServer);

            MailContent mail = new MailContent();
            mail.MailingAdress = email;
            mail.MailFrom = getFromAddress();
            mail.Subject = getEmailSubject();
            mail.HtmlText = getEmailBodyForUser();
            mailService.sendMail(mail, true);
        }

        internal static void sendEmailToAffi()
        {
            var emailServer = getSMTPServer();
            IMailService mailService = new SmptMailService(emailServer);

            MailContent mail = new MailContent();
            mail.MailingAdress = getAFFIToAddress();
            mail.MailFrom = getFromAddress();
            mail.Subject = getEmailSubject();
            mail.HtmlText = getEmaiBodyforAffi();
            mailService.sendMail(mail, true);

        }


        internal static void sendErrorEmail(string message)
        {
            var emailServer = getSMTPServer();
            IMailService mailService = new SmptMailService(emailServer);

            var emailRecipient = System.Configuration.ConfigurationManager.AppSettings["errorEmailRecipient"];

            MailContent mail = new MailContent();
            mail.MailingAdress = emailRecipient;
            mail.MailFrom = getFromAddress();
            mail.Subject = "AFFI Signup Error";
            mail.HtmlText = message;
            mailService.sendMail(mail, true);
        }

        private static string getEmailSubject()
        {
            return "New Facility Registration";
        }

        private static string getSMTPServer()
        {
            return System.Configuration.ConfigurationManager.AppSettings["smtpServer"];
        }

        private static string getFromAddress()
        {
            return System.Configuration.ConfigurationManager.AppSettings["fromEmailAddress"];
        }

        private static string getAFFIToAddress()
        {
            return System.Configuration.ConfigurationManager.AppSettings["AFFIToAddress"];
        }
       

        private static string getEmailBodyForUser()
        {
            string body = @"Dear " + Account.Name + @",
            <br><br>
 
            Your new account has been created and registered for the AFFI FSMA Self-Assessment Readiness Tool for Facility " + Account.Facility + @". Please go to <a href='http://affi-fsma.seneca.com'>http://affi-fsma.seneca.com</a>
            to complete the Readiness Tool. Your login credenetials are:
            <br><br>
            Email: " + Account.EmailForUserId + @"
            <br>
            Password: " + Account.Password + @"
            <br><br>
            Please retain this email for your records.
            <br><br>
            Thank you";

            return body;
        }

        private static string getEmaiBodyforAffi()
        {
            string body = @"A new account has been created and registered for the AFFI FSMA Self-Assessment Readiness Tool:
            <br><br>
            Name: " + Account.Name + @"
            <br>
            Email: " + Account.EmailForUserId + @"
            <br>
            Facility: " + Account.Facility;

            return body;
        }


    }
}