using System.Web.Http;

namespace HttpFileProxy
{
    public class HealthController : ApiController
    {
        /// <summary>
        /// Performs a simple health check on the service.
        /// </summary>
        public IHttpActionResult Get()
        {
            return Ok();
        }
    }
}
