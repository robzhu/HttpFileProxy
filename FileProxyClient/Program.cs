using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

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

        static async Task MainAsync()
        {
            var httpClient = new HttpClient();
            var fileName = @"C:\logs\log-file.txt";     //replace with an actual file you want to upload
            var url = "http://localhost:700/file";

            var content = new MultipartFormDataContent();

            var fileDefinitionContent = new StringContent( JsonConvert.SerializeObject( new
            {
                Name = "log-file.txt",          //this is the name of the file that will be saved on the server.
            } ) );
            content.Add( fileDefinitionContent, "json" );

            var streamContent = new StreamContent( File.OpenRead( fileName ) );
            streamContent.Headers.Add( "Content-Type", "application/octet-stream" );
            content.Add( streamContent, "file", fileName );

            var response = await httpClient.PostAsync( url, content );
            response.EnsureSuccessStatusCode();
        }
    }
}
