using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net.Mime;

namespace Questionnaire.Mailing.Infrastructure
{
    public class SmptMailService : IMailService
    {
        private string host;

        public SmptMailService(string host)
        {
            this.host = host;
        }

        public MailSendStatus sendMail(MailContent mailContent)
        {
            return doSendMail(mailContent, true);
        }

        public MailSendStatus sendMail(MailContent mailContent, bool isBodyHtml)
        {
            return doSendMail(mailContent, isBodyHtml);
        }

        public MailSendStatus doSendMail(MailContent mailContent, bool isBodyHtml)
        {
            MailSendStatus mailSendStatus = new MailSendStatus();
            if (String.IsNullOrEmpty(mailContent.MailingAdress))
            {
                mailSendStatus.ErrorType = mailSendStatus.ErrorType + "Required MailingAdress data ";
                return mailSendStatus;
            }
            if (String.IsNullOrEmpty(mailContent.HtmlText))
            {
                mailSendStatus.ErrorType = mailSendStatus.ErrorType + "Required Text data ";
                return mailSendStatus;
            }

            SmtpClient smtpClient = new SmtpClient(host);

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(mailContent.MailFrom);

            if (!String.IsNullOrEmpty(mailContent.ReplyToAddress))
            {
                mail.ReplyToList.Add(mailContent.ReplyToAddress);
            }
           

            mail.To.Add(mailContent.MailingAdress);

            mail.Subject = mailContent.Subject;

            if (!String.IsNullOrEmpty(mailContent.HtmlText))
            {
                ContentType mimeType = new System.Net.Mime.ContentType("text/html");
                AlternateView alternate = AlternateView.CreateAlternateViewFromString(mailContent.HtmlText, mimeType);
                mail.AlternateViews.Add(alternate);
            }

            mail.Body = mailContent.PlainText;

            if (mailContent.AttachmentPass != null)
                mail.Attachments.Add(new Attachment(mailContent.AttachmentPass));

            smtpClient.Send(mail);

            mailSendStatus.IsSended = true;

            return mailSendStatus;
        }
    }
}


