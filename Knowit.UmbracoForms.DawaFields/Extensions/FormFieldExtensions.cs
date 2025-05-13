using Knowit.UmbracoForms.DawaFields.Models;
using Umbraco.Forms.Core.Models;

namespace Knowit.UmbracoForms.DawaFields.Extensions;

public static class FormFieldExtensions
{
    public static string ToHtmlTable(this FormFieldHtmlModel field)
    {
        var fieldText = field?.GetValue()?.ToString();
        if (string.IsNullOrWhiteSpace(fieldText)) return "";

        var html = "";
        var json = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(fieldText));

        var addresses = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AddressModel>>(fieldText);
        if (addresses != null && addresses.Count > 0)
        {
            html += $"""
                     <div class="dawa-addrs">
                        <div class="dawa-adr" data-filename="{field!.Name}" data-json="{fieldText}"></div>
                        <table class="table table-striped table-bordered">
                            <thead>
                                <tr>
                                    <th>Address Text</th>
                                    <th>Address Id</th>
                                </tr>
                            </thead>
                            <tbody>
                     """;
            foreach (var addr in addresses)
            {
                html += $"""
                                <tr>
                                    <td>{addr.Text}</td>
                                    <td>{addr.Id}</td>
                                </tr>
                         """;
            }

            html += """
                            </tbody>
                        </table>
                    </div>
                    """;
        }
        return html;
    }
}
