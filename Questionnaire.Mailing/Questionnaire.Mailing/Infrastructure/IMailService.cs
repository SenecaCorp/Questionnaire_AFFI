using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Questionnaire.Mailing.Infrastructure
{
    public interface IMailService
    {
        MailSendStatus sendMail(MailContent mailContent);
        MailSendStatus sendMail(MailContent mailContent, bool isBodyHtml);
    }
}
