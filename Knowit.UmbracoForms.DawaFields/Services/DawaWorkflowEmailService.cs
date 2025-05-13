using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Microsoft.AspNetCore.Mvc.Routing;
using Umbraco.Cms.Core.Models.Email;
using Umbraco.Forms.Core.Services;

namespace Knowit.UmbracoForms.DawaFields.Services;

public class DawaWorkflowEmailService: IWorkflowEmailService
{
    private readonly IWorkflowEmailService _inner;

    public DawaWorkflowEmailService(IWorkflowEmailService inner)
    {
        _inner = inner;
    }


    public async Task SendEmailAsync(SendEmailArgs args)
    {

        if (args.Body.Contains("class=\"dawa-adr\"")) // this is a template base email
        {
            Regex pattern = new Regex(@"<div\s*class=""dawa-adr""\s*data-filename=""(.*?)""\s*data-json=""(.*)""\s*>\s*</div>");
            args.Attachments = GetDawaAddressesAsAttachments(args.Body, pattern);
            args.Body = pattern.Replace(args.Body, "");
        }
        else if (args.Body.Contains("</strong></dt><dd>[{&quot;")) // this is a the base email without template
        {
            Regex pattern = new Regex(@"<dt><strong>([^>]*?)</strong></dt><dd>(\[{&quot;.*?&quot;}\])");
            args.Attachments = GetDawaAddressesAsAttachments(args.Body, pattern, true);
        }

        await _inner.SendEmailAsync(args);
    }

    private List<EmailMessageAttachment> GetDawaAddressesAsAttachments(string body, Regex pattern, bool htmlDecode=false )
    {
        var attachments = new List<EmailMessageAttachment>();

        foreach (Match match in pattern.Matches(body))
        {
            var filename = match.Groups[1].Value.Replace(" ", "_") + ".json";
            var json = match.Groups[2].Value;
            var stream = new MemoryStream( Encoding.UTF8.GetBytes(htmlDecode ? HttpUtility.HtmlDecode(json): json));
            attachments.Add(new EmailMessageAttachment(stream, filename));
        }
        return attachments;
    }
}
