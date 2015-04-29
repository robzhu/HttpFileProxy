using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.IO;

namespace HttpFileProxy
{
    /// <summary>
    /// Controller for file-related operations.
    /// </summary>
    public class FileController : ApiController
    {
        public static string Directory = "d:\\http_files\\";

        /// <summary>
        /// Uploads a file to the server. If the file already exists, it will be overwritten.
        /// The file contents should be contained in the form-data.
        /// </summary>
        /// <param name="file">The name of the file to upload.</param>
        [HttpPut]
        public async Task<IHttpActionResult> UploadAsync( string file )
        {
            if( !Request.Content.IsMimeMultipartContent( "form-data" ) )
            {
                throw new HttpResponseException( Request.CreateResponse( HttpStatusCode.UnsupportedMediaType ) );
            }

            var streamProvider = new SaveLocalFileStreamProvider( Directory, file );
            await Request.Content.ReadAsMultipartAsync( streamProvider );
            return Ok( Directory + file );
        }

        [HttpGet]
        public async Task<IHttpActionResult> DownloadAsync( string file )
        {
            FileInfo fi = new FileInfo( Directory + file );
            if( !fi.Exists )
            {
                return NotFound();
            }
            return new FileResult( fi.FullName );
        }
    }
}
