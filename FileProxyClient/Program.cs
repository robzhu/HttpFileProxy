using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FileProxyClient
{
    /// <summary>
    /// This sample app shows how to call the POST variant of the HttpFileProxy API using the a parameter object within the multipart content.
    /// </summary>
    class Program
    {
        static void Main( string[] args )
        {
            MainAsync().Wait();
        }

        const string SourceFile = "_log.txt";
        const string SourceFolder = @"C:\temp\";
        const string SourceFileFullPath = SourceFolder + SourceFile;
        const string DownloadTarget = SourceFolder + "download.txt";

        static async Task MainAsync()
        {
            var httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromHours( 2 ),
            };

            var url = "http://localhost:700/file";
            var content = new MultipartFormDataContent();

            var streamContent = new StreamContent( File.OpenRead( SourceFileFullPath ) );
            streamContent.Headers.Add( "Content-Type", "application/octet-stream" );

            content.Add( streamContent, "file", SourceFile );

            try
            {
                var response = await httpClient.PostAsync( url, content );
                response.EnsureSuccessStatusCode();

                Console.WriteLine( "File upload successful." );

                //try to download the file.
                var location = response.Headers.GetValues( "Location" ).FirstOrDefault();
                Console.WriteLine( "Attempting to download from {0}", location );

                response = await httpClient.GetAsync( location );
                response.EnsureSuccessStatusCode();


                if( File.Exists( DownloadTarget ) )
                {
                    File.Delete( DownloadTarget );
                }

                using( Stream contentStream = await response.Content.ReadAsStreamAsync(),
                       fileStream = new FileStream( DownloadTarget, FileMode.Create ) )
                {
                    await contentStream.CopyToAsync( fileStream );
                }

                Console.WriteLine( "File download successful: {0}", DownloadTarget );
                Process.Start( SourceFolder );
            }
            catch( HttpRequestException ex )
            {
                Console.WriteLine( "An Http Request exception occured: " );
                Console.WriteLine( ex.Message );
                Console.WriteLine( "Is the service running?" );
            }
            finally
            {
                File.Delete( DownloadTarget );
            }
        }
    }
}
