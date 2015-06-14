using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;

namespace HttpFileProxy
{
    /// <summary>
    /// Controller for file-related operations.
    /// </summary>
    //[RoutePrefix( "file" )]
    public class FileController : ApiController
    {
        public static string Directory = "c:\\http_files\\";

        /// <summary>
        /// Uploads a file to the server. If the file already exists, it will be overwritten.
        /// The file contents should be contained in the form-data.
        /// </summary>
        /// <param name="file">The name of the file to upload.</param>
        [HttpPut]
        [Route( "{file}" )]
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

        /// <summary>
        /// Uploads a file to the server with additional parameters expressed in multipart content.
        /// </summary>
        [HttpPost]
        public async Task<IHttpActionResult> AlternateUploadAsync()
        {
            if( !Request.Content.IsMimeMultipartContent( "form-data" ) )
            {
                throw new HttpResponseException( Request.CreateResponse( HttpStatusCode.UnsupportedMediaType ) );
            }

            MultipartMemoryStreamProvider provider = await Request.Content.ReadAsMultipartAsync();

            if( provider.Contents.Count != 2 )
            {
                return BadRequest( "Request should contain exactly two pieces of content: parameters and data." );
            }

            var paramContent = provider.Contents[ 0 ];
            FileDefinition fileDef = JsonConvert.DeserializeObject<FileDefinition>( await paramContent.ReadAsStringAsync() );

            StreamContent fileContent = provider.Contents[ 1 ] as StreamContent;
            var saveFileStreamProvider = new SaveLocalFileStreamProvider( Directory, fileDef.Name );
            var fileName = Directory + fileDef.Name;
            using( FileStream fs = new FileStream( fileName, FileMode.Create ) )
            {
                await fileContent.CopyToAsync( fs );
                await fs.FlushAsync();
                Console.WriteLine( "Successfully saved file: {0}", fileName );
            }

            return Ok( fileName );
        }

        /// <summary>
        /// Downloads the specified file.
        /// </summary>
        /// <param name="file">The name of the file to download.</param>
        [HttpGet]
        [Route( "{file}" )]
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
