using EntityFrameworkCore.Scaffolding.Handlebars.Internal;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace INFINITE.CORE.Data.CodeGenerator
{
    public class ScaffoldingDesignTimeServices : IDesignTimeServices
    {
        [ExcludeFromCodeCoverage]
        public void ConfigureDesignTimeServices(IServiceCollection services)
        {
            //Debugger.Launch();

            services.AddHandlebarsScaffolding();

            var custom_entity = (helperName: "custom-entity", helperFunction: (Action<HandlebarsDotNet.EncodedTextWriter, HandlebarsDotNet.Context, HandlebarsDotNet.Arguments>)CustomEntity);
            services.AddHandlebarsHelpers(custom_entity);

#pragma warning disable EF1001 // Rethrow to preserve stack details
            services.AddSingleton<ICSharpDbContextGenerator, CodeGenerator>();
#pragma warning restore EF1001 // Rethrow to preserve stack details

        }
        void CustomEntity(HandlebarsDotNet.EncodedTextWriter writer, HandlebarsDotNet.Context context, HandlebarsDotNet.Arguments parameters)
        {
            //context.Value["properties"]//["property-type"].Value;
            List<Dictionary<string, object>> properties = (List<Dictionary<string, object>>)context["properties"];//["0"]["property_type"];//,0, "property-type"];
            if (properties != null && properties.Count > 0)
            {
                switch (properties[0]["property-type"])
                {
                    case "int":
                        writer.Write("BaseIntEntity");
                        break;
                    default:
                        writer.Write("IEntity");
                        break;
                }
            }
            else
                writer.Write("IEntity");
        }
    }
}
