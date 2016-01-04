using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Questionnaire.Entities;
using QuestionnairePrototype.Models;

namespace QuestionnairePrototype.Infrastructure
{
    public static class QuestionnaireHelper
    {
        public static MvcHtmlString Question(this HtmlHelper helper,
            Question question)
        {
            var tagBuilder = new TagBuilder("p");
            tagBuilder.SetInnerText(question.Title);
           
            return MvcHtmlString.Create(tagBuilder.ToString() + createAnswers(question.Id.ToString(), question.Answers.ToList()));
        }

        private static String createAnswers(String questionId, List<Answer> answers)
        {
            String result = "";

            foreach (Answer answer in answers)
            {
                var inputTag = new TagBuilder("input");
                inputTag.MergeAttribute("name", questionId);
                inputTag.MergeAttribute("type", "radio");
                inputTag.MergeAttribute("value", answer.Id.ToString());
                inputTag.MergeAttribute("id", answer.Id.ToString());

                var labelTag = new TagBuilder("label");
                labelTag.MergeAttribute("for", answer.Id.ToString());
                labelTag.InnerHtml = answer.Title;
                result += inputTag.ToString() + labelTag.ToString() + "<br>";
            }
            return result;
        }

        public static MvcHtmlString Category(this HtmlHelper helper,
            Category category)
        {
            var tagBuilder = new TagBuilder("h3");
            tagBuilder.SetInnerText(category.Title);
            String generatedQuestions = "";
            foreach (Question question in category.Questions)
            {
                generatedQuestions += Question(helper, question);
            }

            return MvcHtmlString.Create(tagBuilder.ToString() + generatedQuestions);
        }

        public static CssJsViewAppnder cssJsViewAppnder(this HtmlHelper htmlHelper)
        {
            return CssJsViewAppnder.GetInstance(htmlHelper);
        }

        public static MvcHtmlString createLinkToRiskImgMail(this HtmlHelper helper,
           string riskName)
        {
            var tag = new TagBuilder("img");
            tag.MergeAttribute("alt", riskName);
            tag.MergeAttribute("src", getUrlByRiskNameMail(riskName));
            tag.MergeAttribute("height", "52");
            tag.MergeAttribute("width", "116");
            return MvcHtmlString.Create(tag.ToString());
        }

        public static MvcHtmlString wrapLinkToVendor(this HtmlHelper helper,
           string url)
        {
            var tag = new TagBuilder("a");
            tag.MergeAttribute("href", url);
            tag.MergeAttribute("target", "_blank");
            tag.SetInnerText(url);
            MvcHtmlString result = MvcHtmlString.Create(tag.ToString());
            return result;
        }

        public static MvcHtmlString radioButtonCheckedHelper(this HtmlHelper helper,
           bool isChecked)
        {
            return MvcHtmlString.Create(isChecked?"checked":"");
        }

        public static MvcHtmlString createLinkToRiskImgScreen(this HtmlHelper helper,
           string riskName)
        {
            var tag = new TagBuilder("img");
            tag.MergeAttribute("alt", riskName);
            tag.MergeAttribute("src", getUrlByRiskNameScreen(riskName));
            tag.MergeAttribute("height", "53");
            tag.MergeAttribute("width", "186");
            return MvcHtmlString.Create(tag.ToString());
        }

        public static MvcHtmlString getRiskTypeNameById(this HtmlHelper helper, int id)
        {
            switch(id)
            {
                case 1:
                    return MvcHtmlString.Create("Low");
                case 2:
                    return MvcHtmlString.Create("Medium");
                case 3:
                default:
                    return MvcHtmlString.Create("High");
            }
        }

        public static MvcHtmlString getRowColorByRiskId(this HtmlHelper helper, int id)
        {
            switch (id)
            {
                case 1:
                    return MvcHtmlString.Create("success");
                case 2:
                    return MvcHtmlString.Create("warning");
                case 3:
                default:
                    return MvcHtmlString.Create("error");
            }
        }

        private static string getUrlByRiskNameMail(string riskName)
        {
            if (riskName == "High")
                return "http://affi-fsma.seneca.com/Content/img/mail/mail_high_risk.gif";
            if (riskName == "Medium")
                return "http://affi-fsma.seneca.com/Content/img/mail/mail_medium_risk.gif";
            if (riskName == "Low")
                return "http://affi-fsma.seneca.com/Content/img/mail/mail_low_risk.gif";

            return "";
        }
        
        private static string getUrlByRiskNameScreen(string riskName)
        {
            if (riskName == "Low")
                return "../Content/img/screen/screen_low.png";
            if (riskName == "Medium")
                return "../Content/img/screen/screen_medium.png";
            if (riskName == "High")
                return "../Content/img/screen/screen_high.png";

            return "";
        }

        public static List<Category> getCategoriesList()
        {
            QuestionnaireContext context = new QuestionnaireContext();
            return context.Categories.ToList();
        }
    }
}