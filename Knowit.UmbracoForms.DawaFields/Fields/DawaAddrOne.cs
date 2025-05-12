namespace Knowit.UmbracoForms.DawaFields.Fields;

public sealed class DawaAddrOne : DawaAddrBase
{
    public DawaAddrOne(IHttpClientFactory httpClientFactory): base(httpClientFactory)
    {
        Id = new Guid("f6dfed1e-3e4a-4c1c-9935-d7c47b9a4d8f");
        Alias = "DawaAddrOne";
        Name = "DAWA 1 Adr.";
        Description = "Allows users to select a single address using DAWA";
        SortOrder = 10;
        FieldTypeViewName = "DawaAddrOne.cshtml";
    }
}
