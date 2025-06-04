using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Knowit.UmbracoForms.DawaFields.Models;
using Umbraco.Cms.Core.Models.Email;

namespace Knowit.UmbracoForms.DawaFields.DataViews;

public class DawaFieldDefaultView: IDawaFieldView
{
    private readonly Regex TemplatePattern = new(@"<div\s*class=""dawa-adr""\s*data-filename=""(.*?)""\s*data-json=""(.*)""\s*>\s*</div>");
    private readonly Regex BaseTemplatePattern = new(@"<dt><strong>([^>]*?)</strong></dt><dd>(\[{&quot;.*?&quot;}\])");

    public string GetDawaDataView(string fieldName, string fieldValue, List<AddressModel>? addresses)
    {
        var html = $"""
                    <div class="dawa-addrs">
                       <div class="dawa-adr" data-filename="{fieldName}" data-json="{fieldValue}"></div>
                       <table class="table table-striped table-bordered">
                           <thead>
                               <tr><th>Address Text</th><th>Address Id</th></tr>
                           </thead>
                           <tbody>
                    """;

        html += addresses.Aggregate(html, (current, addr) => current + $"<tr><td>{addr.Text}</td><td>{addr.Id}</td></tr>");
        html += "</tbody></table></div>";
        return html;
    }

    public string GetDawaDataViewIfNoTemplate(string textBody)
    {
        return textBody;
    }

    public List<EmailMessageAttachment> GetDawaAttachments(string text)
    {
        var attachments = GetAttachmentsFromTemplate(text);
        attachments.AddRange(GetAttachmentsFromBaseTemplate(text));
        return attachments;
    }

    private List<EmailMessageAttachment> GetAttachmentsFromTemplate(string text)
    {
        if (!text.Contains("class=\"dawa-adr\"")) return [];
        return GetAttachments(text, TemplatePattern);
    }

    private List<EmailMessageAttachment> GetAttachmentsFromBaseTemplate(string text, bool htmlDecode=false)
    {
        if (!text.Contains("</strong></dt><dd>[{&quot;")) return [];
        return GetAttachments(text, BaseTemplatePattern, true);
    }

    private List<EmailMessageAttachment> GetAttachments(string text, Regex pattern, bool htmlDecode=false )
    {
        var attachments = new List<EmailMessageAttachment>();

        foreach (Match match in pattern.Matches(text))
        {
            attachments.Add(GetEmailMessageAttachment(match, htmlDecode));
        }
        return attachments;
    }

    private EmailMessageAttachment GetEmailMessageAttachment(Match match, bool htmlDecode)
    {
        var filename = match.Groups[1].Value.Replace(" ", "_") + ".json";
        var json = match.Groups[2].Value;
        var stream = new MemoryStream( Encoding.UTF8.GetBytes(htmlDecode ? HttpUtility.HtmlDecode(json): json));
        return new EmailMessageAttachment(stream, filename);
    }
}
