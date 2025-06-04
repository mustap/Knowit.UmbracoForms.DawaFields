using Knowit.UmbracoForms.DawaFields.Models;
using Umbraco.Cms.Core.Models.Email;

namespace Knowit.UmbracoForms.DawaFields.DataViews;

public interface IDawaFieldView
{
    public string GetDawaDataView(string fieldName, string fieldValue, List<AddressModel>? addresses);
    public string GetDawaDataVieIfNoTemplate(string textBody);
    public List<EmailMessageAttachment> GetDawaAttachments(string text);
}
