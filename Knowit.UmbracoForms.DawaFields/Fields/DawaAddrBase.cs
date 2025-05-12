using Knowit.UmbracoForms.DawaFields.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Umbraco.Forms.Core;
using Umbraco.Forms.Core.Attributes;
using Umbraco.Forms.Core.Enums;
using Umbraco.Forms.Core.Models;
using Umbraco.Forms.Core.Services;

namespace Knowit.UmbracoForms.DawaFields.Fields;

public class DawaAddrBase: FieldType
{
    private readonly IHttpClientFactory _httpClientFactory;

    public DawaAddrBase(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;

        Icon = "icon-home";
        DataType = FieldDataType.String;
        SortOrder = 10;
        MandatoryByDefault = true;
        SupportsRegex = false;
        SupportsPreValues = false;
    }

    public override Dictionary<string, SettingAttribute> Settings()
    {
        var settings = base.Settings();
        settings.Remove("DefaultValue");
        settings.Remove("SelectPrompt");
        settings.Remove("AutocompleteAttribute");
        return settings;
    }

    public override bool SupportsPreValues => false;
    public override bool SupportsRegex => false;
    public override string GetDesignView() => "~/App_Plugins/Knowit.UmbracoForms.DawaFields/Backoffice/FieldType.html";
    public override string RenderView => "~/App_Plugins/Knowit.UmbracoForms.DawaFields/Backoffice/Common/RenderTypes/Field.html";

    public override IEnumerable<string> ValidateField(Form form, Field field, IEnumerable<object> postedValues, HttpContext context, IPlaceholderParsingService placeholderParsingService, IFieldTypeStorage fieldTypeStorage)
    {
        var returnStrings = new List<string>();

        var addresses = postedValues.FirstOrDefault()?.ToString() ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(addresses))
        {
            var values = JsonConvert.DeserializeObject<List<AddressModel>>(addresses);
            if (values is not null && values.Count > 0)
            {
                returnStrings = Validate(values);
            }
        }

        // Also validate it against the default method (to handle mandatory fields and regular expressions)
        postedValues = addresses == "[]" ? new List<object>() { "" } : postedValues;
        return base.ValidateField(form, field, postedValues, context, placeholderParsingService, fieldTypeStorage, returnStrings);
    }

    public override IEnumerable<string> RequiredCssFiles(Field field)
    {
        return (IEnumerable<string>) new List<string>
        {
            "~/App_Plugins/Knowit.UmbracoForms.DawaFields/dawa.css"
        };
    }

    public override IEnumerable<string> RequiredJavascriptFiles(Field field)
    {
        return (IEnumerable<string>) new List<string>
        {
            "https://cdn.dataforsyningen.dk/dawa/assets/dawa-autocomplete2/1.0.2/dawa-autocomplete2.min.js",
            "~/App_Plugins/Knowit.UmbracoForms.DawaFields/dawa.js"
        };
    }

    private List<string> Validate(List<AddressModel> addresses)
    {
        if(addresses.Count == 0)
        {
            return new List<string>();
        }

        var returnStrings = new List<string>();

        var client = _httpClientFactory.CreateClient();
        client.BaseAddress = new Uri("https://api.dataforsyningen.dk");

        foreach (var addr in addresses)
        {
            try
            {
                var response = client.GetAsync("/autocomplete?id=" + addr.Id).Result;

                if (!response.IsSuccessStatusCode)
                {
                    returnStrings.Add("Cannot validate address data at Dawa api !");
                    continue;
                }

                var json = response.Content.ReadAsStringAsync().Result;
                var addrs = JsonConvert.DeserializeObject<List<dynamic>>(json);
                var valid = addrs?.FirstOrDefault(x => x.data.id == addr.Id);
                if (valid is not null) continue;
                returnStrings.Add($"Invalid address: '{addr.Text}'");
            }
            catch
            {
                returnStrings.Add("Error while validating address data !");
            }
        }
        return returnStrings;
    }
}
