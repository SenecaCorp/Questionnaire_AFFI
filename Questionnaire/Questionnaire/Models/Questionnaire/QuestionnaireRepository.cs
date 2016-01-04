using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.Infrastructure;
using Questionnaire.Entities;
using QuestionnairePrototype.Models.Membership;
using QuestionnairePrototype.Models.Report;

namespace QuestionnairePrototype.Models.Questionnaire
{
    public class QuestionnaireRepository
    {
        private QuestionnaireContext _context = new QuestionnaireContext();
        private UserRepository _userRepository = new UserRepository();

        public QuestionnaireWithSelectedAnswers getQuestionnaireProbablyWithSelectedAnswers(int userId)
        {
            List<Category> categories;
            Dictionary<int, int> questionAnswerMap;

            try
            {
                categories = getQuestionnireCategories();
                questionAnswerMap = getQuestionAnswerMapping(userId);
            }
            catch (Exception)
            {
                return null;
            }

            return new QuestionnaireWithSelectedAnswers(categories, questionAnswerMap);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void rewriteUserAnswersWithNew(int userId, Dictionary<int, int> newUserAnswers)
        {
            try
            {
                List<UserAnswer> answers = _context.UserAnswers.Where(x => x.UserId == userId).ToList();

                foreach (UserAnswer answer in answers)
                {
                    _context.UserAnswers.Remove(answer);
                }

                foreach (int questionId in newUserAnswers.Keys)
                {
                    UserAnswer userAnswer = new UserAnswer
                    {
                        QuestionID = questionId,
                        AnswerID = newUserAnswers[questionId],
                        UserId = userId
                    };

                    _context.UserAnswers.Add(userAnswer);
                }

                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw new Exception("rewriteUserAnswersWithNews failed");
            }
        }

        public void rememberThatUserHasCompleteQuestionnaire(int userId)
        {
            try
            {
                SubmittedStatusForUser status = _context.SubmittedeStatusForUsers.SingleOrDefault(x => x.UserId == userId);
                if (status != null)
                {
                    status.SubmitedDate = DateTime.Now;
                }
                else
                {
                    _context.SubmittedeStatusForUsers.Add(new SubmittedStatusForUser() { UserId = userId, SubmitedDate = DateTime.Now });
                }

                _context.SaveChanges();
            }
            catch
            {
                throw new Exception("rememberThatUserHasCompleteQuestionnaire failed");
            }
        }

        private List<Category> getQuestionnireCategories()
        {
            DbQuery<Category> result = _context.Categories.Include("Questions").Include("Questions.Answers");
            List<Category> categories = result.OrderBy(s => s.Index).ToList();
            foreach (Category category in categories)
            {
                category.Questions = category.Questions.OrderBy(s => s.Index).ToList();
                foreach (var questions in category.Questions)
                {
                    questions.Answers = questions.Answers.OrderBy(s => s.Index).ToList();
                }
            }
            return categories;
        }

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