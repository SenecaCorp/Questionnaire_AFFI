using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Questionnaire.Mailing.Infrastructure;

namespace Questionnaire.Webservices
{
    public class Mail
    {

        internal static void sendEmailToUser(string email, Product.ProudctType productType)
        {
            var emailServer = getSMTPServer();
            IMailService mailService = new SmptMailService(emailServer);

            MailContent mail = new MailContent();
            mail.MailingAdress = email;
            mail.MailFrom = getFromAddress();
            mail.ReplyToAddress = getReplyToAddress();

            //AFFI English
            if(productType == Product.ProudctType.AffiEnglish)
            {
                mail.Subject = getAFFIEmailSubject();
                mail.HtmlText = getEmailBodyForUserAFFISite();
            }
            //AFFI Spanish
            else
            {
                mail.Subject = getAFFISpanishEmailSubject();
                mail.HtmlText = getEmailBodyForUserAFFISpanishSite();
            }
           
            mailService.sendMail(mail, true);
        }

        internal static void sendEmailToAffi(Product.ProudctType productType)
        {
            var emailServer = getSMTPServer();
            IMailService mailService = new SmptMailService(emailServer);

            MailContent mail = new MailContent();
            mail.MailingAdress = getAFFIToAddress();
            mail.MailFrom = getFromAddress();
            mail.ReplyToAddress = getReplyToAddress();
            //AFFI English
            //if (productType == Product.ProudctType.AffiEnglish)
            //{
                mail.Subject = getAFFIEmailSubject();
                mail.HtmlText = getEmaiBodyforAffi();
            //}
            //AFFI Spanish
            //else
            //{
            //    mail.Subject = getAFFISpanishEmailSubject();
            //    mail.HtmlText = getEmaiBodyforAffiSpanish();
            //}
            
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

        private static string getAFFIEmailSubject()
        {
            return "New Facility Registration";
        }

        private static string getAFFISpanishEmailSubject()
        {
            return "Nuevo registro de planta";
        }

        private static string getSMTPServer()
        {
            return System.Configuration.ConfigurationManager.AppSettings["smtpServer"];
        }

        private static string getFromAddress()
        {
            return System.Configuration.ConfigurationManager.AppSettings["fromEmailAddress"];
        }

        private static string getReplyToAddress()
        {
            return System.Configuration.ConfigurationManager.AppSettings["replyToEmailAddress"];
        }

        private static string getAFFIToAddress()
        {
            return System.Configuration.ConfigurationManager.AppSettings["AFFIToAddress"];
        }

        private static string getAFFIURL()
        {
            return System.Configuration.ConfigurationManager.AppSettings["AFFIWebsiteURL"];
        }

        private static string getAFFISpanishURL()
        {
            return System.Configuration.ConfigurationManager.AppSettings["AFFIWebsiteURLSpanish"];
        }
       

        private static string getEmailBodyForUserAFFISite()
        {
            string body = @"Dear " + Account.Name + @",
            <br><br>
 
            Your new account has been created and registered for the AFFI FSMA Self-Assessment Readiness Tool for Facility " + Account.Facility + @". 
            Please go to <a href='" + getAFFIURL() + @"'>" + getAFFIURL() + @"</a>
            to complete the Readiness Tool. Your login credentials are:
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

        private static string getEmailBodyForUserAFFISpanishSite()
        {
            string body = @"Estimado " + Account.Name + @",
            <br><br>
 
            Su nueva cuenta ha sido creado y registrado por la herramienta de preparación de autoevaluación AFFI. Por favor vaya a <a href='" + getAFFISpanishURL() + @"'>" + getAFFISpanishURL() + @"</a>
            para completar la herramienta de preparación. Sus credenciales de inicio de sesión son:
            <br><br>
            Email: " + Account.EmailForUserId + @"
            <br>
            Contrasena: " + Account.Password + @"
            <br><br>
            Por favor guarde este correo electrónico para sus archivos. 
            <br><br>
            Gracias";

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

        private static string getEmaiBodyforAffiSpanish()
        {
            string body = @"A new account has been created and registered for the AFFI FSMA Spanish Self-Assessment Readiness Tool:
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