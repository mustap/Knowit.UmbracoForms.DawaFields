using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Forms.Core.Providers;

namespace Knowit.UmbracoForms.DawaFields.Composers;

public class DawaFieldsComposer: IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        // Register custom field types
        builder.Services.AddSingleton<Knowit.UmbracoForms.DawaFields.Fields.DawaAddrOne>();
        builder.Services.AddSingleton<Knowit.UmbracoForms.DawaFields.Fields.DawaAddrMulti>();

        // Add to field collection
        builder.WithCollectionBuilder<FieldCollectionBuilder>()
            .Add<Knowit.UmbracoForms.DawaFields.Fields.DawaAddrOne>()
            .Add<Knowit.UmbracoForms.DawaFields.Fields.DawaAddrMulti>();
    }
}
