using Microsoft.Extensions.Configuration;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketEngine;
using SuperSocketStudy.SuperSocket;
using System;

namespace SuperSocketStudy
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var bootstrap = BootstrapFactory.CreateBootstrap();
            if (false == bootstrap.Initialize())
            {
                Console.WriteLine("Failed to initialize!");
                Console.ReadKey();
                return;
            }

            var result = bootstrap.Start();
            Console.WriteLine("Start result: {0}!", result);

            if (StartResult.Failed == result)
            {
                Console.WriteLine("Failed to start!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("The server started successfully, press key 'q' to stop it!");

            while (Console.ReadKey().KeyChar != 'q')
            {
                Console.WriteLine();
                continue;
            }

            Console.WriteLine();

            //Stop the appServer
            bootstrap.Stop();

            Console.WriteLine("The server was stopped!");
            Console.ReadKey();
        }
    }
}
