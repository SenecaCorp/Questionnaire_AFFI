using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuestionnairePrototype.Models.Report
{
    public class QuestionRecomendation
    {
        internal int questionId;
        internal int answerId;

        public int maxValue;
        public int checkedValue;
        public int answerIndex;

        public string QuestionSummary;
        public string AnswerComment;

        private QuestionnaireContext context = new QuestionnaireContext();

        public QuestionRecomendation(int questionId, int answerId)
        {
            this.questionId = questionId;
            this.answerId = answerId;

            try
            {
                maxValue = calculateMaxValue();
                checkedValue = context.Answers.Find(answerId).Value;
                QuestionSummary = context.Questions.Find(questionId).Summary;
                AnswerComment = context.Answers.Find(answerId).Comment;
                answerIndex = context.Answers.Find(answerId).Index;
            }
            catch
            {
                throw new Exception("database don't contain answer or question with passed ids");
            }

        }

        //TODO: Handle errors
        private int calculateMaxValue()
        {
            int max = int.MinValue;
            foreach (var answer in context.Questions.Find(questionId).Answers)
            {
                if (answer.Value > max)
                    max = answer.Value;
            }
            return max;
        }
    }
}