using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Questionnaire.Mailing.Infrastructure
{
    public class MailContent
    {
        public string MailFrom;
        public string MailingAdress;
        public string Subject;
        public string PlainText;
        public string HtmlText;
        public string AttachmentPass;
    }
}