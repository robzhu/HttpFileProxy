using System.Web.Http;
using System.Web.Http.Description;

namespace HttpFileProxy
{
    /// <summary>
    /// Default controller that redirects to the API documentation.
    /// </summary>
    [ApiExplorerSettings( IgnoreApi = true )]
    public class RootController : ApiController
    {
        public IHttpActionResult Get()
        {
            var request = Request.RequestUri;
            return Redirect( request.ToString() + "swagger/ui/index" );
        }
    }
}
