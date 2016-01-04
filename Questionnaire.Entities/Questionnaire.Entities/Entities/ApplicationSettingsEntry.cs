using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Questionnaire.Entities
{
    public class ApplicationSettingsEntry
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}