using Knowit.UmbracoForms.DawaFields.DataViews;
using Umbraco.Forms.Core.Services;

namespace Knowit.UmbracoForms.DawaFields.Services;

public class DawaWorkflowEmailService: IWorkflowEmailService
{
    private readonly IWorkflowEmailService _inner;
    private readonly IDawaFieldView _dataFieldView;

    public DawaWorkflowEmailService(IWorkflowEmailService inner, IDawaFieldView dataFieldView)
    {
        _inner = inner;
        _dataFieldView = dataFieldView;
    }

    public async Task SendEmailAsync(SendEmailArgs args)
    {
        var dawaAttachments = _dataFieldView.GetDawaAttachments(args.Body);
        args.Body = _dataFieldView.GetDawaDataViewIfNoTemplate(args.Body);
        args.Attachments = args.Attachments.Concat(dawaAttachments);
        await _inner.SendEmailAsync(args);
    }
}
