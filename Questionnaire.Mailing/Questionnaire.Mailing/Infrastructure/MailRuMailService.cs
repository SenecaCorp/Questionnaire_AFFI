using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;

namespace Questionnaire.Mailing.Infrastructure
{
    public class MailRuMailService : IMailService
    {
        private string login;
        private string password;

        public MailRuMailService(string login, string password)
        {
            this.login = login;
            this.password = password;
        }

        public MailSendStatus sendMail(MailContent mailContent)
        {
            return doSendMail(mailContent, true);
        }

        public MailSendStatus sendMail(MailContent mailContent, bool isBodyHtml)
        {
            return doSendMail(mailContent, isBodyHtml);
        }

        private MailSendStatus doSendMail(MailContent mailContent, bool isBodyHtml)
        {
            MailSendStatus mailSendStatus = new MailSendStatus();

            mailSendStatus.IsSended = false;
            int flag = 0;
            if (mailContent.MailingAdress == "")
            {
                mailSendStatus.ErrorType = mailSendStatus.ErrorType + "Required MailingAdress data ";
                flag = 1;
            }
            if (mailContent.PlainText == "")
            {
                mailSendStatus.ErrorType = mailSendStatus.ErrorType + "Required Text data ";
                flag = 1;
            }
            if (login == "")
            {
                mailSendStatus.ErrorType = mailSendStatus.ErrorType + "Required Login ";
                flag = 1;
            }
            if (password == "")
            {
                mailSendStatus.ErrorType = mailSendStatus.ErrorType + "Required Password ";
                flag = 1;
            }

            if (flag == 0)
            {
                SmtpClient smtpclient = new SmtpClient();

                try
                {
                    var _with1 = (smtpclient);
                    _with1.Host = "Smtp.mail.ru";
                    _with1.Port = 25;
                    _with1.EnableSsl = false;
                    _with1.Credentials = new NetworkCredential(login, password);

                    MailMessage mailMessage = new MailMessage(login + "@mail.ru", mailContent.MailingAdress, mailContent.Subject, mailContent.HtmlText);
                    mailMessage.IsBodyHtml = isBodyHtml;

                    mailMessage.Body = mailContent.HtmlText;

                    smtpclient.Send(mailMessage);
                    mailSendStatus.IsSended=true;
                }
                catch (Exception ex)
                {
                    mailSendStatus.ErrorType = mailSendStatus.ErrorType + ex;
                }
            }
            return mailSendStatus;
        }
    }
}