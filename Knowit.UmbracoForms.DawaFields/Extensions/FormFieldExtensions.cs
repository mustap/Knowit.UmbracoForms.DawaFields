using Knowit.UmbracoForms.DawaFields.DataViews;
using Knowit.UmbracoForms.DawaFields.Models;
using Umbraco.Forms.Core.Models;

namespace Knowit.UmbracoForms.DawaFields.Extensions;

public static class FormFieldExtensions
{
    public static string ToHtml(this FormFieldHtmlModel field, IDawaFieldView dawaFieldView)
    {
        var fieldText = field?.GetValue()?.ToString();
        if (string.IsNullOrWhiteSpace(fieldText)) return "";

        var addresses = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AddressModel>>(fieldText);
        return addresses is { Count: > 0 } ? dawaFieldView.GetDawaDataView(field.Name, fieldText, addresses) : "";
    }
}
