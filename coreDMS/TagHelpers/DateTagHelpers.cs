using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreDMS.TagHelpers
{
    [HtmlTargetElement("td", Attributes = ProgressValueAttributeName)]
    public class DateTagHelpers : TagHelper
    {
        private const string ProgressValueAttributeName = "dt-value";
        private const string FormatValueAttributeName = "dt-format";

        [HtmlAttributeName(ProgressValueAttributeName)]
        public string ProgressValue { get; set; }

        [HtmlAttributeName(FormatValueAttributeName)]
        public string FormatValue { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "td";
            if (!string.IsNullOrEmpty(ProgressValue))
            {
                output.Content.SetContent(GetFormattedDate(ProgressValue, FormatValue));
            }
            else
            {
                output.Content.SetContent(ProgressValue);
            }
        }

        public static string GetFormattedDate(string inputDate, string format)
        {
            DateTime dt;
            DateTime.TryParse(inputDate, out dt);
            if (dt != null)
            {
                return dt.ToString(format);
            }
            return "";
        }
    }
}
