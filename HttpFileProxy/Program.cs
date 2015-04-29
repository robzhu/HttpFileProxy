using System;
using Microsoft.Owin.Hosting;

namespace HttpFileProxy
{
    class Program
    {
        static void Main( string[] args )
        {
            var url = "http://*:700";
            using( WebApp.Start( url ) )
            {
                Console.WriteLine( "Service started on {0}", url );
                Console.WriteLine( "Files will be get/put to {0}", FileController.Directory );
                Console.WriteLine( "Press ENTER to stop." );
                Console.ReadLine();
            }
        }
    }
}
