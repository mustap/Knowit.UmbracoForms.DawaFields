using Umbraco.Forms.Core.Enums;

namespace Knowit.UmbracoForms.DawaFields.Fields;

public sealed class DawaAddrMulti: DawaAddrBase
{
    public DawaAddrMulti(IHttpClientFactory httpClientFactory): base(httpClientFactory)
    {
        Id = new Guid("d0b743d4-a9ed-4c5b-a198-bb9e13a29382");
        DataType = FieldDataType.LongString;
        Alias = "DawaAddrMulti";
        Name = "DAWA Multi Adr.";
        Description = "Allows users to select multiple addresses using DAWA";
        SortOrder = 10;
        FieldTypeViewName = "DawaAddrMulti.cshtml";
    }
}
