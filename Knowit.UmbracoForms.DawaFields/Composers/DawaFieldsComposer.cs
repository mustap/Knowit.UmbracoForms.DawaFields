using Knowit.UmbracoForms.DawaFields.DataViews;
using Knowit.UmbracoForms.DawaFields.Fields;
using Knowit.UmbracoForms.DawaFields.Services;
using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Forms.Core.Providers;
using Umbraco.Forms.Core.Services;

namespace Knowit.UmbracoForms.DawaFields.Composers;

public class DawaFieldsComposer: IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        // Register custom field types
        builder.Services.AddSingleton<DawaAddrOne>();
        builder.Services.AddSingleton<DawaAddrMulti>();
        builder.Services.AddSingleton<IDawaFieldView, DawaFieldDefaultView>();

        // Add to field collection
        builder.WithCollectionBuilder<FieldCollectionBuilder>()
            .Add<DawaAddrOne>()
            .Add<DawaAddrMulti>();

        // Decorate the original IWorkflowEmailService with DawaWorkflowEmailService
        var descriptor = builder.Services.FirstOrDefault(d =>
            d.ServiceType == typeof(IWorkflowEmailService));

        if (descriptor != null)
        {
            builder.Services.Remove(descriptor);

            var originalType = descriptor.ImplementationType!;
            var lifetime = descriptor.Lifetime;

            builder.Services.Add(ServiceDescriptor.Describe(originalType, originalType, lifetime));

            builder.Services.Add(ServiceDescriptor.Describe(
                typeof(IWorkflowEmailService),           // Original interface
                sp => new DawaWorkflowEmailService(
                    (WorkflowEmailService)sp.GetRequiredService(originalType),
                    sp.GetRequiredService<IDawaFieldView>()
                ),
                lifetime));
        }
    }
}
