using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QuestionnairePrototype.Models.Membership;
using QuestionnairePrototype.Models.Report;
using Questionnaire.Entities;

namespace QuestionnairePrototype.Models.Questionnaire
{
    public class QuestionnaireAnswersRepository
    {
        private QuestionnaireContext _context = new QuestionnaireContext();
        private UserRepository _userRepository = new UserRepository();

        public QuestionnaireReport getQuestionnaireReport(int userId)
        {
            return new QuestionnaireReport(getQuestionAnswerMapping(userId));
        }

        private Dictionary<int, int> getQuestionAnswerMapping(int userId)
        {
            Dictionary<int, int> questionAnswerMapping = new Dictionary<int, int>();

            List<UserAnswer> answers = _context.UserAnswers.Where(x => x.UserId == userId).ToList();
            foreach (UserAnswer answer in answers)
            {
                questionAnswerMapping.Add(answer.QuestionID, answer.AnswerID);
            }
            return questionAnswerMapping;
        }
    }
}