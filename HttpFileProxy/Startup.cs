using System.Collections.Generic;
using System.Net.Http.Formatting;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Owin;
using Swashbuckle.Application;

namespace HttpFileProxy
{
    public class Startup
    {
        public const string DefaultApiRouteName = "api";

        public static void UseJsonSerialization( HttpConfiguration config )
        {
            var defaultSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = new List<JsonConverter>
                {
                    new StringEnumConverter { CamelCaseText = true },
                }
            };

            JsonConvert.DefaultSettings = () => defaultSettings;

            config.Formatters.Clear();
            config.Formatters.Add( new JsonMediaTypeFormatter() );
            config.Formatters.JsonFormatter.SerializerSettings = defaultSettings;
        }

        public void Configuration( IAppBuilder app )
        {
            var config = new HttpConfiguration();

            UseJsonSerialization( config );
            config.Routes.MapHttpRoute( DefaultApiRouteName, "{controller}/{id}", defaults: new { controller = "Root", id = RouteParameter.Optional } );
            config.MapHttpAttributeRoutes();

            config.EnableSwagger( c =>
                {
                    c.IncludeXmlComments( "docs.xml" );
                    c.SingleApiVersion( "1.0", "Http File API" );
                } )
                .EnableSwaggerUi();

            app.UseWebApi( config );
        }
    }
}
