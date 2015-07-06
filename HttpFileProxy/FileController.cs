using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

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
        /// <param name="fileName">The name of the file to upload.</param>
        [HttpPut]
        [Route( "{fileName}" )]
        public async Task<IHttpActionResult> UploadAsync( string fileName )
        {
            if( !Request.Content.IsMimeMultipartContent( "form-data" ) )
            {
                throw new HttpResponseException( Request.CreateResponse( HttpStatusCode.UnsupportedMediaType ) );
            }

            var streamProvider = new SaveLocalFileStreamProvider( Directory, fileName );
            await Request.Content.ReadAsMultipartAsync( streamProvider );

            return Ok( Directory + fileName );
        }

        /// <summary>
        /// Uploads a file to the server with additional parameters expressed in multipart content.
        /// </summary>
        [HttpPost]
        public async Task<IHttpActionResult> AltUploadAsync()
        {
            if( !Request.Content.IsMimeMultipartContent( "form-data" ) )
            {
                throw new HttpResponseException( Request.CreateResponse( HttpStatusCode.UnsupportedMediaType ) );
            }

            MultipartMemoryStreamProvider provider = await Request.Content.ReadAsMultipartAsync();
            if( provider.Contents.Count != 1 )
            {
                return BadRequest( "Request should contain exactly one pieces of content." );
            }

            StreamContent fileContent = provider.Contents[ 0 ] as StreamContent;
            var headers = fileContent.Headers;

            var fileName = headers.ContentDisposition.FileNameStar;
            if( string.IsNullOrEmpty( fileName ) )
            {
                fileName = headers.ContentDisposition.FileName;
                fileName = fileName.TrimStart( '\"' );
                fileName = fileName.TrimEnd( '\"' );
            }

            string fullFileName = Directory + fileName;

            using( FileStream fs = new FileStream( fullFileName, FileMode.Create ) )
            {
                await fileContent.CopyToAsync( fs );
                await fs.FlushAsync();
                Console.WriteLine( "Successfully saved file: {0}", fullFileName );
            }

            Process.Start( "explorer.exe", Path.GetDirectoryName( fullFileName ) );

            var text = File.ReadAllText( fullFileName );
            return CreatedAtRoute( Route_Download, new { file = fileName }, text );
        }

        const string Route_Download = "Route_Download";
        /// <summary>
        /// Downloads the specified file.
        /// </summary>
        /// <param name="file">The name of the file to download.</param>
        [HttpGet]
        [Route( "file/{file}", Name = Route_Download )]
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
