using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Razor.Runtime.TagHelpers;

namespace Fritz.Bttf.TagHelpers
{
	
	public enum DateDestType {
		NotSet = 0,
		Destination,
		Present,
		LastDeparted
	}

	public class DateDestTagHelper : TagHelper {
		
		public DateTime Date { get; set; }
		
		public DateDestType Type { get; set; } = DateDestType.NotSet;
		
		private static readonly Dictionary<string,string> FormatDict = new Dictionary<string,string>() {
			{"month", "MMM"},
			{"day", "dd"},
			{"year", "yyyy"},
			{"hour", "hh"},
			{"min", "mm"}	
		};
		
		private static readonly Dictionary<DateDestType, string> CssMap = new Dictionary<DateDestType, string>() {
			{DateDestType.Destination, "dest"},
			{DateDestType.Present, "now"},
			{DateDestType.LastDeparted, "from"}	
		};
		
		private static readonly Dictionary<DateDestType, string> DateLabels = new Dictionary<DateDestType, string>() {
			{DateDestType.Destination, "destination time"},
			{DateDestType.Present, "present time"},
			{DateDestType.LastDeparted, "last time departed"}
		};
		
		public HtmlString Format(string part) {
		
			return new HtmlString(part.Replace("1", "&nbsp;1"));
			
		}

		public override void Process(TagHelperContext context, TagHelperOutput output) {
	
			if (Date == null) throw new ArgumentNullException("Missing date argument on DateDest tag");			
			if (Type == DateDestType.NotSet) throw new ArgumentNullException("Missing date type");
			
			// Build outer row
			output.TagName = "div";
			output.Attributes["class"] = "timeRow";
			output.TagMode = TagMode.StartTagAndEndTag;
			
			AddTopLabels(output);
			
			output.Content.AppendEncoded("<div class=\"row\">");
			
			AddDatePart(output, "month", 3);
			AddDatePart(output, "day", 1);
			AddDatePart(output, "year", 3);
			AddDatePart(output, "pm", 1);
			AddDatePart(output, "hour", 2);
			AddDatePart(output, "min", 2);
			
			output.Content.AppendEncoded("</div>");
			
			AddDateLabel(output);
			
		}

        private void AddDateLabel(TagHelperOutput output)
        {
            
			var sb = new StringBuilder();
			sb.AppendLine("<div class=\"row\">");
			sb.AppendLine("<div class=\"col-sm-12 timeId\">");
			sb.Append(DateLabels[Type]);
			sb.AppendLine("</div>");
			sb.AppendLine("</div>");
			
			output.Content.AppendEncoded(sb.ToString());
			
        }

        private void AddDatePart(TagHelperOutput output, string type, int width)
        {
            
			var sb = new StringBuilder();
			
			sb.AppendLine($"<div class=\"col-sm-{width}\">");
			
			if (FormatDict.ContainsKey(type)) {
				sb.AppendLine($"<span class=\"{type}\">");
				sb.AppendLine($"<i class=\"digit {CssMap[Type]} bg\">88</i>");
				sb.AppendLine($"<i class=\"digit {CssMap[Type]}\">{Format(Date.ToString(FormatDict[type]))}</i>");
				sb.AppendLine("</span>");
			}
			sb.AppendLine("</div>");
			
			output.Content.AppendEncoded(sb.ToString());
			
        }

        private void AddTopLabels(TagHelperOutput output)
        {

			var sb = new StringBuilder();
            
			sb.Append(@"<div class=""row"">
				<div class=""col-sm-3""><span class=""lbl"">Month</span></div>
				<div class=""col-sm-1""><span class=""lbl"">Day</span></div>
				<div class=""col-sm-3""><span class=""lbl"">Year</span></div>
				<div class=""col-sm-1""><span class=""lbl"">PM</span></div>
				<div class=""col-sm-2""><span class=""lbl"">Hour</span></div>
				<div class=""col-sm-2""><span class=""lbl"">Min</span></div>
			</div>");
			
			output.Content.AppendEncoded(sb.ToString());
			
        }
    }
	
}