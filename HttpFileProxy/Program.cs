using System;
using Microsoft.Owin.Hosting;

namespace HttpFileProxy
{
    class Program
    {
        static void Main( string[] args )
        {
            Console.Write( "Local directory: ({0}) [Press ENTER to accept]", FileController.Directory );
            var input = Console.ReadLine();

            if( !string.IsNullOrWhiteSpace( input ) )
            {
                FileController.Directory = input;
            }

            var url = "http://*:700";
            using( WebApp.Start( url ) )
            {
                Console.WriteLine( "Service started on {0}", url );

                Console.WriteLine();

                Console.WriteLine( "Upload:" );
                Console.WriteLine( "curl -X PUT  -i -F form-data=@dump.rdb http://server.com:700/file/dump.rdb" );
                Console.WriteLine( "curl -X POST -i -F form-data=@dump.rdb http://server.com:700/file" );

                Console.WriteLine();

                Console.WriteLine( "Download:" );
                Console.WriteLine( "curl -i http://server.com:700/file/dump.rdb > dump.rdb" );

                Console.WriteLine();

                Console.WriteLine( "Files will be get/put to {0}", FileController.Directory );
                Console.WriteLine( "Press ENTER to stop." );
                Console.ReadLine();
            }
        }
    }
}
