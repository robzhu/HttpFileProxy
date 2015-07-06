using System.Collections.Generic;
using System.Web.Http.Description;
using Swashbuckle.Swagger;

namespace HttpFileProxy
{
    public class AddFileParamTypes : IOperationFilter
    {
        public void Apply( Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription )
        {
            if( operation.operationId == "File_AltUploadAsync" )  // controller and action name
            {
                operation.consumes.Add( "multipart/form-data" );
                operation.parameters = new List<Parameter>
                {
                    new Parameter
                    {
                        name = "file",
                        required = true,
                        type = "file",
                    }
                };
            }
        }
    }
}
