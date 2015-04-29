using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace HttpFileProxy
{
    public class FileResult : IHttpActionResult
    {
        static readonly MediaTypeHeaderValue MediaType = new MediaTypeHeaderValue( "application/octet-stream" );
        private readonly string _filePath;

        public FileResult( string filePath )
        {
            if( filePath == null ) throw new ArgumentNullException( "filePath" );
            _filePath = filePath;
        }

        public Task<HttpResponseMessage> ExecuteAsync( CancellationToken cancelToken )
        {
            return Task.Run( () =>
            {
                var response = new HttpResponseMessage( HttpStatusCode.OK )
                {
                    Content = new StreamContent( File.OpenRead( _filePath ) )
                };

                response.Content.Headers.ContentType = MediaType;
                return response;

            }, cancelToken );
        }
    }
}
