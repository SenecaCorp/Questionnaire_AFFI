using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Expressions;
using System.Reflection;
using System.ComponentModel;
using System.Web.Mvc.Html;
using System.ComponentModel.DataAnnotations;

namespace Dashboard.Infrastructure
{
    public static class FormExtensions
    {
        public static MvcHtmlString LabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes)
        {
            return html.LabelFor(expression, null, htmlAttributes);
        }

        public static MvcHtmlString LabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string labelText, object htmlAttributes)
        {
            return html.LabelHelper(
                ModelMetadata.FromLambdaExpression(expression, html.ViewData),
                ExpressionHelper.GetExpressionText(expression),
                HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes),
                labelText);
        }

        private static MvcHtmlString LabelHelper(this HtmlHelper html, ModelMetadata metadata, string htmlFieldName, IDictionary<string, object> htmlAttributes, string labelText = null)
        {
            var str = labelText
                ?? (metadata.DisplayName
                ?? (metadata.PropertyName
                ?? htmlFieldName.Split(new[] { '.' }).Last()));

            if (string.IsNullOrEmpty(str))
                return MvcHtmlString.Empty;

            var tagBuilder = new TagBuilder("label");
            tagBuilder.MergeAttributes(htmlAttributes);
            tagBuilder.Attributes.Add("for", TagBuilder.CreateSanitizedId(html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(htmlFieldName)));
            tagBuilder.SetInnerText(str);

            return tagBuilder.ToMvcHtmlString(TagRenderMode.Normal);
        }

        private static MvcHtmlString ToMvcHtmlString(this TagBuilder tagBuilder, TagRenderMode renderMode)
        {
            return new MvcHtmlString(tagBuilder.ToString(renderMode));
        }

        public static MvcHtmlString SubmitButton<TModel>(this HtmlHelper<TModel> htmlHelper, string text, object htmlAttributes)
        {
            return htmlHelper.SubmitButton(text, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static MvcHtmlString SubmitButton<TModel>(this HtmlHelper<TModel> html, string text, IDictionary<string, object> htmlAttributes)
        {
            var tagBuilder = new TagBuilder("button");
            tagBuilder.InnerHtml = text;
            tagBuilder.Attributes.Add("type", "submit");
            tagBuilder.MergeAttributes(htmlAttributes);
            return tagBuilder.ToMvcHtmlString(TagRenderMode.Normal);
        }

        public static MvcHtmlString EnumDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression)
        {
            return EnumDropDownListFor(htmlHelper, expression, null);
        }

        public static MvcHtmlString EnumDropDownListFor<TModel, TEnum>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TEnum>> expression,
            object htmlAttributes
        )
        {
            return htmlHelper.EnumDropDownListFor(expression, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        private static readonly SelectListItem[] SingleEmptyItem = new[] { new SelectListItem { Text = "", Value = "" } };

        public static MvcHtmlString EnumDropDownListFor<TModel, TEnum>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TEnum>> expression,
            IDictionary<string, object> htmlAttributes
        )
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            Type enumType = GetNonNullableModelType(metadata);
            IEnumerable<TEnum> values = Enum.GetValues(enumType).Cast<TEnum>();

            IEnumerable<SelectListItem> items = from value in values
                                                select new SelectListItem
                                                {
                                                    Text = GetEnumDescription(value),
                                                    Value = value.ToString(),
                                                    Selected = value.Equals(metadata.Model)
                                                };

            // If the enum is nullable, add an 'empty' item to the collection
            if (metadata.IsNullableValueType)
                items = SingleEmptyItem.Concat(items);

            return htmlHelper.DropDownListFor(expression, items, htmlAttributes);
        }

        private static Type GetNonNullableModelType(ModelMetadata modelMetadata)
        {
            Type realModelType = modelMetadata.ModelType;

            Type underlyingType = Nullable.GetUnderlyingType(realModelType);
            if (underlyingType != null)
            {
                realModelType = underlyingType;
            }
            return realModelType;
        }

        public static string GetEnumDescription<TEnum>(TEnum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] descrAttributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if ((descrAttributes != null) && (descrAttributes.Length > 0))
            {
                return descrAttributes[0].Description;
            }

            DisplayAttribute[] displAttributes = (DisplayAttribute[])fi.GetCustomAttributes(typeof(DisplayAttribute), false);

            if ((displAttributes != null) && (displAttributes.Length > 0))
            {
                return displAttributes[0].Name;
            }

            return value.ToString();
        }
    }
}