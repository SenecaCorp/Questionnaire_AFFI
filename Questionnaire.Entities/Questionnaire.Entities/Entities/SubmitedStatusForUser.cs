using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Questionnaire.Entities
{
    public class SubmittedStatusForUser
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime SubmitedDate { get; set; }
    }
}