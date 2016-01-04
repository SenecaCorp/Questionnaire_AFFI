using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Questionnaire.Entities
{
    public class UserAnswer
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int QuestionID { get; set; }
        public int AnswerID { get; set; }
    }
}