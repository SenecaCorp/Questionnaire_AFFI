using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;

namespace QuestionnairePrototype.Infrastructure
{
    public class CssJsViewAppnder
    {
        private static CssJsViewAppnder _instance;

        public static CssJsViewAppnder GetInstance(HtmlHelper htmlHelper)
        {
            if (_instance == null)
                _instance = new CssJsViewAppnder();

            _instance.SetHtmlHelper(htmlHelper);

            return _instance;
        }

        private HtmlHelper _htmlHelper;

        public ItemRegistrar Styles { get; private set; }
        public ItemRegistrar Scripts { get; private set; }

        public CssJsViewAppnder()
        {
            Styles = new ItemRegistrar(ItemRegistrarFromatters.StyleFormat);
            Scripts = new ItemRegistrar(ItemRegistrarFromatters.ScriptFormat);
        }

        private void SetHtmlHelper(HtmlHelper htmlHelper)
        {
            _htmlHelper = htmlHelper;
        }
    }

    public class ItemRegistrar
    {
        private readonly string _format;
        private readonly List<string> _items;

        public ItemRegistrar(string format)
        {
            _format = format;
            _items = new List<string>();
        }

        public ItemRegistrar Add(string url)
        {
            if (!_items.Contains(url))
                _items.Insert(0, url);

            return this;
        }

        public IHtmlString Render()
        {
            var sb = new StringBuilder();

            foreach (var item in _items)
            {
                var fmt = string.Format(_format, item);
                sb.AppendLine(fmt);
            }

            return new HtmlString(sb.ToString());
        }

        public void Clear()
        {
            _items.Clear();
        }
    }

    public class ItemRegistrarFromatters
    {
        public const string StyleFormat = "<link href=\"{0}\" rel=\"stylesheet\" type=\"text/css\" />";
        public const string ScriptFormat = "<script src=\"{0}\" type=\"text/javascript\"></script>";
    }
}