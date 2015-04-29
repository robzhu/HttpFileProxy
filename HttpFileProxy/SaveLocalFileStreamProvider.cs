using System.Net.Http;

namespace HttpFileProxy
{
    public class SaveLocalFileStreamProvider : MultipartFormDataStreamProvider
    {
        private string _fileName;

        public SaveLocalFileStreamProvider( string directory, string fileName ) : base( directory )
        {
            _fileName = fileName;
        }

        public override string GetLocalFileName( System.Net.Http.Headers.HttpContentHeaders headers )
        {
            return _fileName;
        }
    }
}
