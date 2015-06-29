using System.Collections.Generic;
using System.Web.Http.Description;
using Swashbuckle.Swagger;

namespace HttpFileProxy
{
    public class AddFileParamTypes : IOperationFilter
    {
        public void Apply( Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription )
        {
            if( operation.operationId == "File_AlternateUploadAsync" )  // controller and action name
            {
                operation.consumes.Add( "multipart/form-data" );
                operation.parameters = new List<Parameter>
                {
                    //new Parameter
                    //{
                    //    name = "fileName",
                    //    description = "the name of the file when it is saved on the server.",
                    //    required = true,
                    //    type = "path",
                    //    @in = "formData",
                    //},
                    new Parameter
                    {
                        name = "file",
                        required = true,
                        type = "file",
                    }
                };
            }

            //if( operation.operationId == "File_UploadAsync" )  // controller and action name
            //{
            //    operation.consumes.Add( "multipart/form-data" );
            //    operation.parameters = new List<Parameter>
            //    {
            //        new Parameter
            //        {
            //            name = "fileName",
            //            description = "the name of the file when it is saved on the server.",
            //            required = true,
            //            type = "path",
            //        },
            //        new Parameter
            //        {
            //            name = "file",
            //            required = true,
            //            type = "file",
            //        }
            //    };
            //}
        }
    }
}
