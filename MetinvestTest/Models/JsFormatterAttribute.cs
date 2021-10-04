using System;

namespace MetinvestTest.Models
{
    public class JsFormatterAttribute : Attribute
    {
        public string FormatterName { get; set; }
        public JsFormatterAttribute(string formatterName)
        {
            FormatterName = formatterName;
        }
    }
}