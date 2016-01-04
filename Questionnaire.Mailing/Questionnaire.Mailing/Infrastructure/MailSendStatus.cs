using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Questionnaire.Mailing.Infrastructure
{
    public class MailSendStatus
    {
        public string ErrorType;
        public bool IsSended;

        public MailSendStatus()
        {
            IsSended = false;
        }
    }
}